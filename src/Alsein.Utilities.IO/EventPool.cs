namespace Alsein.Utilities.IO
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