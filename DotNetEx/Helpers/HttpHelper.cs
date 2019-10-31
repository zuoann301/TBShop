using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Text;

namespace DotNet.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class HttpHelper
    {
        /// <summary>
        /// 以 GET 方式发送 Http 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="naked"></param>
        /// <returns></returns>
        public static string Get(string url, bool naked = false)
        {
            return Get(url, 100 * 1000, naked);
        }
        /// <summary>
        /// 以 GET 方式发送 Http 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeout"></param>
        /// <param name="naked"></param>
        /// <returns></returns>
        public static string Get(string url, int timeout, bool naked = false)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            string result = null;

            request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.AllowAutoRedirect = false;
            request.Timeout = timeout;
            request.Method = "GET";

            if (naked == false)
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/38.0.2125.111 Safari/537.36";

            response = (HttpWebResponse)request.GetResponse();

            using (response)
            {
                result = response.GetResponseString();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="timeout"></param>
        /// <param name="naked"></param>
        /// <returns></returns>
        public static string Post(string url, Dictionary<string, object> parameters, int timeout = 100000, bool naked = false)
        {
            StringBuilder postData = new StringBuilder();
            string c = "";
            if (parameters != null)
            {
                foreach (var kv in parameters)
                {
                    string key = kv.Key;
                    object value = parameters[key];
                    postData.AppendFormat("{0}{1}={2}", c, key, value);

                    c = "&";
                }
            }

            string s = postData.ToString();

            string result = null;
            result = Post(url, s, timeout, naked);

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestBody"></param>
        /// <param name="timeout"></param>
        /// <param name="naked"></param>
        /// <returns></returns>
        public static string Post(string url, string requestBody, int timeout = 100000, bool naked = false)
        {
            string result = null;

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";

            if (naked == false)
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/38.0.2125.111 Safari/537.36";

            if (requestBody != null)
            {
                byte[] data = Encoding.UTF8.GetBytes(requestBody);
                Stream postStream = request.GetRequestStream();
                postStream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (response)
            {
                result = response.GetResponseString();
            }

            return result;
        }
    }
}
