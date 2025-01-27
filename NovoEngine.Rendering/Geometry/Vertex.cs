using System.Runtime.InteropServices;
using OpenTK.Mathematics;

namespace NovoEngine.Rendering.Geometry
{
    /// <summary>
    /// Data structure that defines the geometry of a vertex
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        /// <summary>
        /// Position of the vertex (x, y, z)
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Texture coordinates (u, v)
        /// </summary>
        public Vector2 TexCoords;

        /// <summary>
        /// Normal vector (x, y, z)
        /// </summary>
        public Vector3 Normal;

        /// <summary>
        /// Tangent vector (x, y, z)
        /// </summary>
        public Vector3 Tangent;

        /// <summary>
        /// Bitangent vector (x, y, z)
        /// </summary>
        public Vector3 Bitangent;

        /// <summary>
        /// Size of the vertex in bytes
        /// </summary>
        public static readonly int SizeInBytes = Marshal.SizeOf<Vertex>();

        /// <summary>
        /// Creates a new vertex
        /// </summary>
        public Vertex(
            Vector3 position,
            Vector2 texCoords,
            Vector3 normal,
            Vector3 tangent,
            Vector3 bitangent)
        {
            Position = position;
            TexCoords = texCoords;
            Normal = normal;
            Tangent = tangent;
            Bitangent = bitangent;
        }

        /// <summary>
        /// Creates a new vertex with default values
        /// </summary>
        public static Vertex Default => new(
            Vector3.Zero,
            Vector2.Zero,
            Vector3.UnitY,
            Vector3.UnitX,
            Vector3.UnitZ
        );
    }
}
