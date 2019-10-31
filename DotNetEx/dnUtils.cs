using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
    /// 一些公用的方法
    /// </summary>
    public class dnUtils
    {
        /// <summary>
        /// 根据 totalCount 和每页的数据大小计算总页数
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static int GetPageCount(int totalCount, int pageSize)
        {
            int pageCount = 1;

            double db = totalCount % pageSize;

            if (db == 0)
            {
                pageCount = totalCount / pageSize;
            }
            else
            {
                pageCount = totalCount / pageSize + 1;
            }

            return pageCount;
        }
        /// <summary>
        /// 如果 obj 为 null,则引发 ArgumentNullException
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="paramName"></param>
        public static void NotNull(object obj, string paramName = "")
        {
            if (obj == null)
                throw new ArgumentNullException(paramName);
        }
        /// <summary>
        /// 确保 obj 不为 null 或空，如果为 null 或空则引发 ArgumentException
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="paramName"></param>
        public static void NotNullOrEmpty(object obj, string paramName = "")
        {
            if (obj is string)
            {
                if (string.IsNullOrEmpty((string)obj))
                    throw new ArgumentException(paramName);
            }

            if (obj == null)
                throw new ArgumentException(paramName);
        }

        /// <summary>
        /// 分批
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public static List<List<T>> InBatches<T>(List<T> source, int batchSize)
        {
            List<List<T>> batches = new List<List<T>>();

            List<T> batch = new List<T>(source.Count > batchSize ? batchSize : source.Count);
            for (int i = 0; i < source.Count; i++)
            {
                var item = source[i];
                batch.Add(item);
                if (batch.Count >= batchSize)
                {
                    batches.Add(batch);
                    batch = new List<T>();
                }
            }

            if (batch.Count > 0)
            {
                batches.Add(batch);
            }

            return batches;
        }
    }

}
