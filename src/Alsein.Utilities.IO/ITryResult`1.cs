namespace Alsein.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITryResult<out TResult> : ITryResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        new TResult Result { get; }
    }
}