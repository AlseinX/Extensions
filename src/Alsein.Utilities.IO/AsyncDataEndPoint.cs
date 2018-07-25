namespace Alsein.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class AsyncDataEndPoint
    {
        /// <summary>
        /// 
        /// </summary>
        public static (IAsyncDataEndPoint, IAsyncDataEndPoint) CreateDuplex()
        {
            var ((sender1, receiver1), (sender2, receiver2)) = (AsyncDataChannel.Create(), AsyncDataChannel.Create());
            return (new DuplexAsyncDataEndPoint(sender1, receiver2), new DuplexAsyncDataEndPoint(sender2, receiver1));
        }

        /// <summary>
        /// 
        /// </summary>
        public static (IAsyncDataSender, IAsyncDataReceiver) CreateSimplex() => AsyncDataChannel.Create();
    }
}