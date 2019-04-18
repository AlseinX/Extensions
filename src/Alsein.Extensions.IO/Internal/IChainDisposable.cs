using System;

namespace Alsein.Extensions.IO.Internal
{
    internal interface IChainDisposable : IDisposable
    {
        void DisposeSelf();
    }
}