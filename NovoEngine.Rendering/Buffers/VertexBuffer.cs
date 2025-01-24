using OpenTK.Graphics.OpenGL4;
using OvRenderingCS.Settings;

namespace OvRenderingCS.Buffers
{
    /// <summary>
    /// OpenGL Vertex Buffer Object wrapper
    /// </summary>
    public class VertexBuffer : IDisposable
    {
        private readonly int _handle;
        private readonly EBufferUsage _usage;
        private int _size;

        /// <summary>
        /// Creates a new vertex buffer
        /// </summary>
        /// <param name="usage">Buffer usage pattern</param>
        public VertexBuffer(EBufferUsage usage = EBufferUsage.StaticDraw)
        {
            _handle = GL.GenBuffer();
            _usage = usage;
            _size = 0;
        }

        /// <summary>
        /// Bind the vertex buffer
        /// </summary>
        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _handle);
        }

        /// <summary>
        /// Unbind the vertex buffer
        /// </summary>
        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        /// <summary>
        /// Update buffer data
        /// </summary>
        public unsafe void SetData<T>(T[] data, bool dynamic = false) where T : unmanaged
        {
            int size = data.Length * sizeof(T);
            Bind();

            if (size != _size || dynamic)
            {
                GL.BufferData(BufferTarget.ArrayBuffer, size, data, ConvertBufferUsage(_usage));
                _size = size;
            }
            else
            {
                GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, size, data);
            }
        }

        private BufferUsageHint ConvertBufferUsage(EBufferUsage usage)
        {
            return usage switch
            {
                EBufferUsage.StreamDraw => BufferUsageHint.StreamDraw,
                EBufferUsage.StaticDraw => BufferUsageHint.StaticDraw,
                EBufferUsage.DynamicDraw => BufferUsageHint.DynamicDraw,
                _ => throw new ArgumentException($"Unsupported buffer usage: {usage}")
            };
        }

        /// <summary>
        /// Gets the size of the buffer in bytes
        /// </summary>
        public int Size => _size;

        /// <summary>
        /// Gets the OpenGL handle of the buffer
        /// </summary>
        public int Handle => _handle;

        /// <summary>
        /// Dispose the vertex buffer
        /// </summary>
        public void Dispose()
        {
            GL.DeleteBuffer(_handle);
        }
    }
}
