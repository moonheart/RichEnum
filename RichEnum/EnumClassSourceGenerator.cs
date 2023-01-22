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
            context.SyntaxProvider.ForAttributeWithMetadataName(
                    RichEnumAttribute,
                    static (node, _) => node is EnumDeclarationSyntax,
                    GetTypeToGenerate)
                .Where(static d => d is not null);

        context.RegisterSourceOutput(incrementalValuesProvider, Execute);
    }

    private static void Execute(SourceProductionContext context, EnumToGenerate? enumToGenerate)
    {
        if (enumToGenerate is { } eg)
        {
            var result = GeneratorHelper.GenerateEnumRecord(eg);
            context.AddSource($"{eg.Namespace}.{eg.Name}.g.cs", SourceText.From(result, Encoding.UTF8));
        }
    }

    private static EnumToGenerate? GetTypeToGenerate(GeneratorAttributeSyntaxContext context, CancellationToken ct)
    {
        if (context.TargetSymbol is not INamedTypeSymbol namedTypeSymbol)
        {
            return null;
        }

        ct.ThrowIfCancellationRequested();

        var enumName = namedTypeSymbol.Name;
        var containingNamespace = namedTypeSymbol.ContainingNamespace.ToString();
        var underlyingType = namedTypeSymbol.EnumUnderlyingType?.ToString() ?? "int";

        var immutableArray = namedTypeSymbol.GetMembers()
            .Select(d => d as IFieldSymbol).Where(d => d != null).ToArray();
        var enumValues = new List<EnumValue>();
        foreach (var member in immutableArray)
        {
            if (member == null) continue;
            var desc = GetDesc(member);

            var enumValue = new EnumValue(member.Name, member.ConstantValue!, desc ?? member.Name);
            enumValues.Add(enumValue);
        }

        var enableLocalization = false;
        var resourceNamager = "";
        var attributeData = namedTypeSymbol.GetAttributes()
            .FirstOrDefault(d => d.AttributeClass?.ToString() == RichEnumAttribute);
        if (attributeData != null)
            foreach (var attributeDataNamedArgument in attributeData.NamedArguments)
            {
                if (attributeDataNamedArgument.Key == "EnableLocalization" &&
                    attributeDataNamedArgument.Value.Value is bool el)
                {
                    enableLocalization = el;
                }

                if (attributeDataNamedArgument.Key == "ResourceManager" &&
                    attributeDataNamedArgument.Value.Value?.ToString() is { } rm)
                {
                    resourceNamager = rm;
                }
            }

        return new EnumToGenerate(enumName, containingNamespace, underlyingType, enumValues, enableLocalization,
            resourceNamager);
    }

    private static string? GetDesc(IFieldSymbol member)
    {
        foreach (var attributeData in member.GetAttributes())
        {
            if (attributeData.AttributeClass?.ToDisplayString() == DescriptionAttribute &&
                attributeData.ConstructorArguments.Length == 1 &&
                attributeData.ConstructorArguments[0].Value?.ToString() is { } s &&
                s != "")
            {
                return s;
            }
        }

        return null;
    }
}