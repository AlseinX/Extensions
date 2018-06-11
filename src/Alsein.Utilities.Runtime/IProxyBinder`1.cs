namespace Alsein.Utilities.Runtime
{
    public interface IProxyBinder<T> : IProxyBinder
    {
        new T GetProxy();
    }
}