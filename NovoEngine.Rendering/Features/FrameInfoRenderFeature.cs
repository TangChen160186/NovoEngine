using NovoEngine.Rendering.Data;
using NovoEngine.Rendering.Entities;

namespace NovoEngine.Rendering.Features
{
    /// <summary>
    /// Render feature for frame information
    /// </summary>
    public class FrameInfoRenderFeature : ARenderFeature
    {
        private FrameDescriptor _frameDescriptor;

        /// <summary>
        /// Creates a new frame info render feature
        /// </summary>
        public FrameInfoRenderFeature()
        {
            _frameDescriptor = new FrameDescriptor();
        }

        /// <summary>
        /// Setup the feature
        /// </summary>
        public override void Setup()
        {
            // No setup needed
        }

        /// <summary>
        /// Destroy the feature
        /// </summary>
        public override void Destroy()
        {
            // No cleanup needed
        }

        /// <summary>
        /// Update frame information
        /// </summary>
        public override void Render(Camera camera)
        {
            if (!Enabled)
                return;

            // Update frame descriptor
            _frameDescriptor.ViewProjection = camera.ViewProjectionMatrix;
            _frameDescriptor.View = camera.ViewMatrix;
            _frameDescriptor.Projection = camera.ProjectionMatrix;
            _frameDescriptor.CameraPosition = camera.Position;
            _frameDescriptor.Time = (float)DateTime.Now.TimeOfDay.TotalSeconds;
        }

        /// <summary>
        /// Gets or sets the frame descriptor
        /// </summary>
        public FrameDescriptor FrameDescriptor
        {
            get => _frameDescriptor;
            set => _frameDescriptor = value;
        }
    }
}
