using OpenTK.Graphics.OpenGL4;

namespace OvRenderingCS.Resources
{
    /// <summary>
    /// OpenGL texture wrapper
    /// </summary>
    public class Texture : IDisposable
    {
        private readonly int _handle;
        private readonly TextureTarget _target;
        private readonly int _width;
        private readonly int _height;

        /// <summary>
        /// Creates a new texture
        /// </summary>
        public Texture(TextureTarget target = TextureTarget.Texture2D)
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
            T[] data,
            PixelInternalFormat internalFormat = PixelInternalFormat.Rgba,
            PixelFormat format = PixelFormat.Rgba,
            PixelType type = PixelType.UnsignedByte) where T : unmanaged
        {
            Bind();

            GL.TexImage2D(
                _target,
                0,
                internalFormat,
                width,
                height,
                0,
                format,
                type,
                data
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
        /// Dispose the texture
        /// </summary>
        public void Dispose()
        {
            GL.DeleteTexture(_handle);
        }
    }
}
