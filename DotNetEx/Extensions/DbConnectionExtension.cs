
namespace System.Data
{
    public static class DbConnectionExtension
    {
        public static object ExecuteScalar(this IDbConnection conn, string cmdText, params DataParam[] dbParams)
        {
            return conn.ExecuteScalar(cmdText, CommandType.Text, dbParams);
        }
        public static object ExecuteScalar(this IDbConnection conn, string cmdText, CommandType cmdType, params DataParam[] dbParams)
        {
            return conn.ExecuteScalar(cmdText, cmdType, null, dbParams);
        }
        public static object ExecuteScalar(this IDbConnection conn, string cmdText, IDbTransaction tran, params DataParam[] dbParams)
        {
            return conn.ExecuteScalar(cmdText, CommandType.Text, tran, dbParams);
        }
        public static object ExecuteScalar(this IDbConnection conn, string cmdText, CommandType cmdType, IDbTransaction tran, params DataParam[] dbParams)
        {
            return conn.ExecuteScalar(cmdText, cmdType, null, tran, dbParams);
        }
        public static object ExecuteScalar(this IDbConnection conn, string cmdText, CommandType cmdType, int? cmdTimeout, IDbTransaction tran, params DataParam[] dbParams)
        {
            return conn.Execute(cmdText, cmdType, cmdTimeout, tran, dbParams, cmd =>
            {
                return cmd.ExecuteScalar();
            });
        }


        public static int ExecuteNonQuery(this IDbConnection conn, string cmdText, params DataParam[] dbParams)
        {
            return conn.ExecuteNonQuery(cmdText, CommandType.Text, dbParams);
        }
        public static int ExecuteNonQuery(this IDbConnection conn, string cmdText, CommandType cmdType, params DataParam[] dbParams)
        {
            return conn.ExecuteNonQuery(cmdText, cmdType, null, dbParams);
        }
        public static int ExecuteNonQuery(this IDbConnection conn, string cmdText, IDbTransaction tran, params DataParam[] dbParams)
        {
            return conn.ExecuteNonQuery(cmdText, CommandType.Text, tran, dbParams);
        }
        public static int ExecuteNonQuery(this IDbConnection conn, string cmdText, CommandType cmdType, IDbTransaction tran, params DataParam[] dbParams)
        {
            return conn.ExecuteNonQuery(cmdText, cmdType, null, tran, dbParams);
        }
        public static int ExecuteNonQuery(this IDbConnection conn, string cmdText, CommandType cmdType, int? cmdTimeout, IDbTransaction tran, params DataParam[] dbParams)
        {
            return conn.Execute(cmdText, cmdType, cmdTimeout, tran, dbParams, cmd =>
            {
                return cmd.ExecuteNonQuery();
            });
        }


        public static IDataReader ExecuteReader(this IDbConnection conn, string cmdText, params DataParam[] dbParams)
        {
            return conn.ExecuteReader(cmdText, CommandType.Text, dbParams);
        }
        public static IDataReader ExecuteReader(this IDbConnection conn, string cmdText, CommandType cmdType, params DataParam[] dbParams)
        {
            return conn.ExecuteReader(cmdText, cmdType, null, dbParams);
        }
        public static IDataReader ExecuteReader(this IDbConnection conn, string cmdText, IDbTransaction tran, params DataParam[] dbParams)
        {
            return conn.ExecuteReader(cmdText, CommandType.Text, tran, dbParams);
        }
        public static IDataReader ExecuteReader(this IDbConnection conn, string cmdText, CommandType cmdType, IDbTransaction tran, params DataParam[] dbParams)
        {
            return conn.ExecuteReader(cmdText, cmdType, null, tran, dbParams);
        }
        public static IDataReader ExecuteReader(this IDbConnection conn, string cmdText, CommandType cmdType, int? cmdTimeout, IDbTransaction tran, params DataParam[] dbParams)
        {
            if (conn.State != ConnectionState.Open)
                throw new Exception("调用 ExecuteReader 请先确保 conn 保持 Open 状态");

            return conn.Execute(cmdText, cmdType, cmdTimeout, tran, dbParams, cmd =>
            {
                return cmd.ExecuteReader();
            });
        }


        static T Execute<T>(this IDbConnection conn, string cmdText, CommandType cmdType, int? cmdTimeout, IDbTransaction tran, DataParam[] dbParams, Func<IDbCommand, T> action)
        {
            bool shouldCloseConnection = false;

            try
            {
                using (IDbCommand cmd = conn.CreateCommand())
                {
                    SetupCommand(cmd, cmdText, cmdType, cmdTimeout, dbParams, tran);

                    if (conn.State != ConnectionState.Open)
                    {
                        shouldCloseConnection = true;
                        conn.Open();
                    }

                    return action(cmd);
                }
            }
            finally
            {
                if (shouldCloseConnection && conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }

        static void SetupCommand(IDbCommand cmd, string cmdText, CommandType cmdType, int? cmdTimeout, DataParam[] dbParams, IDbTransaction tran)
        {
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;
            if (cmdTimeout != null)
                cmd.CommandTimeout = cmdTimeout.Value;

            if (tran != null)
            {
                cmd.Transaction = tran;
            }

            if (dbParams != null)
            {
                foreach (var dbParam in dbParams)
                {
                    IDbDataParameter dataParameter = cmd.CreateParameter();
                    dataParameter.Value = dbParam.Value;
                    cmd.Parameters.Add(dataParameter);
                }
            }
        }
    }
    public class DataParam
    {
        string _name;
        object _value;
        Type _type;

        public DataParam()
        {
        }
        public DataParam(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }
        public DataParam(string name, object value, Type type)
        {
            this.Name = name;
            this.Value = value;
            this.Type = type;
        }

        public string Name { get { return this._name; } set { this._name = value; } }
        public object Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
                if (value != null)
                    this._type = value.GetType();
            }
        }
        public byte? Precision { get; set; }
        public byte? Scale { get; set; }
        public int? Size { get; set; }
        public Type Type { get { return this._type; } set { this._type = value; } }

        public static DataParam Create<T>(string name, T value)
        {
            var param = new DataParam(name, value);
            if (value == null)
                param.Type = typeof(T);
            return param;
        }
        public static DataParam Create(string name, object value)
        {
            return new DataParam(name, value);
        }
        public static DataParam Create(string name, object value, Type type)
        {
            return new DataParam(name, value, type);
        }
    }
}
