using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Runtime.Serialization;
using System.Security;
using System.ServiceModel;
using LeastPrivilege.IdentityModel.Debug;
using LeastPrivilege.IdentityModel.Extensions;

namespace Service
{
    [ServiceContract(Namespace = "urn:msdnmag", ConfigurationName = "Contract")]
    interface IService
    {
        [OperationContract]
        void PlaceOrder(Order order);
    }

    [ServiceBehavior(ConfigurationName = "Service")]
    class Service : IService
    {
        public void PlaceOrder(Order order)
        {
            ClaimsDebug.ShowClaims(ServiceSecurityContext.Current.AuthorizationContext.ClaimSets);

            int purchaseLimit = GetPurchaseLimit();
            if (order.Total > purchaseLimit)
            {
                throw new SecurityException();
            }
        }

        private int GetPurchaseLimit()
        {
            AuthorizationContext context = ServiceSecurityContext.Current.AuthorizationContext;

            Claim claim = context.ClaimSets.FindClaim(
                Constants.PurchaseLimitClaimType,
                Constants.ApplicationIssuerIdentityClaim);

            return claim.Get<int>();
        }
    }

    [DataContract(Namespace = "urn:msdnmag")]
    class Order
    {
        [DataMember]
        public int Total;
    }
}
