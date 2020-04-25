using VRageMath;

namespace DataValidateFix
{
    internal static class MathExtensions
    {
        internal static Vector3 Clamp(this ref Vector3 vector3, ref Vector3 min, ref Vector3 max)
        {
            var result = new Vector3(vector3.X, vector3.Y, vector3.Z);
            if ((double) vector3.X > max.X)
                result.X = max.X;
            else if ((double) vector3.X < min.X)
                result.X = min.X;
            if ((double) vector3.Y > max.Y)
                result.Y = max.Y;
            else if ((double) vector3.Y < min.Y)
                result.Y = min.Y;
            if ((double) vector3.Z > max.Z)
                result.Z = max.Z;
            else if ((double) vector3.Z < min.Z)
                result.Z = min.Z;
            return result;
        }
    }
}