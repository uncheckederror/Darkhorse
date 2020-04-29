using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DarkHorse.DataAccess
{
    public static class ClassExtensions
    {
        public static IDbConnection Connect(this IDbConnection prototype)
        {
            return (prototype is SqlConnection)
                ? new SqlConnection(prototype.ConnectionString) as IDbConnection
                : new OracleConnection(prototype.ConnectionString) as IDbConnection;
        }
    }
}
