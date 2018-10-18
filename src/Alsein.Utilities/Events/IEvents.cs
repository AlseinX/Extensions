using System;
using System.Threading.Tasks;

namespace Alsein.Utilities.Events
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    public interface IEvents<out TTarget> : IDisposable
    where TTarget : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventObject"></param>
        /// <param name="options"></param>
        /// <typeparam name="TEvent"></typeparam>
        IEvents<TTarget> FireEvent<TEvent>(TEvent eventObject, EventDiffusionOptions options)
        where TEvent : class;

        /// <summary>
        /// 
        /// </summary>
        IEvents<TTarget> Flush();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        IEvents<TTarget> Flush<TEvent>()
        where TEvent : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <typeparam name="TEvent"></typeparam>
        IEvents<TTarget> AddEventHandler<TEvent>(Action<IEventContext<TEvent>> action)
        where TEvent : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <typeparam name="TEvent"></typeparam>
        IEvents<TTarget> RemoveEventHandler<TEvent>(Action<IEventContext<TEvent>> action)
        where TEvent : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        bool HasEventHandler<TEvent>(Action<IEventContext<TEvent>> action)
        where TEvent : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        Task<IEventContext<TEvent>> NextEventAsync<TEvent>()
        where TEvent : class;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        IEvents<object> Parent { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>

        IEvents<TTarget> SetParent<TParent>(Func<TParent> selector)
        where TParent : class;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        TTarget Target { get; }
    }
}