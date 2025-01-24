namespace OvRenderingCS.Settings
{
    /// <summary>
    /// Defines operations that can be performed on stencil buffer
    /// </summary>
    public enum Operation
    {
        Keep,
        Zero,
        Replace,
        Increment,
        IncrementWrap,
        Decrement,
        DecrementWrap,
        Invert
    }
}
