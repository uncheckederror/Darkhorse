using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
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

        #endregion

        public static async Task<NoxiousWeedAssessment> GetAsync(int noxWeedAssessmentId, IDbConnection dbConnection)
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
    }
}
