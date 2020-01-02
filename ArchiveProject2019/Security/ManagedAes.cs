using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace ArchiveProject2019.Security
{
    public class ManagedAes
    {

        public static bool IsSaveInDb = false;
        public static bool IsCipher = false;


        // Encryption Key
        // private static string password = "wtk2019companypasswordwtkwtk2019companypasswordwtk";
        private static string password = "wtk2019companypassword";

        public static byte[] DecodeUrlBase64(string s)
        {
            //s = s.Replace('-', '+').Replace('_', '/').PadRight(4 * ((s.Length + 3) / 4), '=');
            var arrayOfString = s.Split(',');
            return Convert.FromBase64String(arrayOfString[1]);
        }



        /*
         * Core Encryption Code Start
         */
        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }
        /*
         * Core Encryption Code End
         */


        /*
         * Encrypting String Start
         */
        public static string EncryptText(string input)
        {
            if (input == null || input == "")
                return "";
            // Get the bytes of the string
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            string result = Convert.ToBase64String(bytesEncrypted);

            return result;
        }

        public static string DecryptText(string input)
        {
            if (input == null || input == "")
                return "";
            // Get the bytes of the string
            byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            string result = Encoding.UTF8.GetString(bytesDecrypted);

            return result;
        }
        /*
         * Encrypting String End
         */


        /*
         * Encrypting Array Byte Start
         */
        public static byte[] EncryptArrayByte(byte[] bytesToBeEncrypted)
        {
            if (bytesToBeEncrypted == null)
                return null;
            // Get the bytes of the string
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            return bytesEncrypted;
        }

        public static byte[] DecryptArrayByte(byte[] bytesToBeDecrypted)
        {
            if (bytesToBeDecrypted == null)
                return null;
            // Get the bytes of the string
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            return bytesDecrypted;
        }
        /*
         * Encrypting Array Byte End
         */


        /*
         * Encrypting File Start
         */
        public static void EncryptFile(HttpPostedFileBase file, string path)
        {
            if (file != null && path != "")
            {
                byte[] bytesToBeEncrypted = null;

                using (Stream inputStream = file.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    bytesToBeEncrypted = memoryStream.ToArray();
                }

                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Hash the password with SHA256
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

                File.WriteAllBytes(path, bytesEncrypted);
            }
        }
        
        public static void EncryptFile(byte[] bytesToBeEncrypted, string path)
        {
            if (path != "")
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Hash the password with SHA256
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

                File.WriteAllBytes(path, bytesEncrypted);
            }
        }

        public static byte[] DecryptFile(string path)
        {
            byte[] bytesDecrypted = null;
            if (path != "")
            {
                byte[] bytesToBeDecrypted = null;
                var file = new FileStream(path, FileMode.Open, FileAccess.Read);

                using (Stream inputStream = file)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    bytesToBeDecrypted = memoryStream.ToArray();
                }

                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

                //File.WriteAllBytes(path, bytesDecrypted);
            }
                return bytesDecrypted;
        }

        public static void DecryptFileInSaveFile(string path)
        {
            if (path != "")
            {
                byte[] bytesToBeDecrypted = null;
                var file = new FileStream(path, FileMode.Open, FileAccess.Read);

                using (Stream inputStream = file)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    bytesToBeDecrypted = memoryStream.ToArray();
                }

                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

                File.WriteAllBytes(path, bytesDecrypted);
            }
        }

        /*
        * Encrypting File End
        */
    }
}