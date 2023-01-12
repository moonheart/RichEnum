using System.ComponentModel;
using RichEnum.Attribute;

namespace MyNamespace;

static partial class Program
{
    static void Main()
    {
        RichEnum.Generated.MyNamespace.States state1 = 1;
        RichEnum.Generated.MyNamespace.States.TryParse("Stopped", out var state2);
        RichEnum.Generated.MyNamespace.States.TryParse("StOPPed", true, out var state3);
        RichEnum.Generated.MyNamespace.States r1 = state1 | state2;
        RichEnum.Generated.MyNamespace.States r2 = state1 & state2;
        RichEnum.Generated.MyNamespace.States r3 = state1 ^ state2;

        Console.WriteLine(state1);
        Console.WriteLine(state2);
        Console.WriteLine(state3);
        Console.WriteLine(r1);
        Console.WriteLine(r2);
        Console.WriteLine(r3);
    }
}

[Flags]
[RichEnum]
public enum States
{
    [Description("Unknow State")] None,
    [Description("Service Running")] Running,
    [Description("Service Stopped")] Stopped
}