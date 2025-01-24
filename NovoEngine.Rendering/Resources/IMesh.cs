namespace OvRenderingCS.Resources
{
    /// <summary>
    /// Interface for mesh resources
    /// </summary>
    public interface IMesh
    {
        /// <summary>
        /// Gets whether the mesh has indices
        /// </summary>
        bool HasIndices { get; }

        /// <summary>
        /// Gets the number of indices
        /// </summary>
        int IndexCount { get; }

        /// <summary>
        /// Gets the number of vertices
        /// </summary>
        int VertexCount { get; }

        /// <summary>
        /// Bind the mesh for rendering
        /// </summary>
        void Bind();
    }
}
