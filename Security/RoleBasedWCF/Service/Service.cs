using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Security.Permissions;
using System.Threading;
using System.Security;
using System.Web.Security;

namespace Service
{
    [ServiceContract(Namespace = "urn:msdnmag", ConfigurationName = "Contract")]
    interface IService
    {
        [OperationContract]
        string[] GetRoles(string username);
    }

    // don't put a PrincipalPermission on the class itself (or its ctor) 
    // Thread.CurrentPrincipal gets populated after object creation
    [ServiceBehavior(ConfigurationName = "Service")]
    class Service : IService
    {
        // only "users" role member can call this method
        [PrincipalPermission(SecurityAction.Demand, Role = "users")]
        public string[] GetRoles(string username)
        {
            if (ServiceSecurityContext.Current.PrimaryIdentity.Name == username)
            {
                return CustomPrincipal.Current.Roles;
            }
            else
            {
                return Roles.GetRolesForUser(username);
            }

            //// inline authorization

            //// only administrators can retrieve the role information for other users
            //if (ServiceSecurityContext.Current.PrimaryIdentity.Name != username)
            //{
            //    if (Thread.CurrentPrincipal.IsInRole("administrators"))
            //    {
            //        // return roles for given user
            //        return Roles.GetRolesForUser(username);
            //    }
            //    else
            //    {
            //        // access denied
            //        throw new SecurityException();
            //    }
            //}

            //// return roles for current user
            //return CustomPrincipal.Current.Roles;
        }
    }
}
