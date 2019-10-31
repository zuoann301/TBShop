///*
 
//CCCCCC  H    H  L      OOOOOO  EEEEE
//C       H    H  L      O    O  E
//C       H HH H  L      O    O  EEEE
//C       H    H  L      O    O  E
//CCCCCC  H    H  LLLLL  OOOOOO  EEEEE

//*/

///*
// * Chloe.ORM documentation：http://www.52chloe.com/Wiki/Document
// */

//using Ace;
//using Chloe;
//using Chloe.Infrastructure.Interception;
//using Chloe.MySql;
//using Chloe.Oracle;
//using Chloe.SQLite;
//using Chloe.SqlServer;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Ace.Data
//{
//    public class DbContextFactory
//    {
//        public static string ConnectionString { get; private set; }
//        public static string DbType { get; private set; }
//        static DbContextFactory()
//        {
//            ConnectionString = Globals.Configuration["db:ConnString"];

//            string dbType = Globals.Configuration["db:DbType"];
//            if (string.IsNullOrEmpty(dbType) == false)
//                DbType = dbType.ToLower();

//#if DEBUG
//            IDbCommandInterceptor interceptor = new DbCommandInterceptor();
//            DbInterception.Add(interceptor);
//#endif
//        }
//        public static IDbContext CreateContext()
//        {
//            IDbContext dbContext = CreateContext(ConnectionString);
//            return dbContext;
//        }
//        public static IDbContext CreateContext(string connString)
//        {
//            IDbContext dbContext = null;

//            if (DbType == "sqlite")
//            {
//                dbContext = CreateSQLiteContext(connString);
//            }
//            else if (DbType == "sqlserver")
//            {
//                dbContext = CreateSqlServerContext(connString);
//            }
//            else if (DbType == "mysql")
//            {
//                dbContext = CreateMySqlContext(connString);
//            }
//            else if (DbType == "oracle")
//            {
//                dbContext = CreateOracleContext(connString);
//            }
//            else
//            {
//                dbContext = CreateSqlServerContext(connString);
//            }

//            return dbContext;
//        }

//        static IDbContext CreateSqlServerContext(string connString)
//        {
//            MsSqlContext dbContext = new MsSqlContext(connString);
//            dbContext.PagingMode = PagingMode.OFFSET_FETCH;
//            return dbContext;
//        }
//        static IDbContext CreateMySqlContext(string connString)
//        {
//            MySqlContext dbContext = new MySqlContext(new MySqlConnectionFactory(connString));
//            return dbContext;
//        }
//        static IDbContext CreateOracleContext(string connString)
//        {
//            OracleContext dbContext = new OracleContext(new OracleConnectionFactory(connString));
//            return dbContext;
//        }
//        static IDbContext CreateSQLiteContext(string connString)
//        {
//            SQLiteContext dbContext = new SQLiteContext(new SQLiteConnectionFactory(connString));
//            return dbContext;
//        }
//    }
//}
