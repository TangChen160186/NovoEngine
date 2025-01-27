namespace NovoEngine.Rendering.Resources.Loaders
{
    /// <summary>
    /// Utility class for loading shaders from files
    /// </summary>
    public static class ShaderLoader
    {
        /// <summary>
        /// Create a shader program from vertex and fragment shader source files
        /// </summary>
        public static Shader CreateFromFiles(string vertexPath, string fragmentPath)
        {
            string vertexSource = File.ReadAllText(vertexPath);
            string fragmentSource = File.ReadAllText(fragmentPath);

            return CreateFromSource(vertexSource, fragmentSource);
        }

        /// <summary>
        /// Create a shader program from vertex and fragment shader sources
        /// </summary>
        public static Shader CreateFromSource(string vertexSource, string fragmentSource)
        {
            var shader = new Shader();

            try
            {
                shader.AttachShader(vertexSource, ShaderType.VertexShader);
                shader.AttachShader(fragmentSource, ShaderType.FragmentShader);
                shader.Link();
            }
            catch
            {
                shader.Dispose();
                throw;
            }

            return shader;
        }

        /// <summary>
        /// Create a compute shader program from a compute shader source file
        /// </summary>
        public static Shader CreateComputeFromFile(string computePath)
        {
            string computeSource = File.ReadAllText(computePath);
            return CreateComputeFromSource(computeSource);
        }

        /// <summary>
        /// Create a compute shader program from a compute shader source
        /// </summary>
        public static Shader CreateComputeFromSource(string computeSource)
        {
            var shader = new Shader();

            try
            {
                shader.AttachShader(computeSource, ShaderType.ComputeShader);
                shader.Link();
            }
            catch
            {
                shader.Dispose();
                throw;
            }

            return shader;
        }
    }
}
