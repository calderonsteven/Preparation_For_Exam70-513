using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using LeastPrivilege.IdentityModel.Extensions;

namespace Service
{
    class CustomerAuthorizationPolicy : IAuthorizationPolicy
    {
        // id and issuer
        Guid _id = Guid.NewGuid();
        DefaultClaimSet _issuer = new DefaultClaimSet(ClaimSet.System,
            new List<Claim>
            {
                new Claim(ClaimTypes.System, Constants.ApplicationIssuerName, Rights.Identity),
                new Claim(ClaimTypes.System, Constants.ApplicationIssuerName, Rights.PossessProperty),
            });

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            // find identity
            Claim id = evaluationContext.ClaimSets.FindIdentityClaim();
            string userId = Map(id);

            evaluationContext.AddClaimSet(this, new CustomerClaimSet(userId, Issuer));

            return true;
        }

        private string Map(Claim id)
        {
            // inspect claim type and resource here
            // and map to user id
            return "Customer";
        }

        public ClaimSet Issuer
        {
            get { return _issuer; }
        }

        public string Id
        {
            get { return "CustomerAuthorizationPolicy: " + _id.ToString(); }
        }
    }

}
