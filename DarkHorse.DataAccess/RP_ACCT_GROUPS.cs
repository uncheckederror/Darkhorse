using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class RealPropertyAccountGroup : BaseTableClass
    {
        public int GROUP_NO { get; set; }
        public int RP_ACCT_GROUP_ID { get; set; }
        public int ACCT_GROUP_ID { get; set; }
        public int RP_ACCT_OWNER_ID { get; set; }
        public DateTime BEGIN_DT { get; set; }
        public DateTime END_DT { get; set; }
        public string GRP_ACCT_NO { get; set; }
        public string GROUP_NAME { get; set; }
        public string ACTIVE { get; set; }
        public string CONTACT_NAME { get; set; }
        public string STREET_ADDR { get; set; }
        public string MISC_LINE1 { get; set; }
        public string MISC_LINE2 { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIP_CODE { get; set; }
        public string PHONE { get; set; }
        public char MAIL_TS_FLAG { get; set; }
        public char MORTGAGE_FLAG { get; set; }

        public static async Task<RealPropertyAccountGroup> GetNumberAsync(int realAccountOwnerId, string connectionString)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = $@"SELECT ag.group_no
	                        FROM   acct_groups ag, rp_acct_groups rag
	                        WHERE  rag.rp_acct_owner_id = {realAccountOwnerId}
	                        AND    rag.end_dt IS NULL
	                        AND    rag.acct_group_id    = ag.acct_group_id";

            var result = await connection.QueryFirstOrDefaultAsync<RealPropertyAccountGroup>(sql).ConfigureAwait(false) ?? new RealPropertyAccountGroup();

            return result;
        }

        public static async Task<IEnumerable<RealPropertyAccountGroup>> GetAsync(int realAccountOwnerId, string connectionString)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = $@"SELECT  RP_ACCT_GROUPS.RP_ACCT_GROUP_ID, RP_ACCT_GROUPS.ACCT_GROUP_ID, RP_ACCT_GROUPS.RP_ACCT_OWNER_ID, RP_ACCT_GROUPS.BEGIN_DT, RP_ACCT_GROUPS.END_DT, RP_ACCT_GROUPS.CREATED_BY, RP_ACCT_GROUPS.CREATED_DT, RP_ACCT_GROUPS.MODIFIED_BY, RP_ACCT_GROUPS.MODIFIED_DT, RP_ACCT_GROUPS.GRP_ACCT_NO,
                                    ACCT_GROUPS.GROUP_NAME, ACCT_GROUPS.GROUP_NO, ACCT_GROUPS.ACTIVE, ACCT_GROUPS.CONTACT_NAME, ACCT_GROUPS.STREET_ADDR, ACCT_GROUPS.MISC_LINE1, ACCT_GROUPS.MISC_LINE2, ACCT_GROUPS.CITY, ACCT_GROUPS.STATE, ACCT_GROUPS.ZIP_CODE, ACCT_GROUPS.PHONE, ACCT_GROUPS.MAIL_TS_FLAG, ACCT_GROUPS.MORTGAGE_FLAG
                            FROM    RP_ACCT_GROUPS, ACCT_GROUPS
                            WHERE   RP_ACCT_GROUPS.ACCT_GROUP_ID = ACCT_GROUPS.ACCT_GROUP_ID
                            AND     RP_ACCT_GROUPS.RP_ACCT_OWNER_ID = {realAccountOwnerId}
                            ORDER BY RP_ACCT_GROUPS.BEGIN_DT DESC";

            var result = await connection.QueryAsync<RealPropertyAccountGroup>(sql).ConfigureAwait(false);

            return result;
        }
    }
}