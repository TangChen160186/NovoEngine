using OpenTK.Mathematics;

namespace OvRenderingCS.Utils
{
    /// <summary>
    /// Utility class for type conversions
    /// </summary>
    public static class Conversions
    {
        /// <summary>
        /// Convert a Vector3 to a Color3
        /// </summary>
        public static Color3 ToColor3(Vector3 vector)
        {
            return new Color3(vector.X, vector.Y, vector.Z);
        }

        /// <summary>
        /// Convert a Vector4 to a Color4
        /// </summary>
        public static Color4 ToColor4(Vector4 vector)
        {
            return new Color4(vector.X, vector.Y, vector.Z, vector.W);
        }

        /// <summary>
        /// Convert a Color3 to a Vector3
        /// </summary>
        public static Vector3 ToVector3(Color3 color)
        {
            return new Vector3(color.R, color.G, color.B);
        }

        /// <summary>
        /// Convert a Color4 to a Vector4
        /// </summary>
        public static Vector4 ToVector4(Color4 color)
        {
            return new Vector4(color.R, color.G, color.B, color.A);
        }
    }
}
