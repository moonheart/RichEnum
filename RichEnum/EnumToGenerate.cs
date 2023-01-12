namespace RichEnum;

public record EnumToGenerate(
    string Name,
    string? Namespace,
    string UnderlyingType,
    List<EnumValue> EnumValues
);