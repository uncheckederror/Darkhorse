using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class SeniorCitizenRate : BaseTableClass
    {
        #region Fields

        public string srcit_type { get; set; }

        #endregion

        public static async Task<SeniorCitizenRate> GetAsync(int realAccountTaxYearId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: I haven't verified that this works.
                var sql = @$"SELECT     srcit_type
                             FROM       srcit_incomes si,
                                        srcit_rates sr,
                                        srcit_appls sa
                             WHERE      si.rp_acct_yr_id = {realAccountTaxYearId}
                                        AND si.srcit_appl_id   = sa.srcit_appl_id
                                        AND si.srcit_rate_id   = sr.srcit_rate_id
                                        AND sa.inactive_dt    IS NULL";

                return await connection.QueryFirstOrDefaultAsync<SeniorCitizenRate>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = @$"SELECT     srcit_type
                             FROM       srcit_incomes si,
                                        srcit_rates sr,
                                        srcit_appls sa
                             WHERE      si.rp_acct_yr_id = {realAccountTaxYearId}
                                        AND si.srcit_appl_id   = sa.srcit_appl_id
                                        AND si.srcit_rate_id   = sr.srcit_rate_id
                                        AND sa.inactive_dt    IS NULL";

                return await connection.QueryFirstOrDefaultAsync<SeniorCitizenRate>(sql).ConfigureAwait(false);
            }
        }
    }
}
