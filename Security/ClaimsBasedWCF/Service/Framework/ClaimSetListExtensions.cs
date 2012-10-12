using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace LeastPrivilege.IdentityModel.Extensions
{
    public static class ClaimSetListExtensions
    {
        #region ContainsClaim
        public static bool ContainsClaim(this IEnumerable<ClaimSet> claimSets, Claim searchClaim)
        {
            return claimSets.ContainsClaim(searchClaim, null, ClaimSearchMode.Issued, null);
        }

        public static bool ContainsClaim(this IEnumerable<ClaimSet> claimSets, Claim searchClaim, Claim issuerIdentityClaim)
        {
            return claimSets.ContainsClaim(searchClaim, issuerIdentityClaim, ClaimSearchMode.Issued, null);
        }

        public static bool ContainsClaim(this IEnumerable<ClaimSet> claimSets, Claim searchClaim, ClaimSet issuerClaimSet)
        {
            return claimSets.ContainsClaim(searchClaim, issuerClaimSet.GetIdentityClaim(), ClaimSearchMode.Issued, null);
        }

        public static bool ContainsClaim(this IEnumerable<ClaimSet> claimSets, string claimType, object resource, string right)
        {
            Claim searchClaim = new Claim(claimType, resource, right);
            return claimSets.ContainsClaim(searchClaim, null, ClaimSearchMode.Issued, null);
        }

        public static bool ContainsClaim(this IEnumerable<ClaimSet> claimSets, string claimType, object resource, string right, Claim issuerIdentityClaim)
        {
            Claim searchClaim = new Claim(claimType, resource, right);
            return claimSets.ContainsClaim(searchClaim, issuerIdentityClaim, ClaimSearchMode.Issued, null);
        }

        public static bool ContainsClaim(this IEnumerable<ClaimSet> claimSets, string claimType, object resource, string right, ClaimSet issuerClaimSet)
        {
            Claim searchClaim = new Claim(claimType, resource, right);
            return claimSets.ContainsClaim(searchClaim, issuerClaimSet.GetIdentityClaim(), ClaimSearchMode.Issued, null);
        }

        public static bool ContainsClaim(this IEnumerable<ClaimSet> claimSets, Claim searchClaim, Claim issuerIdentityClaim, ClaimSearchMode searchMode, IEqualityComparer<Claim> comparer)
        {
            IEnumerable<ClaimSet> searchSet = 
                GetClaimSetsCore(claimSets, issuerIdentityClaim, searchMode);

            foreach (ClaimSet set in searchSet)
            {
                bool found = false;
                if (comparer != null)
                {
                    found = set.ContainsClaim(searchClaim, comparer);
                }
                else
                {
                    found = set.ContainsClaim(searchClaim);
                }

                if (found)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region FindClaim
        public static Claim FindClaim(this IEnumerable<ClaimSet> claimSets, string claimType)
        {
            return claimSets.FindClaim(claimType, Rights.PossessProperty, null, ClaimSearchMode.Issued);
        }

        public static Claim FindClaim(this IEnumerable<ClaimSet> claimSets, string claimType, ClaimSearchMode searchMode)
        {
            return claimSets.FindClaim(claimType, Rights.PossessProperty, null, searchMode);
        }

        public static Claim FindClaim(this IEnumerable<ClaimSet> claimSets, string claimType, string right)
        {
            return claimSets.FindClaim(claimType, right, null, ClaimSearchMode.Issued);
        }

        public static Claim FindClaim(this IEnumerable<ClaimSet> claimSets, string claimType, Claim issuerIdentityClaim)
        {
            return claimSets.FindClaim(claimType, Rights.PossessProperty, issuerIdentityClaim, ClaimSearchMode.Issued);
        }

        public static Claim FindClaim(this IEnumerable<ClaimSet> claimSets, string claimType, string right, Claim issuerIdentityClaim)
        {
            return claimSets.FindClaim(claimType, right, issuerIdentityClaim, ClaimSearchMode.Issued);
        }

        public static Claim FindClaim(this IEnumerable<ClaimSet> claimSets, string claimType, string right, ClaimSet issuerClaimSet)
        {
            return claimSets.FindClaim(claimType, right, issuerClaimSet.GetIdentityClaim(), ClaimSearchMode.Issued);
        }

        public static Claim FindClaim(this IEnumerable<ClaimSet> claimSets, string claimType, ClaimSet issuerClaimSet)
        {
            return claimSets.FindClaim(claimType, Rights.PossessProperty, issuerClaimSet.GetIdentityClaim(), ClaimSearchMode.Issued);
        }

        public static Claim FindClaim(this IEnumerable<ClaimSet> claimSets, string claimType, string right, Claim issuerIdentityClaim, ClaimSearchMode searchMode)
        {
            foreach (Claim claim in claimSets.FindClaims(claimType, right, issuerIdentityClaim, searchMode))
            {
                return claim;
            }

            throw new ClaimNotFoundException(claimType);
        }
        #endregion

        #region FindClaims
        public static IEnumerable<Claim> FindClaims(this IEnumerable<ClaimSet> claimSets, string claimType)
        {
            return claimSets.FindClaims(claimType, Rights.PossessProperty, null, ClaimSearchMode.Issued);
        }

        public static IEnumerable<Claim> FindClaims(this IEnumerable<ClaimSet> claimSets, string claimType, string right)
        {
            return claimSets.FindClaims(claimType, right, null, ClaimSearchMode.Issued);
        }

        public static IEnumerable<Claim> FindClaims(this IEnumerable<ClaimSet> claimSets, string claimType, Claim issuerIdentityClaim)
        {
            return claimSets.FindClaims(claimType, Rights.PossessProperty, issuerIdentityClaim, ClaimSearchMode.Issued);
        }

        public static IEnumerable<Claim> FindClaims(this IEnumerable<ClaimSet> claimSets, string claimType, string right, Claim issuerIdentityClaim)
        {
            return claimSets.FindClaims(claimType, right, issuerIdentityClaim, ClaimSearchMode.Issued);
        }

        public static IEnumerable<Claim> FindClaims(this IEnumerable<ClaimSet> claimSets, string claimType, ClaimSet issuerClaimSet)
        {
            return claimSets.FindClaims(claimType, Rights.PossessProperty, issuerClaimSet.GetIdentityClaim(), ClaimSearchMode.Issued);
        }

        public static IEnumerable<Claim> FindClaims(this IEnumerable<ClaimSet> claimSets, string claimType, string right, ClaimSet issuerClaimSet)
        {
            return claimSets.FindClaims(claimType, right, issuerClaimSet.GetIdentityClaim(), ClaimSearchMode.Issued);
        }

        public static IEnumerable<Claim> FindClaims(this IEnumerable<ClaimSet> claimSets, string claimType, string right, Claim issuerIdentityClaim, ClaimSearchMode searchMode)
        {
            bool found = false;

            foreach (ClaimSet set in GetClaimSetsCore(claimSets, issuerIdentityClaim, searchMode))
            {
                foreach (Claim claim in set.FindClaims(claimType, right))
                {
                    yield return claim;
                    found = true;
                }
            }

            if (!found)
            {
                throw new ClaimNotFoundException(claimType);
            }
        }
        #endregion

        #region IdentityClaim
        public static Claim FindIdentityClaim(this IEnumerable<ClaimSet> claimSets)
        {
            return claimSets.FindIdentityClaim((Claim)null);
        }
        
        public static Claim FindIdentityClaim(this IEnumerable<ClaimSet> claimSets, ClaimSet issuerClaimSet)
        {
            return claimSets.FindIdentityClaim(issuerClaimSet.GetIdentityClaim());
        }

        public static Claim FindIdentityClaim(this IEnumerable<ClaimSet> claimSets, Claim issuerIdentityClaim)
        {
            foreach (ClaimSet set in GetClaimSetsCore(claimSets, issuerIdentityClaim, ClaimSearchMode.Issued))
            {
                return set.GetIdentityClaim();
            }

            throw new ClaimNotFoundException();
        }
        #endregion

        #region Get
        public static IEnumerable<ClaimSet> GetIssuerClaimSets(this IEnumerable<ClaimSet> claimSets)
        {
            foreach (ClaimSet set in claimSets)
            {
                yield return set.Issuer;
            }
        }

        public static IEnumerable<ClaimSet> GetClaimSetsByIssuer(this IEnumerable<ClaimSet> claimSets, ClaimSet issuerClaimSet)
        {
            if (issuerClaimSet == null)
            {
                throw new ArgumentNullException("issuerClaimSet");
            }

            return claimSets.GetClaimSetsByIssuer(issuerClaimSet.GetIdentityClaim());
        }

        public static IEnumerable<ClaimSet> GetClaimSetsByIssuer(this IEnumerable<ClaimSet> claimSets, Claim issuerIdentityClaim)
        {
            if (issuerIdentityClaim == null)
            {
                foreach (ClaimSet set in claimSets)
                {
                    yield return set;
                }
            }
            else
            {
                foreach (ClaimSet set in claimSets)
                {
                    if (set.Issuer.ContainsClaim(issuerIdentityClaim))
                    {
                        yield return set;
                    }
                }
            }
        }

        #endregion

        #region Serialization
        /// <summary>
        /// Serializes a list of claim sets
        /// </summary>
        /// <param name="claimSets">The claim sets to serialize</param>
        /// <param name="knownTypes">known serialization types</param>
        /// <returns></returns>
        public static XElement Serialize(this IEnumerable<ClaimSet> claimSets, IEnumerable<Type> knownTypes)
        {
            return new XElement("ClaimSets",
                            from cs in claimSets
                            select cs.Serialize(knownTypes));
        }

        public static XElement Serialize(this IEnumerable<ClaimSet> claimSets, string rootName, string rootNamespace, IEnumerable<Type> knownTypes)
        {
            XNamespace ns = XNamespace.Get(rootNamespace);

            return new XElement(ns + rootName,
                            from cs in claimSets
                            select cs.Serialize(knownTypes));
        }
        #endregion

        #region Specialized
        public static RSACryptoServiceProvider FindIssuerRsaClaim(this IEnumerable<ClaimSet> claimSets)
        {
            return claimSets.FindClaim(
                ClaimTypes.Rsa,
                ClaimSearchMode.Issuer).Get<RSACryptoServiceProvider>();
        }
        #endregion

        #region Core
        private static IEnumerable<ClaimSet> GetClaimSetsCore(IEnumerable<ClaimSet> claimSets, Claim issuerIdentityClaim, ClaimSearchMode searchMode)
        {
            IEnumerable<ClaimSet> searchSet;
            if (searchMode == ClaimSearchMode.Issued)
            {
                searchSet = claimSets.GetClaimSetsByIssuer(issuerIdentityClaim);
            }
            else if (searchMode == ClaimSearchMode.Issuer)
            {
                searchSet = claimSets.GetIssuerClaimSets();
            }
            else
            {
                throw new ArgumentOutOfRangeException("searchMode");
            }

            return searchSet;
        }
        #endregion    
    }
}


public enum ClaimSearchMode
{
    Issued,
    Issuer
}
