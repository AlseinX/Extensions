namespace Alsein.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class FailableResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static IFailableResult<TResult> Create<TResult>(bool isSuccess, TResult result) => new RuntimeInternal.FailableResult<TResult>(isSuccess, result);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isSuccess"></param>
        /// <param name="result"></param>
        public static void Deconstruct<TResult>(this IFailableResult<TResult> obj, out bool isSuccess, out TResult result)
        {
            isSuccess = obj.IsSuccess;
            result = obj.Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryGetValue<TResult>(this IFailableResult<TResult> obj, out TResult result)
        {
            result = obj.Result;
            return obj.IsSuccess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IFailableResult<TResult> Cast<TResult>(this IFailableResult source) =>
            new RuntimeInternal.FailableResult<TResult>(source.IsSuccess, (TResult)source.Result);
    }
}