namespace RichEnum;

public struct EnumValue
{
    public EnumValue(string name, object underlyingValue, string description)
    {
        Name = name;
        UnderlyingValue = underlyingValue;
        Description = description;
    }

    public string Name { get; }
    public object UnderlyingValue { get; }
    public string Description { get; }
}