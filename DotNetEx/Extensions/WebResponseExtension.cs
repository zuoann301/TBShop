using System.IO;
using System.Net;
using System.Text;

namespace System.Net
{
    /// <summary>
    /// NetExtensions
    /// </summary>
    public static class WebResponseExtension
    {
        /// <summary>
        /// 获取数据流内容
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string GetResponseString(this HttpWebResponse response)
        {
            string result = string.Empty;
            StreamReader sr = null;
            if (!string.IsNullOrEmpty(response.CharacterSet))
            {
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                sr = new StreamReader(response.GetResponseStream(), responseEncoding);
            }
            else
                sr = new StreamReader(response.GetResponseStream());
            using (sr)
            {
                result = sr.ReadToEnd();
            }
            return result;
        }

        /// <summary>
        /// 获取数据流内容
        /// </summary>
        /// <param name="response"></param>
        /// <param name="encoding">要使用的字符编码</param>
        /// <returns></returns>
        public static string GetResponseString(this HttpWebResponse response, Encoding encoding)
        {
            string result = string.Empty;
            StreamReader sr = null;

            sr = new StreamReader(response.GetResponseStream(), encoding);

            using (sr)
            {
                result = sr.ReadToEnd();
            }
            return result;
        }
    }
}
