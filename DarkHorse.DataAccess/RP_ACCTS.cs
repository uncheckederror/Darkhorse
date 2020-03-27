using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Dapper;

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
            string sql = $@"SELECT * FROM LIS.RP_ACCTS WHERE RP_ACCT_ID = {realAccountId}";

            return await dbConnection.QueryAsync<RealPropertyAccount>(sql).ConfigureAwait(false);
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
