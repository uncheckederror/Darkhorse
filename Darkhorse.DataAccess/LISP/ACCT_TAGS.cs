using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Darkhorse.DataAccess
{
    public class AccountTags
    {
        public string TAG_CODE { get; set; }

        public static async Task<IEnumerable<AccountTags>> GetAsync(int realAccountOwnerId, string connectionString)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = $@"SELECT DISTINCT tag_code
	                        FROM   acct_tags tc
	                        WHERE  tc.rp_acct_owner_id = {realAccountOwnerId}
	                        AND    tc.end_dt IS NULL
	                        ORDER BY tag_code";

            var result = await connection.QueryAsync<AccountTags>(sql).ConfigureAwait(false);

            return result;
        }
    }
}
