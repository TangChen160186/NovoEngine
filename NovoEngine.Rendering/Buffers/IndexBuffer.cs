using OpenTK.Graphics.OpenGL4;
using OvRenderingCS.Settings;

namespace OvRenderingCS.Buffers
{
    /// <summary>
    /// OpenGL Index Buffer Object wrapper
    /// </summary>
    public class IndexBuffer : IDisposable
    {
        private readonly int _handle;
        private readonly EBufferUsage _usage;
        private int _count;

        /// <summary>
        /// Creates a new index buffer
        /// </summary>
        /// <param name="usage">Buffer usage pattern</param>
        public IndexBuffer(EBufferUsage usage = EBufferUsage.StaticDraw)
        {
            _handle = GL.GenBuffer();
            _usage = usage;
            _count = 0;
        }

        /// <summary>
        /// Bind the index buffer
        /// </summary>
        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _handle);
        }

        /// <summary>
        /// Unbind the index buffer
        /// </summary>
        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        /// <summary>
        /// Update buffer data
        /// </summary>
        public void SetData(uint[] data, bool dynamic = false)
        {
            _count = data.Length;
            Bind();

            GL.BufferData(
                BufferTarget.ElementArrayBuffer,
                data.Length * sizeof(uint),
                data,
                ConvertBufferUsage(_usage)
            );
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
        /// Gets the number of indices in the buffer
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// Gets the OpenGL handle of the buffer
        /// </summary>
        public int Handle => _handle;

        /// <summary>
        /// Dispose the index buffer
        /// </summary>
        public void Dispose()
        {
            GL.DeleteBuffer(_handle);
        }
    }
}
