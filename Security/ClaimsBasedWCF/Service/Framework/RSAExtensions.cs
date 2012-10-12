using System;
using System.Security.Cryptography;
using System.Text;

namespace LeastPrivilege.IdentityModel.Extensions
{
    public static class RSAExtensions
    {
        public static byte[] GetKeyHash(this RSACryptoServiceProvider rsa)
        {
            return rsa.GetKeyHash(string.Empty);
        }

        public static byte[] GetKeyHash(this RSACryptoServiceProvider rsa, string entropy)
        {
            int entropyLength = Encoding.UTF8.GetByteCount(entropy);
            RSAParameters rsaParams = rsa.ExportParameters(false);
            byte[] shaInput;
            byte[] shaOutput;

            int i = 0;
            shaInput = new byte[rsaParams.Modulus.Length + rsaParams.Exponent.Length + entropyLength];
            rsaParams.Modulus.CopyTo(shaInput, i);
            i += rsaParams.Modulus.Length;
            rsaParams.Exponent.CopyTo(shaInput, i);
            i += rsaParams.Exponent.Length;
            i += Encoding.UTF8.GetBytes(entropy, 0, entropy.Length, shaInput, i);

            using (SHA256 sha = SHA256.Create())
            {
                shaOutput = sha.ComputeHash(shaInput);
            }

            return shaOutput;
        }
    
        public static string GetKeyHashString(this RSACryptoServiceProvider rsa)
        {
            return Convert.ToBase64String(rsa.GetKeyHash(string.Empty));
        }

        public static string GetKeyHashString(this RSACryptoServiceProvider rsa, string entropy)
        {
            return Convert.ToBase64String(rsa.GetKeyHash(entropy));
        }
    }
}
