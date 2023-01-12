// See https://aka.ms/new-console-template for more information

using System.ComponentModel;

Console.WriteLine("1");

[Flags]
enum State
{
    [Description("Rrrrrrr")]
    Running = 1,
    [Description("Sssssss")]
    Stopped = 2
}

