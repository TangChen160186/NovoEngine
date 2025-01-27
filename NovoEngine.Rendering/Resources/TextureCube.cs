using OpenTK.Graphics.OpenGL;

namespace NovoEngine.Rendering.Resources;

public class TextureCube : ITexture
{
    private readonly int _handle;
    private int _width;
    private int _height;
    private PixelInternalFormat _internalFormat;
    private PixelFormat _format;
    private PixelType _type;

    public TextureCube()
    {
        _handle = GL.GenTexture();
        _width = 0;
        _height = 0;
    }

    public void SetFaceData<T>(
        CubemapFace face,
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
            TextureTarget.TextureCubeMapPositiveX + (int)face,
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

    public void SetAllFaces<T>(
        int width,
        int height,
        T[][] faceData,
        PixelInternalFormat internalFormat = PixelInternalFormat.Rgba16f,
        PixelFormat format = PixelFormat.Rgba,
        PixelType type = PixelType.Float) where T : unmanaged
    {
        if (faceData.Length != 6)
            throw new ArgumentException("Cubemap requires exactly 6 faces");

        for (int i = 0; i < 6; i++)
        {
            SetFaceData((CubemapFace)i, width, height, faceData[i], internalFormat, format, type);
        }
    }

    public void SetParameters(
        TextureMinFilter minFilter = TextureMinFilter.Linear,
        TextureMagFilter magFilter = TextureMagFilter.Linear,
        TextureWrapMode wrapMode = TextureWrapMode.ClampToEdge)
    {
        Bind();

        GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)minFilter);
        GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int)magFilter);
        GL.TexParameter(Target, TextureParameterName.TextureWrapS, (int)wrapMode);
        GL.TexParameter(Target, TextureParameterName.TextureWrapT, (int)wrapMode);
        GL.TexParameter(Target, TextureParameterName.TextureWrapR, (int)wrapMode);

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
        for (int i = 0; i < 6; i++)
        {
            GL.TexImage2D(
                TextureTarget.TextureCubeMapPositiveX + i,
                0,
                _internalFormat,
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

    public void Dispose()
    {
        GL.DeleteTexture(_handle);
    }

    public int Width => _width;
    public int Height => _height;
    public int Handle => _handle;
    public TextureTarget Target => TextureTarget.TextureCubeMap;
}

public enum CubemapFace
{
    PositiveX = 0,
    NegativeX = 1,
    PositiveY = 2,
    NegativeY = 3,
    PositiveZ = 4,
    NegativeZ = 5
} 