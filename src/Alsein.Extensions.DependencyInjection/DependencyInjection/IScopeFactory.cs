namespace Alsein.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IScopeFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IScope CreateChildScope();
    }
}