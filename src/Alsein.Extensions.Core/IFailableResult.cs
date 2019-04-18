namespace Alsein.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFailableResult
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