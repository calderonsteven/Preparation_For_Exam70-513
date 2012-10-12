using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // for debugging purposes
            1.Delay();

            var proxy = new ChannelFactory<IService>("*").CreateChannel();

            try
            {
                Console.WriteLine("Total: 4000");
                proxy.PlaceOrder(new Order() { Total = 4000 });
                Console.WriteLine("OK");

                Console.WriteLine("Total: 6000");
                proxy.PlaceOrder(new Order() { Total = 6000 });
                Console.WriteLine("OK");
            }
            catch (SecurityAccessDeniedException)
            {
                Console.WriteLine("Access Denied");
            }
        }
    }

    [ServiceContract(Namespace = "urn:msdnmag", ConfigurationName = "Contract")]
    interface IService
    {
        [OperationContract]
        void PlaceOrder(Order order);
    }

    [DataContract(Namespace = "urn:msdnmag")]
    class Order
    {
        [DataMember]
        public int Total;
    }


    internal static class SillyHelper
    {
        public static void Delay(this int value)
        {
            System.Threading.Thread.Sleep(value);
        }
    }
}