using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ace.Security
{
    public static class EncryptHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="key">长度必须是 16</param>
        /// <returns></returns>
        public static string AESEncrypt(string text, string key)
        {
            byte[] encryptKey = Encoding.UTF8.GetBytes(key);

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = encryptKey;
                aesAlg.IV = encryptKey;
                using (var encryptor = aesAlg.CreateEncryptor())
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(text);
                            }

                            var iv = aesAlg.IV;

                            var decryptedContent = msEncrypt.ToArray();

                            var result = new byte[iv.Length + decryptedContent.Length];

                            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                            Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                            return Convert.ToBase64String(result);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <param name="key">长度必须是 16</param>
        /// <returns></returns>
        public static string AESDecrypt(string encryptedText, string key)
        {
            var fullCipher = Convert.FromBase64String(encryptedText);

            byte[] iv = new byte[16];

            var cipher = new byte[fullCipher.Length - iv.Length];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length);
            var decryptKey = Encoding.UTF8.GetBytes(key);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(decryptKey, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }
    }
}
