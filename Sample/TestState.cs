namespace Sample
{
    /// <summary>
    /// Generated Rich Enum from <see cref=""/>
    /// </summary>
    public record States: IComparable<States>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly States None = new(0, nameof(None), "Unknow State");
        public static readonly States Running = new(1, nameof(Running), "Service Running");
        public static readonly States Stopped = new(2, nameof(Stopped), "Service Stopped");
        private readonly int _value;
        private readonly string _name;
        private readonly string _description;

        /// <summary>
        /// Get Rich Enum Name
        /// </summary>
        /// <returns>Enum Name</returns>
        public override string ToString() => _name;
        public string Description() => _description;
        public string Description1() => _description;

        /// <summary>
        /// Get Rich Enums names array
        /// </summary>
        /// <returns></returns>
        public static string[] GetNames() => new[]
        {
            nameof(None),
            nameof(Running),
            nameof(Stopped),
        };
        /// <summary>
        /// Get Rich Enums values array
        /// </summary>
        /// <returns></returns>
        public static States[] GetValues() => new[]
        {
            None,
            Running,
            Stopped,
        };

        public static States operator |(States a, States b) => a._value | b._value;
        public static States operator &(States a, States b) => a._value & b._value;
        public static States operator ^(States a, States b) => a._value ^ b._value;

        public static implicit operator int(States val) => val._value;
        public static implicit operator States(int value)
        {
            switch (value)
            {
                case 0: return None;
                case 1: return Running;
                case 2: return Stopped;
                default: return new States(value, "", "");
            }
        }
        private static bool TryParseIgnoreCase([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? name, out States value)
        {
            switch (name)
            {
                case { } s when nameof(None).Equals(s, System.StringComparison.OrdinalIgnoreCase):
                    value = None;
                    return true;
                case { } s when nameof(Running).Equals(s, System.StringComparison.OrdinalIgnoreCase):
                    value = Running;
                    return true;
                case { } s when nameof(Stopped).Equals(s, System.StringComparison.OrdinalIgnoreCase):
                    value = Stopped;
                    return true;
                default:
                    value = default;
                    return false;
            }
        }
        public static bool TryParse([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? name, out States value)
        {
            switch (name)
            {
                case nameof(None):
                    value = None;
                    return true;
                case nameof(Running):
                    value = Running;
                    return true;
                case nameof(Stopped):
                    value = Stopped;
                    return true;
                default:
                    value = default;
                    return false;
            }
        }
        public static bool TryParse(
            [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? name,
            bool ignoreCase,
            out States value)
            => ignoreCase ? TryParseIgnoreCase(name, out value) : TryParse(name, out value);
        private States(int value, string name, string description)
        {
            _value = value;
            _name = name;
            _description = description;
        }

        public int CompareTo(States other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return _value.CompareTo(other._value);
        }
    }
}