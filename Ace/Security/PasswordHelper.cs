using Ace.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Security
{
    public static class PasswordHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="password">明文密码</param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public static string Encrypt(string password, string secretKey)
        {
            string ret = EncryptMD5Password(password.ToMD5(), secretKey);
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="md5Password">经过md5加密的密码</param>
        /// <param name="secretKey"></param>
        /// <returns></returns>
        public static string EncryptMD5Password(string md5Password, string secretKey)
        {
            secretKey = secretKey.ToMD5().Substring(0, 16);
            string encryptedPassword = EncryptHelper.AESEncrypt(md5Password.ToLower(), secretKey).ToLower();
            string ret = encryptedPassword.ToMD5().ToLower();
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pwdText">明文密码</param>
        public static void EnsurePasswordLegal(string pwdText)
        {
            if (pwdText == null || pwdText.Length < 6 || pwdText.Length > 15)
                throw new InvalidInputException("密码必须是6-15位");
        }

        public static string Md5String(string inputValue)
        {
            string str = string.Empty;
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(inputValue));
                var strResult = BitConverter.ToString(result);
                str= strResult.Replace("-", "").ToLower();
            }
            return str;
        }

    }
}
