using System.Collections.Immutable;
using RichEnum;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RichEnum.Attribute;

namespace UnitTest;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var input = """
using System.ComponentModel;

namespace Test
{
    [Flags]
    [RichEnum.Attribute.RichEnumAttribute]
    enum State
    {
        [Description("Rrrrrrr")]
        Running,
        [Description("Sssssss")]
        Stopped
    }
}
""";
        var (diagnostics, output) = GetGeneratedOutput<EnumClassSourceGenerator>(input);
        
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
                MetadataReference.CreateFromFile(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute).Assembly.Location),
            });

        var compilation = CSharpCompilation.Create(
            "generator",
            new[] { syntaxTree },
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