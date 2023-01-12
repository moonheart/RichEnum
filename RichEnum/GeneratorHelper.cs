using System.Text;

namespace RichEnum;

public static class GeneratorHelper
{
    public static string Attribute = @"
namespace RichEnum.Attribute
{
    [AttributeUsage(AttributeTargets.Enum)]
    public class RichEnumAttribute: System.Attribute
    {
        
    }
}

";

    public static string GenerateEnumRecord(EnumToGenerate enumToGenerate)
    {
        var name = enumToGenerate.Name;
        var underlyingType = enumToGenerate.UnderlyingType;
        var sb = new StringBuilder();
        sb.AppendFormat(@"
namespace RichEnum.Generated.{1}
{{
    public record {0}: IComparable<{0}>
    {{", name, enumToGenerate.Namespace);
        foreach (var enumValue in enumToGenerate.EnumValues)
        {
            sb.AppendFormat(@"
        public static {0} {1} = new({2}, nameof({1}), ""{3}"");", name, enumValue.Name,
                enumValue.UnderlyingValue, enumValue.Description);
        }

        sb.Append(@"
        private readonly int _value;
        private readonly string _name;
        private readonly string _description;

        public override string ToString() => _name;
        public string Description() => _description;").AppendFormat(@"

        public static {1} operator |({1} a, {1} b) => a._value | b._value;
        public static {1} operator &({1} a, {1} b) => a._value & b._value;
        public static {1} operator ^({1} a, {1} b) => a._value ^ b._value;

        public static implicit operator {0}({1} val) => val._value;", underlyingType,
            name).AppendFormat(@"
        public static implicit operator {0}({1} value)
        {{
            switch (value)
            {{", name, underlyingType);
        foreach (var enumValue in enumToGenerate.EnumValues)
        {
            sb.AppendFormat(@"
                case {0}: return {1};", enumValue.UnderlyingValue, enumValue.Name);
        }

        sb.Append(@"
                default: throw new ArgumentOutOfRangeException(nameof(value));
            }
        }").AppendFormat(@"
        public static implicit operator {0}(string name)
        {{
            switch (name)
            {{", name);
        foreach (var enumValue in enumToGenerate.EnumValues)
        {
            sb.AppendFormat(@"
                case nameof({0}): return {0};", enumValue.Name);
        }

        sb.Append(@"
                default: throw new ArgumentOutOfRangeException(nameof(name));
            }
        }").AppendFormat(@"
        private {0}({1} value, string name, string description)
        {{
            _value = value;
            _name = name;
            _description = description;
        }}", name, underlyingType).AppendFormat(@"

        public int CompareTo({0} other)
        {{
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return _value.CompareTo(other._value);
        }}
    }}
}}", name);
        
        return sb.ToString();
    }
}