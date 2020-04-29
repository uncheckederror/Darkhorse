using Dapper;

using Oracle.ManagedDataAccess.Client;

using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class Cashier : BaseTableClass
    {
        public int CASHIER_ID { get; set; }
        public DateTime OPEN_DT { get; set; }
        public char EXTEND_HALF_PAYMENT_FLAG { get; set; }
        public string DEFAULT_INTEREST_MONTH { get; set; }
        public int DEFAULT_INTEREST_YEAR { get; set; }

        public static async Task<Cashier> GetAsync(string userName, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: Not sure if this works in MS-SQL
                var sql = $@"SELECT csh.cashier_id,
                                       csh.open_dt,
                                       csh.extend_half_payment_flag,
                                       csh.default_interest_month,
                                       csh.default_interest_year
                                FROM   cashiers csh,
                                       lis_users luser
                                WHERE  luser.logon_name     = {userName}
                                AND    luser.cashier_id     = csh.cashier_id;";

                return await connection.QueryFirstOrDefaultAsync<Cashier>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT csh.cashier_id,
                                       csh.open_dt,
                                       csh.extend_half_payment_flag,
                                       csh.default_interest_month,
                                       csh.default_interest_year
                                FROM   cashiers csh,
                                       lis_users luser
                                WHERE  luser.logon_name     = {userName}
                                AND    luser.cashier_id     = csh.cashier_id;";

                return await connection.QueryFirstOrDefaultAsync<Cashier>(sql).ConfigureAwait(false);
            }
        }
    }
}
