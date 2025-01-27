using OpenTK.Graphics.OpenGL;

namespace NovoEngine.Rendering.Resources;

public class Texture2DArray : ITexture
{
    private readonly int _handle;
    private int _width;
    private int _height;
    private int _layers;
    private PixelInternalFormat _internalFormat;
    private PixelFormat _format;
    private PixelType _type;

    public Texture2DArray()
    {
        _handle = GL.GenTexture();
        _width = 0;
        _height = 0;
        _layers = 0;
    }

    public void SetData<T>(
        int width,
        int height,
        int layers,
        T[]? data,
        PixelInternalFormat internalFormat = PixelInternalFormat.Rgba16f,
        PixelFormat format = PixelFormat.Rgba,
        PixelType type = PixelType.Float) where T : unmanaged
    {
        Bind();

        _internalFormat = internalFormat;
        _format = format;
        _type = type;

        GL.TexImage3D(
            Target,
            0,
            internalFormat,
            width,
            height,
            layers,
            0,
            format,
            type,
            data as Array
        );

        _width = width;
        _height = height;
        _layers = layers;
    }

    public void SetLayerData<T>(
        int layer,
        T[]? data,
        PixelInternalFormat internalFormat = PixelInternalFormat.Rgba16f,
        PixelFormat format = PixelFormat.Rgba,
        PixelType type = PixelType.Float) where T : unmanaged
    {
        if (layer >= _layers)
            throw new ArgumentException("Layer index exceeds array size");

        Bind();

        GL.TexSubImage3D(
            Target,
            0,
            0, 0, layer,
            _width,
            _height,
            1,
            format,
            type,
            data as Array
        );
    }

    public void SetParameters(
        TextureMinFilter minFilter = TextureMinFilter.Linear,
        TextureMagFilter magFilter = TextureMagFilter.Linear,
        TextureWrapMode wrapS = TextureWrapMode.Repeat,
        TextureWrapMode wrapT = TextureWrapMode.Repeat)
    {
        Bind();

        GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)minFilter);
        GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int)magFilter);
        GL.TexParameter(Target, TextureParameterName.TextureWrapS, (int)wrapS);
        GL.TexParameter(Target, TextureParameterName.TextureWrapT, (int)wrapT);

        if (minFilter != TextureMinFilter.Nearest && minFilter != TextureMinFilter.Linear)
        {
            GL.GenerateMipmap((GenerateMipmapTarget)Target);
        }
    }

    public void Bind(TextureUnit unit = TextureUnit.Texture0)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(Target, _handle);
    }

    public void Unbind()
    {
        GL.BindTexture(Target, 0);
    }

    public void Resize(int width, int height)
    {
        if (width == _width && height == _height)
            return;

        Bind();
        GL.TexImage3D(
            Target,
            0,
            _internalFormat,
            width,
            height,
            _layers,
            0,
            _format,
            _type,
            IntPtr.Zero
        );
        _width = width;
        _height = height;
    }

    public void Dispose()
    {
        GL.DeleteTexture(_handle);
    }

    public int Width => _width;
    public int Height => _height;
    public int Layers => _layers;
    public int Handle => _handle;
    public TextureTarget Target => TextureTarget.Texture2DArray;
} 