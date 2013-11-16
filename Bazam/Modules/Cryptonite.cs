using System;
using System.Security.Cryptography;
using System.Text;

namespace Bazam.Modules
{
    public static class Cryptonite
    {
        public static string Encrypt(string input, string key)
        {
            return Encrypt(input, key, "esGl$-RSCg^s^6Uj_6ltAQhTrYAyIib9", Encoding.ASCII);
        }

        public static string Decrypt(string input, string key)
        {
            return Decrypt(input, key, "esGl$-RSCg^s^6Uj_6ltAQhTrYAyIib9", Encoding.ASCII);
        }

        public static string Encrypt(string input, string key, string initializationVector, Encoding encoding)
        {
            // convert the input, key, and initialization vector to bytes
            byte[] bInput = encoding.GetBytes(input);
            byte[] bKey = encoding.GetBytes(key);
            byte[] bIV = encoding.GetBytes(initializationVector);

            // initialization of the encryptor from a RijndaelManaged object
            RijndaelManaged rj = new RijndaelManaged();
            rj.BlockSize = 256;
            rj.KeySize = 256;
            rj.Padding = PaddingMode.PKCS7;
            ICryptoTransform crypto = rj.CreateEncryptor(bKey, bIV);

            // encrypt
            byte[] bEncrypted = crypto.TransformFinalBlock(bInput, 0, bInput.Length);

            // return stringified version
            return Convert.ToBase64String(bEncrypted).TrimEnd();
        }

        public static string Decrypt(string input, string key, string initializationVector, Encoding encoding)
        {
            // convert the input, key, and initalization vector to bytes
            byte[] bInput = Convert.FromBase64String(input);
            byte[] bKey = encoding.GetBytes(key);
            byte[] bIV = encoding.GetBytes(initializationVector);

            // initialization of the decryptor from a RijndaelManaged object
            RijndaelManaged rj = new RijndaelManaged();
            rj.BlockSize = 256;
            rj.KeySize = 256;
            rj.Padding = PaddingMode.PKCS7;
            ICryptoTransform crypto = rj.CreateDecryptor(bKey, bIV);

            // decrypt
            byte[] bDecrypted = crypto.TransformFinalBlock(bInput, 0, bInput.Length);

            // return stringified version
            return encoding.GetString(bDecrypted).TrimEnd();
        }
    }
}