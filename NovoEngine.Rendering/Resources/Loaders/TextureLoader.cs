namespace NovoEngine.Rendering.Resources.Loaders
{
    /// <summary>
    /// Utility class for loading textures from files
    /// </summary>
    public static class TextureLoader
    {
        /// <summary>
        /// Load a texture from a file
        /// </summary>
        public static Texture LoadFromFile(
            string path,
            bool flipVertically = true,
            TextureTarget target = TextureTarget.Texture2D,
            bool generateMipmaps = true)
        {
            // Load image data
            StbImage.stbi_set_flip_vertically_on_load(flipVertically ? 1 : 0);
            ImageResult image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);

            // Create texture
            var texture = new Texture(target);
            texture.SetData(
                image.Width,
                image.Height,
                image.Data,
                PixelInternalFormat.Rgba,
                PixelFormat.Rgba,
                PixelType.UnsignedByte
            );

            // Set parameters
            texture.SetParameters(
                minFilter: generateMipmaps ? TextureMinFilter.LinearMipmapLinear : TextureMinFilter.Linear,
                magFilter: TextureMagFilter.Linear,
                wrapS: TextureWrapMode.Repeat,
                wrapT: TextureWrapMode.Repeat
            );

            return texture;
        }

        /// <summary>
        /// Load a cubemap texture from files
        /// </summary>
        public static Texture LoadCubemapFromFiles(
            string[] paths,
            bool flipVertically = false)
        {
            if (paths.Length != 6)
                throw new ArgumentException("Cubemap requires exactly 6 texture paths");

            var texture = new Texture(TextureTarget.TextureCubeMap);
            texture.Bind();

            StbImage.stbi_set_flip_vertically_on_load(flipVertically ? 1 : 0);

            for (int i = 0; i < 6; i++)
            {
                ImageResult image = ImageResult.FromStream(File.OpenRead(paths[i]), ColorComponents.RedGreenBlueAlpha);

                GL.TexImage2D(
                    TextureTarget.TextureCubeMapPositiveX + i,
                    0,
                    PixelInternalFormat.Rgba,
                    image.Width,
                    image.Height,
                    0,
                    PixelFormat.Rgba,
                    PixelType.UnsignedByte,
                    image.Data
                );
            }

            texture.SetParameters(
                minFilter: TextureMinFilter.LinearMipmapLinear,
                magFilter: TextureMagFilter.Linear,
                wrapS: TextureWrapMode.ClampToEdge,
                wrapT: TextureWrapMode.ClampToEdge
            );

            return texture;
        }
    }
}
