namespace RichEnum;

public class EnumToGenerate
{
    public string Name { get; set; }
    public string? Namespace { get; set; }
    public string UnderlyingType { get; set; }
    public List<EnumValue> EnumValues { get; set; }

}