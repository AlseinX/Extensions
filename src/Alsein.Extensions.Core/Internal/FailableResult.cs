namespace Alsein.Extensions.RuntimeInternal
{
    internal class FailableResult<TResult> : IFailableResult<TResult>
    {
        public bool IsSuccess { get; }

        public TResult Result { get; }

        object IFailableResult.Result => Result;

        public FailableResult(bool isSuccess, TResult result)
        {
            IsSuccess = isSuccess;
            Result = result;
        }
    }
}