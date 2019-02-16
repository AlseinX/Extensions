using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alsein.Utilities.Patterns
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TIService"></typeparam>
    /// <typeparam name="TOptions"></typeparam>
    public interface IServiceBuilder<TIService, TOptions>
        where TIService : class
        where TOptions : class
    {
        /// <summary>
        /// 
        /// </summary>
        IServiceCollection Services { get; }
    }
}
