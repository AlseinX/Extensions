using System;
using System.Linq;

namespace Alsein.Utilities
{
    public static partial class FunctionalExtensions
    {
        private class UsingQuotation
        {
            private Delegate Evaluater { get; }

            private UsingQuotation Parent { get; }

            protected virtual object Argument => Parent.Argument;

            protected UsingQuotation(Delegate evaluater, UsingQuotation parent)
            {
                Evaluater = evaluater;
                Parent = parent;
            }

            protected object Return()
            {
                var data = Argument;
                var e = this.Recurse(x => x.Parent).Reverse().Select(x => x.Evaluater).GetEnumerator();
                void Do()
                {
                    if (e.MoveNext())
                    {
                        switch (data = e.Current.DynamicInvoke(data))
                        {
                            case IDisposable disposable:
                                using (disposable)
                                {
                                    Do();
                                }
                                break;
                            default:
                                Do();
                                break;
                        }
                    }
                }
                Do();
                return data;
            }
        }

        private class UsingQuotation<TResult> : UsingQuotation, IUsingQuotation<TResult>
        {
            public UsingQuotation(Delegate evaluater, UsingQuotation parent) : base(evaluater, parent) { }

            IUsingQuotation<TNew> IUsingQuotation<TResult>.Using<TNew>(Func<TResult, TNew> func) => new UsingQuotation<TNew>(func, this);

            TResult IUsingQuotation<TResult>.Return() => (TResult)Return();
        }

        private class TopUsingQuotation<TResult> : UsingQuotation<TResult>
        {
            public TopUsingQuotation(Delegate evaluater, object arguement) : base(evaluater, null) => Argument = arguement;
            protected override object Argument { get; }
        }
    }
}