//using NovoEngine.Rendering.Data;
//using NovoEngine.Rendering.Resources;
//using NovoEngine.Rendering.Settings;
//using OpenTK.Graphics.OpenGL;
//using OpenTK.Mathematics;

//namespace NovoEngine.Rendering.Context
//{
//    /// <summary>
//    /// Handles the lifecycle of the underlying graphics context
//    /// </summary>
//    public sealed class Driver
//    {
//        private readonly DriverSettings _settings;
//        private PipelineState _currentPipelineState;
//        private PipelineState _defaultPipelineState;

//        public string Vendor => GL.GetString(StringName.Vendor);
//        public string Hardware => GL.GetString(StringName.Renderer);
//        public string Version => GL.GetString(StringName.Version);
//        public string ShadingLanguageVersion => GL.GetString(StringName.ShadingLanguageVersion);

//        /// <summary>
//        /// Creates the driver
//        /// </summary>
//        /// <param name="settings">Driver settings</param>
//        public Driver(DriverSettings settings)
//        {
//            _settings = settings;
//            Initialize();
            
//            _defaultPipelineState = CreatePipelineState();
//            if (settings.DefaultPipelineState != null)
//            {
//                _defaultPipelineState = settings.DefaultPipelineState;
//            }
            
//            _currentPipelineState = _defaultPipelineState;
//            SetPipelineState(_defaultPipelineState);
//        }

//        private void Initialize()
//        {
//            // Enable depth testing if requested
//            if (_settings.DepthTest)
//            {
//                GL.Enable(EnableCap.DepthTest);
//            }

//            // Enable stencil testing if requested
//            if (_settings.StencilTest)
//            {
//                GL.Enable(EnableCap.StencilTest);
//            }

//            // Enable back face culling if requested
//            if (_settings.BackFaceCulling)
//            {
//                GL.Enable(EnableCap.CullFace);
//                GL.CullFace(CullFaceMode.Back);
//            }

//            // Enable MSAA if requested
//            if (_settings.Multisampling)
//            {
//                GL.Enable(EnableCap.Multisample);
//            }
//        }

//        /// <summary>
//        /// Set the viewport dimensions
//        /// </summary>
//        public void SetViewport(uint x, uint y, uint width, uint height)
//        {
//            GL.Viewport((int)x, (int)y, (int)width, (int)height);
//        }

//        /// <summary>
//        /// Clear the screen buffers
//        /// </summary>
//        public void Clear(bool colorBuffer, bool depthBuffer, bool stencilBuffer, Vector4? color = null)
//        {
//            if (colorBuffer && color.HasValue)
//            {
//                GL.ClearColor(color.Value.X, color.Value.Y, color.Value.Z, color.Value.W);
//            }

//            var pso = CreatePipelineState();
//            if (stencilBuffer)
//            {
//                pso.StencilWriteMask = ~0u;
//            }
//            pso.ScissorTest = false;
//            SetPipelineState(pso);

//            ClearBufferMask mask = 0;
//            if (colorBuffer) mask |= ClearBufferMask.ColorBufferBit;
//            if (depthBuffer) mask |= ClearBufferMask.DepthBufferBit;
//            if (stencilBuffer) mask |= ClearBufferMask.StencilBufferBit;

//            if (mask != 0)
//                GL.Clear(mask);
//        }

//        /// <summary>
//        /// Read pixels from the currently bound framebuffer
//        /// </summary>
//        public void ReadPixels(uint x, uint y, uint width, uint height, PixelFormat format, PixelType type, IntPtr data)
//        {
//            GL.ReadPixels((int)x, (int)y, (int)width, (int)height, format, type, data);
//        }

//        /// <summary>
//        /// Draw a mesh
//        /// </summary>
//        public void Draw(PipelineState pso, IMesh mesh, PrimitiveMode primitiveMode, uint instances = 1)
//        {
//            if (instances > 0)
//            {
//                SetPipelineState(pso);
//                mesh.Bind();
                
//                if (mesh.HasIndices)
//                {
//                    if (instances == 1)
//                        GL.DrawElements(primitiveMode, mesh.IndexCount, DrawElementsType.UnsignedInt, 0);
//                    else
//                        GL.DrawElementsInstanced(primitiveMode, mesh.IndexCount, DrawElementsType.UnsignedInt, IntPtr.Zero, (int)instances);
//                }
//                else
//                {
//                    if (instances == 1)
//                        GL.DrawArrays(primitiveMode, 0, mesh.VertexCount);
//                    else
//                        GL.DrawArraysInstanced(primitiveMode, 0, mesh.VertexCount, (int)instances);
//                }
//            }
//        }

//        /// <summary>
//        /// Set the current pipeline state
//        /// </summary>
//        public void SetPipelineState(PipelineState state)
//        {
//            if (state.DepthTest != _currentPipelineState.DepthTest)
//            {
//                if (state.DepthTest) GL.Enable(EnableCap.DepthTest);
//                else GL.Disable(EnableCap.DepthTest);
//            }

//            if (state.DepthWriting != _currentPipelineState.DepthWriting)
//            {
//                GL.DepthMask(state.DepthWriting);
//            }

//            if (state.DepthFunction != _currentPipelineState.DepthFunction)
//            {
//                GL.DepthFunc(ConvertComparisonAlgorithm(state.DepthFunction));
//            }

//            if (state.StencilTest != _currentPipelineState.StencilTest)
//            {
//                if (state.StencilTest) GL.Enable(EnableCap.StencilTest);
//                else GL.Disable(EnableCap.StencilTest);
//            }

//            if (state.StencilWriteMask != _currentPipelineState.StencilWriteMask ||
//                state.StencilReadMask != _currentPipelineState.StencilReadMask)
//            {
//                GL.StencilMask(state.StencilWriteMask);
//            }

//            if (state.StencilFunction != _currentPipelineState.StencilFunction ||
//                state.StencilReference != _currentPipelineState.StencilReference)
//            {
//                GL.StencilFunc(
//                    ConvertComparisonAlgorithm(state.StencilFunction),
//                    state.StencilReference,
//                    (int)state.StencilReadMask
//                );
//            }

//            if (state.StencilFailOperation != _currentPipelineState.StencilFailOperation ||
//                state.StencilPassOperation != _currentPipelineState.StencilPassOperation ||
//                state.StencilDepthFailOperation != _currentPipelineState.StencilDepthFailOperation)
//            {
//                GL.StencilOp(
//                    ConvertOperation(state.StencilFailOperation),
//                    ConvertOperation(state.StencilDepthFailOperation),
//                    ConvertOperation(state.StencilPassOperation)
//                );
//            }

//            if (state.Blending != _currentPipelineState.Blending)
//            {
//                if (state.Blending) GL.Enable(EnableCap.Blend);
//                else GL.Disable(EnableCap.Blend);
//            }

//            if (state.SourceColorFactor != _currentPipelineState.SourceColorFactor ||
//                state.DestinationColorFactor != _currentPipelineState.DestinationColorFactor ||
//                state.SourceAlphaFactor != _currentPipelineState.SourceAlphaFactor ||
//                state.DestinationAlphaFactor != _currentPipelineState.DestinationAlphaFactor)
//            {
//                GL.BlendFuncSeparate(
//                    (BlendingFactor)state.SourceColorFactor,
//                    (BlendingFactor)state.DestinationColorFactor,
//                    (BlendingFactor)state.SourceAlphaFactor,
//                    (BlendingFactor)state.DestinationAlphaFactor
//                );
//            }

//            if (state.ColorWriting != _currentPipelineState.ColorWriting)
//            {
//                GL.ColorMask(state.ColorWriting, state.ColorWriting, state.ColorWriting, state.ColorWriting);
//            }

//            if (state.BackFaceCulling != _currentPipelineState.BackFaceCulling)
//            {
//                if (state.BackFaceCulling) GL.Enable(EnableCap.CullFace);
//                else GL.Disable(EnableCap.CullFace);
//            }

//            if (state.CullFace != _currentPipelineState.CullFace)
//            {
//                GL.CullFace((CullFaceMode)state.CullFace);
//            }

//            if (state.FrontFace != _currentPipelineState.FrontFace)
//            {
//                GL.FrontFace((FrontFaceDirection)state.FrontFace);
//            }

//            if (state.ScissorTest != _currentPipelineState.ScissorTest)
//            {
//                if (state.ScissorTest) GL.Enable(EnableCap.ScissorTest);
//                else GL.Disable(EnableCap.ScissorTest);
//            }

//            if (state.RasterizationMode != _currentPipelineState.RasterizationMode)
//            {
//                switch (state.RasterizationMode)
//                {
//                    case RasterizationMode.Point:
//                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Point);
//                        break;
//                    case RasterizationMode.Line:
//                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
//                        break;
//                    case RasterizationMode.Fill:
//                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
//                        break;
//                }
//            }

//            _currentPipelineState = state;
//        }

//        /// <summary>
//        /// Reset the pipeline state to default
//        /// </summary>
//        public void ResetPipelineState()
//        {
//            SetPipelineState(_defaultPipelineState);
//        }

//        /// <summary>
//        /// Create a new pipeline state
//        /// </summary>
//        public PipelineState CreatePipelineState()
//        {
//            return new PipelineState();
//        }

//        private DepthFunction ConvertComparisonAlgorithm(ComparisonAlgorithm algorithm)
//        {
//            return algorithm switch
//            {
//                ComparisonAlgorithm.Never => DepthFunction.Never,
//                ComparisonAlgorithm.Less => DepthFunction.Less,
//                ComparisonAlgorithm.Equal => DepthFunction.Equal,
//                ComparisonAlgorithm.LessEqual => DepthFunction.Lequal,
//                ComparisonAlgorithm.Greater => DepthFunction.Greater,
//                ComparisonAlgorithm.NotEqual => DepthFunction.Notequal,
//                ComparisonAlgorithm.GreaterEqual => DepthFunction.Gequal,
//                ComparisonAlgorithm.Always => DepthFunction.Always,
//                _ => throw new ArgumentException($"Unsupported comparison algorithm: {algorithm}")
//            };
//        }

//        private StencilOp ConvertOperation(Operation operation)
//        {
//            return operation switch
//            {
//                Operation.Keep => StencilOp.Keep,
//                Operation.Zero => StencilOp.Zero,
//                Operation.Replace => StencilOp.Replace,
//                Operation.Increment => StencilOp.Incr,
//                Operation.IncrementWrap => StencilOp.IncrWrap,
//                Operation.Decrement => StencilOp.Decr,
//                Operation.DecrementWrap => StencilOp.DecrWrap,
//                Operation.Invert => StencilOp.Invert,
//                _ => throw new ArgumentException($"Unsupported operation: {operation}")
//            };
//        }
//    }
//}
