using Microsoft.Extensions.DependencyInjection;

namespace Alsein.Extensions.Patterns
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TIService"></typeparam>
    /// <typeparam name="TService"></typeparam>
    /// <typeparam name="TOptions"></typeparam>
    public abstract class ServiceBuilder<TIService, TService, TOptions> : IServiceBuilder<TIService, TOptions>
        where TIService : class
        where TService : class, TIService
        where TOptions : class
    {
        /// <summary>
        /// 
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        protected ServiceBuilder(IServiceCollection services = null)
        {
            Services = services ?? new ServiceCollection();
            Services.AddSingleton<TIService, TService>();
        }
    }
}
