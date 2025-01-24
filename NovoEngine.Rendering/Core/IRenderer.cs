using OvRenderingCS.Data;

namespace OvRenderingCS.Core
{
    /// <summary>
    /// Interface for renderers
    /// </summary>
    public interface IRenderer
    {
        /// <summary>
        /// Begin a new frame
        /// </summary>
        /// <param name="frameDescriptor">Frame descriptor containing frame properties</param>
        void BeginFrame(FrameDescriptor frameDescriptor);

        /// <summary>
        /// Draw the frame
        /// </summary>
        void DrawFrame();

        /// <summary>
        /// End the current frame
        /// </summary>
        void EndFrame();
    }
}
