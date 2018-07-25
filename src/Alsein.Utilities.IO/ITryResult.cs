namespace Alsein.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITryResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IsSuccess { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        object Result { get; }
    }
}