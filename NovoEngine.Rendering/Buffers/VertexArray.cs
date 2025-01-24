using OpenTK.Graphics.OpenGL4;

namespace OvRenderingCS.Buffers
{
    /// <summary>
    /// OpenGL Vertex Array Object wrapper
    /// </summary>
    public class VertexArray : IDisposable
    {
        private readonly int _handle;
        private readonly Dictionary<int, VertexBuffer> _vertexBuffers;
        private IndexBuffer? _indexBuffer;

        /// <summary>
        /// Creates a new vertex array
        /// </summary>
        public VertexArray()
        {
            _handle = GL.GenVertexArray();
            _vertexBuffers = new Dictionary<int, VertexBuffer>();
        }

        /// <summary>
        /// Bind the vertex array
        /// </summary>
        public void Bind()
        {
            GL.BindVertexArray(_handle);
        }

        /// <summary>
        /// Unbind the vertex array
        /// </summary>
        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        /// <summary>
        /// Add a vertex buffer with specified attributes
        /// </summary>
        public void AddVertexBuffer(VertexBuffer vertexBuffer, params (int index, int size, VertexAttribPointerType type, bool normalized, int stride, int offset)[] attributes)
        {
            Bind();
            vertexBuffer.Bind();

            foreach (var (index, size, type, normalized, stride, offset) in attributes)
            {
                GL.EnableVertexAttribArray(index);
                GL.VertexAttribPointer(index, size, type, normalized, stride, offset);
                _vertexBuffers[index] = vertexBuffer;
            }
        }

        /// <summary>
        /// Set the index buffer
        /// </summary>
        public void SetIndexBuffer(IndexBuffer indexBuffer)
        {
            Bind();
            indexBuffer.Bind();
            _indexBuffer = indexBuffer;
        }

        /// <summary>
        /// Gets whether the vertex array has an index buffer
        /// </summary>
        public bool HasIndexBuffer => _indexBuffer != null;

        /// <summary>
        /// Gets the index buffer if one is set
        /// </summary>
        public IndexBuffer? IndexBuffer => _indexBuffer;

        /// <summary>
        /// Gets the OpenGL handle of the vertex array
        /// </summary>
        public int Handle => _handle;

        /// <summary>
        /// Dispose the vertex array and its resources
        /// </summary>
        public void Dispose()
        {
            GL.DeleteVertexArray(_handle);
        }
    }
}
