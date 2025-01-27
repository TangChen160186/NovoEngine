using NovoEngine.Rendering.Context;
using NovoEngine.Rendering.Data;

namespace NovoEngine.Rendering.Core
{
    /// <summary>
    /// A base renderer that provides common functionality for rendering
    /// </summary>
    public abstract class BaseRenderer : IRenderer
    {
        protected readonly Driver Driver;
        protected FrameDescriptor? CurrentFrameDescriptor;
        protected bool IsFrameInProgress;

        /// <summary>
        /// Creates a new base renderer
        /// </summary>
        /// <param name="driver">Graphics driver</param>
        protected BaseRenderer(Driver driver)
        {
            Driver = driver;
            IsFrameInProgress = false;
        }

        /// <summary>
        /// Begin a new frame
        /// </summary>
        public virtual void BeginFrame(FrameDescriptor frameDescriptor)
        {
            if (IsFrameInProgress)
                throw new InvalidOperationException("Cannot begin a new frame while another frame is in progress");

            CurrentFrameDescriptor = frameDescriptor;
            IsFrameInProgress = true;

            // Set viewport
            Driver.SetViewport(
                (uint)frameDescriptor.Viewport.Min.X,
                (uint)frameDescriptor.Viewport.Min.Y,
                (uint)frameDescriptor.Viewport.Size.X,
                (uint)frameDescriptor.Viewport.Size.Y
            );

            // Clear buffers
            Driver.Clear(
                frameDescriptor.ClearColor,
                frameDescriptor.ClearDepth,
                frameDescriptor.ClearStencil,
                frameDescriptor.ClearColor
            );
        }

        /// <summary>
        /// Draw the frame
        /// </summary>
        public abstract void DrawFrame();

        /// <summary>
        /// End the current frame
        /// </summary>
        public virtual void EndFrame()
        {
            if (!IsFrameInProgress)
                throw new InvalidOperationException("Cannot end frame: no frame is in progress");

            CurrentFrameDescriptor = null;
            IsFrameInProgress = false;
        }

        /// <summary>
        /// Gets the current frame descriptor
        /// </summary>
        protected FrameDescriptor GetCurrentFrameDescriptor()
        {
            if (!IsFrameInProgress)
                throw new InvalidOperationException("Cannot get frame descriptor: no frame is in progress");

            return CurrentFrameDescriptor!;
        }
    }
}
