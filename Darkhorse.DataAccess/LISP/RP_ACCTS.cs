using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class RealPropertyAccount
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
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DT { get; set; }
        public DateTime MODIFIED_DT { get; set; }
        public string MODIFIED_BY { get; set; }
        public string NEIGHBORHOOD_CODE { get; set; }

        public static async Task<IEnumerable<RealPropertyAccount>> GetAsync(int realAccountId, string connectionString)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = $@"SELECT  MAP_NO, QUARTER_SECTION, PP_AS_RP_FLAG, SEC_TWN_RNG, WORK_GROUP, INACTIVE_DT, REFERENCE_DT, CREATED_BY, CREATED_DT, ACCT_NO, RP_ACCT_ID, MODIFIED_DT, MODIFIED_BY, NEIGHBORHOOD_CODE
                            FROM    RP_ACCTS
                            WHERE   RP_ACCT_ID = {realAccountId}";

            var result = await connection.QueryAsync<RealPropertyAccount>(sql).ConfigureAwait(false);

            return result;
        }
    }
}
