using NovoEngine.Rendering.Context;
using NovoEngine.Rendering.Data;

namespace NovoEngine.Rendering.Core
{
    /// <summary>
    /// Abstract base class for render passes
    /// </summary>
    public abstract class RenderPass : BaseRenderer
    {
        protected PipelineState RenderState;

        /// <summary>
        /// Creates a new render pass
        /// </summary>
        /// <param name="driver">Graphics driver</param>
        protected RenderPass(Driver driver) : base(driver)
        {
            RenderState = driver.CreatePipelineState();
        }

        /// <summary>
        /// Begin frame with the render pass state
        /// </summary>
        public override void BeginFrame(FrameDescriptor frameDescriptor)
        {
            base.BeginFrame(frameDescriptor);
            Driver.SetPipelineState(RenderState);
        }

        /// <summary>
        /// End frame and reset pipeline state
        /// </summary>
        public override void EndFrame()
        {
            Driver.ResetPipelineState();
            base.EndFrame();
        }
    }
}
