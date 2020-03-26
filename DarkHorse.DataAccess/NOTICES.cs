using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class Notice : BaseTableClass
    {
        public string DESCRIPTION { get; set; }
        public string NAME { get; set; }
        public int NOTICE_ID { get; set; }
        public int RP_ACCT_OWNER_ID { get; set; }
        public int PP_ACCT_OWNER_ID { get; set; }
        public char SUPPRESS_FLAG { get; set; }
        public int MODULE_ID { get; set; }
        public string SUBMISSION_TYPE { get; set; }
        public string GENERATION_TYPE { get; set; }
        public string STOCK_TYPE { get; set; }
        public int TAX_YR { get; set; }
        public int LID_ACCT_ID { get; set; }
        public int CONTACT_ID { get; set; }
        public int TAX_SERVICE_ID { get; set; }
        public string INTEREST_MONTH { get; set; }
        public int BATCH_NO { get; set; }
        public int ACCT_GROUP_ID { get; set; }
        public DateTime NOTICE_GENERATION_DT { get; set; }
        public DateTime NOTICE_DT { get; set; }
        public int INTEREST_YR { get; set; }
        public string NON_OWING_FLAG { get; set; }

        public static async Task<IEnumerable<Notice>> GetAsync(int realAccountOwnerId, string connectionString)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = $@"SELECT  L.DESCRIPTION, C.NAME, N.NOTICE_ID, N.RP_ACCT_OWNER_ID, N.PP_ACCT_OWNER_ID, N.SUPPRESS_FLAG, N.MODULE_ID, N.SUBMISSION_TYPE, N.GENERATION_TYPE, N.STOCK_TYPE, N.TAX_YR, N.LID_ACCT_ID, N.CONTACT_ID, N.TAX_SERVICE_ID, N.INTEREST_MONTH, N.BATCH_NO, N.ACCT_GROUP_ID, N.NOTICE_GENERATION_DT, N.NOTICE_DT, N.INTEREST_YR, N.NON_OWING_FLAG
                            FROM    NOTICES N, RP_CONTACTS RC, CONTACTS C, LIS_MODULES L
                            WHERE   N.RP_ACCT_OWNER_ID = {realAccountOwnerId}
                            AND     RC.RP_ACCT_OWNER_ID = N.RP_ACCT_OWNER_ID
                            AND     RC.CONTACT_ID = C.CONTACT_ID
                            AND     N.MODULE_ID = L.MODULE_ID
                            ORDER BY N.NOTICE_DT DESC";

            var result = await connection.QueryAsync<Notice>(sql).ConfigureAwait(false);

            return result;
        }
    }
}
