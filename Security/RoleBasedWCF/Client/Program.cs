using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1 - try to get roles as user 'bob'
            // allowed only for own roles
            var factory1 = new ChannelFactory<IService>("*");
            factory1.Credentials.UserName.UserName = "bob";
            factory1.Credentials.UserName.Password = "bob";

            var proxy = factory1.CreateChannel();

            try
            {
                Console.WriteLine("\nBob: roles for Bob --");
                proxy.GetRoles("bob").ToList().ForEach(i => Console.WriteLine(i));

                Console.WriteLine("\nBob: roles for Alice --");
                proxy.GetRoles("alice").ToList().ForEach(i => Console.WriteLine(i));
            }
            catch (SecurityAccessDeniedException)
            {
                Console.WriteLine("Access Denied\n");
            }

            // 2 - try to get roles as administrator
            // works for every account
            var factory2 = new ChannelFactory<IService>("*");
            factory2.Credentials.UserName.UserName = "administrator";
            factory2.Credentials.UserName.Password = "administrator";

            proxy = factory2.CreateChannel();

            try
            {
                Console.WriteLine("\nAdmin: roles for Admin --");
                proxy.GetRoles("administrator").ToList().ForEach(i => Console.WriteLine(i));

                Console.WriteLine("\nAdmin: roles for Alice --");
                proxy.GetRoles("alice").ToList().ForEach(i => Console.WriteLine(i));
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
        string[] GetRoles(string username);
    }
}
