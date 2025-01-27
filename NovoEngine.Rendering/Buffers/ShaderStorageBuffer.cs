using OpenTK.Graphics.OpenGL;

namespace NovoEngine.Rendering.Buffers;

/// <summary>
/// OpenGL Shader Storage Buffer Object wrapper
/// </summary>
public class ShaderStorageBuffer : IDisposable
{
    /// <summary>
    /// Gets the size of the buffer in bytes
    /// </summary>
    public int Size { get; private set; }

    /// <summary>
    /// Gets the OpenGL handle of the buffer
    /// </summary>
    public int Handle { get; }
    /// <summary>
    /// Creates a new shader storage buffer
    /// </summary>
    /// <param name="bindingPoint">Binding point index</param>
    public ShaderStorageBuffer(
        uint bindingPoint)
    {
        Handle = GL.GenBuffer();
        // Initialize the buffer with empty data
        Bind();
        GL.BindBufferBase(BufferTarget.ShaderStorageBuffer, bindingPoint, Handle);
    }

    /// <summary>
    /// Bind the shader storage buffer
    /// </summary>
    public void Bind()
    {
        GL.BindBuffer(BufferTarget.ShaderStorageBuffer, Handle);
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

        if (size != Size)
        {
            GL.BufferData(BufferTarget.ShaderStorageBuffer, size, data,BufferUsage.DynamicDraw);
            Size = size;
        }
        else
        {
            GL.BufferSubData(BufferTarget.ShaderStorageBuffer, IntPtr.Zero, size, data);
        }
    }

    /// <summary>
    /// Update a portion of the buffer data
    /// </summary>
    /// <typeparam name="T">Type of the data (must be unmanaged)</typeparam>
    /// <param name="data">Array of data</param>
    /// <param name="offset">Offset in bytes where to start updating</param>
    public unsafe void SetSubData<T>(T[] data, uint offset) where T : unmanaged
    {
        int size = data.Length * sizeof(T);
        if (offset + size > Size)
            throw new ArgumentException("Data exceeds buffer size");

        Bind();
        GL.BufferSubData(BufferTarget.ShaderStorageBuffer, (IntPtr)offset, size, data);
    }
    /// <summary>
    /// Get buffer data
    /// </summary>
    public unsafe void GetData<T>(T[] data) where T : unmanaged
    {
        int size = data.Length * sizeof(T);
        if (size > Size)
            throw new ArgumentException("Data buffer is larger than storage buffer");

        Bind();
        GL.GetBufferSubData(BufferTarget.ShaderStorageBuffer, IntPtr.Zero, size, data);
    }



    /// <summary>
    /// Dispose the shader storage buffer
    /// </summary>
    public void Dispose()
    {
        GL.DeleteBuffer(Handle);
    }
}