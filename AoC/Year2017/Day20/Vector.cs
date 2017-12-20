using System;

namespace AoC.Year2017.Day20
{
    public class Vector
    {
        public long X;
        public long Y;
        public long Z;
        public long Distance => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);

        public Vector Add(Vector other)
        {
            return new Vector
            {
                X = this.X + other.X,
                Y = this.Y + other.Y,
                Z = this.Z + other.Z,
            };
        }

        protected bool Equals(Vector other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vector) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }
    }
}