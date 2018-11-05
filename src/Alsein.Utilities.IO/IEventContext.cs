using System;
using System.Threading.Tasks;

namespace Alsein.Utilities.IO
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public interface IEventContext<out TEvent>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        TEvent Event { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        IEvents<object> Sender { get; }
    }
}