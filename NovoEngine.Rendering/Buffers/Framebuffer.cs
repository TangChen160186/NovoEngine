using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using NovoEngine.Rendering.Resources;

namespace NovoEngine.Rendering.Buffers;

/// <summary>
/// OpenGL Framebuffer Object wrapper
/// </summary>
public class Framebuffer : IDisposable
{
    private readonly Dictionary<FramebufferAttachment, ITexture> _colorTextures;
    private ITexture? _depthTexture;
    private bool _isComplete;

    /// <summary>
    /// Gets the size of the framebuffer
    /// </summary>
    public Vector2i Size { get; private set; }

    /// <summary>
    /// Gets the OpenGL handle of the framebuffer
    /// </summary>
    public int Handle { get; }

    /// <summary>
    /// Creates a new framebuffer with color and depth attachments
    /// </summary>
    /// <param name="width">Width of the framebuffer</param>
    /// <param name="height">Height of the framebuffer</param>
    /// <param name="colorFormat">Format for color attachments</param>
    /// <param name="attachmentCount">Number of color attachments</param>
    /// <param name="hasDepth">Whether to create a depth attachment</param>
    public Framebuffer(int width, int height, InternalFormat colorFormat = InternalFormat.Srgb8,
        int attachmentCount = 1, bool hasDepth = true)
    {
        if (width <= 0 || height <= 0)
            throw new ArgumentException("Width and height must be positive");
        if (attachmentCount <= 0)
            throw new ArgumentException("Must have at least one color attachment");

        Handle = GL.GenFramebuffer();
        Size = new Vector2i(width, height);
        _colorTextures = new Dictionary<FramebufferAttachment, ITexture>();

        Bind();

        // Create color attachments
        var drawBuffers = new List<DrawBuffersEnum>();
        for (int i = 0; i < attachmentCount; i++)
        {
            var attachment = FramebufferAttachment.ColorAttachment0 + i;
            var colorTexture = CreateColorTexture(width, height, colorFormat);
            AttachTexture(attachment, colorTexture);
            drawBuffers.Add(DrawBuffersEnum.ColorAttachment0 + i);
        }

        // Set draw buffers
        GL.DrawBuffers(drawBuffers.Count, drawBuffers.ToArray());

        // Create depth attachment if needed
        if (hasDepth)
        {
            _depthTexture = CreateDepthTexture(width, height);
            AttachTexture(FramebufferAttachment.DepthAttachment, _depthTexture);
        }

        _isComplete = CheckFramebufferStatus();
        Unbind();

        if (!_isComplete)
        {
            Dispose();
            throw new Exception("Failed to create framebuffer");
        }
    }

    /// <summary>
    /// Resize the framebuffer and all its attachments
    /// </summary>
    public void Resize(int width, int height)
    {
        if (width <= 0 || height <= 0)
            throw new ArgumentException("Width and height must be positive");
        if (width == Size.X && height == Size.Y) return;

        Size = new Vector2i(width, height);
        Bind();

        // Resize color attachments
        foreach (var kvp in _colorTextures)
        {
            var texture = kvp.Value;
            texture.Resize(width, height);
            GL.FramebufferTexture2D(
                FramebufferTarget.Framebuffer,
                kvp.Key,
                TextureTarget.Texture2d,
                texture.Handle,
                0
            );
        }

        // Resize depth attachment
        if (_depthTexture != null)
        {
            _depthTexture.Resize(width, height);
            GL.FramebufferTexture2D(
                FramebufferTarget.Framebuffer,
                FramebufferAttachment.DepthAttachment,
                TextureTarget.Texture2d,
                _depthTexture.Handle,
                0
            );
        }

        _isComplete = CheckFramebufferStatus();
        Unbind();

        if (!_isComplete)
        {
            throw new Exception("Failed to resize framebuffer");
        }
    }

    private ITexture CreateColorTexture(int width, int height, PixelInternalFormat format)
    {
        var texture = new Texture2D();
        texture.SetData(width, height, (byte[])null!, format);
        texture.SetParameters(
            TextureMinFilter.Linear,
            TextureMagFilter.Linear,
            TextureWrapMode.ClampToEdge,
            TextureWrapMode.ClampToEdge
        );
        return texture;
    }

    private ITexture CreateDepthTexture(int width, int height)
    {
        var texture = new Texture2D();
        texture.SetData(width, height, (byte[])null!, PixelInternalFormat.DepthComponent24);
        texture.SetParameters(
            TextureMinFilter.Nearest,
            TextureMagFilter.Nearest,
            TextureWrapMode.ClampToEdge,
            TextureWrapMode.ClampToEdge
        );
        return texture;
    }

    private void AttachTexture(FramebufferAttachment attachment, ITexture texture)
    {
        GL.FramebufferTexture2D(
            FramebufferTarget.Framebuffer,
            attachment,
            TextureTarget.Texture2d,
            texture.Handle,
            0
        );

        if (attachment == FramebufferAttachment.DepthAttachment)
        {
            _depthTexture = texture;
        }
        else
        {
            _colorTextures[attachment] = texture;
        }
    }

    /// <summary>
    /// Get a color attachment texture
    /// </summary>
    public ITexture GetColorTexture(int index = 0)
    {
        var attachment = FramebufferAttachment.ColorAttachment0 + index;
        return _colorTextures[attachment];
    }

    /// <summary>
    /// Get the depth texture
    /// </summary>
    public ITexture? GetDepthTexture() => _depthTexture;

    /// <summary>
    /// Clear the framebuffer
    /// </summary>
    public void Clear(Color4? clearColor = null, bool clearDepth = true)
    {
        Bind();
        ClearBufferMask mask = 0;

        if (clearColor.HasValue)
        {
            GL.ClearColor(clearColor.Value);
            mask |= ClearBufferMask.ColorBufferBit;
        }

        if (clearDepth && _depthTexture != null)
        {
            mask |= ClearBufferMask.DepthBufferBit;
        }

        if (mask != 0)
        {
            GL.Clear(mask);
        }
    }

    /// <summary>
    /// Check if the framebuffer is complete
    /// </summary>
    private bool CheckFramebufferStatus()
    {
        Bind();
        var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
        if (status != FramebufferStatus.FramebufferComplete)
        {
            throw new Exception($"Framebuffer is not complete. Status: {status}");
        }
        return true;
    }

    /// <summary>
    /// Bind the framebuffer
    /// </summary>
    public void Bind()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, Handle);
    }

    /// <summary>
    /// Unbind the framebuffer
    /// </summary>
    public void Unbind()
    {
        GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
    }

    /// <summary>
    /// Dispose the framebuffer
    /// </summary>
    public void Dispose()
    {
        foreach (var texture in _colorTextures.Values)
        {
            texture.Dispose();
        }
        _depthTexture?.Dispose();
        GL.DeleteFramebuffer(Handle);
    }
}