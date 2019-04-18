using System.Threading.Tasks;
using Alsein.Extensions.IO.Internal;

namespace Alsein.Extensions.IO
{
    /// <summary>
    /// 
    /// </summary>
    public static class Tube
    {
        /// <summary>
        /// 
        /// </summary>
        public static (ITubeEndPoint, ITubeEndPoint) CreateDuplex()
        {
            var ((sender1, receiver1), (sender2, receiver2)) = (SimplexTube.Create(), SimplexTube.Create());
            return (new TubeEndPoint(sender1, receiver2), new TubeEndPoint(sender2, receiver1));
        }

        /// <summary>
        /// 
        /// </summary>
        public static (ITubeInlet, ITubeOutlet) CreateSimplex() => SimplexTube.Create();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public static Task<object> ReceiveAsync(this ITubeEndPoint endPoint) => endPoint.ReceiveAsync<object>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="endPoint"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<TData> RequestAsync<TData>(this ITubeEndPoint endPoint, object data)
        {
            await endPoint.SendAsync(data);
            return await endPoint.ReceiveAsync<TData>();
        }
    }
}