namespace Alsein.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFailableResult<out TResult> : IFailableResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        new TResult Result { get; }
    }
}