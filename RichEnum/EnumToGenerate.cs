namespace RichEnum;

public struct EnumToGenerate
{
    public EnumToGenerate(string name,
        string? @namespace,
        string underlyingType,
        List<EnumValue> enumValues)
    {
        Name = name;
        Namespace = @namespace;
        UnderlyingType = underlyingType;
        EnumValues = enumValues;
    }

    public string Name { get; }
    public string? Namespace { get; }
    public string UnderlyingType { get; }
    public List<EnumValue> EnumValues { get; }

}