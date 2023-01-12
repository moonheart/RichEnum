using System.ComponentModel;
using RichEnum.Attribute;

namespace MyNamespace;

static partial class Program
{
    static void Main()
    {
        RichEnum.Generated.MyNamespace.State state = 1;
        RichEnum.Generated.MyNamespace.State state2 = 2;
        RichEnum.Generated.MyNamespace.State i = state | state2;

        State s1 = (State) 1;
        State s2 = (State) 2;
        var x = s1 & s2;
        var x2 = s1 ^ s2;
        var x3 = ~s1;
        var x4 = ++s1;
        var x5 = --s1;
    }
    
}

[Flags]
[RichEnum]
enum State
{
    [Description("ddd")]
    Running, 
    [Description("Sssssss")]
    Stopped
}

