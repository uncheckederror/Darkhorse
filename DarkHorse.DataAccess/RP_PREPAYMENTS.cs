using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class RealPropertyPrepayment : BaseTableClass
    {
        #region Fields
        public decimal MONTHLY_PAYMENT { get; set; }
        public DateTime BEGIN_DT { get; set; }
        public DateTime? END_DT { get; set; }
        #endregion

        public static async Task<IEnumerable<RealPropertyPrepayment>> GetAsync(int realAccountTaxYearId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: Verify that this works in MS-SQL.
                var sql = $@"SELECT MONTHLY_PAYMENT, BEGIN_DT, END_DT FROM RP_PREPAYMENTS WHERE RP_ACCT_YR_ID = {realAccountTaxYearId}";

                return await connection.QueryAsync<RealPropertyPrepayment>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT MONTHLY_PAYMENT, BEGIN_DT, END_DT FROM RP_PREPAYMENTS WHERE RP_ACCT_YR_ID = {realAccountTaxYearId}";

                return await connection.QueryAsync<RealPropertyPrepayment>(sql).ConfigureAwait(false);
            }
        }
    }
}
