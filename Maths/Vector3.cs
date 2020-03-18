using System;

namespace ioSS.Util.Maths.Geometry
{
    
    public struct Vector3 : IEquatable<Vector3>
    {
        public const float kEpsilon = 1E-05f;
        public const float kEpsilonNormalSqrt = 1E-15f;

        /// <summary>
        ///     <para>X component of the vector.</para>
        /// </summary>
        public float x;

        /// <summary>
        ///     <para>Y component of the vector.</para>
        /// </summary>
        public float y;

        /// <summary>
        ///     <para>Z component of the vector.</para>
        /// </summary>
        public float z;

        /// <summary>
        ///     <para>Creates a new vector with given x, y, z components.</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        ///     <para>Creates a new vector with given x, y components and sets z to zero.</para>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector3(float x, float y)
        {
            this.x = x;
            this.y = y;
            z = 0.0f;
        }

        /// <summary>
        ///     <para>Linearly interpolates between two vectors.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        public static Vector3 LerpUnclamped(Vector3 a, Vector3 b, float t)
        {
            return new Vector3(a.x + (b.x - a.x) * t, a.y + (b.y - a.y) * t, a.z + (b.z - a.z) * t);
        }

        /// <summary>
        ///     <para>Moves a point current in a straight line towards a target point.</para>
        /// </summary>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="maxDistanceDelta"></param>
        public static Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta)
        {
            var vector3 = target - current;
            var magnitude = vector3.magnitude;
            if (magnitude <= (double) maxDistanceDelta || magnitude < 1.40129846432482E-45)
                return target;
            return current + vector3 / magnitude * maxDistanceDelta;
        }


        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector3 index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid Vector3 index!");
                }
            }
        }

        /// <summary>
        ///     <para>Set x, y and z components of an existing Vector3.</para>
        /// </summary>
        /// <param name="newX"></param>
        /// <param name="newY"></param>
        /// <param name="newZ"></param>
        public void Set(float newX, float newY, float newZ)
        {
            x = newX;
            y = newY;
            z = newZ;
        }

        /// <summary>
        ///     <para>Multiplies two vectors component-wise.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static Vector3 Scale(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        /// <summary>
        ///     <para>Multiplies every component of this vector by the same component of scale.</para>
        /// </summary>
        /// <param name="scale"></param>
        public void Scale(Vector3 scale)
        {
            x *= scale.x;
            y *= scale.y;
            z *= scale.z;
        }

        /// <summary>
        ///     <para>Cross Product of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static Vector3 Cross(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3((float) (lhs.y * (double) rhs.z - lhs.z * (double) rhs.y),
                (float) (lhs.z * (double) rhs.x - lhs.x * (double) rhs.z),
                (float) (lhs.x * (double) rhs.y - lhs.y * (double) rhs.x));
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
        }

        /// <summary>
        ///     <para>Returns true if the given vector is exactly equal to this vector.</para>
        /// </summary>
        /// <param name="other"></param>
        public override bool Equals(object other)
        {
            if (!(other is Vector3))
                return false;
            return Equals((Vector3) other);
        }

        public bool Equals(Vector3 other)
        {
            return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z);
        }

        /// <summary>
        ///     <para>Reflects a vector off the plane defined by a normal.</para>
        /// </summary>
        /// <param name="inDirection"></param>
        /// <param name="inNormal"></param>
        public static Vector3 Reflect(Vector3 inDirection, Vector3 inNormal)
        {
            return -2f * Dot(inNormal, inDirection) * inNormal + inDirection;
        }

        /// <summary>
        ///     <para>Makes this vector have a magnitude of 1.</para>
        /// </summary>
        /// <param name="value"></param>
        public static Vector3 Normalize(Vector3 value)
        {
            var num = Magnitude(value);
            if (num > 9.99999974737875E-06)
                return value / num;
            return zero;
        }

        public void Normalize()
        {
            var num = Magnitude(this);
            if (num > 9.99999974737875E-06)
                this = this / num;
            else
                this = zero;
        }

        /// <summary>
        ///     <para>Returns this vector with a magnitude of 1 (Read Only).</para>
        /// </summary>
        public Vector3 normalized => Normalize(this);

        /// <summary>
        ///     <para>Dot Product of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static float Dot(Vector3 lhs, Vector3 rhs)
        {
            return (float) (lhs.x * (double) rhs.x + lhs.y * (double) rhs.y + lhs.z * (double) rhs.z);
        }


        public static float Magnitude(Vector3 vector)
        {
            return (float) Math.Sqrt((float) (vector.x * (double) vector.x + vector.y * (double) vector.y +
                                              vector.z * (double) vector.z));
        }

        /// <summary>
        ///     <para>Returns the length of this vector (Read Only).</para>
        /// </summary>
        public float magnitude => (float) Math.Sqrt((float) (x * (double) x + y * (double) y + z * (double) z));

        public static float SqrMagnitude(Vector3 vector)
        {
            return (float) (vector.x * (double) vector.x + vector.y * (double) vector.y + vector.z * (double) vector.z);
        }

        /// <summary>
        ///     <para>Returns the squared length of this vector (Read Only).</para>
        /// </summary>
        public float sqrMagnitude => (float) (x * (double) x + y * (double) y + z * (double) z);

        /// <summary>
        ///     <para>Returns a vector that is made from the smallest components of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static Vector3 Min(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(Math.Min(lhs.x, rhs.x), Math.Min(lhs.y, rhs.y), Math.Min(lhs.z, rhs.z));
        }

        /// <summary>
        ///     <para>Returns a vector that is made from the largest components of two vectors.</para>
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static Vector3 Max(Vector3 lhs, Vector3 rhs)
        {
            return new Vector3(Math.Max(lhs.x, rhs.x), Math.Max(lhs.y, rhs.y), Math.Max(lhs.z, rhs.z));
        }

        /// <summary>
        ///     <para>Shorthand for writing Vector3(0, 0, 0).</para>
        /// </summary>
        public static Vector3 zero { get; } = new Vector3(0.0f, 0.0f, 0.0f);

        /// <summary>
        ///     <para>Shorthand for writing Vector3(1, 1, 1).</para>
        /// </summary>
        public static Vector3 one { get; } = new Vector3(1f, 1f, 1f);

        /// <summary>
        ///     <para>Shorthand for writing Vector3(0, 0, 1).</para>
        /// </summary>
        public static Vector3 forward { get; } = new Vector3(0.0f, 0.0f, 1f);

        /// <summary>
        ///     <para>Shorthand for writing Vector3(0, 0, -1).</para>
        /// </summary>
        public static Vector3 back { get; } = new Vector3(0.0f, 0.0f, -1f);

        /// <summary>
        ///     <para>Shorthand for writing Vector3(0, 1, 0).</para>
        /// </summary>
        public static Vector3 up { get; } = new Vector3(0.0f, 1f, 0.0f);

        /// <summary>
        ///     <para>Shorthand for writing Vector3(0, -1, 0).</para>
        /// </summary>
        public static Vector3 down { get; } = new Vector3(0.0f, -1f, 0.0f);

        /// <summary>
        ///     <para>Shorthand for writing Vector3(-1, 0, 0).</para>
        /// </summary>
        public static Vector3 left { get; } = new Vector3(-1f, 0.0f, 0.0f);

        /// <summary>
        ///     <para>Shorthand for writing Vector3(1, 0, 0).</para>
        /// </summary>
        public static Vector3 right { get; } = new Vector3(1f, 0.0f, 0.0f);

        /// <summary>
        ///     <para>Shorthand for writing Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity).</para>
        /// </summary>
        public static Vector3 positiveInfinity { get; } =
            new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity);

        /// <summary>
        ///     <para>Shorthand for writing Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity).</para>
        /// </summary>
        public static Vector3 negativeInfinity { get; } =
            new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3 operator -(Vector3 a)
        {
            return new Vector3(-a.x, -a.y, -a.z);
        }

        public static Vector3 operator *(Vector3 a, float d)
        {
            return new Vector3(a.x * d, a.y * d, a.z * d);
        }

        public static Vector3 operator *(float d, Vector3 a)
        {
            return new Vector3(a.x * d, a.y * d, a.z * d);
        }

        public static Vector3 operator /(Vector3 a, float d)
        {
            return new Vector3(a.x / d, a.y / d, a.z / d);
        }

        public static bool operator ==(Vector3 lhs, Vector3 rhs)
        {
            return SqrMagnitude(lhs - rhs) < 9.99999943962493E-11;
        }

        public static bool operator !=(Vector3 lhs, Vector3 rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        ///     <para>Returns a nicely formatted string for this vector.</para>
        /// </summary>
        /// <param name="format"></param>
        public override string ToString()
        {
            return $"({(object) x:F1}, {(object) y:F1}, {(object) z:F1})";
        }

        /// <summary>
        ///     <para>Returns a nicely formatted string for this vector.</para>
        /// </summary>
        /// <param name="format"></param>
        public string ToString(string format)
        {
            return
                $"({(object) x.ToString(format)}, {(object) y.ToString(format)}, {(object) z.ToString(format)})";
        }
    }
}