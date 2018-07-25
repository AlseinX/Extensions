namespace Alsein.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class TryResultExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="isSuccess"></param>
        /// <param name="result"></param>
        public static void Deconstruct<TResult>(this ITryResult<TResult> obj, out bool isSuccess, out TResult result)
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
        public static bool TryGetValue<TResult>(this ITryResult<TResult> obj, out TResult result)
        {
            result = obj.Result;
            return obj.IsSuccess;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static ITryResult<TResult> Cast<TResult>(this ITryResult source) =>
            new TryResult<TResult>(source.IsSuccess, (TResult)source.Result);
    }
}