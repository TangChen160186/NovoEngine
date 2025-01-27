namespace NovoEngine.Rendering.Settings
{
    /// <summary>
    /// Defines how a buffer will be used
    /// </summary>
    public enum EBufferUsage
    {
        /// <summary>
        /// The data store contents will be modified once and used at most a few times
        /// </summary>
        StreamDraw,

        /// <summary>
        /// The data store contents will be modified once and used many times
        /// </summary>
        StaticDraw,

        /// <summary>
        /// The data store contents will be modified repeatedly and used many times
        /// </summary>
        DynamicDraw
    }
}
