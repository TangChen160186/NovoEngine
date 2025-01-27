using OpenTK.Graphics.OpenGL;

namespace NovoEngine.Rendering.Resources;

public class Texture2D : ITexture
{
    private readonly int _handle;
    private int _width;
    private int _height;
    private PixelInternalFormat _internalFormat;
    private PixelFormat _format;
    private PixelType _type;

    public Texture2D()
    {
        _handle = GL.GenTexture();
        _width = 0;
        _height = 0;
    }

    public void SetData<T>(
        int width,
        int height,
        T[]? data,
        TextureFormatType formatType = TextureFormatType.Uncompressed,
        TextureQuality quality = TextureQuality.High,
        bool isSRGB = false) where T : unmanaged
    {
        if (!TextureFormatManager.IsPlatformSupported(formatType))
        {
            // 如果平台不支持该压缩格式,回退到未压缩格式
            formatType = TextureFormatType.Uncompressed;
        }

        var internalFormat = TextureFormatManager.GetFormat(formatType, quality, isSRGB);
        
        Bind();
        _internalFormat = internalFormat;
        _format = isSRGB ? PixelFormat.Srgb : PixelFormat.Rgba;
        _type = PixelType.UnsignedByte;

        if (data != null && formatType == TextureFormatType.Uncompressed)
        {
            // 未压缩数据
            GL.TexImage2D(
                Target,
                0,
                internalFormat,
                width,
                height,
                0,
                _format,
                _type,
                data as Array
            );
        }
        else if (data != null)
        {
            // 已压缩数据
            GL.CompressedTexImage2D(
                Target,
                0,
                internalFormat,
                width,
                height,
                0,
                data.Length * sizeof(T),
                data as Array
            );
        }
        else
        {
            // 分配空间但不上传数据
            GL.TexImage2D(
                Target,
                0,
                internalFormat,
                width,
                height,
                0,
                _format,
                _type,
                IntPtr.Zero
            );
        }

        _width = width;
        _height = height;
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
        GL.TexImage2D(
            Target,
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

    public void Dispose()
    {
        GL.DeleteTexture(_handle);
    }

    public int Width => _width;
    public int Height => _height;
    public int Handle => _handle;
    public TextureTarget Target => TextureTarget.Texture2d;
} 