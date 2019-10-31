//using System;
//using System.Collections.Generic;
//using System.Configuration;
//using System.Data;
//using System.Data.Common;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace Database
//{
//    public static class DbHelper
//    {
//        public static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

//        public static SqlConnection CreateConnection()
//        {
//            SqlConnection conn = CreateConnection(ConnectionString);
//            return conn;
//        }
//        public static SqlConnection CreateConnection(string connString)
//        {
//            SqlConnection conn = new SqlConnection(connString);
//            return conn;
//        }

//        public static void SqlBulkCopy<TModel>(List<TModel> modelList, int batchSize, string destinationTableName = null, int? bulkCopyTimeout = null, SqlTransaction externalTransaction = null)
//        {
//            using (SqlConnection conn = DbHelper.CreateConnection())
//            {
//                conn.BulkCopy(modelList, batchSize, destinationTableName, bulkCopyTimeout, externalTransaction);
//            }
//        }
//    }
//}
