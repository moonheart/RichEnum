namespace RichEnum;


public record RunningState : IComparable<RunningState>
{
    public static RunningState Stopped = new(0, nameof(RunningState));

    public int Value { get; }
    public string Name { get; }

    public override string ToString() => Name;

    public static implicit operator int(RunningState runningState) => runningState.Value;

    public static explicit operator RunningState(int value)
    {
        return value switch
        {
            0 => Stopped,
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }

    public static explicit operator RunningState(string name)
    {
        return name switch
        {
            nameof(Stopped) => Stopped,
            _ => throw new ArgumentOutOfRangeException(nameof(name))
        };
    }

    private RunningState(int value, string name)
    {
        Value = value;
        Name = name;
    }

    public int CompareTo(RunningState? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Value.CompareTo(other.Value);
    }
}