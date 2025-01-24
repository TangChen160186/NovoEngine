using OpenTK.Mathematics;

namespace OvRenderingCS.Entities
{
    /// <summary>
    /// Base class for all renderable entities
    /// </summary>
    public abstract class Entity
    {
        private Matrix4 _transform;

        /// <summary>
        /// Creates a new entity with an identity transform
        /// </summary>
        protected Entity()
        {
            _transform = Matrix4.Identity;
        }

        /// <summary>
        /// Gets or sets the entity's transform matrix
        /// </summary>
        public Matrix4 Transform
        {
            get => _transform;
            set => _transform = value;
        }
    }
}
