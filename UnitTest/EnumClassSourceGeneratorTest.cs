using System.Collections.Immutable;
using RichEnum;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RichEnum.Attribute;

namespace UnitTest;

[UsesVerify]
public class EnumClassSourceGeneratorTest
{
    [Fact]
    public Task GenerateFullTest()
    {
        var input = """
namespace Test
{
    [RichEnum.Attribute.RichEnumAttribute]
    public enum States
    {
        None,
        [System.ComponentModel.Description("Service Running")] Running,
        [System.ComponentModel.Description("Service Stopped")] Stopped
    }
}
""";
        var (diagnostics, output) = GetGeneratedOutput<EnumClassSourceGenerator>(input);

        Assert.Empty(diagnostics);
        return Verify(output).UseDirectory("Snapshots");
    }

    [Fact]
    public Task GenerateNothingTest()
    {
        var input = """
namespace Test
{
    public enum States
    {
        [System.ComponentModel.Description("Unknown State")] None,
        [System.ComponentModel.Description("Service Running")] Running,
        [System.ComponentModel.Description("Service Stopped")] Stopped
    }
}
""";
        var (diagnostics, output) = GetGeneratedOutput<EnumClassSourceGenerator>(input);

        Assert.Empty(diagnostics);
        return Verify(output).UseDirectory("Snapshots");
    }


    public static (ImmutableArray<Diagnostic> Diagnostics, string Output) GetGeneratedOutput<T>(string source)
        where T : IIncrementalGenerator, new()
    {
        var syntaxTree = CSharpSyntaxTree.ParseText(source);
        var references = AppDomain.CurrentDomain.GetAssemblies()
            .Where(_ => !_.IsDynamic && !string.IsNullOrWhiteSpace(_.Location))
            .Select(_ => MetadataReference.CreateFromFile(_.Location))
            .Concat(new[]
            {
                MetadataReference.CreateFromFile(typeof(T).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(RichEnumAttribute).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute).Assembly
                    .Location),
            });

        var compilation = CSharpCompilation.Create(
            "generator",
            new[] {syntaxTree},
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var originalTreeCount = compilation.SyntaxTrees.Length;
        var generator = new T();

        var driver = CSharpGeneratorDriver.Create(generator);
        driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

        var trees = outputCompilation.SyntaxTrees.ToList();

        return (diagnostics, trees.Count != originalTreeCount ? trees[trees.Count - 1].ToString() : string.Empty);
    }
}