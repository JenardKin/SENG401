using NServiceBus;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messages.NServiceBus.Events
{
    [Serializable]
    public class AsIsEchoEvent : IEvent
    {
        public string data { get; set; }
        public string username { get; set; } = "";
    }
}
