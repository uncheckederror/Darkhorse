using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class RealPropertyAdjustment : BaseTableClass
    {
        #region Fields
        public string ADJ_REASON { get; set; }
        public DateTime CHG_DT { get; set; }
        public string TAX_ADJ_TYPE { get; set; }
        public decimal? TAX_ADJ_AMT { get; set; }
        public string SSWM_ADJ_TYPE { get; set; }
        public decimal? SSWM_ADJ_AMT { get; set; }
        public string FFP_ADJ_TYPE { get; set; }
        public decimal? FFP_ADJ_AMT { get; set; }
        public string NOX_WEED_ADJ_TYPE { get; set; }
        public decimal? NOX_WEED_ADJ_AMT { get; set; }
        #endregion

        public static async Task<IEnumerable<RealPropertyAdjustment>> GetAsync(int realAccountTaxYearId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: Verify that this works in MS-SQL.
                var sql = $@"SELECT     ADJ_REASON, CHG_DT, TAX_ADJ_TYPE, TAX_ADJ_AMT, SSWM_ADJ_TYPE, SSWM_ADJ_AMT, FFP_ADJ_TYPE, FFP_ADJ_AMT, NOX_WEED_ADJ_TYPE, NOX_WEED_ADJ_AMT 
                            FROM        RP_ACCT_ADJUSTS RAA 
                            WHERE       RAA.RP_ACCT_YR_ID = {realAccountTaxYearId}";

                return await connection.QueryAsync<RealPropertyAdjustment>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT     ADJ_REASON, CHG_DT, TAX_ADJ_TYPE, TAX_ADJ_AMT, SSWM_ADJ_TYPE, SSWM_ADJ_AMT, FFP_ADJ_TYPE, FFP_ADJ_AMT, NOX_WEED_ADJ_TYPE, NOX_WEED_ADJ_AMT 
                            FROM        RP_ACCT_ADJUSTS RAA 
                            WHERE       RAA.RP_ACCT_YR_ID = {realAccountTaxYearId}";

                return await connection.QueryAsync<RealPropertyAdjustment>(sql).ConfigureAwait(false);
            }
        }
    }
}
