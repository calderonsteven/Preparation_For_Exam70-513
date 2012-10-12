using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Runtime.Serialization;

namespace LeastPrivilege.IdentityModel.Claims
{
    /// <summary>
    /// A claim set class that loads the claims when they are first accessed
    /// </summary>
    [DataContract(Namespace = "http://schemas.xmlsoap.org/ws/2005/05/identity")]
    [KnownType(typeof(DefaultClaimSet))]
    [KnownType(typeof(ClaimSet))]
    public abstract class DeferredLoadClaimSet : ClaimSet
    {
        [DataMember(Name = "Claims")]
        private IList<Claim> _claims;

        [DataMember(Name = "Issuer")]
        private ClaimSet _issuer;

        /// <summary>
        /// lock for claims lazy loading
        /// </summary>
        private object _loadLock = new object();

        /// <summary>
        /// Loads the claims.
        /// </summary>
        /// <param name="issuer">The issuer.</param>
        /// <param name="claims">The claims.</param>
        protected abstract void LoadClaims(out ClaimSet issuer, out IList<Claim> claims);

        /// <summary>
        /// Forces claims loading.
        /// </summary>
        public void Load()
        {
            EnsureClaims();
        }

        /// <summary>
        /// When overridden in a derived class, gets the number of claims in this claim set.
        /// </summary>
        /// <value></value>
        /// <returns>The number of claims in this <see cref="T:System.IdentityModel.Claims.ClaimSet"/>.</returns>
        public override int Count
        {
            get
            {
                EnsureClaims();
                return _claims.Count;
            }
        }

        /// <summary>
        /// When overridden in a derived class, searches for <see cref="T:System.IdentityModel.Claims.Claim"/> object that matches the specified claim type and rights in the <see cref="T:System.IdentityModel.Claims.ClaimSet"/>.
        /// </summary>
        /// <param name="claimType">The uniform resource identifier (URI) of a claim type. Several claim types are available as static properties of the <see cref="T:System.IdentityModel.Claims.ClaimTypes"/> class.</param>
        /// <param name="right">The URI of the right associated with the new claim. Several rights are available as static properties of the <see cref="T:System.IdentityModel.Claims.Rights"/> class.</param>
        /// <returns>
        /// A<see cref="T:System.Collections.Generic.IEnumerable`1"/> of type <see cref="T:System.IdentityModel.Claims.Claim"/> that enables you to enumerate the claims that matches the specified criteria.
        /// </returns>
        public override IEnumerable<Claim> FindClaims(string claimType, string right)
        {
            EnsureClaims();

            return from claim in _claims
                   where claim.ClaimType.Equals(claimType) &&
                         claim.Right.Equals(right)
                   select claim;
        }

        /// <summary>
        /// When overridden in a derived class, gets a <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to enumerate the <see cref="T:System.IdentityModel.Claims.Claim"/> object in the <see cref="T:System.IdentityModel.Claims.ClaimSet"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to enumerate the <see cref="T:System.IdentityModel.Claims.Claim"/> object in the <see cref="T:System.IdentityModel.Claims.ClaimSet"/>.
        /// </returns>
        public override IEnumerator<Claim> GetEnumerator()
        {
            EnsureClaims();

            return _claims.GetEnumerator();
        }

        /// <summary>
        /// When overridden in a derived class, gets the entity that issued this <see cref="T:System.IdentityModel.Claims.ClaimSet"/>.
        /// </summary>
        /// <value></value>
        /// <returns>The <see cref="T:System.IdentityModel.Claims.ClaimSet"/> object that issued this <see cref="T:System.IdentityModel.Claims.ClaimSet"/> object.</returns>        
        public override ClaimSet Issuer
        {
            get
            {
                EnsureClaims();

                return _issuer;
            }
        }

        /// <summary>
        /// Gets the <see cref="System.IdentityModel.Claims.Claim"/> at the specified index.
        /// </summary>
        /// <value></value>
        public override Claim this[int index]
        {
            get
            {
                EnsureClaims();

                return _claims[index];
            }
        }

        /// <summary>
        /// Calls LoadClaims in the derived class to load the claims.
        /// </summary>
        private void EnsureClaims()
        {
            if (_claims == null)
            {
                lock (_loadLock)
                {
                    if (_claims == null)
                    {
                        ClaimSet issuer;
                        IList<Claim> claims;

                        LoadClaims(out issuer, out claims);

                        _claims = claims;
                        _issuer = issuer;
                    }
                }
            }
        }
    }
}