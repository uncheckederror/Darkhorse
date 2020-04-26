using Dapper;

using Oracle.ManagedDataAccess.Client;

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class SSWMAssessment : BaseTableClass
    {
        #region Fields
        public string SSWM_TYPE { get; set; }
        public int TAX_YR { get; set; }
        public string FUND_NO { get; set; }
        public decimal? RATE_PER_ESU { get; set; }
        public int? AREA_PER_ESU { get; set; }
        public string DESCRIPTION { get; set; }

        #endregion

        public static async Task<SSWMAssessment> GetTypeAsync(int stormwaterAssessmentId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: I haven't verified that this works.
                var sql = @$"SELECT     SSWM_TYPE
                             FROM       SSWM_ASMTS
                             WHERE      SSWM_ASMT_ID = {stormwaterAssessmentId}";

                return await connection.QueryFirstOrDefaultAsync<SSWMAssessment>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = @$"SELECT     SSWM_TYPE
                             FROM       SSWM_ASMTS
                             WHERE      SSWM_ASMT_ID = {stormwaterAssessmentId}";

                return await connection.QueryFirstOrDefaultAsync<SSWMAssessment>(sql).ConfigureAwait(false);
            }
        }

        public static async Task<IEnumerable<SSWMAssessment>> GetAllAsync(IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT SA.SSWM_TYPE,
                              SA.TAX_YR,
                              AF.FUND_NO,
                              SA.RATE_PER_ESU,
                              SA.AREA_PER_ESU,
                              SA.DESCRIPTION
                            FROM SSWM_ASMTS SA
                            INNER JOIN ASMT_FUNDS AF
                            ON AF.ASMT_FUND_ID = SA.ASMT_FUND_ID
                            ORDER BY SA.TAX_YR DESC";

                return await connection.QueryAsync<SSWMAssessment>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT SA.SSWM_TYPE,
                              SA.TAX_YR,
                              AF.FUND_NO,
                              SA.RATE_PER_ESU,
                              SA.AREA_PER_ESU,
                              SA.DESCRIPTION
                            FROM SSWM_ASMTS SA
                            INNER JOIN ASMT_FUNDS AF
                            ON AF.ASMT_FUND_ID = SA.ASMT_FUND_ID
                            ORDER BY SA.TAX_YR DESC";

                return await connection.QueryAsync<SSWMAssessment>(sql).ConfigureAwait(false);
            }
        }
    }
}