namespace NovoEngine.Rendering.HAL
{
    /// <summary>
    /// Hardware Abstraction Layer for graphics API
    /// </summary>
    public static class GraphicsAPI
    {
        /// <summary>
        /// Enable a capability
        /// </summary>
        public static void Enable(EnableCap cap)
        {
            GL.Enable(cap);
        }

        /// <summary>
        /// Disable a capability
        /// </summary>
        public static void Disable(EnableCap cap)
        {
            GL.Disable(cap);
        }

        /// <summary>
        /// Set the depth function
        /// </summary>
        public static void SetDepthFunction(DepthFunction func)
        {
            GL.DepthFunc(func);
        }

        /// <summary>
        /// Set the blend function
        /// </summary>
        public static void SetBlendFunction(BlendingFactor src, BlendingFactor dst)
        {
            GL.BlendFunc(src, dst);
        }

        /// <summary>
        /// Set the cull face
        /// </summary>
        public static void SetCullFace(CullFaceMode mode)
        {
            GL.CullFace(mode);
        }

        /// <summary>
        /// Set the front face
        /// </summary>
        public static void SetFrontFace(FrontFaceDirection mode)
        {
            GL.FrontFace(mode);
        }

        /// <summary>
        /// Set the polygon mode
        /// </summary>
        public static void SetPolygonMode(MaterialFace face, PolygonMode mode)
        {
            GL.PolygonMode(face, mode);
        }

        /// <summary>
        /// Set the line width
        /// </summary>
        public static void SetLineWidth(float width)
        {
            GL.LineWidth(width);
        }

        /// <summary>
        /// Set the point size
        /// </summary>
        public static void SetPointSize(float size)
        {
            GL.PointSize(size);
        }

        /// <summary>
        /// Set the viewport
        /// </summary>
        public static void SetViewport(int x, int y, int width, int height)
        {
            GL.Viewport(x, y, width, height);
        }

        /// <summary>
        /// Clear the color buffer
        /// </summary>
        public static void ClearColor(float r, float g, float b, float a)
        {
            GL.ClearColor(r, g, b, a);
        }

        /// <summary>
        /// Clear specified buffers
        /// </summary>
        public static void Clear(ClearBufferMask mask)
        {
            GL.Clear(mask);
        }

        /// <summary>
        /// Draw arrays
        /// </summary>
        public static void DrawArrays(PrimitiveType mode, int first, int count)
        {
            GL.DrawArrays(mode, first, count);
        }

        /// <summary>
        /// Draw elements
        /// </summary>
        public static void DrawElements(PrimitiveType mode, int count, DrawElementsType type, int offset)
        {
            GL.DrawElements(mode, count, type, offset);
        }

        /// <summary>
        /// Draw arrays instanced
        /// </summary>
        public static void DrawArraysInstanced(PrimitiveType mode, int first, int count, int instanceCount)
        {
            GL.DrawArraysInstanced(mode, first, count, instanceCount);
        }

        /// <summary>
        /// Draw elements instanced
        /// </summary>
        public static void DrawElementsInstanced(PrimitiveType mode, int count, DrawElementsType type, int offset, int instanceCount)
        {
            GL.DrawElementsInstanced(mode, count, type, offset, instanceCount);
        }

        /// <summary>
        /// Get the vendor name
        /// </summary>
        public static string GetVendor()
        {
            return GL.GetString(StringName.Vendor);
        }

        /// <summary>
        /// Get the renderer name
        /// </summary>
        public static string GetRenderer()
        {
            return GL.GetString(StringName.Renderer);
        }

        /// <summary>
        /// Get the version string
        /// </summary>
        public static string GetVersion()
        {
            return GL.GetString(StringName.Version);
        }

        /// <summary>
        /// Get the GLSL version string
        /// </summary>
        public static string GetShadingLanguageVersion()
        {
            return GL.GetString(StringName.ShadingLanguageVersion);
        }

        /// <summary>
        /// Get a boolean parameter
        /// </summary>
        public static bool GetBoolean(GetPName pname)
        {
            GL.GetBoolean(pname, out bool result);
            return result;
        }

        /// <summary>
        /// Get an integer parameter
        /// </summary>
        public static int GetInteger(GetPName pname)
        {
            GL.GetInteger(pname, out int result);
            return result;
        }

        /// <summary>
        /// Get a float parameter
        /// </summary>
        public static float GetFloat(GetPName pname)
        {
            GL.GetFloat(pname, out float result);
            return result;
        }

        /// <summary>
        /// Get a double parameter
        /// </summary>
        public static double GetDouble(GetPName pname)
        {
            GL.GetDouble(pname, out double result);
            return result;
        }
    }
}
