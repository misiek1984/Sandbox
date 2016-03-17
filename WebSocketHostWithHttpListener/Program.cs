using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketHostWithHttpListener
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("WebSocket HttpListener server started...");

            var l = new HttpListener();
            l.Prefixes.Add("http://localhost:8733/WebSocketService_HttpListener/");

            l.Start();

            while (true)
            {
                var ctx = l.GetContext();

                Console.WriteLine("New connection");

                if (!ctx.Request.IsWebSocketRequest)
                {
                    Console.WriteLine("Not a web socket request");
                    continue;
                }

                Handle(ctx);
            }
        }

        private async static void Handle(HttpListenerContext ctx)
        {
            Console.WriteLine($"START {nameof(Handle)}");

            try
            {
                var wsc = await ctx.AcceptWebSocketAsync(null);
                var ws = wsc.WebSocket;

                var i = 0;
                while (ws.State == System.Net.WebSockets.WebSocketState.Open)
                {
                    var bytes = UTF8Encoding.UTF8.GetBytes(i++.ToString());
                    var buffer = new ArraySegment<byte>(bytes);
                    await ws.SendAsync(buffer, System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);

                    await Task.Delay(1000);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine($"END {nameof(Handle)}");
        }
    }
}
