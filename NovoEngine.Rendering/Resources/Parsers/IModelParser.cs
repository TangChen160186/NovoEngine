namespace NovoEngine.Rendering.Resources.Parsers
{
    /// <summary>
    /// Interface for model parsers
    /// </summary>
    public interface IModelParser
    {
        /// <summary>
        /// Load a model from a file
        /// </summary>
        Model LoadFromFile(string path, ModelParserFlags flags = ModelParserFlags.Default);

        /// <summary>
        /// Gets whether the parser supports the given file format
        /// </summary>
        bool SupportsExtension(string extension);
    }
}
