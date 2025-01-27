using System;
using OpenTK.Graphics.OpenGL;

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