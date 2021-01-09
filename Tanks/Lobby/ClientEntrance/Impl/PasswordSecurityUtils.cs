namespace Tanks.Lobby.ClientEntrance.Impl
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class PasswordSecurityUtils
    {
        private static SHA256Managed DIGEST = new SHA256Managed();

        private static byte[] ConcatenateArrays(byte[] a, byte[] b)
        {
            byte[] array = new byte[a.Length + b.Length];
            a.CopyTo(array, 0);
            b.CopyTo(array, a.Length);
            return array;
        }

        public static byte[] GetDigest(string data) => 
            DIGEST.ComputeHash(Encoding.UTF8.GetBytes(data));

        public static string GetDigestAsString(string data) => 
            Convert.ToBase64String(DIGEST.ComputeHash(Encoding.UTF8.GetBytes(data)));

        public static string GetDigestAsString(byte[] data) => 
            Convert.ToBase64String(DIGEST.ComputeHash(data));

        public static byte[] RSAEncrypt(string publicKeyBase64, byte[] data)
        {
            using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
            {
                char[] separator = new char[] { ':' };
                string[] strArray = publicKeyBase64.Split(separator, 2);
                string[] textArray1 = new string[] { "<RSAKeyValue><Modulus>", strArray[0], "</Modulus><Exponent>", strArray[1], "</Exponent></RSAKeyValue>" };
                provider.FromXmlString(string.Concat(textArray1));
                return provider.Encrypt(data, false);
            }
        }

        public static string RSAEncryptAsString(string publicKeyBase64, byte[] data) => 
            Convert.ToBase64String(RSAEncrypt(publicKeyBase64, data));

        public static string SaltPassword(string passcode, string password)
        {
            byte[] digest = GetDigest(password);
            return GetDigestAsString(ConcatenateArrays(XorArrays(digest, Convert.FromBase64String(passcode)), digest));
        }

        private static byte[] XorArrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
            {
                throw new ArgumentException();
            }
            byte[] buffer = new byte[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                buffer[i] = (byte) (a[i] ^ b[i]);
            }
            return buffer;
        }
    }
}

