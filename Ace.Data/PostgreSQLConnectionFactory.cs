using Chloe.Infrastructure;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Ace.Data
{
    public class PostgreSQLConnectionFactory : IDbConnectionFactory
    {
        string _connString = null;
        public PostgreSQLConnectionFactory(string connString)
        {
            this._connString = connString;
        }
        public IDbConnection CreateConnection()
        {
            NpgsqlConnection conn = new NpgsqlConnection(this._connString);
            return conn;
        }
    }
}
