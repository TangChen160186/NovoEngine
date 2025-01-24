using OpenTK.Graphics.OpenGL4;
using OvRenderingCS.Settings;

namespace OvRenderingCS.Buffers
{
    /// <summary>
    /// OpenGL Shader Storage Buffer Object wrapper
    /// </summary>
    public class ShaderStorageBuffer : IDisposable
    {
        private readonly int _handle;
        private readonly EBufferUsage _usage;
        private readonly EAccessSpecifier _accessSpecifier;
        private int _size;

        /// <summary>
        /// Creates a new shader storage buffer
        /// </summary>
        /// <param name="bindingPoint">Binding point index</param>
        /// <param name="size">Size of the buffer in bytes</param>
        /// <param name="usage">Buffer usage pattern</param>
        /// <param name="accessSpecifier">Buffer access specifier</param>
        public ShaderStorageBuffer(
            uint bindingPoint,
            int size,
            EBufferUsage usage = EBufferUsage.DynamicDraw,
            EAccessSpecifier accessSpecifier = EAccessSpecifier.ReadWrite)
        {
            _handle = GL.GenBuffer();
            _usage = usage;
            _accessSpecifier = accessSpecifier;
            _size = size;

            // Initialize the buffer with empty data
            Bind();
            GL.BufferData(BufferTarget.ShaderStorageBuffer, size, IntPtr.Zero, ConvertBufferUsage(usage));
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, (int)bindingPoint, _handle);
        }

        /// <summary>
        /// Bind the shader storage buffer
        /// </summary>
        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, _handle);
        }

        /// <summary>
        /// Unbind the shader storage buffer
        /// </summary>
        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
        }

        /// <summary>
        /// Update buffer data
        /// </summary>
        public unsafe void SetData<T>(T[] data) where T : unmanaged
        {
            int size = data.Length * sizeof(T);
            Bind();

            if (size != _size)
            {
                GL.BufferData(BufferTarget.ShaderStorageBuffer, size, data, ConvertBufferUsage(_usage));
                _size = size;
            }
            else
            {
                GL.BufferSubData(BufferTarget.ShaderStorageBuffer, IntPtr.Zero, size, data);
            }
        }

        /// <summary>
        /// Get buffer data
        /// </summary>
        public unsafe void GetData<T>(T[] data) where T : unmanaged
        {
            int size = data.Length * sizeof(T);
            if (size > _size)
                throw new ArgumentException("Data buffer is larger than storage buffer");

            Bind();
            GL.GetBufferSubData(BufferTarget.ShaderStorageBuffer, IntPtr.Zero, size, data);
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
        /// Dispose the shader storage buffer
        /// </summary>
        public void Dispose()
        {
            GL.DeleteBuffer(_handle);
        }
    }
}
