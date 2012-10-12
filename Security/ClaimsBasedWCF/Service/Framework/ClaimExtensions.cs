using System;
using System.IdentityModel.Claims;
using System.IdentityModel.Tokens;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace LeastPrivilege.IdentityModel.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="T:System.IdentityModel.Claims.Claim"/>
    /// </summary>
    public static class ClaimExtensions
    {
        /// <summary>
        /// Determines whether the specified claim is an identity claim.
        /// </summary>
        /// <param name="claim">The claim.</param>
        /// <returns>
        /// 	<c>true</c> if the specified claim is an identity claim; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIdentity(this Claim claim)
        {
            return claim.Right.Equals(Rights.Identity, System.StringComparison.Ordinal);
        }

        /// <summary>
        /// Casts the claim resource to a specified type.
        /// </summary>
        /// <typeparam name="T">Type to cast to</typeparam>
        /// <param name="claim">The claim.</param>
        /// <returns></returns>
        public static T Get<T>(this Claim claim)
        {
            return (T)claim.Resource;
        }

        /// <summary>
        /// Tries to casts the claim resource to a specified type.
        /// </summary>
        /// <typeparam name="T">Type to cast to</typeparam>
        /// <param name="claim">The claim.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// 	<c>true</c> if the cast was successful; otherwise, <c>false</c>.
        /// </returns>
        public static bool TryGet<T>(this Claim claim, out T value)
        {
            try
            {
                value = (T)claim.Resource;
                return true;
            }
            catch
            {
                value = default(T);
                return false;
            }
        }

        public static string Stringify(this Claim claim)
        {
            SecurityIdentifier sid = claim.Resource as SecurityIdentifier;
            if (sid != null)
            {
                try
                {
                    return ((SecurityIdentifier)claim.Resource).Translate(typeof(NTAccount)).Value;
                }
                catch
                {
                    return sid.Value;
                }
            }

            RSACryptoServiceProvider rsa = claim.Resource as RSACryptoServiceProvider;
            if (rsa != null)
            {
                return ((RSACryptoServiceProvider)claim.Resource).GetKeyHashString();
            }

            MailAddress mail = claim.Resource as MailAddress;
            if (null != mail)
            {
                return mail.ToString();
            }

            byte[] bufferValue = claim.Resource as byte[];
            if (null != bufferValue)
            {
                return Convert.ToBase64String(bufferValue);
            }

            X500DistinguishedName x500 = claim.Resource as X500DistinguishedName;
            if (x500 != null)
            {
                return x500.Name;
            }

            SamlNameIdentifierClaimResource samlNameId = claim.Resource as SamlNameIdentifierClaimResource;
            if (samlNameId != null)
            {
                return samlNameId.Name;
            }

            return claim.Resource.ToString();
        }
    }
}
