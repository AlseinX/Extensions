using System;
using System.Linq;

namespace Alsein.Utilities
{
    public static partial class FunctionalExtensions
    {
        private class UsingPromise
        {
            private Delegate Evaluater { get; }

            private UsingPromise Parent { get; }

            protected virtual object Argument => Parent.Argument;

            protected UsingPromise(Delegate evaluater, UsingPromise parent)
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

        private class UsingPromise<TResult> : UsingPromise, IUsingPromise<TResult>
        {
            public UsingPromise(Delegate evaluater, UsingPromise parent) : base(evaluater, parent) { }

            IUsingPromise<TNew> IUsingPromise<TResult>.Using<TNew>(Func<TResult, TNew> func) => new UsingPromise<TNew>(func, this);

            TResult IUsingPromise<TResult>.Return() => (TResult)Return();
        }

        private class TopUsingPromise<TResult> : UsingPromise<TResult>
        {
            public TopUsingPromise(Delegate evaluater, object arguement) : base(evaluater, null) => Argument = arguement;
            protected override object Argument { get; }
        }
    }
}