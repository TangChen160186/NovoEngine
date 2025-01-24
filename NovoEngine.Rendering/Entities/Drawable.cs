using OvRenderingCS.Resources;

namespace OvRenderingCS.Entities
{
    /// <summary>
    /// Base class for drawable entities
    /// </summary>
    public abstract class Drawable : Entity
    {
        private readonly Model _model;

        /// <summary>
        /// Creates a new drawable entity with a model
        /// </summary>
        protected Drawable(Model model)
        {
            _model = model;
        }

        /// <summary>
        /// Gets the model of the drawable entity
        /// </summary>
        public Model Model => _model;

        /// <summary>
        /// Draw the entity
        /// </summary>
        public abstract void Draw(Shader shader);
    }
}
