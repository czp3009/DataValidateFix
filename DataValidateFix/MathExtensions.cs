using System;
using VRageMath;

namespace DataValidateFix
{
    internal static class MathExtensions
    {
        internal static T Clamp<T>(this T value, T min, T max) where T : IComparable
        {
            if (value.CompareTo(max) > 0) return max;
            // ReSharper disable once ConvertIfStatementToReturnStatement
            //NaN will be set to min
            if (value.CompareTo(min) < 0) return min;
            return value;
        }

        internal static Vector3 Clamp(this ref Vector3 vector3, ref Vector3 min, ref Vector3 max)
        {
            return new Vector3(
                vector3.X.Clamp(min.X, max.X),
                vector3.Y.Clamp(min.Y, max.Y),
                vector3.Z.Clamp(min.Z, max.Z)
            );
        }
    }
}