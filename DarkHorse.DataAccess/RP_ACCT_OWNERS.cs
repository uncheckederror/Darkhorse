using Dapper;

using Oracle.ManagedDataAccess.Client;

using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class RealAccountOwner : BaseTableClass
    {
        #region Fields
        public int RP_ACCT_OWNER_ID { get; set; }
        public string ACCT_NO { get; set; }
        public int RP_ACCT_ID { get; set; }
        public int? TAX_SERVICE_ID { get; set; }
        public DateTime BEGIN_DT { get; set; }
        public DateTime? END_DT { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DT { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DT { get; set; }
        #endregion

        public static async Task<RealAccountOwner> GetAsync(int realAccountOwnerId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: Verify that this works in MS-SQL.
                var sql = $@"SELECT RP_ACCT_OWNER_ID,
                                      ACCT_NO,
                                      RP_ACCT_ID,
                                      TAX_SERVICE_ID,
                                      BEGIN_DT,
                                      END_DT,
                                      CREATED_BY,
                                      CREATED_DT,
                                      MODIFIED_BY,
                                      MODIFIED_DT
                                    FROM RP_ACCT_OWNERS
                                    WHERE RP_ACCT_OWNER_ID = {realAccountOwnerId}";

                return await connection.QueryFirstOrDefaultAsync<RealAccountOwner>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT RP_ACCT_OWNER_ID,
                                      ACCT_NO,
                                      RP_ACCT_ID,
                                      TAX_SERVICE_ID,
                                      BEGIN_DT,
                                      END_DT,
                                      CREATED_BY,
                                      CREATED_DT,
                                      MODIFIED_BY,
                                      MODIFIED_DT
                                    FROM RP_ACCT_OWNERS
                                    WHERE RP_ACCT_OWNER_ID = {realAccountOwnerId}";

                return await connection.QueryFirstOrDefaultAsync<RealAccountOwner>(sql).ConfigureAwait(false);
            }
        }
    }
}
