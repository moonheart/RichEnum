using System.Text;

namespace RichEnum;

public static class GeneratorHelper
{
    public const string Attribute = @"
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
            sb.AppendFormat(@"
        public static readonly {0} {1} = new({2}, nameof({1}), ""{3}"");", name, enumValue.Name,
                enumValue.UnderlyingValue, enumValue.Description);
        sb.Append(@"
        private readonly int _value;
        private readonly string _name;
        private readonly string _description;

        public override string ToString() => _name;
        public string Description() => _description;

        public static string[] GetNames() => new[]
        {");
        foreach (var enumValue in enumToGenerate.EnumValues)
            sb.AppendFormat(@"
            nameof({0}),", enumValue.Name);
        sb.AppendFormat(@"
        }};
        public static {0}[] GetValues() => new[]
        {{", name);
        foreach (var enumValue in enumToGenerate.EnumValues)
            sb.AppendFormat(@"
            {0},", enumValue.Name);
        sb.AppendFormat(@"
        }};

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
            sb.AppendFormat(@"
                case {0}: return {1};", enumValue.UnderlyingValue, enumValue.Name);
        sb.AppendFormat(@"
                default: return new {0}(value, """", """");
            }}
        }}", name).AppendFormat(@"
        private static bool TryParseIgnoreCase([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? name, out {0} value)
        {{
            switch (name)
            {{", name);
        foreach (var enumValue in enumToGenerate.EnumValues)
            sb.AppendFormat(@"
                case {{ }} s when nameof({0}).Equals(s, System.StringComparison.OrdinalIgnoreCase):
                    value = {0};
                    return true;", enumValue.Name);
        sb.Append(@"
                default:
                    value = default;
                    return false;
            }
        }").AppendFormat(@"
        public static bool TryParse([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? name, out {0} value)
        {{
            switch (name)
            {{", name);
        foreach (var enumValue in enumToGenerate.EnumValues)
            sb.AppendFormat(@"
                case nameof({0}):
                    value = {0};
                    return true;", enumValue.Name);
        sb.Append(@"
                default:
                    value = default;
                    return false;
            }
        }");
        sb.AppendFormat(@"
        public static bool TryParse(
            [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? name,
            bool ignoreCase,
            out {0} value)
            => ignoreCase ? TryParseIgnoreCase(name, out value) : TryParse(name, out value);", name);
        sb.AppendFormat(@"
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