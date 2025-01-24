using OpenTK.Mathematics;

namespace OvRenderingCS.Data
{
    /// <summary>
    /// Describes the properties of a frame to be rendered
    /// </summary>
    public class FrameDescriptor
    {
        /// <summary>
        /// Clear color for the frame
        /// </summary>
        public Vector4 ClearColor { get; set; } = Vector4.Zero;

        /// <summary>
        /// Clear depth buffer
        /// </summary>
        public bool ClearDepth { get; set; } = true;

        /// <summary>
        /// Clear stencil buffer
        /// </summary>
        public bool ClearStencil { get; set; } = true;

        /// <summary>
        /// Clear color buffer
        /// </summary>
        public bool ClearColor { get; set; } = true;

        /// <summary>
        /// Viewport position and size
        /// </summary>
        public Box2i Viewport { get; set; }
    }
}
