using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using LeastPrivilege.IdentityModel.Claims;

namespace LeastPrivilege.IdentityModel.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="T:System.IdentityModel.Claims.ClaimSet"/>
    /// </summary>
    public static class ClaimSetExtensions
    {
        /// <summary>
        /// Gets the identity claim from a claim set
        /// </summary>
        /// <param name="set">The set.</param>
        /// <returns>The identity claim</returns>
        public static Claim GetIdentityClaim(this ClaimSet set)
        {
            try
            {
                return set.First(claim =>
                    {
                        return claim.IsIdentity() == true;
                    });
            }
            catch (InvalidOperationException)
            {
                throw new ClaimNotFoundException("Identity Claim for: " + set.ToString());
            }
        }

        /// <summary>
        /// Determines whether the specified set has an issuer.
        /// </summary>
        /// <param name="set">The set</param>
        /// <returns>
        /// 	<c>true</c> if the specified set has an issuer; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasIssuer(this ClaimSet set)
        {
            return !(object.ReferenceEquals(set, set.Issuer));
        }

        /// <summary>
        /// Serializes a claim set using the DataContractSerializer.
        /// </summary>
        /// <param name="set">The claim set to serialize</param>
        /// <returns></returns>
        public static XElement Serialize(this ClaimSet set)
        {
            return set.Serialize(null);
        }

        /// <summary>
        /// Serializes a claim set using the DataContractSerializer.
        /// </summary>
        /// <param name="set">the claim set to serialize</param>
        /// <param name="knownTypes">known serialization types</param>
        /// <returns></returns>
        public static XElement Serialize(this ClaimSet set, IEnumerable<Type> knownTypes)
        {
            // make sure lazy loading has been done before serialization
            var dlset = set as DeferredLoadClaimSet;
            if (dlset != null)
            {
                dlset.Load();
            }

            DataContractSerializer dcs = new DataContractSerializer(
                set.GetType(),
                knownTypes,
                int.MaxValue,
                false,
                true,
                null);
            
            MemoryStream ms = new MemoryStream();
            dcs.WriteObject(ms, set);
            ms.Seek(0, SeekOrigin.Begin);
            
            return XElement.Load(new XmlTextReader(ms));
        }

        public static T Deserialize<T>(XElement xml, IEnumerable<Type> knownTypes)
        {
            DataContractSerializer dcs = new DataContractSerializer(
                typeof(T),
                knownTypes,
                int.MaxValue,
                false,
                true,
                null);

            return (T)dcs.ReadObject(xml.CreateReader());
        }
    }
}
