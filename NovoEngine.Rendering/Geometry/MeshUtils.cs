using OpenTK.Mathematics;

namespace NovoEngine.Rendering.Geometry
{
    /// <summary>
    /// Utility class for mesh operations
    /// </summary>
    public static class MeshUtils
    {
        /// <summary>
        /// Calculate tangent and bitangent vectors for a mesh
        /// </summary>
        public static void CalculateTangentSpace(Vertex[] vertices, uint[] indices)
        {
            for (int i = 0; i < indices.Length; i += 3)
            {
                var v0 = vertices[indices[i]];
                var v1 = vertices[indices[i + 1]];
                var v2 = vertices[indices[i + 2]];

                Vector3 edge1 = v1.Position - v0.Position;
                Vector3 edge2 = v2.Position - v0.Position;

                Vector2 deltaUV1 = v1.TexCoords - v0.TexCoords;
                Vector2 deltaUV2 = v2.TexCoords - v0.TexCoords;

                float f = 1.0f / (deltaUV1.X * deltaUV2.Y - deltaUV2.X * deltaUV1.Y);

                Vector3 tangent = new(
                    f * (deltaUV2.Y * edge1.X - deltaUV1.Y * edge2.X),
                    f * (deltaUV2.Y * edge1.Y - deltaUV1.Y * edge2.Y),
                    f * (deltaUV2.Y * edge1.Z - deltaUV1.Y * edge2.Z)
                );

                Vector3 bitangent = new(
                    f * (-deltaUV2.X * edge1.X + deltaUV1.X * edge2.X),
                    f * (-deltaUV2.X * edge1.Y + deltaUV1.X * edge2.Y),
                    f * (-deltaUV2.X * edge1.Z + deltaUV1.X * edge2.Z)
                );

                // Normalize and assign
                vertices[indices[i]].Tangent = Vector3.Normalize(tangent);
                vertices[indices[i + 1]].Tangent = Vector3.Normalize(tangent);
                vertices[indices[i + 2]].Tangent = Vector3.Normalize(tangent);

                vertices[indices[i]].Bitangent = Vector3.Normalize(bitangent);
                vertices[indices[i + 1]].Bitangent = Vector3.Normalize(bitangent);
                vertices[indices[i + 2]].Bitangent = Vector3.Normalize(bitangent);
            }
        }

        /// <summary>
        /// Calculate smooth normals for a mesh
        /// </summary>
        public static void CalculateSmoothNormals(Vertex[] vertices, uint[] indices)
        {
            // Reset all normals
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Normal = Vector3.Zero;
            }

            // Calculate face normals and accumulate
            for (int i = 0; i < indices.Length; i += 3)
            {
                var v0 = vertices[indices[i]];
                var v1 = vertices[indices[i + 1]];
                var v2 = vertices[indices[i + 2]];

                Vector3 edge1 = v1.Position - v0.Position;
                Vector3 edge2 = v2.Position - v0.Position;
                Vector3 normal = Vector3.Normalize(Vector3.Cross(edge1, edge2));

                vertices[indices[i]].Normal += normal;
                vertices[indices[i + 1]].Normal += normal;
                vertices[indices[i + 2]].Normal += normal;
            }

            // Normalize accumulated normals
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Normal = Vector3.Normalize(vertices[i].Normal);
            }
        }

        /// <summary>
        /// Calculate a bounding sphere for a mesh
        /// </summary>
        public static BoundingSphere CalculateBoundingSphere(Vertex[] vertices)
        {
            return BoundingSphere.FromPoints(vertices.Select(v => v.Position));
        }
    }
}
