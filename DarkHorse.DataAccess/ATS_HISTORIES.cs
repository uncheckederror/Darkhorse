using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class ATSHistory : BaseTableClass
    {
        public int ATS_HIST_ID { get; set; }
        public int RP_ACCT_ID { get; set; }
        public int PP_ACCT_ID { get; set; }
        public string AHST_NUM { get; set; }
        public string AHST_TYPE { get; set; }
        public DateTime AHST_DT { get; set; }
        public string AHST_USER { get; set; }
        public int LID_ACCT_ID { get; set; }
        public int ACCT_GROUP_ID { get; set; }

        public static async Task<IEnumerable<ATSHistory>> GetAsync(int realPropertyAccountId, string connectionString)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = $@"SELECT  ATS_HIST_ID, RP_ACCT_ID, PP_ACCT_ID, AHST_NUM, AHST_TYPE, AHST_DT, AHST_USER, CREATED_BY, CREATED_DT, MODIFIED_BY, MODIFIED_DT, LID_ACCT_ID, ACCT_GROUP_ID
                            FROM    ATS_HISTORIES H
                            WHERE   H.RP_ACCT_ID = {realPropertyAccountId}
                            ORDER BY H.AHST_DT DESC";

            var result = await connection.QueryAsync<ATSHistory>(sql).ConfigureAwait(false);

            return result;
        }
    }
}
