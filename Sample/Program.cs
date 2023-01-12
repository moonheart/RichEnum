namespace MyNamespace;

static partial class Program
{
    static void Main()
    {
        Generated.States state1 = 1;
        Generated.States.TryParse("Stopped", out var state2);
        Generated.States.TryParse("StOPPed", true, out var state3);
        Generated.States r1 = state1 | state2;
        Generated.States r2 = state1 & state2;
        Generated.States r3 = state1 ^ state2;

        Console.WriteLine(state1);
        Console.WriteLine(state2);
        Console.WriteLine(state3);
        Console.WriteLine(r1);
        Console.WriteLine(r2);
        Console.WriteLine(r3);
    }
}

[RichEnum.Attribute.RichEnumAttribute]
public enum States
{
    [System.ComponentModel.Description("Unknow State")] None,
    [System.ComponentModel.Description("Service Running")] Running,
    [System.ComponentModel.Description("Service Stopped")] Stopped
}