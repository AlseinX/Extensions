namespace Alsein.Extensions.DependencyInjection
{
    /// <summary>
    /// 
    /// </summary>
    public static class Container
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IContainer Create() => new Internal.Container();
    }
}