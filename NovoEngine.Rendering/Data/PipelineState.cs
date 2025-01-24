using OpenTK.Graphics.OpenGL4;
using OvRenderingCS.Settings;

namespace OvRenderingCS.Data
{
    /// <summary>
    /// Represents the state of the rendering pipeline
    /// </summary>
    public class PipelineState
    {
        public bool DepthTest { get; set; } = true;
        public bool DepthWriting { get; set; } = true;
        public ComparisonAlgorithm DepthFunction { get; set; } = ComparisonAlgorithm.Less;
        
        public bool StencilTest { get; set; } = false;
        public uint StencilWriteMask { get; set; } = ~0u;
        public uint StencilReadMask { get; set; } = ~0u;
        public ComparisonAlgorithm StencilFunction { get; set; } = ComparisonAlgorithm.Always;
        public int StencilReference { get; set; } = 0;
        
        public Operation StencilFailOperation { get; set; } = Operation.Keep;
        public Operation StencilPassOperation { get; set; } = Operation.Keep;
        public Operation StencilDepthFailOperation { get; set; } = Operation.Keep;
        
        public bool Blending { get; set; } = false;
        public BlendFactor SourceColorFactor { get; set; } = BlendFactor.SrcAlpha;
        public BlendFactor DestinationColorFactor { get; set; } = BlendFactor.OneMinusSrcAlpha;
        public BlendFactor SourceAlphaFactor { get; set; } = BlendFactor.One;
        public BlendFactor DestinationAlphaFactor { get; set; } = BlendFactor.Zero;
        
        public bool ColorWriting { get; set; } = true;
        public bool BackFaceCulling { get; set; } = true;
        public CullFace CullFace { get; set; } = CullFace.Back;
        public FrontFace FrontFace { get; set; } = FrontFace.Ccw;
        
        public bool ScissorTest { get; set; } = false;
        public RasterizationMode RasterizationMode { get; set; } = RasterizationMode.Fill;
    }
}
