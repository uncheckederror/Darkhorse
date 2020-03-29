using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Dapper;
using Oracle.ManagedDataAccess.Client;

namespace DarkHorse.DataAccess
{
    public class RealPropertyAccount : BaseTableClass
    {
        public int RP_ACCT_ID { get; set; }
        public string ACCT_NO { get; set; }
        public string MAP_NO { get; set; }
        public string QUARTER_SECTION { get; set; }
        public string PP_AS_RP_FLAG { get; set; }
        public string SEC_TWN_RNG { get; set; }
        public string WORK_GROUP { get; set; }
        public DateTime INACTIVE_DT { get; set; }
        public DateTime REFERENCE_DT { get; set; }
        public string NEIGHBORHOOD_CODE { get; set; }

        public static async Task<IEnumerable<RealPropertyAccount>> GetAsync(int realAccountId, IDbConnection dbConnection)
        {
            // If the concrete type of the IDbConnection is the MS-SQL connection object, run the new query, if not run the old query.
            if (dbConnection.GetType()?.Name == "SqlConnection")
            {
                // Explicitly create and dispose of a new database connection. Let the framework handle the pooling and reuse of these objects.
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                string sql = $@"SELECT * FROM LIS.RP_ACCTS WHERE RP_ACCT_ID = {realAccountId}";

                var result = await dbConnection.QueryAsync<RealPropertyAccount>(sql).ConfigureAwait(false);

                // Add customer getters and setters here like mapping a tax year to a DateTime or setting null dates from the database to a known value for their C# counter part.

                return result;
            }
            else
            {
                // This is legacy Oracle version. It's preserved here as an instant fall back/optional path depending on the type of the IDbConnection.
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                string sql = $@"SELECT  MAP_NO, QUARTER_SECTION, PP_AS_RP_FLAG, SEC_TWN_RNG, WORK_GROUP, INACTIVE_DT, REFERENCE_DT, CREATED_BY, CREATED_DT, ACCT_NO, RP_ACCT_ID, MODIFIED_DT, MODIFIED_BY, NEIGHBORHOOD_CODE
                            FROM    RP_ACCTS
                            WHERE   RP_ACCT_ID = {realAccountId}";

                var result = await connection.QueryAsync<RealPropertyAccount>(sql).ConfigureAwait(false);

                return result;
            }
        }

        public static async Task<IEnumerable<RealPropertyAccount>> GetAsync(string accountNumber, IDbConnection dbConnection)
        {
            string sql = $@"SELECT * FROM LIS.RP_ACCTS WHERE ACCT_NO = '{accountNumber}'";

            return await dbConnection.QueryAsync<RealPropertyAccount>(sql).ConfigureAwait(false);
        }

        public static async Task<IEnumerable<RealPropertyAccount>> GetAsync(Regex accountRegEx, IDbConnection dbConnection)
        {
            string sql = $@"SELECT * FROM LIS.RP_ACCTS WHERE ACCT_NO LIKE '{accountRegEx}%' ORDER BY ACCT_NO";

            return await dbConnection.QueryAsync<RealPropertyAccount>(sql).ConfigureAwait(false);
        }
    }
}
