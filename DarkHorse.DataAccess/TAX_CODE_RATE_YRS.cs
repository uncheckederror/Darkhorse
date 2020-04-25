using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class TaxCodeRateYear : BaseTableClass
    {
        #region Fields
        public decimal tax_rate { get; set; }
        #endregion

        public static async Task<TaxCodeRateYear> GetAsync(string taxStatus, string taxCode, int taxYear, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: Verify that this works in MS-SQL.
                var sql = $@"SELECT     tax_rate
                             FROM       TAX_CODE_RATE_YRS
                             WHERE      TAX_STATUS = '{taxStatus}'
                             AND        TAX_CODE   = {taxCode}
                             AND        TAX_YR     = {taxYear}";

                return await connection.QueryFirstOrDefaultAsync<TaxCodeRateYear>(sql).ConfigureAwait(false) ?? new TaxCodeRateYear();
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT     tax_rate
                             FROM       TAX_CODE_RATE_YRS
                             WHERE      TAX_STATUS = '{taxStatus}'
                             AND        TAX_CODE   = {taxCode}
                             AND        TAX_YR     = {taxYear}";

                return await connection.QueryFirstOrDefaultAsync<TaxCodeRateYear>(sql).ConfigureAwait(false) ?? new TaxCodeRateYear();
            }
        }
    }
}
