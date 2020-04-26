using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class FFPAssessment : BaseTableClass
    {
        #region Fields
        public int TAX_YR { get; set; }
        public string FUND_NO { get; set; }
        public decimal MIN_TAX { get; set; }
        public decimal TAX_RATE { get; set; }
        public decimal ADMIN_FEE { get; set; }
        public decimal MIN_TAX_MAX_ACRES { get; set; }
        public decimal EXEMPT_MAX_ACRES { get; set; }
        public decimal NON_TAX_MAX_ACRES { get; set; }
        #endregion

        public static async Task<FFPAssessment> GetRateAsync(int ffpAssessmentId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: I haven't verified that this works.
                var sql = @$"SELECT     TAX_RATE
                            FROM        FFP_ASMTS
                            WHERE       FFP_ASMT_ID = {ffpAssessmentId}";

                return await connection.QueryFirstOrDefaultAsync<FFPAssessment>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = @$"SELECT     TAX_RATE
                            FROM        FFP_ASMTS
                            WHERE       FFP_ASMT_ID = {ffpAssessmentId}";

                return await connection.QueryFirstOrDefaultAsync<FFPAssessment>(sql).ConfigureAwait(false);
            }
        }

        public static async Task<IEnumerable<FFPAssessment>> GetAllAsync(IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: I haven't verified that this works.
                var sql = @$"SELECT FA.TAX_YR, AF.FUND_NO, FA.TAX_RATE, FA.MIN_TAX, FA.ADMIN_FEE, FA.MIN_TAX_MAX_ACRES, FA.EXEMPT_MAX_ACRES, FA.NON_TAX_MAX_ACRES
                            FROM FFP_ASMTS FA
                            INNER JOIN ASMT_FUNDS AF
                            ON AF.ASMT_FUND_ID = FA.ASMT_FUND_ID
                            ORDER BY FA.TAX_YR DESC";

                return await connection.QueryAsync<FFPAssessment>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = @$"SELECT FA.TAX_YR, AF.FUND_NO, FA.TAX_RATE, FA.MIN_TAX, FA.ADMIN_FEE, FA.MIN_TAX_MAX_ACRES, FA.EXEMPT_MAX_ACRES, FA.NON_TAX_MAX_ACRES
                            FROM FFP_ASMTS FA
                            INNER JOIN ASMT_FUNDS AF
                            ON AF.ASMT_FUND_ID = FA.ASMT_FUND_ID
                            ORDER BY FA.TAX_YR DESC";

                return await connection.QueryAsync<FFPAssessment>(sql).ConfigureAwait(false);
            }
        }
    }
}
