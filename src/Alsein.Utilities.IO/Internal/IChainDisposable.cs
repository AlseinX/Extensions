using System;

namespace Alsein.Utilities.IO.Internal
{
    internal interface IChainDisposable : IDisposable
    {
        void DisposeSelf();
    }
}