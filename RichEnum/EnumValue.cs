namespace RichEnum;

public class EnumValue
{
    public readonly string Name;
    public readonly object UnderlyingValue;
    public readonly string Description;

    public EnumValue(string name, object underlyingValue, string description)
    {
        Name = name;
        UnderlyingValue = underlyingValue;
        Description = description;
    }
}