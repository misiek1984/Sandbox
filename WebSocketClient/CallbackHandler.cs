using System;
using System.Threading.Tasks;
using WebSockeLib;
using WebSocketClient.ServiceReference1;

namespace WebSocketClient
{
    public class CallbackHandler : IWebSocketCallback, ServiceReference1.IWebSocketServiceCallback
    {
        async Task IWebSocketCallback.Send(int i)
        {
            Console.WriteLine(i);
        }

        void IWebSocketServiceCallback.Send(int i)
        {
            Console.WriteLine(i);
        }
    }
}
