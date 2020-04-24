using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class StormwaterManagement : BaseTableClass
    {
        #region Fields

        public string SSWM_TYPE { get; set; }

        #endregion

        public static async Task<StormwaterManagement> GetTypeAsync(int stormwaterAssessmentId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: I haven't verified that this works.
                var sql = @$"SELECT     SSWM_TYPE
                             FROM       SSWM_ASMTS
                             WHERE      SSWM_ASMT_ID = {stormwaterAssessmentId}";

                return await connection.QueryFirstOrDefaultAsync<StormwaterManagement>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = @$"SELECT     SSWM_TYPE
                             FROM       SSWM_ASMTS
                             WHERE      SSWM_ASMT_ID = {stormwaterAssessmentId}";

                return await connection.QueryFirstOrDefaultAsync<StormwaterManagement>(sql).ConfigureAwait(false);
            }
        }
    }
}
