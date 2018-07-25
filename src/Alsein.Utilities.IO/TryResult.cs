namespace Alsein.Utilities
{
    internal class TryResult<TResult> : ITryResult<TResult>
    {
        public bool IsSuccess { get; }

        public TResult Result { get; }

        object ITryResult.Result => Result;

        public TryResult(bool isSuccess, TResult result)
        {
            IsSuccess = isSuccess;
            Result = result;
        }
    }
}