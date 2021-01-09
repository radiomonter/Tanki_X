namespace Lobby.ClientPayment.Impl
{
    using Org.BouncyCastle.Crypto;
    using Org.BouncyCastle.Crypto.Engines;
    using Org.BouncyCastle.Crypto.Modes;
    using Org.BouncyCastle.Crypto.Parameters;
    using Org.BouncyCastle.Math;
    using Org.BouncyCastle.Security;
    using System;
    using System.Text;

    public class Encrypter
    {
        public const string PREFIX = "adyenc#";
        public const string VERSION = "0_1_15";
        public const string SEPARATOR = "$";
        private string publicKey;
        private CcmBlockCipher aesCipher;
        private IBufferedCipher rsaCipher;

        public Encrypter(string publicKey)
        {
            this.publicKey = publicKey;
            this.InitializeRSA();
        }

        public string Encrypt(string data)
        {
            SecureRandom random = new SecureRandom();
            byte[] buffer = new byte[0x20];
            random.NextBytes(buffer);
            byte[] buffer2 = new byte[12];
            random.NextBytes(buffer2);
            byte[] inArray = this.rsaCipher.DoFinal(buffer);
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            AeadParameters parameters = new AeadParameters(new KeyParameter(buffer), 0x40, buffer2, new byte[0]);
            this.aesCipher = new CcmBlockCipher(new AesFastEngine());
            this.aesCipher.Init(true, parameters);
            byte[] src = new byte[this.aesCipher.GetOutputSize(bytes.Length)];
            this.aesCipher.DoFinal(src, this.aesCipher.ProcessBytes(bytes, 0, bytes.Length, src, 0));
            byte[] dst = new byte[buffer2.Length + src.Length];
            Buffer.BlockCopy(buffer2, 0, dst, 0, buffer2.Length);
            Buffer.BlockCopy(src, 0, dst, buffer2.Length, src.Length);
            return ("adyenc#0_1_15$" + Convert.ToBase64String(inArray) + "$" + Convert.ToBase64String(dst));
        }

        private void InitializeRSA()
        {
            char[] separator = new char[] { '|' };
            string[] strArray = this.publicKey.Split(separator);
            RsaKeyParameters parameters = new RsaKeyParameters(false, new BigInteger(strArray[1].ToLower(), 0x10), new BigInteger(strArray[0].ToLower(), 0x10));
            this.rsaCipher = CipherUtilities.GetCipher("RSA/None/PKCS1Padding");
            this.rsaCipher.Init(true, parameters);
        }
    }
}

