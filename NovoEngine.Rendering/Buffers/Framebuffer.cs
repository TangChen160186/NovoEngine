using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace OvRenderingCS.Buffers
{
    /// <summary>
    /// OpenGL Framebuffer Object wrapper
    /// </summary>
    public class Framebuffer : IDisposable
    {
        private readonly int _handle;
        private readonly Dictionary<FramebufferAttachment, int> _colorAttachments;
        private int? _depthAttachment;
        private Vector2i _size;

        /// <summary>
        /// Creates a new framebuffer
        /// </summary>
        /// <param name="width">Width of the framebuffer</param>
        /// <param name="height">Height of the framebuffer</param>
        public Framebuffer(int width, int height)
        {
            _handle = GL.GenFramebuffer();
            _colorAttachments = new Dictionary<FramebufferAttachment, int>();
            _size = new Vector2i(width, height);
        }

        /// <summary>
        /// Bind the framebuffer
        /// </summary>
        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _handle);
        }

        /// <summary>
        /// Unbind the framebuffer
        /// </summary>
        public void Unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        /// <summary>
        /// Attach a texture to the framebuffer
        /// </summary>
        public void AttachTexture(FramebufferAttachment attachment, int textureHandle)
        {
            Bind();

            GL.FramebufferTexture2D(
                FramebufferTarget.Framebuffer,
                attachment,
                TextureTarget.Texture2D,
                textureHandle,
                0
            );

            if (attachment == FramebufferAttachment.DepthAttachment)
            {
                _depthAttachment = textureHandle;
            }
            else
            {
                _colorAttachments[attachment] = textureHandle;
            }

            CheckFramebufferStatus();
        }

        /// <summary>
        /// Check if the framebuffer is complete
        /// </summary>
        private void CheckFramebufferStatus()
        {
            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (status != FramebufferStatus.FramebufferComplete)
            {
                throw new Exception($"Framebuffer is not complete. Status: {status}");
            }
        }

        /// <summary>
        /// Gets the color attachment texture handle
        /// </summary>
        public int GetColorAttachment(FramebufferAttachment attachment)
        {
            return _colorAttachments.TryGetValue(attachment, out int handle) ? handle : 0;
        }

        /// <summary>
        /// Gets the depth attachment texture handle
        /// </summary>
        public int? GetDepthAttachment()
        {
            return _depthAttachment;
        }

        /// <summary>
        /// Gets the size of the framebuffer
        /// </summary>
        public Vector2i Size => _size;

        /// <summary>
        /// Gets the OpenGL handle of the framebuffer
        /// </summary>
        public int Handle => _handle;

        /// <summary>
        /// Dispose the framebuffer
        /// </summary>
        public void Dispose()
        {
            GL.DeleteFramebuffer(_handle);
        }
    }
}
