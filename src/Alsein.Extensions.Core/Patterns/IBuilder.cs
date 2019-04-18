namespace Alsein.Extensions.Patterns
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBuilder<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        T Build();
    }
}