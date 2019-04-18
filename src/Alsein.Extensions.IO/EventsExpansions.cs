namespace Alsein.Extensions.IO
{
    /// <summary>
    /// 
    /// </summary>
    public static class EventsExpansions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="events"></param>
        /// <param name="parent"></param>
        /// <typeparam name="TTarget"></typeparam>
        /// <returns></returns>
        public static IEvents<TTarget> SetParent<TTarget>(this IEvents<TTarget> events, object parent)
        where TTarget : class
        {
            if (parent is IEvents<object> _parent)
            {
                parent = _parent.Target;
            }
            return events.SetParent(() => parent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="events"></param>
        /// <param name="eventObject"></param>
        /// <typeparam name="TTarget"></typeparam>
        /// <typeparam name="TEvent"></typeparam>
        /// <returns></returns>
        public static IEvents<TTarget> FireEvent<TTarget, TEvent>(this IEvents<TTarget> events, TEvent eventObject)
        where TTarget : class
        where TEvent : class =>
            events.FireEvent(eventObject, EventDiffusionOptions.None);


    }
}