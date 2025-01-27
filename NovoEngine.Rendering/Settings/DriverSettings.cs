//using NovoEngine.Rendering.Data;

namespace NovoEngine.Rendering.Settings
{
    /// <summary>
    /// Settings for configuring the graphics driver
    /// </summary>
    public class DriverSettings
    {
        /// <summary>
        /// Enable depth testing
        /// </summary>
        public bool DepthTest { get; set; } = true;

        /// <summary>
        /// Enable stencil testing
        /// </summary>
        public bool StencilTest { get; set; } = false;

        /// <summary>
        /// Enable back face culling
        /// </summary>
        public bool BackFaceCulling { get; set; } = true;

        /// <summary>
        /// Enable multisampling (MSAA)
        /// </summary>
        public bool Multisampling { get; set; } = true;

        /// <summary>
        /// Major version of OpenGL context
        /// </summary>
        public int OpenGLMajorVersion { get; set; } = 4;

        /// <summary>
        /// Minor version of OpenGL context
        /// </summary>
        public int OpenGLMinorVersion { get; set; } = 6;

        /// <summary>
        /// Default pipeline state
        /// </summary>
        //public PipelineState? DefaultPipelineState { get; set; }

        /// <summary>
        /// Enable debug mode
        /// </summary>
        public bool DebugMode { get; set; } = false;
    }
}
