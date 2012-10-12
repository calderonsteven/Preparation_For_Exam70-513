using System;
using System.ServiceModel;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(Service));
            host.Open();

            Console.WriteLine("ready...");
            Console.ReadLine();
        }
    }
}
