using System;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    public struct TypeName : IEquatable<TypeName>
    {

        public bool Equals(TypeName other)
        {
            return string.Equals(Namespace, other.Namespace, StringComparison.InvariantCultureIgnoreCase) && string.Equals(Name, other.Name, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return obj is TypeName other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (StringComparer.InvariantCultureIgnoreCase.GetHashCode(Namespace) * 397) ^ StringComparer.InvariantCultureIgnoreCase.GetHashCode(Name);
            }
        }

        public static bool operator ==(TypeName left, TypeName right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TypeName left, TypeName right)
        {
            return !left.Equals(right);
        }

        public string Namespace { get; }
        public string Name { get; }

        public TypeName(string ns, string name)
        {
            Namespace = ns ?? throw new ArgumentNullException($"{nameof(ns)}");
            Name = name ?? throw new ArgumentNullException($"{nameof(name)}");
        }
    }
}
