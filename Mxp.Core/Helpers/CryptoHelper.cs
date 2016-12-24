using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace Mxp.Core.Helpers
{
    public class CryptoHelper
    {
        public static byte[] GetMacAlgorithmHash(IDigest digest, byte[] key, byte[] value)
        {
            HMac hmac = new HMac (digest);
            hmac.Init (new KeyParameter (key));
			byte[] dataBuffer = new byte [hmac.GetMacSize ()];
            hmac.BlockUpdate (value, 0, value.Length);
            hmac.DoFinal (dataBuffer, 0);

            return dataBuffer;
        }

        public static byte[] GetMacAlgorithmHash (IDigest digest, byte[] key, string value) {
            return GetMacAlgorithmHash (digest,key, new UTF8Encoding ().GetBytes (value));
        }
    }
}
