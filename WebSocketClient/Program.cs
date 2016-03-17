using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("------------------ Use WCF ------------------");

            try
            {
                var context = new InstanceContext(new CallbackHandler());

                using (var client = new ServiceReference1.WebSocketServiceClient(context))
                {
                    client.Hello("Hello World!");

                    Console.WriteLine("Press any key to stop...");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("------------------ Use ClientWebSocket ------------------");

            try
            {
                UseClientWebSocket();

                Console.WriteLine("Press any key to stop...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private async static void UseClientWebSocket()
        {
            using (var ws = new ClientWebSocket())
            {
                await ws.ConnectAsync(new Uri("ws://localhost:8733/WebSocketService_HttpListener/"), CancellationToken.None);

                var i = 0;
                var buffer = new byte[1024];
                while (i < 10)
                {
                    var segment = new ArraySegment<byte>(buffer);

                    var result = await ws.ReceiveAsync(segment, CancellationToken.None);

                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    Console.WriteLine(message);
                    ++i;
                }
            }
        }
    }
}
