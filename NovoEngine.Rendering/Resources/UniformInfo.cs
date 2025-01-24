namespace OvRenderingCS.Resources
{
    /// <summary>
    /// Information about a uniform variable in a shader
    /// </summary>
    public class UniformInfo
    {
        /// <summary>
        /// Location of the uniform in the shader program
        /// </summary>
        public int Location { get; }

        /// <summary>
        /// Type of the uniform
        /// </summary>
        public UniformType Type { get; }

        /// <summary>
        /// Name of the uniform
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Creates a new uniform info
        /// </summary>
        public UniformInfo(int location, UniformType type, string name)
        {
            Location = location;
            Type = type;
            Name = name;
        }
    }
}
