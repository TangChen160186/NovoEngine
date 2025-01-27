using NovoEngine.Rendering.Settings;
using OpenTK.Graphics.OpenGL;

namespace NovoEngine.Rendering.Buffers;

/// <summary>
/// OpenGL Vertex Buffer Object wrapper
/// </summary>
public class VertexBuffer : IDisposable
{
    private readonly EBufferUsage _usage;

    /// <summary>
    /// Gets the size of the buffer in bytes
    /// </summary>
    public int Size { get; private set; }

    /// <summary>
    /// Gets the OpenGL handle of the buffer
    /// </summary>
    public int Handle { get; }
    /// <summary>
    /// Creates a new vertex buffer
    /// </summary>
    /// <param name="usage">Buffer usage pattern</param>
    public VertexBuffer(EBufferUsage usage = EBufferUsage.StaticDraw)
    {
        Handle = GL.GenBuffer();
        _usage = usage;
        Size = 0;
    }

    /// <summary>
    /// Bind the vertex buffer
    /// </summary>
    public void Bind()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
    }

    /// <summary>
    /// Unbind the vertex buffer
    /// </summary>
    public void Unbind()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
    }

    /// <summary>
    /// Set or update the buffer data
    /// </summary>
    /// <typeparam name="T">Type of the vertex data (must be unmanaged)</typeparam>
    /// <param name="data">Array of vertex data</param>
    public unsafe void SetData<T>(T[] data) where T : unmanaged
    {
        int size = data.Length * sizeof(T);
        Bind();

        // 根据缓冲区的使用提示选择更新策略
        switch (_usage)
        {
            case EBufferUsage.DynamicDraw:
            case EBufferUsage.StreamDraw:
                // 对于动态/流数据，使用buffer orphaning
                GL.BufferData(BufferTarget.ArrayBuffer, size, data, ConvertBufferUsage(_usage));
                Size = size;
                break;

            case EBufferUsage.StaticDraw:
            default:
                // 对于静态数据，只在大小改变时重新分配
                if (size != Size)
                {
                    GL.BufferData(BufferTarget.ArrayBuffer, size, data, ConvertBufferUsage(_usage));
                    Size = size;
                }
                else
                {
                    GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, size, data);
                }
                break;
        }
    }

    /// <summary>
    /// Update a portion of the buffer data
    /// </summary>
    /// <typeparam name="T">Type of the vertex data (must be unmanaged)</typeparam>
    /// <param name="data">Array of vertex data</param>
    /// <param name="offset">Offset in bytes where to start updating</param>
    public unsafe void SetSubData<T>(T[] data, uint offset) where T : unmanaged
    {
        int size = data.Length * sizeof(T);
        if (offset + size > Size)
            throw new ArgumentException("Data exceeds buffer size");

        Bind();
        GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)offset, size, data);
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
    /// Dispose the vertex buffer
    /// </summary>
    public void Dispose()
    {
        GL.DeleteBuffer(Handle);
    }
}