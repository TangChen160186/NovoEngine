using OvRenderingCS.Context;
using OvRenderingCS.Data;

namespace OvRenderingCS.Core
{
    /// <summary>
    /// A renderer that can combine multiple renderers
    /// </summary>
    public class CompositeRenderer : BaseRenderer
    {
        private readonly List<IRenderer> _renderers = new();

        /// <summary>
        /// Creates a new composite renderer
        /// </summary>
        /// <param name="driver">Graphics driver</param>
        public CompositeRenderer(Driver driver) : base(driver)
        {
        }

        /// <summary>
        /// Add a renderer to the composite
        /// </summary>
        public void AddRenderer(IRenderer renderer)
        {
            _renderers.Add(renderer);
        }

        /// <summary>
        /// Remove a renderer from the composite
        /// </summary>
        public bool RemoveRenderer(IRenderer renderer)
        {
            return _renderers.Remove(renderer);
        }

        /// <summary>
        /// Begin frame for all renderers
        /// </summary>
        public override void BeginFrame(FrameDescriptor frameDescriptor)
        {
            base.BeginFrame(frameDescriptor);
            foreach (var renderer in _renderers)
            {
                renderer.BeginFrame(frameDescriptor);
            }
        }

        /// <summary>
        /// Draw frame using all renderers
        /// </summary>
        public override void DrawFrame()
        {
            foreach (var renderer in _renderers)
            {
                renderer.DrawFrame();
            }
        }

        /// <summary>
        /// End frame for all renderers
        /// </summary>
        public override void EndFrame()
        {
            foreach (var renderer in _renderers)
            {
                renderer.EndFrame();
            }
            base.EndFrame();
        }
    }
}
