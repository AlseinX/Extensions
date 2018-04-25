using System;

namespace Alsein.Utilities
{
    public interface IUsingPromise<out TResult>
    {
        IUsingPromise<TNew> Using<TNew>(Func<TResult, TNew> func);
        TResult Return();
    }
}