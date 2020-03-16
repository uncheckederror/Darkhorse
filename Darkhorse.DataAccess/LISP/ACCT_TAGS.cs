using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Darkhorse.DataAccess
{
    public class AccountTags
    {
        public string TAG_CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public DateTime BEGIN_DT { get; set; }
        public DateTime END_DT { get; set; }
        public string REMOVED_BY { get; set; }
        public char LOCK_ACCT_FLAG { get; set; }
        public char ALERT_FLAG { get; set; }
        public char SYSTEM_TAG_FLAG { get; set; }
        public char NO_STATEMENT_FLAG { get; set; }
        public char PROGRAM_GEN_FLAG { get; set; }
        public char TEMP_FLAG { get; set; }
        public char QUE_STATEMENT_FLAG { get; set; }
        public char TRANSFER_FLAG { get; set; }

        public static async Task<IEnumerable<AccountTags>> GetCodeAsync(int realAccountOwnerId, string connectionString)
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

        public static async Task<IEnumerable<AccountTags>> GetAsync(int realAccountOwnerId, string connectionString)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = $@"SELECT  TA.TAG_CODE, TC.DESCRIPTION, TA.BEGIN_DT, TA.END_DT, TA.REMOVED_BY, TC.LOCK_ACCT_FLAG, TC.ALERT_FLAG, TC.SYSTEM_TAG_FLAG, TC.NO_STATEMENT_FLAG, TC.PROGRAM_GEN_FLAG, TC.TEMP_FLAG, TC.QUE_STATEMENT_FLAG, TC.TRANSFER_FLAG
                            FROM    ACCT_TAGS TA, TAG_CODES TC
                            WHERE   RP_ACCT_OWNER_ID = {realAccountOwnerId}
                            AND     TA.TAG_CODE = TC.TAG_CODE
                            ORDER BY TA.BEGIN_DT DESC";

            var result = await connection.QueryAsync<AccountTags>(sql).ConfigureAwait(false);

            return result;
        }
    }
}
