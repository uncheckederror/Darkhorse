using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class OtherAssessment : BaseTableClass
    {
        #region Fields
        public decimal? OTHER_ASMT_BILLED { get; set; }
        public decimal? OTHER_ASMT_PAID { get; set; }
        // This is not nullable and a default value of 0 in LISP.
        public decimal OTHER_ASMT_REFUND { get; set; }
        public string OTHER_ASMT_TYPE { get; set; }
        public string ACTIVE { get; set; }
        public string DESCRIPTION { get; set; }
        #endregion

        public static async Task<IEnumerable<OtherAssessment>> GetAsync(int realAccountTaxYearId, int taxYear, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: Verify that this works in MS-SQL.
                var sql = $@"SELECT OTHER_ASMT_BILLED, OTHER_ASMT_PAID, OTHER_ASMT_REFUND, OTHER_ASMT_TYPE, ACTIVE, DESCRIPTION
                            FROM OTHER_ASMTS,
                             OTHER_ASMT_TYPES,
                            RP_ACCT_YRS
                            WHERE OTHER_ASMTS.OTHER_ASMT_TYPE_ID = OTHER_ASMT_TYPES.OTHER_ASMT_TYPE_ID
                            AND OTHER_ASMTS.RP_ACCT_YR_ID        = RP_ACCT_YRS.RP_ACCT_YR_ID
                            AND RP_ACCT_YRS.RP_ACCT_YR_ID = {realAccountTaxYearId}
                            AND RP_ACCT_YRS.TAX_YR = {taxYear}";

                return await connection.QueryAsync<OtherAssessment>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT OTHER_ASMT_BILLED, OTHER_ASMT_PAID, OTHER_ASMT_REFUND, OTHER_ASMT_TYPE, ACTIVE, DESCRIPTION
                            FROM OTHER_ASMTS,
                             OTHER_ASMT_TYPES,
                            RP_ACCT_YRS
                            WHERE OTHER_ASMTS.OTHER_ASMT_TYPE_ID = OTHER_ASMT_TYPES.OTHER_ASMT_TYPE_ID
                            AND OTHER_ASMTS.RP_ACCT_YR_ID        = RP_ACCT_YRS.RP_ACCT_YR_ID
                            AND RP_ACCT_YRS.RP_ACCT_YR_ID = {realAccountTaxYearId}
                            AND RP_ACCT_YRS.TAX_YR = {taxYear}";

                return await connection.QueryAsync<OtherAssessment>(sql).ConfigureAwait(false);
            }
        }
    }
}
