using NovoEngine.Rendering.Settings;
using OpenTK.Graphics.OpenGL;

namespace NovoEngine.Rendering.Buffers;

/// <summary>
/// OpenGL Index Buffer Object wrapper
/// </summary>
public class IndexBuffer : IDisposable
{
    private readonly EBufferUsage _usage;

    /// <summary>
    /// Gets the size of the buffer in bytes
    /// </summary>
    public int Size { get; private set; }

    /// <summary>
    /// Gets the number of indices in the buffer
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Gets the OpenGL handle of the buffer
    /// </summary>
    public int Handle { get; }

    /// <summary>
    /// Creates a new index buffer
    /// </summary>
    /// <param name="usage">Buffer usage pattern</param>
    public IndexBuffer(EBufferUsage usage = EBufferUsage.StaticDraw)
    {
        Handle = GL.GenBuffer();
        _usage = usage;
        Size = 0;
        Count = 0;
    }

    /// <summary>
    /// Bind the index buffer
    /// </summary>
    public void Bind()
    {
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, Handle);
    }

    /// <summary>
    /// Unbind the index buffer
    /// </summary>
    public void Unbind()
    {
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
    }

    /// <summary>
    /// Set or update the buffer data
    /// </summary>
    public unsafe void SetData(uint[] data)
    {
        int size = data.Length * sizeof(uint);
        Count = data.Length;
        Bind();

        switch (_usage)
        {
            case EBufferUsage.DynamicDraw:
            case EBufferUsage.StreamDraw:
                // 对于动态/流数据，使用buffer orphaning
                GL.BufferData(BufferTarget.ElementArrayBuffer, size, data, ConvertBufferUsage(_usage));
                Size = size;
                break;

            case EBufferUsage.StaticDraw:
            default:
                // 对于静态数据，只在大小改变时重新分配
                if (size != Size)
                {
                    GL.BufferData(BufferTarget.ElementArrayBuffer, size, data, ConvertBufferUsage(_usage));
                    Size = size;
                }
                else
                {
                    GL.BufferSubData(BufferTarget.ElementArrayBuffer, IntPtr.Zero, size, data);
                }
                break;
        }
    }

    /// <summary>
    /// Update a portion of the buffer data
    /// </summary>
    public unsafe void SetSubData(uint[] data, uint offset)
    {
        int size = data.Length * sizeof(uint);
        if (offset + size > Size)
            throw new ArgumentException("Data exceeds buffer size");

        Bind();
        GL.BufferSubData(BufferTarget.ElementArrayBuffer, (IntPtr)offset, size, data);
    }

    private BufferUsage ConvertBufferUsage(EBufferUsage usage)
    {
        return usage switch
        {
            EBufferUsage.StreamDraw => BufferUsage.StreamDraw,
            EBufferUsage.StaticDraw => BufferUsage.StaticDraw,
            EBufferUsage.DynamicDraw => BufferUsage.DynamicDraw,
            _ => throw new ArgumentException($"Unsupported buffer usage: {usage}")
        };
    }

    /// <summary>
    /// Dispose the index buffer
    /// </summary>
    public void Dispose()
    {
        GL.DeleteBuffer(Handle);
    }
}