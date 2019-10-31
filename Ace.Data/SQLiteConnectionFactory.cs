using Chloe.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Data
{
    public class SQLiteConnectionFactory : IDbConnectionFactory
    {
        string _connString = null;
        public SQLiteConnectionFactory(string connString)
        {
            this._connString = connString;
        }
        public IDbConnection CreateConnection()
        {
            SqliteConnection conn = new SqliteConnection(this._connString);
            return conn;
        }
    }
}
