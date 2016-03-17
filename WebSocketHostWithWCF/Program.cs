using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

using WebSockeLib;

namespace WebSocketHostWithWCF
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(WebSocketService)))
            {
                var binding = new CustomBinding();
                binding.Elements.Add(new ByteStreamMessageEncodingBindingElement());
                HttpTransportBindingElement transport = new HttpTransportBindingElement();
                transport.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
                transport.WebSocketSettings.CreateNotificationOnConnection = true;
                binding.Elements.Add(transport);

                host.AddServiceEndpoint(typeof(IWebSocketServiceForJS), binding, "http://localhost:8733/WebSocketService_WCF_ForJS/");

                host.Open();
                Console.WriteLine("WebSocket WCF server started...");
                Console.WriteLine("Press any key to stop...");
                Console.ReadLine();
            }
        }
    }
}
