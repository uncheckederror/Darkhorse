using Dapper;

using Oracle.ManagedDataAccess.Client;

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class NoxiousWeedAssessment : BaseTableClass
    {
        #region Fields
        public string NOX_WEED_TYPE { get; set; }
        public int TAX_YR { get; set; }
        public string FUND_NO { get; set; }
        public decimal? ASMT_PER_PARCEL { get; set; }
        public decimal? ASMT_RATE_PER_ACRE { get; set; }
        public string DESCRIPTION { get; set; }
        #endregion

        public static async Task<NoxiousWeedAssessment> GetTypeAsync(int noxWeedAssessmentId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: I haven't verified that this works.
                var sql = @$"SELECT NOX_WEED_TYPE
                             FROM   NOX_WEED_ASMTS
                             WHERE  NOX_WEED_ASMT_ID = {noxWeedAssessmentId}";

                return await connection.QueryFirstOrDefaultAsync<NoxiousWeedAssessment>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = @$"SELECT NOX_WEED_TYPE
                             FROM   NOX_WEED_ASMTS
                             WHERE  NOX_WEED_ASMT_ID = {noxWeedAssessmentId}";

                return await connection.QueryFirstOrDefaultAsync<NoxiousWeedAssessment>(sql).ConfigureAwait(false);
            }
        }

        public static async Task<IEnumerable<NoxiousWeedAssessment>> GetAllAsync(IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: I haven't verified that this works.
                var sql = @$"SELECT NWA.NOX_WEED_TYPE,
                              NWA.TAX_YR,
                              AF.FUND_NO,
                              NWA.ASMT_PER_PARCEL,
                              NWA.ASMT_RATE_PER_ACRE,
                              NWA.DESCRIPTION
                            FROM NOX_WEED_ASMTS NWA
                            INNER JOIN ASMT_FUNDS AF
                            ON AF.ASMT_FUND_ID = NWA.ASMT_FUND_ID
                            ORDER BY NWA.TAX_YR DESC";

                return await connection.QueryAsync<NoxiousWeedAssessment>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = @$"SELECT NWA.NOX_WEED_TYPE,
                              NWA.TAX_YR,
                              AF.FUND_NO,
                              NWA.ASMT_PER_PARCEL,
                              NWA.ASMT_RATE_PER_ACRE,
                              NWA.DESCRIPTION
                            FROM NOX_WEED_ASMTS NWA
                            INNER JOIN ASMT_FUNDS AF
                            ON AF.ASMT_FUND_ID = NWA.ASMT_FUND_ID
                            ORDER BY NWA.TAX_YR DESC";

                return await connection.QueryAsync<NoxiousWeedAssessment>(sql).ConfigureAwait(false);
            }
        }
    }
}
