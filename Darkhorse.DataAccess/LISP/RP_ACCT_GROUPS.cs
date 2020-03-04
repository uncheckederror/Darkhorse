using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;

namespace Darkhorse.DataAccess
{
    public class RealPropertyAccountGroups
    {
        public int GROUP_NO { get; set; }

        public static async Task<RealPropertyAccountGroups> GetAsync(int realAccountOwnerId, string connectionString)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = $@"SELECT ag.group_no
	                        FROM   acct_groups ag, rp_acct_groups rag
	                        WHERE  rag.rp_acct_owner_id = {realAccountOwnerId}
	                        AND    rag.end_dt IS NULL
	                        AND    rag.acct_group_id    = ag.acct_group_id";

            var result = await connection.QueryFirstOrDefaultAsync<RealPropertyAccountGroups>(sql).ConfigureAwait(false) ?? new RealPropertyAccountGroups();

            return result;
        }
    }
}
