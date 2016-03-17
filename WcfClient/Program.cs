using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfClient
{
    class Program
    {
        static void Main(string[] args)
        {
            TestService1();
            TestService2();
        }

        private static void TestService2()
        {
            using (ServiceReference2.ServiceClient client = new ServiceReference2.ServiceClient())
            {
                var a = client.GetData(1);
                var b = client.GetDataAsync(2).Result;
                var c = client.GetDataUsingDataContract(new ServiceReference2.CompositeType() {BoolValue = true, StringValue= "aaa"});
                var d = client.GetDataUsingDataContractAsync(new ServiceReference2.CompositeType() {BoolValue = true, StringValue = "aaa" }).Result;
            }
        }

        private static void TestService1()
        {
            using (ServiceReference1.ServiceClient client = new ServiceReference1.ServiceClient())
            {
                var a = client.GetData(1);
                var b = client.GetDataAsync(2).Result;
                var c = client.GetDataUsingDataContract(new ServiceReference1.CompositeType() {BoolValue = true, StringValue = "aaa" });
                var d = client.GetDataUsingDataContractAsync(new ServiceReference1.CompositeType() {BoolValue = true, StringValue = "aaa" }).Result;
            }
        }
    }
}
