using OvRenderingCS.Buffers;
using OvRenderingCS.Geometry;

namespace OvRenderingCS.Resources
{
    /// <summary>
    /// Mesh resource that contains vertex and index data
    /// </summary>
    public class Mesh : IMesh, IDisposable
    {
        private readonly VertexArray _vertexArray;
        private readonly VertexBuffer _vertexBuffer;
        private IndexBuffer? _indexBuffer;
        private readonly int _vertexCount;
        private readonly int _indexCount;
        private readonly BoundingSphere _boundingSphere;

        /// <summary>
        /// Creates a new mesh from vertex and index data
        /// </summary>
        public Mesh(Vertex[] vertices, uint[]? indices = null)
        {
            _vertexCount = vertices.Length;
            _indexCount = indices?.Length ?? 0;
            _boundingSphere = MeshUtils.CalculateBoundingSphere(vertices);

            // Create vertex buffer and array
            _vertexBuffer = new VertexBuffer();
            _vertexArray = new VertexArray();

            // Set vertex data
            _vertexBuffer.SetData(vertices);

            // Configure vertex attributes
            _vertexArray.AddVertexBuffer(_vertexBuffer, new[]
            {
                // Position
                new VertexBufferElement(0, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0),
                // TexCoords
                new VertexBufferElement(1, 2, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 12),
                // Normal
                new VertexBufferElement(2, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 20),
                // Tangent
                new VertexBufferElement(3, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 32),
                // Bitangent
                new VertexBufferElement(4, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 44)
            });

            // Set index data if provided
            if (indices != null)
            {
                _indexBuffer = new IndexBuffer();
                _indexBuffer.SetData(indices);
                _vertexArray.SetIndexBuffer(_indexBuffer);
            }
        }

        /// <summary>
        /// Creates a new mesh from vertex and index data with tangent space calculation
        /// </summary>
        public static Mesh CreateWithTangentSpace(Vertex[] vertices, uint[] indices)
        {
            // Calculate tangent space
            MeshUtils.CalculateTangentSpace(vertices, indices);
            return new Mesh(vertices, indices);
        }

        /// <summary>
        /// Creates a new mesh from vertex and index data with smooth normals
        /// </summary>
        public static Mesh CreateWithSmoothNormals(Vertex[] vertices, uint[] indices)
        {
            // Calculate smooth normals
            MeshUtils.CalculateSmoothNormals(vertices, indices);
            return new Mesh(vertices, indices);
        }

        /// <summary>
        /// Bind the mesh for rendering
        /// </summary>
        public void Bind()
        {
            _vertexArray.Bind();
        }

        /// <summary>
        /// Gets whether the mesh has indices
        /// </summary>
        public bool HasIndices => _indexBuffer != null;

        /// <summary>
        /// Gets the number of indices
        /// </summary>
        public int IndexCount => _indexCount;

        /// <summary>
        /// Gets the number of vertices
        /// </summary>
        public int VertexCount => _vertexCount;

        /// <summary>
        /// Gets the bounding sphere of the mesh
        /// </summary>
        public BoundingSphere BoundingSphere => _boundingSphere;

        /// <summary>
        /// Gets the vertex array object
        /// </summary>
        public VertexArray VertexArray => _vertexArray;

        /// <summary>
        /// Dispose the mesh and its resources
        /// </summary>
        public void Dispose()
        {
            _vertexArray.Dispose();
            _vertexBuffer.Dispose();
            _indexBuffer?.Dispose();
        }
    }
}
