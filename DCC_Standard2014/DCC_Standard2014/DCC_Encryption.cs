using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DCC_Standard
{
    /// <summary>
    /// Provides RSA services
    /// </summary>
    public class DCC_Encryption
    {
        //public static string[] xx()
        //{
        //    RSACryptoServiceProvider csp = new RSACryptoServiceProvider(1024);
        //    string s1 = csp.ToXmlString(false);
        //    string s2 = csp.ToXmlString(true);
        //    return new string[] {s1,s2};
        //}

        /// <summary>
        /// Encrypts given inputString using RSA params from the xmlString and keySize provided
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="keySize"></param>
        /// <param name="xml_String"></param>
        /// <returns></returns>
        public static string Encrypt(string inputString, int keySize, string xml_String)
        {
            RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(keySize);
            rsaCryptoServiceProvider.FromXmlString(xml_String);
            int newKeySize = keySize / 8;
            byte[] bytes = Encoding.UTF32.GetBytes(inputString);
            int maxLength = newKeySize - 42;
            int dataLength = bytes.Length;
            int iterations = dataLength / maxLength;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i <= iterations; i++)
            {
                byte[] tempBytes = new byte[(dataLength - maxLength * i > maxLength) ? maxLength : dataLength - maxLength * i];
                Buffer.BlockCopy(bytes, maxLength * i, tempBytes, 0, tempBytes.Length);
                byte[] encryptedBytes = rsaCryptoServiceProvider.Encrypt(tempBytes, true);
                stringBuilder.Append(Convert.ToBase64String(encryptedBytes));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Decrypts given inputString using RSA params from the xmlString and keySize provided
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="keySize"></param>
        /// <param name="xml_String"></param>
        /// <returns></returns>
        public static string Decrypt(string inputString, int keySize, string xml_String)
        {
            RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(keySize);
            rsaCryptoServiceProvider.FromXmlString(xml_String);
            int base64BlockSize = ((keySize / 8) % 3 != 0) ? (((keySize / 8) / 3) * 4) + 4 : ((keySize / 8) / 3) * 4;
            int iterations = inputString.Length / base64BlockSize;
            ArrayList arrayList = new ArrayList();
            for (int i = 0; i < iterations; i++)
            {
                byte[] encryptedBytes = Convert.FromBase64String(inputString.Substring(base64BlockSize * i, base64BlockSize));
                arrayList.AddRange(rsaCryptoServiceProvider.Decrypt(encryptedBytes, true));
            }
            return Encoding.UTF32.GetString(arrayList.ToArray(Type.GetType("System.Byte")) as byte[]);
        }

        /// http://www.obviex.com/samples/Encryption.aspx
        /// <summary>
        /// This class uses a symmetric key algorithm (Rijndael/AES) to encrypt and
        /// decrypt data. As long as encryption and decryption routines use the same
        /// parameters to generate the keys, the keys are guaranteed to be the same.
        /// The class uses static functions with duplicate code to make it easier to
        /// demonstrate encryption and decryption logic. In a real-life application,
        /// this may not be the most efficient way of handling encryption, so - as
        /// soon as you feel comfortable with it - you may want to redesign this class.
        /// </summary>
        public class RijndaelSimple
        {
            /// <summary>
            /// Encrypts specified plaintext using Rijndael symmetric key algorithm
            /// and returns a base64-encoded result.
            /// </summary>
            /// <param name="plainText">
            /// Plaintext value to be encrypted.
            /// </param>
            /// <param name="passPhrase">
            /// Passphrase from which a pseudo-random password will be derived. The
            /// derived password will be used to generate the encryption key.
            /// Passphrase can be any string. In this example we assume that this
            /// passphrase is an ASCII string.
            /// </param>
            /// <param name="saltValue">
            /// Salt value used along with passphrase to generate password. Salt can
            /// be any string. In this example we assume that salt is an ASCII string.
            /// </param>
            /// <param name="hashAlgorithm">
            /// Hash algorithm used to generate password. Allowed values are: "MD5" and
            /// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
            /// </param>
            /// <param name="passwordIterations">
            /// Number of iterations used to generate password. One or two iterations
            /// should be enough.
            /// </param>
            /// <param name="initVector">
            /// Initialization vector (or IV). This value is required to encrypt the
            /// first block of plaintext data. For RijndaelManaged class IV must be
            /// exactly 16 ASCII characters long.
            /// </param>
            /// <param name="keySize">
            /// Size of encryption key in bits. Allowed values are: 128, 192, and 256.
            /// Longer keys are more secure than shorter keys.
            /// </param>
            /// <returns>
            /// Encrypted value formatted as a base64-encoded string.
            /// </returns>
            public static string Encrypt(string plainText,
                                         string passPhrase,
                                         string saltValue,
                                         string hashAlgorithm,
                                         int passwordIterations,
                                         string initVector,
                                         int keySize)
            {
                // Convert strings into byte arrays.
                // Let us assume that strings only contain ASCII codes.
                // If strings include Unicode characters, use Unicode, UTF7, or UTF8
                // encoding.
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

                // Convert our plaintext into a byte array.
                // Let us assume that plaintext contains UTF8-encoded characters.
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                // First, we must create a password, from which the key will be derived.
                // This password will be generated from the specified passphrase and
                // salt value. The password will be created using the specified hash
                // algorithm. Password creation can be done in several iterations.
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase,
                                                                       saltValueBytes,
                                                                       hashAlgorithm,
                                                                       passwordIterations);

                // Use the password to generate pseudo-random bytes for the encryption
                // key. Specify the size of the key in bytes (instead of bits).
                byte[] keyBytes = password.GetBytes(keySize / 8);

                // Create uninitialized Rijndael encryption object.
                RijndaelManaged symmetricKey = new RijndaelManaged();

                // It is reasonable to set encryption mode to Cipher Block Chaining
                // (CBC). Use default options for other symmetric key parameters.
                symmetricKey.Mode = CipherMode.CBC;

                // Generate encryptor from the existing key bytes and initialization
                // vector. Key size will be defined based on the number of the key
                // bytes.
                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

                // Define memory stream which will be used to hold encrypted data.
                MemoryStream memoryStream = new MemoryStream();

                // Define cryptographic stream (always use Write mode for encryption).
                CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                // Start encrypting.
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);

                // Finish encrypting.
                cryptoStream.FlushFinalBlock();

                // Convert our encrypted data from a memory stream into a byte array.
                byte[] cipherTextBytes = memoryStream.ToArray();

                // Close both streams.
                memoryStream.Close();
                cryptoStream.Close();

                // Convert encrypted data into a base64-encoded string.
                string cipherText = Convert.ToBase64String(cipherTextBytes);

                // Return encrypted string.
                return cipherText;
            }

            /// <summary>
            /// Decrypts specified ciphertext using Rijndael symmetric key algorithm.
            /// </summary>
            /// <param name="cipherText">
            /// Base64-formatted ciphertext value.
            /// </param>
            /// <param name="passPhrase">
            /// Passphrase from which a pseudo-random password will be derived. The
            /// derived password will be used to generate the encryption key.
            /// Passphrase can be any string. In this example we assume that this
            /// passphrase is an ASCII string.
            /// </param>
            /// <param name="saltValue">
            /// Salt value used along with passphrase to generate password. Salt can
            /// be any string. In this example we assume that salt is an ASCII string.
            /// </param>
            /// <param name="hashAlgorithm">
            /// Hash algorithm used to generate password. Allowed values are: "MD5" and
            /// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
            /// </param>
            /// <param name="passwordIterations">
            /// Number of iterations used to generate password. One or two iterations
            /// should be enough.
            /// </param>
            /// <param name="initVector">
            /// Initialization vector (or IV). This value is required to encrypt the
            /// first block of plaintext data. For RijndaelManaged class IV must be
            /// exactly 16 ASCII characters long.
            /// </param>
            /// <param name="keySize">
            /// Size of encryption key in bits. Allowed values are: 128, 192, and 256.
            /// Longer keys are more secure than shorter keys.
            /// </param>
            /// <returns>
            /// Decrypted string value.
            /// </returns>
            /// <remarks>
            /// Most of the logic in this function is similar to the Encrypt
            /// logic. In order for decryption to work, all parameters of this function
            /// - except cipherText value - must match the corresponding parameters of
            /// the Encrypt function which was called to generate the
            /// ciphertext.
            /// </remarks>
            public static string Decrypt(string cipherText,
                                         string passPhrase,
                                         string saltValue,
                                         string hashAlgorithm,
                                         int passwordIterations,
                                         string initVector,
                                         int keySize)
            {
                if (cipherText == null) return "";
                if (cipherText.Length == 0) return "";

                // Convert strings defining encryption key characteristics into byte
                // arrays. Let us assume that strings only contain ASCII codes.
                // If strings include Unicode characters, use Unicode, UTF7, or UTF8
                // encoding.
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

                // Convert our ciphertext into a byte array.
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

                // First, we must create a password, from which the key will be
                // derived. This password will be generated from the specified
                // passphrase and salt value. The password will be created using
                // the specified hash algorithm. Password creation can be done in
                // several iterations.
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase,
                                                                       saltValueBytes,
                                                                       hashAlgorithm,
                                                                       passwordIterations);

                // Use the password to generate pseudo-random bytes for the encryption
                // key. Specify the size of the key in bytes (instead of bits).
                byte[] keyBytes = password.GetBytes(keySize / 8);

                // Create uninitialized Rijndael encryption object.
                RijndaelManaged symmetricKey = new RijndaelManaged();

                // It is reasonable to set encryption mode to Cipher Block Chaining
                // (CBC). Use default options for other symmetric key parameters.
                symmetricKey.Mode = CipherMode.CBC;

                // Generate decryptor from the existing key bytes and initialization
                // vector. Key size will be defined based on the number of the key
                // bytes.
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

                // Define memory stream which will be used to hold encrypted data.
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

                // Define cryptographic stream (always use Read mode for encryption).
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                // Since at this point we don't know what the size of decrypted data
                // will be, allocate the buffer long enough to hold ciphertext;
                // plaintext is never longer than ciphertext.
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                // Start decrypting.
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                // Close both streams.
                memoryStream.Close();
                cryptoStream.Close();

                // Convert decrypted data into a string.
                // Let us assume that the original plaintext string was UTF8-encoded.
                string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

                // Return decrypted string.
                return plainText;
            }
        }

    }

    public static class DataField
    {

        public static string Encrypt(string original, string saltValue)
        {
            if (original == null) return null;
            if (original.Length == 0) return "";
            return DCC_Encryption.RijndaelSimple.Encrypt(original,
                                                     passPhrase,
                                                     saltValue,
                                                     hashAlgorithm,
                                                     passwordIterations,
                                                     initVector,
                                                     keySize);
        }

        public static string Decrypt(string cipherText, string saltValue)
        {
            if (cipherText == null) return null;
            if (cipherText.Length == 0) return "";
            return DCC_Encryption.RijndaelSimple.Decrypt(cipherText,
                                                     passPhrase,
                                                     saltValue,
                                                     hashAlgorithm,
                                                     passwordIterations,
                                                     initVector,
                                                     keySize);
        }

        // DO NOT CHANGE THESE WHILST *ANY* DATABASE CONTAINS ENCRYPTED DATA //              **** VERY IMPORTANT ****
        static string passPhrase = "Durham";           // can be any string
        static string hashAlgorithm = "SHA1";          // can be "MD5"
        static int passwordIterations = 2;             // can be any number
        static string initVector = "senrab-b1953-yar"; // must be 16 bytes
        static int keySize = 256;                      // can also be 192 or 128
    }


    public static class Password
    {
        //create constant strings for each type of characters
        static string alphaCaps = "QWERTYUIOPASDFGHJKLZXCVBNM";
        static string alphaLow  = "qwertyuiopasdfghjklzxcvbnm";
        static string numerics  = "1234567890";
        static string special   = "!#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
        //create another string which is a concatenation of all above
        static string allChars = alphaCaps + alphaLow + numerics + special;

        public static string GeneratePassword(int length)
        {
            Random r = new Random();
            char[] generatedPassword = new char[length];
            for (int i = 0; i < length; i++)
            {
                double rand = r.NextDouble();
                if (i == 0)
                {
                    //First character is an upper case alphabet
                    generatedPassword[0] = alphaCaps[(int)Math.Floor(rand * alphaCaps.Length)];
                }
                else
                {
                    generatedPassword[i] = allChars[(int)Math.Floor(rand * allChars.Length)];
                }
            }
            return new string(generatedPassword);
        }
    }
}
