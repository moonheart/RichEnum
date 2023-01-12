namespace Test
{
    public record State : IComparable<State>
    {
        public static State Running = new(0, nameof(Running), "ddd");
        public static State Stopped = new(1, nameof(Stopped), "Sssssss");
        private readonly int _value;
        private readonly string _name;
        private readonly string _description;

        public override string ToString() => _name;
        public string Description() => _description;

        public static State operator |(State a, State b) => a._value | b._value;
        public static State operator &(State a, State b) => a._value & b._value;
        public static State operator ^(State a, State b) => a._value ^ b._value;

        public static implicit operator int(State val) => val._value;

        public static implicit operator State(int value)
        {
            switch (value)
            {
                case 0: return Running;
                case 1: return Stopped;
                default: throw new ArgumentOutOfRangeException(nameof(value));
            }
        }

        public static implicit operator State(string name)
        {
            switch (name)
            {
                case nameof(Running): return Running;
                case nameof(Stopped): return Stopped;
                default: throw new ArgumentOutOfRangeException(nameof(name));
            }
        }

        private State(int value, string name, string description)
        {
            _value = value;
            _name = name;
            _description = description;
        }

        public int CompareTo(State other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return _value.CompareTo(other._value);
        }
    }
}