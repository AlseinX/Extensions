namespace Alsein.Utilities.Runtime
{
    public interface IProxyBinder<T> : IProxyBinder
    {
        T GetProxy();
    }
}