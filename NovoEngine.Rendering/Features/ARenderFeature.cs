using NovoEngine.Rendering.Entities;
using NovoEngine.Rendering.Resources;

namespace NovoEngine.Rendering.Features
{
    /// <summary>
    /// Base class for render features
    /// </summary>
    public abstract class ARenderFeature
    {
        private bool _enabled;
        private Shader? _shader;

        /// <summary>
        /// Creates a new render feature
        /// </summary>
        protected ARenderFeature()
        {
            _enabled = true;
            _shader = null;
        }

        /// <summary>
        /// Gets or sets whether the feature is enabled
        /// </summary>
        public bool Enabled
        {
            get => _enabled;
            set => _enabled = value;
        }

        /// <summary>
        /// Gets or sets the shader used by the feature
        /// </summary>
        public Shader? Shader
        {
            get => _shader;
            set => _shader = value;
        }

        /// <summary>
        /// Called when the feature needs to be set up
        /// </summary>
        public abstract void Setup();

        /// <summary>
        /// Called when the feature needs to be destroyed
        /// </summary>
        public abstract void Destroy();

        /// <summary>
        /// Called when the feature needs to render
        /// </summary>
        public abstract void Render(Camera camera);
    }
}
