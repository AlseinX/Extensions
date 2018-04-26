using System;

namespace Alsein.Utilities
{
    public interface IUsingQuotation<out TResult>
    {
        IUsingQuotation<TNew> Using<TNew>(Func<TResult, TNew> func);
        TResult Return();
    }
}