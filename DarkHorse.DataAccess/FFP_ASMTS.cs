using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class FFPRate : BaseTableClass
    {
        #region Fields

        public decimal TAX_RATE { get; set; }

        #endregion

        public static async Task<FFPRate> GetAsync(int ffpAssessmentId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: I haven't verified that this works.
                var sql = @$"SELECT     TAX_RATE
                            FROM        FFP_ASMTS
                            WHERE       FFP_ASMT_ID = {ffpAssessmentId}";

                return await connection.QueryFirstOrDefaultAsync<FFPRate>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = @$"SELECT     TAX_RATE
                            FROM        FFP_ASMTS
                            WHERE       FFP_ASMT_ID = {ffpAssessmentId}";

                return await connection.QueryFirstOrDefaultAsync<FFPRate>(sql).ConfigureAwait(false);
            }
        }
    }
}
