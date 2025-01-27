using OpenTK.Graphics.OpenGL;

namespace NovoEngine.Rendering.Buffers;

/// <summary>
/// OpenGL Uniform Buffer Object wrapper
/// </summary>
public class UniformBuffer : IDisposable
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
    /// Gets the binding point of this uniform buffer
    /// </summary>
    public uint BindingPoint { get; }

    /// <summary>
    /// Creates a new uniform buffer
    /// </summary>
    /// <param name="bindingPoint">Binding point index</param>
    public UniformBuffer(uint bindingPoint)
    {
        Handle = GL.GenBuffer();
        Size = 0;
        BindingPoint = bindingPoint;
        Bind();
        GL.BindBufferBase(BufferTarget.UniformBuffer, bindingPoint, Handle);
    }

    /// <summary>
    /// Bind the uniform buffer
    /// </summary>
    public void Bind()
    {
        GL.BindBuffer(BufferTarget.UniformBuffer, Handle);
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
    public unsafe void SetSubData<T>(T[] data, uint offset) where T : unmanaged
    {
        int size = data.Length * sizeof(T);
        if (offset + size > Size)
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

        if (size != Size)
        {
            GL.BufferData(BufferTarget.UniformBuffer, size, data, BufferUsage.DynamicDraw);
            Size = size;
        }
        else
        {
            GL.BufferSubData(BufferTarget.UniformBuffer, IntPtr.Zero, size, data);
        }
    }

    /// <summary>
    /// Dispose the uniform buffer
    /// </summary>
    public void Dispose()
    {
        GL.DeleteBuffer(Handle);
    }
}