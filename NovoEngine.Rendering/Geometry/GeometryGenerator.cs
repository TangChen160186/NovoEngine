using OpenTK.Mathematics;

namespace NovoEngine.Rendering.Geometry
{
    /// <summary>
    /// Utility class for generating basic geometric shapes
    /// </summary>
    public static class GeometryGenerator
    {
        /// <summary>
        /// Generate a quad (rectangle) mesh data
        /// </summary>
        public static (Vertex[] vertices, uint[] indices) CreateQuad()
        {
            var vertices = new Vertex[]
            {
                new(new Vector3(-0.5f, -0.5f, 0.0f), new Vector2(0.0f, 0.0f), Vector3.UnitZ, Vector3.UnitX, Vector3.UnitY),
                new(new Vector3( 0.5f, -0.5f, 0.0f), new Vector2(1.0f, 0.0f), Vector3.UnitZ, Vector3.UnitX, Vector3.UnitY),
                new(new Vector3( 0.5f,  0.5f, 0.0f), new Vector2(1.0f, 1.0f), Vector3.UnitZ, Vector3.UnitX, Vector3.UnitY),
                new(new Vector3(-0.5f,  0.5f, 0.0f), new Vector2(0.0f, 1.0f), Vector3.UnitZ, Vector3.UnitX, Vector3.UnitY)
            };

            var indices = new uint[] { 0, 1, 2, 2, 3, 0 };

            return (vertices, indices);
        }

        /// <summary>
        /// Generate a cube mesh data
        /// </summary>
        public static (Vertex[] vertices, uint[] indices) CreateCube()
        {
            var vertices = new List<Vertex>();
            var indices = new List<uint>();

            // Front face
            vertices.AddRange(new[]
            {
                new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), new Vector2(0.0f, 0.0f), Vector3.UnitZ, Vector3.UnitX, Vector3.UnitY),
                new Vertex(new Vector3( 0.5f, -0.5f,  0.5f), new Vector2(1.0f, 0.0f), Vector3.UnitZ, Vector3.UnitX, Vector3.UnitY),
                new Vertex(new Vector3( 0.5f,  0.5f,  0.5f), new Vector2(1.0f, 1.0f), Vector3.UnitZ, Vector3.UnitX, Vector3.UnitY),
                new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), new Vector2(0.0f, 1.0f), Vector3.UnitZ, Vector3.UnitX, Vector3.UnitY)
            });

            // Back face
            vertices.AddRange(new[]
            {
                new Vertex(new Vector3( 0.5f, -0.5f, -0.5f), new Vector2(0.0f, 0.0f), -Vector3.UnitZ, -Vector3.UnitX, Vector3.UnitY),
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(1.0f, 0.0f), -Vector3.UnitZ, -Vector3.UnitX, Vector3.UnitY),
                new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), new Vector2(1.0f, 1.0f), -Vector3.UnitZ, -Vector3.UnitX, Vector3.UnitY),
                new Vertex(new Vector3( 0.5f,  0.5f, -0.5f), new Vector2(0.0f, 1.0f), -Vector3.UnitZ, -Vector3.UnitX, Vector3.UnitY)
            });

            // Right face
            vertices.AddRange(new[]
            {
                new Vertex(new Vector3( 0.5f, -0.5f,  0.5f), new Vector2(0.0f, 0.0f), Vector3.UnitX, -Vector3.UnitZ, Vector3.UnitY),
                new Vertex(new Vector3( 0.5f, -0.5f, -0.5f), new Vector2(1.0f, 0.0f), Vector3.UnitX, -Vector3.UnitZ, Vector3.UnitY),
                new Vertex(new Vector3( 0.5f,  0.5f, -0.5f), new Vector2(1.0f, 1.0f), Vector3.UnitX, -Vector3.UnitZ, Vector3.UnitY),
                new Vertex(new Vector3( 0.5f,  0.5f,  0.5f), new Vector2(0.0f, 1.0f), Vector3.UnitX, -Vector3.UnitZ, Vector3.UnitY)
            });

            // Left face
            vertices.AddRange(new[]
            {
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0.0f, 0.0f), -Vector3.UnitX, Vector3.UnitZ, Vector3.UnitY),
                new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), new Vector2(1.0f, 0.0f), -Vector3.UnitX, Vector3.UnitZ, Vector3.UnitY),
                new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), new Vector2(1.0f, 1.0f), -Vector3.UnitX, Vector3.UnitZ, Vector3.UnitY),
                new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), new Vector2(0.0f, 1.0f), -Vector3.UnitX, Vector3.UnitZ, Vector3.UnitY)
            });

            // Top face
            vertices.AddRange(new[]
            {
                new Vertex(new Vector3(-0.5f,  0.5f,  0.5f), new Vector2(0.0f, 0.0f), Vector3.UnitY, Vector3.UnitX, -Vector3.UnitZ),
                new Vertex(new Vector3( 0.5f,  0.5f,  0.5f), new Vector2(1.0f, 0.0f), Vector3.UnitY, Vector3.UnitX, -Vector3.UnitZ),
                new Vertex(new Vector3( 0.5f,  0.5f, -0.5f), new Vector2(1.0f, 1.0f), Vector3.UnitY, Vector3.UnitX, -Vector3.UnitZ),
                new Vertex(new Vector3(-0.5f,  0.5f, -0.5f), new Vector2(0.0f, 1.0f), Vector3.UnitY, Vector3.UnitX, -Vector3.UnitZ)
            });

            // Bottom face
            vertices.AddRange(new[]
            {
                new Vertex(new Vector3(-0.5f, -0.5f, -0.5f), new Vector2(0.0f, 0.0f), -Vector3.UnitY, Vector3.UnitX, Vector3.UnitZ),
                new Vertex(new Vector3( 0.5f, -0.5f, -0.5f), new Vector2(1.0f, 0.0f), -Vector3.UnitY, Vector3.UnitX, Vector3.UnitZ),
                new Vertex(new Vector3( 0.5f, -0.5f,  0.5f), new Vector2(1.0f, 1.0f), -Vector3.UnitY, Vector3.UnitX, Vector3.UnitZ),
                new Vertex(new Vector3(-0.5f, -0.5f,  0.5f), new Vector2(0.0f, 1.0f), -Vector3.UnitY, Vector3.UnitX, Vector3.UnitZ)
            });

            // Generate indices for all faces
            for (uint i = 0; i < 6; i++)
            {
                uint baseIndex = i * 4;
                indices.AddRange(new[] {
                    baseIndex, baseIndex + 1, baseIndex + 2,
                    baseIndex + 2, baseIndex + 3, baseIndex
                });
            }

            return (vertices.ToArray(), indices.ToArray());
        }

        /// <summary>
        /// Generate a UV sphere mesh data
        /// </summary>
        public static (Vertex[] vertices, uint[] indices) CreateSphere(int segments = 32, int rings = 16)
        {
            var vertices = new List<Vertex>();
            var indices = new List<uint>();

            float deltaRingAngle = MathF.PI / rings;
            float deltaSegmentAngle = 2.0f * MathF.PI / segments;

            // Generate vertices
            for (int ring = 0; ring <= rings; ring++)
            {
                float r0 = MathF.Sin(ring * deltaRingAngle);
                float y0 = MathF.Cos(ring * deltaRingAngle);

                for (int segment = 0; segment <= segments; segment++)
                {
                    float x0 = r0 * MathF.Sin(segment * deltaSegmentAngle);
                    float z0 = r0 * MathF.Cos(segment * deltaSegmentAngle);

                    Vector3 position = new(x0 * 0.5f, y0 * 0.5f, z0 * 0.5f);
                    Vector3 normal = Vector3.Normalize(position);
                    Vector2 texCoord = new(segment / (float)segments, ring / (float)rings);
                    Vector3 tangent = Vector3.Normalize(new Vector3(-z0, 0, x0));
                    Vector3 bitangent = Vector3.Normalize(Vector3.Cross(normal, tangent));

                    vertices.Add(new Vertex(position, texCoord, normal, tangent, bitangent));
                }
            }

            // Generate indices
            for (int ring = 0; ring < rings; ring++)
            {
                for (int segment = 0; segment < segments; segment++)
                {
                    uint current = (uint)(ring * (segments + 1) + segment);
                    uint next = (uint)(current + segments + 1);

                    indices.AddRange(new uint[]
                    {
                        current + 1, current, next,
                        next + 1, current + 1, next
                    });
                }
            }

            return (vertices.ToArray(), indices.ToArray());
        }
    }
}
