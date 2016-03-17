using System;
using System.Net.WebSockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using WebSockeLib;

namespace WebSocketHostWithWCF
{
    /// <summary>
    /// http://stackoverflow.com/questions/24239953/wcf-self-hosted-websocket-service-with-javascript-client
    /// </summary>
    public class WebSocketService : IWebSocketService, IWebSocketServiceForJS
    {
        public async Task Hello(Message m)
        {
            Console.WriteLine($"START {nameof(Hello)}");

            try
            {
                byte[] body = m.GetBody<byte[]>();
                string msgTextFromClient = Encoding.UTF8.GetString(body);

                Console.WriteLine("Message from a client: " + msgTextFromClient);

                var callback = OperationContext.Current.GetCallbackChannel<IWebSocketCallbackForJS>();

                var i = 0;
                while (((IChannel)callback).State == CommunicationState.Opened)
                {
                    await callback.Send(CreateMessage(i++));
                    await Task.Delay(1000);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine($"END {nameof(Hello)}");
        }

        public async Task Hello(string name)
        {
            Console.WriteLine($"START {nameof(Hello)}");

            try
            {
                Console.WriteLine("Message from a client: " + name);

                var callback = OperationContext.Current.GetCallbackChannel<IWebSocketCallback>();

                var i = 0;
                while (((IChannel)callback).State == CommunicationState.Opened)
                {
                    await callback.Send(i++);
                    await Task.Delay(1000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine($"END {nameof(Hello)}");
        }

        private Message CreateMessage(int i)
        {
            var msg = ByteStreamMessage.CreateMessage(new ArraySegment<byte>(Encoding.UTF8.GetBytes(i.ToString())));

            msg.Properties["WebSocketMessageProperty"] =
                new WebSocketMessageProperty
                {
                    MessageType = WebSocketMessageType.Text
                };

            return msg;
        }
    }
}