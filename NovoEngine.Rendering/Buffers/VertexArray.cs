using NovoEngine.Rendering.Settings;
using OpenTK.Graphics.OpenGL;

namespace NovoEngine.Rendering.Buffers;

/// <summary>
/// OpenGL Vertex Array Object wrapper
/// </summary>
public class VertexArray : IDisposable
{
    /// <summary>
    /// Gets the index buffer if one is set
    /// </summary>
    public IndexBuffer? IndexBuffer { get; private set; }

    /// <summary>
    /// Gets the OpenGL handle of the vertex array
    /// </summary>
    public int Handle { get; }

    /// <summary>
    /// Gets whether the vertex array has an index buffer
    /// </summary>
    public bool HasIndexBuffer => IndexBuffer != null;

    /// <summary>
    /// Creates a new vertex array
    /// </summary>
    public VertexArray()
    {
        Handle = GL.GenVertexArray();
    }

    /// <summary>
    /// Bind the vertex array
    /// </summary>
    public void Bind()
    {
        GL.BindVertexArray(Handle);
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
    public void AddVertexBuffer(VertexBuffer vertexBuffer, uint index, int size, EDataType type, bool normalized,
        int stride, IntPtr offset)
    {
        Bind();
        vertexBuffer.Bind();
        GL.EnableVertexAttribArray(index);
        GL.VertexAttribPointer(index, size, ConvertDataType(type), normalized, stride, offset);
    }

    /// <summary>
    /// Set the index buffer
    /// </summary>
    public void SetIndexBuffer(IndexBuffer indexBuffer)
    {
        Bind();
        indexBuffer.Bind();
        IndexBuffer = indexBuffer;
    }


    /// <summary>
    /// Dispose the vertex array and its resources
    /// </summary>
    public void Dispose()
    {
        GL.DeleteVertexArray(Handle);
    }


    private VertexAttribPointerType ConvertDataType(EDataType type)
    {
        return type switch
        {
            EDataType.Byte => VertexAttribPointerType.Byte,
            EDataType.UnisgnedByte => VertexAttribPointerType.UnsignedByte,
            EDataType.Short => VertexAttribPointerType.Short,
            EDataType.UnsignedShort => VertexAttribPointerType.UnsignedShort,
            EDataType.Int => VertexAttribPointerType.Int,
            EDataType.UnsignedIn => VertexAttribPointerType.UnsignedInt,
            EDataType.Float => VertexAttribPointerType.Float,
            EDataType.Double => VertexAttribPointerType.Double,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}