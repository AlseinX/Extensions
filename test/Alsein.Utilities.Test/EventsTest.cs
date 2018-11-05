using Xunit;
using System;
using Alsein.Utilities;
using Alsein.Utilities.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

public class EventsTest
{
    [Fact]
    public async Task MainTest()
    {
        var pool = EventPool.Create();
        var (o1, o2, o3, o4, o5, o6, o7, o8) =
        (new object(), new object[1], new object[1, 1], new object[1][],
        new object[1, 1][], new string[1], new string[1, 1], new string[1][]);
        var (e1, e2, e3, e4, e5, e6, e7, e8) =
        (pool.GetEvents(o1), pool.GetEvents(o2), pool.GetEvents(o3), pool.GetEvents(o4),
        pool.GetEvents(o5), pool.GetEvents(o6), pool.GetEvents(o7), pool.GetEvents(o8));

        e2.SetParent(e1);
        e3.SetParent(e1);

        e4.SetParent(e2);
        e5.SetParent(e2);
        e6.SetParent(e3);
        e7.SetParent(e3);

        var log = new List<string>();

        void handle(IEventContext<string> context)
        {
            log.Add(context.Sender.GetType() + "");
        }

        e1.AddEventHandler<string>(handle);
        e2.AddEventHandler<string>(handle);
        e3.AddEventHandler<string>(handle);
        e4.AddEventHandler<string>(handle);
        e5.AddEventHandler<string>(handle);
        e6.AddEventHandler<string>(handle);
        e7.AddEventHandler<string>(handle);

        async void fire(EventDiffusionOptions options)
        {
            await Task.Delay(200);
            e2.FireEvent("aaaa", options);
        }

        fire(EventDiffusionOptions.Popup);

        var c = await e1.NextEventAsync<string>();

        fire(EventDiffusionOptions.RecurseDown);

        var d = await e4.NextEventAsync<object>();

        await Task.Delay(200);

    }

}
