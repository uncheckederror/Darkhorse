using Dapper;

using Oracle.ManagedDataAccess.Client;

using System;
using System.Collections.Generic;
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
        public string TAX_SERVICE_ID { get; set; }
        public DateTime BEGIN_DT { get; set; }
        public DateTime? END_DT { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DT { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DT { get; set; }
        // This is from the Contacts table.
        public string NAME { get; set; }
        public int CONTACT_ID { get; set; }

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

        public static async Task<IEnumerable<RealAccountOwner>> GetByRpAcctIdAsync(int realPropertyAccountId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: Verify that this works in MS-SQL.
                var sql = $@"SELECT C.NAME, C.CONTACT_ID, RAO.RP_ACCT_OWNER_ID, RAO.ACCT_NO, RAO.RP_ACCT_ID, RAO.TAX_SERVICE_ID, RAO.BEGIN_DT, RAO.END_DT, RAO.CREATED_BY, RAO.CREATED_DT, RAO.MODIFIED_BY, RAO.MODIFIED_DT
                                    FROM RP_ACCT_OWNERS RAO
                                    INNER JOIN RP_CONTACTS RC
                                    ON (RAO.RP_ACCT_OWNER_ID = RC.RP_ACCT_OWNER_ID)
                                    INNER JOIN CONTACTS C
                                    ON (C.CONTACT_ID = RC.CONTACT_ID)
                                    WHERE RAO.RP_ACCT_ID = {realPropertyAccountId}
                                    AND RAO.END_DT IS NOT NULL
                                    ORDER BY RAO.END_DT DESC";

                return await connection.QueryAsync<RealAccountOwner>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT C.NAME, C.CONTACT_ID, RAO.RP_ACCT_OWNER_ID, RAO.ACCT_NO, RAO.RP_ACCT_ID, RAO.TAX_SERVICE_ID, RAO.BEGIN_DT, RAO.END_DT, RAO.CREATED_BY, RAO.CREATED_DT, RAO.MODIFIED_BY, RAO.MODIFIED_DT
                                    FROM RP_ACCT_OWNERS RAO
                                    INNER JOIN RP_CONTACTS RC
                                    ON (RAO.RP_ACCT_OWNER_ID = RC.RP_ACCT_OWNER_ID)
                                    INNER JOIN CONTACTS C
                                    ON (C.CONTACT_ID = RC.CONTACT_ID)
                                    WHERE RAO.RP_ACCT_ID = {realPropertyAccountId}
                                    AND RAO.END_DT IS NOT NULL
                                    ORDER BY RAO.END_DT DESC";

                return await connection.QueryAsync<RealAccountOwner>(sql).ConfigureAwait(false);
            }
        }

    }
}
