using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Alsein.Utilities.WebTest.TestModule.Hubs
{
    [Route("/hubs/test")]
    public class TestHub : Hub
    {
        public static void Configure(HttpConnectionDispatcherOptions options)
        {
            ;
        }
    }
}