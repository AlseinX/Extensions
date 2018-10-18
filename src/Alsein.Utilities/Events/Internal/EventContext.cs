using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alsein.Utilities.Events.Internal
{
    internal class EventContext<TEvent> : IEventContext<TEvent>
    where TEvent : class
    {
        public TEvent Event { get; }

        public IEvents<object> Sender { get; }

        public EventContext(TEvent eventObject, IEvents<object> sender)
        {
            Event = eventObject;
            Sender = sender;
        }
    }
}