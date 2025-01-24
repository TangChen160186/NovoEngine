using OvRenderingCS.Entities;
using OvRenderingCS.Resources;

namespace OvRenderingCS.Features
{
    /// <summary>
    /// Render feature for lighting calculations
    /// </summary>
    public class LightingRenderFeature : ARenderFeature
    {
        private readonly List<Light> _lights;

        /// <summary>
        /// Creates a new lighting render feature
        /// </summary>
        public LightingRenderFeature()
        {
            _lights = new List<Light>();
        }

        /// <summary>
        /// Add a light to the feature
        /// </summary>
        public void AddLight(Light light)
        {
            _lights.Add(light);
        }

        /// <summary>
        /// Remove a light from the feature
        /// </summary>
        public void RemoveLight(Light light)
        {
            _lights.Remove(light);
        }

        /// <summary>
        /// Clear all lights from the feature
        /// </summary>
        public void ClearLights()
        {
            _lights.Clear();
        }

        /// <summary>
        /// Setup the feature
        /// </summary>
        public override void Setup()
        {
            // Load lighting shader if needed
        }

        /// <summary>
        /// Destroy the feature
        /// </summary>
        public override void Destroy()
        {
            Shader?.Dispose();
        }

        /// <summary>
        /// Render the lighting
        /// </summary>
        public override void Render(Camera camera)
        {
            if (!Enabled || Shader == null)
                return;

            Shader.Use();

            // Set camera uniforms
            Shader.SetMatrix4("u_ViewProjection", camera.ViewProjectionMatrix);
            Shader.SetVector3("u_ViewPos", camera.Position);

            // Set light uniforms
            for (int i = 0; i < _lights.Count; i++)
            {
                var light = _lights[i];
                string prefix = $"u_Lights[{i}].";

                Shader.SetVector3($"{prefix}Position", light.Transform.ExtractTranslation());
                Shader.SetVector3($"{prefix}Color", light.Color);
                Shader.SetFloat($"{prefix}Intensity", light.Intensity);
            }

            Shader.SetInt("u_LightCount", _lights.Count);
        }

        /// <summary>
        /// Gets the lights in the feature
        /// </summary>
        public IReadOnlyList<Light> Lights => _lights;
    }
}
