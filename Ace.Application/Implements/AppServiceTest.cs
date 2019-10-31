//using Ace.Application;
//using Chloe;
//using Ace.Entity;
//using Chloe.MySql;
//using Chloe.Oracle;
//using Chloe.SQLite;
//using Chloe.SqlServer;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Ace.Application.Implements
//{
//    public class AppServiceTest : AppServiceBase, IAppServiceTest
//    {
//        public string Console(string s)
//        {
//            this.Copy<Sys_Department>();
//            this.Copy<Sys_Duty>();
//            this.Copy<Sys_Module>();
//            this.Copy<Sys_Role>();
//            this.Copy<Sys_RoleAuthorize>();

//            this.Copy<Sys_User>();
//            this.Copy<Sys_UserLogOn>();

//            this.Copy<WikiDocumentDetail>();
//            this.Copy<WikiMenuItem>();

//            return s;
//        }

//        void Copy<T>() where T : new()
//        {
//            var docs = this.DbContext.Query<T>().ToList();

//            using (IDbContext context = CreateTargetDbContext())
//            {
//                foreach (var item in docs)
//                {
//                    try
//                    {
//                        //string clob = "";

//                        //for (int i = 0; i < 39999; i++)
//                        //{
//                        //    clob += "树";
//                        //}

//                        //WikiDocumentDetail d = item as WikiDocumentDetail;
//                        //if(d.Id!="3325141727961939968")
//                        //    continue;
//                        //d.HtmlContent = clob;
//                        //d.MarkdownCode = clob;
//                        //if (d.MarkdownCode != null && d.MarkdownCode.Length > 2500)
//                        //    d.MarkdownCode = "";

//                        context.Insert(item);
//                    }
//                    catch (Exception ex)
//                    {

//                        throw;
//                    }
//                }
//            }
//        }

//        IDbContext CreateTargetDbContext()
//        {
//            IDbContext dbContext = null;
//            //dbContext = new SQLiteContext(new SQLiteConnectionFactory(@"Data Source=D:\MyProject\chloesite\db\Chloe.db;Version=3;Pooling=True;Max Pool Size=100;"));
//            //dbContext = new MySqlContext(new MySqlConnectionFactory("Database='Chloe';Data Source=localhost;User ID=root;Password=sasa;CharSet=utf8;SslMode=None"));
//            //dbContext = new OracleContext(new OracleConnectionFactory("Data Source=localhost/orcl;User ID=system;Password=sa;"));
//            //dbContext = new MsSqlContext("data source=.;initial catalog=Chloe;user id=sa;password=sa;pooling=true;max pool size = 5120;min pool size=0;Connect Timeout=10;");
//            return dbContext;
//        }
//    }
//}
