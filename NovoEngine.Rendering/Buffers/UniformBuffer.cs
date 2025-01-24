using OpenTK.Graphics.OpenGL4;
using OvRenderingCS.Settings;

namespace OvRenderingCS.Buffers
{
    /// <summary>
    /// OpenGL Uniform Buffer Object wrapper
    /// </summary>
    public class UniformBuffer : IDisposable
    {
        private readonly int _handle;
        private readonly EBufferUsage _usage;
        private int _size;

        /// <summary>
        /// Creates a new uniform buffer
        /// </summary>
        /// <param name="size">Size of the buffer in bytes</param>
        /// <param name="bindingPoint">Binding point index</param>
        /// <param name="usage">Buffer usage pattern</param>
        public UniformBuffer(int size, uint bindingPoint, EBufferUsage usage = EBufferUsage.DynamicDraw)
        {
            _handle = GL.GenBuffer();
            _usage = usage;
            _size = size;

            // Initialize the buffer with empty data
            Bind();
            GL.BufferData(BufferTarget.UniformBuffer, size, IntPtr.Zero, ConvertBufferUsage(usage));
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, (int)bindingPoint, _handle);
        }

        /// <summary>
        /// Bind the uniform buffer
        /// </summary>
        public void Bind()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, _handle);
        }

        /// <summary>
        /// Unbind the uniform buffer
        /// </summary>
        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
        }

        /// <summary>
        /// Update a portion of the buffer data
        /// </summary>
        public unsafe void SetSubData<T>(T[] data, int offset) where T : unmanaged
        {
            int size = data.Length * sizeof(T);
            if (offset + size > _size)
                throw new ArgumentException("Data exceeds buffer size");

            Bind();
            GL.BufferSubData(BufferTarget.UniformBuffer, (IntPtr)offset, size, data);
        }

        /// <summary>
        /// Update the entire buffer data
        /// </summary>
        public unsafe void SetData<T>(T[] data) where T : unmanaged
        {
            int size = data.Length * sizeof(T);
            Bind();

            if (size != _size)
            {
                GL.BufferData(BufferTarget.UniformBuffer, size, data, ConvertBufferUsage(_usage));
                _size = size;
            }
            else
            {
                GL.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, size, data);
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
        /// Dispose the uniform buffer
        /// </summary>
        public void Dispose()
        {
            GL.DeleteBuffer(_handle);
        }
    }
}
