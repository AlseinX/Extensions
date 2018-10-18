namespace Alsein.Utilities.Events
{
    /// <summary>
    /// 
    /// </summary>
    public static class EventPool
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEventPool Create() => new Internal.EventPool();
    }
}