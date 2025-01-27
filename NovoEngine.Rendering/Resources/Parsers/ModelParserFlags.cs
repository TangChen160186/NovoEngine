namespace NovoEngine.Rendering.Resources.Parsers
{
    /// <summary>
    /// Flags for model parsing options
    /// </summary>
    [Flags]
    public enum ModelParserFlags
    {
        None = 0,

        /// <summary>
        /// Calculate tangent space (tangent and bitangent vectors)
        /// </summary>
        CalculateTangentSpace = 1 << 0,

        /// <summary>
        /// Join identical vertices to reduce memory usage
        /// </summary>
        JoinIdenticalVertices = 1 << 1,

        /// <summary>
        /// Make left-handed coordinate system right-handed
        /// </summary>
        MakeLeftHanded = 1 << 2,

        /// <summary>
        /// Triangulate polygons with more than 3 vertices
        /// </summary>
        Triangulate = 1 << 3,

        /// <summary>
        /// Remove redundant components like normals or texture coordinates
        /// </summary>
        RemoveRedundantComponents = 1 << 4,

        /// <summary>
        /// Generate smooth normals if the model has no normals
        /// </summary>
        GenerateSmoothNormals = 1 << 5,

        /// <summary>
        /// Generate normals if the model has no normals
        /// </summary>
        GenerateNormals = 1 << 6,

        /// <summary>
        /// Fix invalid normals
        /// </summary>
        FixInvalidNormals = 1 << 7,

        /// <summary>
        /// Optimize meshes
        /// </summary>
        OptimizeMeshes = 1 << 8,

        /// <summary>
        /// Optimize graph
        /// </summary>
        OptimizeGraph = 1 << 9,

        /// <summary>
        /// Flip UVs vertically
        /// </summary>
        FlipUVs = 1 << 10,

        /// <summary>
        /// Calculate bounding boxes
        /// </summary>
        CalculateBoundingBoxes = 1 << 11,

        /// <summary>
        /// Default flags for most use cases
        /// </summary>
        Default = CalculateTangentSpace | JoinIdenticalVertices | Triangulate |
                 GenerateNormals | FixInvalidNormals | OptimizeMeshes
    }
}
