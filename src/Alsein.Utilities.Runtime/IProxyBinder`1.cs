namespace Alsein.Utilities.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IProxyBinder<T> : IProxyBinder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        new T GetProxy();
    }
}