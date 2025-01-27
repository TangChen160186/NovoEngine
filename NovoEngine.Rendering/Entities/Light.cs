using OpenTK.Mathematics;

namespace NovoEngine.Rendering.Entities
{
    /// <summary>
    /// Base class for light entities
    /// </summary>
    public abstract class Light : Entity
    {
        private Vector3 _color;
        private float _intensity;

        /// <summary>
        /// Creates a new light with default parameters
        /// </summary>
        protected Light()
        {
            _color = Vector3.One;
            _intensity = 1.0f;
        }

        /// <summary>
        /// Creates a new light with specified parameters
        /// </summary>
        protected Light(Vector3 color, float intensity)
        {
            _color = color;
            _intensity = intensity;
        }

        /// <summary>
        /// Gets or sets the light's color
        /// </summary>
        public Vector3 Color
        {
            get => _color;
            set => _color = value;
        }

        /// <summary>
        /// Gets or sets the light's intensity
        /// </summary>
        public float Intensity
        {
            get => _intensity;
            set => _intensity = value;
        }

        /// <summary>
        /// Gets the light's effective color (color * intensity)
        /// </summary>
        public Vector3 EffectiveColor => _color * _intensity;
    }
}
