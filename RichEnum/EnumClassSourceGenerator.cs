using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace RichEnum;

[Generator]
public class EnumClassSourceGenerator : IIncrementalGenerator
{
    private const string DescriptionAttribute = "System.ComponentModel.DescriptionAttribute";
    private const string RichEnumAttribute = "RichEnum.Attribute.RichEnumAttribute";


    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
            ctx.AddSource("RichEnumAttribute.g.cs", SourceText.From(GeneratorHelper.Attribute, Encoding.UTF8)));
        IncrementalValuesProvider<EnumToGenerate?> incrementalValuesProvider =
            context.SyntaxProvider.ForAttributeWithMetadataName(RichEnumAttribute,
                    (node, _) => { return node is EnumDeclarationSyntax; }, GetTypeToGenerate)
                .Where(static d => d is not null);

        context.RegisterSourceOutput(incrementalValuesProvider, Execute);
    }

    static void Execute(SourceProductionContext context, EnumToGenerate? enumToGenerate)
    {
        if (enumToGenerate is { } eg)
        {
            StringBuilder sb = new StringBuilder();
            var result = GeneratorHelper.GenerateEnumRecord(enumToGenerate);
            context.AddSource($"{eg.Namespace}.{eg.Name}.g.cs", SourceText.From(result, Encoding.UTF8));
        }
    }

    private static EnumToGenerate? GetTypeToGenerate(GeneratorAttributeSyntaxContext context, CancellationToken ct)
    {
        var namedTypeSymbol = context.TargetSymbol as INamedTypeSymbol;
        if (namedTypeSymbol == null)
        {
            return null;
        }

        ct.ThrowIfCancellationRequested();

        var enumName = namedTypeSymbol.Name;
        var containingNamespace = namedTypeSymbol.ContainingNamespace.ToString();
        var fullyQualifiedName = namedTypeSymbol.ToString();
        var underlyingType = namedTypeSymbol.EnumUnderlyingType?.ToString() ?? "int";

        var immutableArray = namedTypeSymbol.GetMembers()
            .Select(d => d as IFieldSymbol).Where(d => d != null).ToArray();
        var enumValues = new List<EnumValue>();
        foreach (var member in immutableArray)
        {
            if (member == null) continue;
            string? desc = null;
            foreach (var attributeData in member.GetAttributes())
            {
                if (attributeData.AttributeClass?.ToDisplayString() == DescriptionAttribute &&
                    attributeData.ConstructorArguments.Length == 1 &&
                    attributeData.ConstructorArguments[0].Value?.ToString() is { } s)
                {
                    desc = s;
                }

                if (!string.IsNullOrEmpty(desc))
                {
                    break;
                }
            }

            var enumValue = new EnumValue(member.Name, member.ConstantValue!, desc ?? member.Name);
            enumValues.Add(enumValue);
        }

        return new EnumToGenerate()
        {
            Name = enumName,
            UnderlyingType = underlyingType,
            EnumValues = enumValues,
            Namespace = containingNamespace
        };
    }
}