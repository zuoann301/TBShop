using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace System.Xml.Linq
{
    /// <summary>
    /// XmlExtensions
    /// </summary>
    public static class XmlExtensions
    {
        /// <summary>
        /// 获取节点 value，element 为 null 则 返回 null
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static string GetValue(this XElement element)
        {
            if (element == null)
                return null;
            return element.Value;
        }


        public static XDeclaration SetDeclaration(this XDocument doc, string version = "1.0", string encoding = "utf-8", string standalone = null)
        {
            if (doc.Declaration != null)
            {
                doc.Declaration.Version = version;
                doc.Declaration.Encoding = encoding;
                doc.Declaration.Standalone = standalone;
            }
            else
            {
                XDeclaration xd = new XDeclaration(version, encoding, standalone);
                doc.Declaration = xd;
            }

            return doc.Declaration;
        }
        public static XElement AddNode(this XContainer parentNode, string nodeName)
        {
            XElement node = new XElement(nodeName);
            parentNode.Add(node);
            return node;
        }
        public static XElement AddNode(this XContainer parentNode, string nodeName, object content)
        {
            XElement node = new XElement(nodeName, content);
            parentNode.Add(node);
            return node;
        }
        public static XElement AddNode(this XContainer parentNode, string nodeName, params object[] content)
        {
            XElement node = new XElement(nodeName, content);
            parentNode.Add(node);
            return node;
        }

        public static XElement AddTextNode(this XContainer parentNode, string nodeName, object value)
        {
            string val = string.Empty;
            if (value != null)
                val = value.ToString();
            XElement node = parentNode.AddTextNode(nodeName, val);
            return node;
        }
        public static XElement AddTextNode(this XContainer parentNode, string nodeName, string value)
        {
            string val = ReplaceInvalidChar(value);
            XElement node = new XElement(nodeName);
            node.Value = val;
            parentNode.Add(node);
            return node;
        }
        public static XElement AddTextNode(this XContainer parentNode, string nodeName, int value)
        {
            return parentNode.AddTextNode(nodeName, value.ToString());
        }

        public static XElement AddCDataNode(this XContainer parentNode, string nodeName, object value)
        {
            string val = string.Empty;
            if (value != null)
                val = value.ToString();
            XElement node = parentNode.AddCDataNode(nodeName, val);
            return node;
        }
        public static XElement AddCDataNode(this XContainer parentNode, string nodeName, string value)
        {
            XElement node = new XElement(nodeName);
            XCData cDataNode = new XCData(ReplaceInvalidChar(value));
            node.Add(cDataNode);
            parentNode.Add(node);
            return node;
        }

        static string ReplaceInvalidChar(string s)
        {
            s = s ?? "";

            string re = "[\x00-\x08]|[\x0B-\x0C]|[\x0E-\x1F]";

            s = s.Replace("\uFFFF", "");
            s = Regex.Replace(s, re, "");

            return s;
        }
    }

}
