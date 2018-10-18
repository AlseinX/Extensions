using System;

namespace Alsein.Utilities.Events.Internal
{
    internal interface IChainDisposable : IDisposable
    {
        void DisposeSelf();
    }
}