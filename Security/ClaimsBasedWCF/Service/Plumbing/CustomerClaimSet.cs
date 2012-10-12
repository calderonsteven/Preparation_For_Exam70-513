using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using LeastPrivilege.IdentityModel.Claims;

namespace Service
{
    class CustomerClaimSet : DeferredLoadClaimSet
    {
        string _customerId;
        ClaimSet _issuer;

        public CustomerClaimSet(string customerId, ClaimSet issuer)
        {
            _customerId = customerId;
            _issuer = issuer;
        }

        protected override void LoadClaims(out ClaimSet issuer, out IList<Claim> claims)
        {
            issuer = _issuer;

            claims = new List<Claim>()
            {
                CreateCustomerClaimId(),
                CreateCustomerClaimProp(),
                CreatePurchaseLimitClaim(),
                CreateLastActivityClaim(),
                CreateStatusClaim(),
            };

        }

        #region Claims Creation
        private Claim CreateLastActivityClaim()
        {
            return new Claim(Constants.LastActivityClaimType, DateTime.Now, Rights.PossessProperty);
        }

        private Claim CreatePurchaseLimitClaim()
        {
            return new Claim(Constants.PurchaseLimitClaimType, 5000, Rights.PossessProperty);
        }

        private Claim CreateCustomerClaimProp()
        {
            return new Claim(Constants.CustomerIdClaimType, _customerId, Rights.PossessProperty);
        }

        private Claim CreateCustomerClaimId()
        {
            return new Claim(Constants.CustomerIdClaimType, _customerId, Rights.Identity);
        }

        private Claim CreateStatusClaim()
        {
            return new Claim(Constants.StatusClaimType, "Gold", Rights.PossessProperty);
        }

        #endregion
    }
}
