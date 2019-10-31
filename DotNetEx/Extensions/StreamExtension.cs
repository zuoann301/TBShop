using System.IO;
using System.Text;

namespace System.IO
{
    /// <summary>
    /// IOExtensions
    /// </summary>
    public static class StreamExtension
    {
        /// <summary>
        /// 向 stream 写入字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="s"></param>
        public static void WriteString(this Stream stream, string s)
        {
            stream.WriteString(s, Encoding.UTF8);
        }
        /// <summary>
        /// 向 stream 写入字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="s"></param>
        /// <param name="encoding"></param>
        public static void WriteString(this Stream stream, string s, Encoding encoding)
        {
            byte[] bytes = s.ToBytes(encoding);
            stream.Write(bytes, 0, bytes.Length);
        }

    }
}
