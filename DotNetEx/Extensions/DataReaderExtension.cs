
namespace System.Data
{
    public static class DataReaderExtension
    {
        public static DbValue GetDbValue(this IDataReader reader, string name)
        {
            int ordinal = reader.GetOrdinal(name);
            return reader.GetDbValue(ordinal);
        }
        public static DbValue GetDbValue(this IDataReader reader, int ordinal)
        {
            if (reader.IsDBNull(ordinal))
            {
                return new DbValue();
            }

            return new DbValue(reader.GetValue(ordinal));
        }

        public static DataTable FillDataTable(this IDataReader reader)
        {
            DataTable dt = new DataTable();
            int fieldCount = reader.FieldCount;
            for (int i = 0; i < fieldCount; i++)
            {
                DataColumn dc = new DataColumn(reader.GetName(i), reader.GetFieldType(i));
                dt.Columns.Add(dc);
            }
            while (reader.Read())
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < fieldCount; i++)
                {
                    var val = reader[i];
                    dr[i] = val;
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
        public static DataSet FillDataSet(this IDataReader reader)
        {
            DataSet ds = new DataSet();
            var dt = FillDataTable(reader);
            ds.Tables.Add(dt);

            while (reader.NextResult())
            {
                dt = FillDataTable(reader);
                ds.Tables.Add(dt);
            }

            return ds;
        }
    }
    public class DbValue
    {
        object _value;

        public DbValue()
        {
        }
        public DbValue(object value)
        {
            if (value == DBNull.Value)
                this._value = null;
            else
                this._value = value;
        }

        public object Value { get { return this._value; } }

        public static implicit operator byte(DbValue dbValue)
        {
            EnsureNotNull(dbValue);

            if (dbValue._value.GetType() == typeof(byte))
                return (byte)dbValue._value;

            return Convert.ToByte(dbValue._value);
        }
        public static implicit operator byte?(DbValue dbValue)
        {
            if (dbValue._value == null)
                return null;

            byte ret = dbValue;
            return ret;
        }
        public static implicit operator short(DbValue dbValue)
        {
            EnsureNotNull(dbValue);

            if (dbValue._value.GetType() == typeof(short))
                return (short)dbValue._value;

            return Convert.ToInt16(dbValue._value);
        }
        public static implicit operator short?(DbValue dbValue)
        {
            if (dbValue._value == null)
                return null;

            short ret = dbValue;
            return ret;
        }
        public static implicit operator int(DbValue dbValue)
        {
            EnsureNotNull(dbValue);

            if (dbValue._value.GetType() == typeof(int))
                return (int)dbValue._value;

            return Convert.ToInt32(dbValue._value);
        }
        public static implicit operator int?(DbValue dbValue)
        {
            if (dbValue._value == null)
                return null;

            int ret = dbValue;
            return ret;
        }
        public static implicit operator long(DbValue dbValue)
        {
            EnsureNotNull(dbValue);

            if (dbValue._value.GetType() == typeof(long))
                return (long)dbValue._value;

            return Convert.ToInt64(dbValue._value);
        }
        public static implicit operator long?(DbValue dbValue)
        {
            if (dbValue._value == null)
                return null;

            long ret = dbValue;
            return ret;
        }

        public static implicit operator float(DbValue dbValue)
        {
            EnsureNotNull(dbValue);

            if (dbValue._value.GetType() == typeof(float))
                return (float)dbValue._value;

            return Convert.ToSingle(dbValue._value);
        }
        public static implicit operator float?(DbValue dbValue)
        {
            if (dbValue._value == null)
                return null;

            float ret = dbValue;
            return ret;
        }
        public static implicit operator double(DbValue dbValue)
        {
            EnsureNotNull(dbValue);

            if (dbValue._value.GetType() == typeof(double))
                return (double)dbValue._value;

            return Convert.ToDouble(dbValue._value);
        }
        public static implicit operator double?(DbValue dbValue)
        {
            if (dbValue._value == null)
                return null;

            double ret = dbValue;
            return ret;
        }
        public static implicit operator decimal(DbValue dbValue)
        {
            EnsureNotNull(dbValue);

            if (dbValue._value.GetType() == typeof(decimal))
                return (decimal)dbValue._value;

            return Convert.ToDecimal(dbValue._value);
        }
        public static implicit operator decimal?(DbValue dbValue)
        {
            if (dbValue._value == null)
                return null;

            decimal ret = dbValue;
            return ret;
        }

        public static implicit operator Guid(DbValue dbValue)
        {
            EnsureNotNull(dbValue);

            var valType = dbValue._value.GetType();

            if (valType == typeof(Guid))
                return (Guid)dbValue._value;

            if (valType == typeof(string))
                return Guid.Parse((string)dbValue._value);

            if (valType == typeof(byte[]))
                return new Guid((byte[])dbValue._value);

            throw new InvalidCastException();
        }
        public static implicit operator Guid?(DbValue dbValue)
        {
            if (dbValue._value == null)
                return null;

            Guid ret = dbValue;
            return ret;
        }

        public static implicit operator DateTime(DbValue dbValue)
        {
            EnsureNotNull(dbValue);

            if (dbValue._value.GetType() == typeof(DateTime))
                return (DateTime)dbValue._value;

            return Convert.ToDateTime(dbValue._value);
        }
        public static implicit operator DateTime?(DbValue dbValue)
        {
            if (dbValue._value == null)
                return null;

            DateTime ret = dbValue;
            return ret;
        }

        public static implicit operator bool(DbValue dbValue)
        {
            EnsureNotNull(dbValue);

            if (dbValue._value.GetType() == typeof(bool))
                return (bool)dbValue._value;

            return Convert.ToBoolean(dbValue._value);
        }
        public static implicit operator bool?(DbValue dbValue)
        {
            if (dbValue._value == null)
                return null;

            bool ret = dbValue;
            return ret;
        }

        public static implicit operator char(DbValue dbValue)
        {
            EnsureNotNull(dbValue);

            if (dbValue._value.GetType() == typeof(char))
                return (char)dbValue._value;

            return Convert.ToChar(dbValue._value);
        }
        public static implicit operator char?(DbValue dbValue)
        {
            if (dbValue._value == null)
                return null;

            char ret = dbValue;
            return ret;
        }
        public static implicit operator string(DbValue dbValue)
        {
            return (string)dbValue._value;
        }


        static void EnsureNotNull(DbValue dbValue)
        {
            if (dbValue._value == null || dbValue._value == DBNull.Value)
                throw new InvalidCastException();
        }
    }
}
