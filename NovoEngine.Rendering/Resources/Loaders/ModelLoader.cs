using OvRenderingCS.Resources.Parsers;

namespace OvRenderingCS.Resources.Loaders
{
    /// <summary>
    /// Utility class for loading models from files
    /// </summary>
    public static class ModelLoader
    {
        private static readonly List<IModelParser> _parsers = new()
        {
            new AssimpParser()
        };

        /// <summary>
        /// Load a model from a file using an appropriate parser
        /// </summary>
        public static Model LoadFromFile(string path, ModelParserFlags flags = ModelParserFlags.Default)
        {
            string extension = Path.GetExtension(path);

            foreach (var parser in _parsers)
            {
                if (parser.SupportsExtension(extension))
                {
                    return parser.LoadFromFile(path, flags);
                }
            }

            throw new Exception($"No parser found for file extension: {extension}");
        }

        /// <summary>
        /// Register a new model parser
        /// </summary>
        public static void RegisterParser(IModelParser parser)
        {
            _parsers.Add(parser);
        }
    }
}
