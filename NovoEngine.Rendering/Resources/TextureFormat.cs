using OpenTK.Graphics.OpenGL;

namespace NovoEngine.Rendering.Resources;

/// <summary>
/// 纹理质量预设
/// </summary>
public enum TextureQuality
{
    Low,    // 低质量,高压缩率
    Medium, // 中等质量和压缩率
    High,   // 高质量,低压缩率
    Ultra   // 无损/最高质量
}

/// <summary>
/// 纹理格式类型
/// </summary>
public enum TextureFormatType
{
    Uncompressed,    // 未压缩
    CompressedRGB,   // RGB压缩
    CompressedRGBA,  // RGBA压缩
    CompressedSRGB,  // sRGB压缩
    CompressedSRGBA  // sRGBA压缩
}

/// <summary>
/// 纹理格式管理器
/// </summary>
public static class TextureFormatManager
{
    public static PixelInternalFormat GetFormat(
        TextureFormatType type, 
        TextureQuality quality, 
        bool isSRGB = false)
    {
        return (type, quality, isSRGB) switch
        {
            // 未压缩格式
            (TextureFormatType.Uncompressed, _, false) => PixelInternalFormat.Rgba8,
            (TextureFormatType.Uncompressed, _, true) => PixelInternalFormat.Srgb8Alpha8,

            // 压缩RGB格式
            (TextureFormatType.CompressedRGB, TextureQuality.Low, false) => PixelInternalFormat.CompressedRgbS3tcDxt1Ext,
            (TextureFormatType.CompressedRGB, TextureQuality.Medium, false) => PixelInternalFormat.CompressedRgbBptcUnsignedFloat,
            (TextureFormatType.CompressedRGB, TextureQuality.High, false) => PixelInternalFormat.CompressedRgb8Etc2,
            
            // 压缩RGBA格式
            (TextureFormatType.CompressedRGBA, TextureQuality.Low, false) => PixelInternalFormat.CompressedRgbaS3tcDxt5Ext,
            (TextureFormatType.CompressedRGBA, TextureQuality.Medium, false) => PixelInternalFormat.CompressedRgbaBptcUnorm,
            (TextureFormatType.CompressedRGBA, TextureQuality.High, false) => PixelInternalFormat.CompressedRgba8Etc2Eac,

            // 压缩sRGB格式
            (TextureFormatType.CompressedSRGB, TextureQuality.Low, true) => PixelInternalFormat.CompressedSrgbS3tcDxt1Ext,
            (TextureFormatType.CompressedSRGB, TextureQuality.Medium, true) => PixelInternalFormat.CompressedSrgbBptcUnorm,
            (TextureFormatType.CompressedSRGB, TextureQuality.High, true) => PixelInternalFormat.CompressedSrgb8Etc2,

            // 压缩sRGBA格式
            (TextureFormatType.CompressedSRGBA, TextureQuality.Low, true) => PixelInternalFormat.CompressedSrgbaS3tcDxt5Ext,
            (TextureFormatType.CompressedSRGBA, TextureQuality.Medium, true) => PixelInternalFormat.CompressedSrgbaBptcUnorm,
            (TextureFormatType.CompressedSRGBA, TextureQuality.High, true) => PixelInternalFormat.CompressedSrgba8Etc2Eac,

            _ => PixelInternalFormat.Rgba8 // 默认格式
        };
    }

    public static bool IsPlatformSupported(TextureFormatType type)
    {
        // 检查平台支持的压缩格式
        return type switch
        {
            TextureFormatType.CompressedRGB or 
            TextureFormatType.CompressedRGBA => 
                GL.GetString(StringName.Extensions).Contains("GL_EXT_texture_compression_s3tc"),
            
            TextureFormatType.CompressedSRGB or 
            TextureFormatType.CompressedSRGBA => 
                GL.GetString(StringName.Extensions).Contains("GL_EXT_texture_sRGB"),
            
            _ => true
        };
    }

    public static PixelInternalFormat GetPlatformFormat(BuildTarget target, TextureFormatType type)
    {
        return target switch
        {
            BuildTarget.Windows or BuildTarget.MacOS or BuildTarget.Linux =>
                type switch
                {
                    TextureFormatType.CompressedRGB => PixelInternalFormat.CompressedRgbS3tcDxt1Ext,
                    TextureFormatType.CompressedRGBA => PixelInternalFormat.CompressedRgbaS3tcDxt5Ext,
                    _ => PixelInternalFormat.Rgba8
                },
            BuildTarget.Android =>
                type switch
                {
                    TextureFormatType.CompressedRGB => PixelInternalFormat.CompressedRgb8Etc2,
                    TextureFormatType.CompressedRGBA => PixelInternalFormat.CompressedRgba8Etc2Eac,
                    _ => PixelInternalFormat.Rgba8
                },
            BuildTarget.iOS =>
                type switch
                {
                    TextureFormatType.CompressedRGB => PixelInternalFormat.CompressedRgbaBptcUnorm,
                    TextureFormatType.CompressedRGBA => PixelInternalFormat.CompressedRgbaBptcUnorm,
                    _ => PixelInternalFormat.Rgba8
                },
            _ => PixelInternalFormat.Rgba8
        };
    }
} 