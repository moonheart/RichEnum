namespace RichEnum;

public struct EnumToGenerate
{
    public EnumToGenerate(string name,
        string? @namespace,
        string underlyingType,
        List<EnumValue> enumValues, bool enableLocalization, string resourceManager)
    {
        Name = name;
        Namespace = @namespace;
        UnderlyingType = underlyingType;
        EnumValues = enumValues;
        EnableLocalization = enableLocalization;
        ResourceManager = resourceManager;
    }

    public string Name { get; }
    public string? Namespace { get; }
    public string UnderlyingType { get; }
    public List<EnumValue> EnumValues { get; }
    public bool EnableLocalization { get; }
    public string ResourceManager { get; }

}