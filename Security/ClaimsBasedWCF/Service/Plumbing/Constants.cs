using System.IdentityModel.Claims;

namespace Service
{
    internal static class Constants
    {
        public const string CustomerIdClaimType = "http://www.leastprivilege.com/claims/customers/id";
        public const string PurchaseLimitClaimType = "http://www.leastprivilege.com/claims/customers/purchaselimit";
        public const string LastActivityClaimType = "http://www.leastprivilege.com/claims/customers/lastactivity";
        public const string StatusClaimType = "http://www.leastprivilege.com/claims/customers/status";

        public const string ApplicationIssuerName = "MSDN Sample Issuer";

        public static Claim ApplicationIssuerIdentityClaim
        {
            get
            {
                return new Claim(ClaimTypes.System, ApplicationIssuerName, Rights.Identity);
            }
        }
    }
}
