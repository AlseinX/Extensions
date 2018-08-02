using System.Threading.Tasks;

namespace Alsein.Utilities.IO
{
    /// <summary>
    /// 
    /// </summary>
    public static class AsyncDataEndPoint
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static Task<object> ReceiveAsync(this IAsyncDataEndPoint endPoint) => endPoint.ReceiveAsync<object>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<TData>> RequestAsync<TData>(this IAsyncDataEndPoint endPoint, object data)
        {
            await endPoint.SendAsync(data);
            return await endPoint.ReceiveAsync<TData>();
        }
    }
}