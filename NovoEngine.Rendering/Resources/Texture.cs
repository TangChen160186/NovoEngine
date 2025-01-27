using OpenTK.Graphics.OpenGL;

namespace NovoEngine.Rendering.Resources;

/// <summary>
/// Base interface for all texture types
/// </summary>
public interface ITexture : IDisposable
{
    int Width { get; }
    int Height { get; }
    int Handle { get; }
    TextureTarget Target { get; }

    void Bind(TextureUnit unit = TextureUnit.Texture0);
    void Unbind();
    void Resize(int width, int height);
}

/// <summary>
/// OpenGL texture wrapper
/// </summary>
public class Texture : IDisposable
{
    private readonly int _handle;
    private readonly TextureTarget _target;
    private int _width;
    private int _height;
    private InternalFormat _internalFormat;
    private PixelFormat _format;
    private PixelType _type;

    /// <summary>
    /// Creates a new texture
    /// </summary>
    public Texture(TextureTarget target = TextureTarget.Texture2d)
    {
        _handle = GL.GenTexture();
        _target = target;
        _width = 0;
        _height = 0;
    }

    /// <summary>
    /// Bind the texture
    /// </summary>
    public void Bind(TextureUnit unit = TextureUnit.Texture0)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(_target, _handle);
    }

    /// <summary>
    /// Unbind the texture
    /// </summary>
    public void Unbind()
    {
        GL.BindTexture(_target, 0);
    }

    /// <summary>
    /// Set texture data
    /// </summary>
    public void SetData<T>(
        int width,
        int height,
        T[]? data,
        PixelInternalFormat internalFormat = PixelInternalFormat.Rgba16f,
        PixelFormat format = PixelFormat.Rgba,
        PixelType type = PixelType.Float) where T : unmanaged
    {
        Bind();
            
        _internalFormat = internalFormat;
        _format = format;
        _type = type;

        GL.TexImage2D(
            _target,
            0,
            internalFormat,
            width,
            height,
            0,
            format,
            type,
            data as Array
        );

        _width = width;
        _height = height;
    }

    /// <summary>
    /// Set texture parameters
    /// </summary>
    public void SetParameters(
        TextureMinFilter minFilter = TextureMinFilter.Linear,
        TextureMagFilter magFilter = TextureMagFilter.Linear,
        TextureWrapMode wrapS = TextureWrapMode.Repeat,
        TextureWrapMode wrapT = TextureWrapMode.Repeat)
    {
        Bind();

        GL.TexParameter(_target, TextureParameterName.TextureMinFilter, (int)minFilter);
        GL.TexParameter(_target, TextureParameterName.TextureMagFilter, (int)magFilter);
        GL.TexParameter(_target, TextureParameterName.TextureWrapS, (int)wrapS);
        GL.TexParameter(_target, TextureParameterName.TextureWrapT, (int)wrapT);

        if (minFilter != TextureMinFilter.Nearest && minFilter != TextureMinFilter.Linear)
        {
            GL.GenerateMipmap((GenerateMipmapTarget)_target);
        }
    }

    /// <summary>
    /// Gets the width of the texture
    /// </summary>
    public int Width => _width;

    /// <summary>
    /// Gets the height of the texture
    /// </summary>
    public int Height => _height;

    /// <summary>
    /// Gets the OpenGL handle of the texture
    /// </summary>
    public int Handle => _handle;

    /// <summary>
    /// Gets the texture target
    /// </summary>
    public TextureTarget Target => _target;

    /// <summary>
    /// Resize the texture
    /// </summary>
    public void Resize(int width, int height)
    {
        if (width == _width && height == _height)
            return;

        Bind();
        GL.TexImage2D(
            _target,
            0,
            _internalFormat,
            width,
            height,
            0,
            _format,
            _type,
            IntPtr.Zero
        );
        _width = width;
        _height = height;
    }

    /// <summary>
    /// Dispose the texture
    /// </summary>
    public void Dispose()
    {
        GL.DeleteTexture(_handle);
    }
}