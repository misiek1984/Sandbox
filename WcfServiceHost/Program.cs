using System;
using System.ServiceModel;

using WcfServiceLibrary;

namespace WcfServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(Service)))
            {
                host.Open();
                Console.WriteLine("Press any key to stop...");
                Console.ReadLine();
            }
        }
    }
}
