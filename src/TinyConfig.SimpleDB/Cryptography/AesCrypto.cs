using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TinyConfig.SimpleDB.Cryptography
{
    /// <summary>
    /// AES helper
    /// </summary>
    public class AesCrypto
    {
        private readonly byte[] _passwordBytes;
        private const int SALT_SIZE = 16;

        /// <summary>
        /// creates a new <see cref="AesCrypto"/>
        /// </summary>
        /// <param name="password"></param>
        public AesCrypto(string password)
        {
            _passwordBytes = Encoding.UTF8.GetBytes(password);
        }

        #region helper methods
        private static byte[] GenerateSalt()
        {
            // initialize a byte array to hold salt
            var data = new byte[SALT_SIZE];
            // create an instance of random number generator
            using var rng = new RNGCryptoServiceProvider();
            // generate random numbers and fill salt with the generated value
            for (int i = 0; i < 10; i++) rng.GetBytes(data);
            // return filled byte array
            return data;
        }

        private RijndaelManaged CreateAes(byte[] salt)
        {
            const int keySize = 256;
            const int blockSize = 128;
            const int ITERATIONS = 200;
            // create key from password and salt
            var key = new Rfc2898DeriveBytes(_passwordBytes, salt, ITERATIONS);

            // create an managed aes instance
            return new RijndaelManaged()
            {
                KeySize = keySize,
                BlockSize = blockSize,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                Key = key.GetBytes(keySize / 8),
                IV = key.GetBytes(blockSize / 8)
            };
        }
        #endregion

        #region Encrypt
        /// <summary>
        /// encrypt <paramref name="data"/> and return encrypted string
        /// </summary>
        /// <param name="data"></param>
        public string Encrypt(string data)
        {
            // generate salt, unique per call
            var salt = GenerateSalt();

            // create an managed aes instance
            var aes = CreateAes(salt);

            // write salt to output file
            using var resultStream = new MemoryStream();
            resultStream.Write(salt, 0, salt.Length);

            // create a crypto stream in write mode with outputfile and configured aes instance
            using(var cryptoStream = new CryptoStream(resultStream, aes.CreateEncryptor(), CryptoStreamMode.Write, true))
            {
                using var swEncrypt = new StreamWriter(cryptoStream, Encoding.UTF8);
                //Write all data to the stream.
                swEncrypt.Write(data);
            }

            resultStream.Position = 0;
            return Convert.ToBase64String(resultStream.ToArray());
        }
        #endregion

        #region Decrypt
        /// <summary>
        /// decrypt <paramref name="data"/> and return decrypted string
        /// </summary>
        public string Decrypt(string data)
        {
            var plainText = string.Empty;
            var salt = new byte[SALT_SIZE];
            using var inputStream = new MemoryStream(Convert.FromBase64String(data));
            inputStream.Read(salt, 0, salt.Length);
            // create an managed aes instance
            var aes = CreateAes(salt);

            // create a crypto stream in read mode with input file and configured aes instance
            using(var cryptoStream = new CryptoStream(inputStream, aes.CreateDecryptor(), CryptoStreamMode.Read, true))
            {
                using var srDecrypt = new StreamReader(cryptoStream);
                // Read the decrypted bytes from the decrypting stream
                // and place them in a string.
                plainText = srDecrypt.ReadToEnd();
            }
            return plainText;
        }
        #endregion
    }
}
