using System;

namespace Sekhmet.Serialization
{
    public sealed class AdviceType : IEquatable<AdviceType>
    {
        public string Name { get; private set; }

        private AdviceType(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name");
            Name = name;
        }

        public bool Equals(AdviceType other)
        {
            if (other == null)
                return false;

            return Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as AdviceType);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        public static bool operator ==(AdviceType type1, AdviceType type2)
        {
            return Equals(type1, type2);
        }

        public static implicit operator AdviceType(string name)
        {
            return new AdviceType(name);
        }

        public static implicit operator string(AdviceType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            return type.Name;
        }

        public static bool operator !=(AdviceType type1, AdviceType type2)
        {
            return !(type1 == type2);
        }
    }
}