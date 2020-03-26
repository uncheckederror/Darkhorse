using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class RealPropertySiteAddress
    {
        public int OBJECTID { get; set; }
        public int SITUS_ID { get; set; }
        public int RP_ACCT_ID { get; set; }
        public int LAND_ACCT_ID { get; set; }
        public string JURISDICTION { get; set; }
        public string ST_NO { get; set; }
        public string PREFIX { get; set; }
        public string STREET_NAME { get; set; }
        public string IDENTIFIER { get; set; }
        public string SUFFIX { get; set; }
        public string SUITE_UNIT { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIP_CODE { get; set; }
        public string PRIMARY_FLAG { get; set; }
        public string IN_USE_FLAG { get; set; }
        public string STREET_ADDR { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DT { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DT { get; set; }
        public string VERIFIED_BY { get; set; }
        public DateTime VERIFIED_DT { get; set; }
        public DateTime INACTIVE_DT { get; set; }

        public static async Task<IEnumerable<RealPropertySiteAddress>> GetAsync(int realPropertyAccountId, string connectionString)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = $@"SELECT OBJECTID, SITUS_ID, RP_ACCT_ID, LAND_ACCT_ID, JURISDICTION, ST_NO, PREFIX, STREET_NAME, IDENTIFIER, SUFFIX, SUITE_UNIT, CITY, STATE, ZIP_CODE, PRIMARY_FLAG, IN_USE_FLAG, STREET_ADDR, CREATED_BY, CREATED_DT, MODIFIED_BY, MODIFIED_DT, VERIFIED_BY, VERIFIED_DT, INACTIVE_DT
                            FROM RP_SITUSES_VW
                            WHERE RP_ACCT_ID = {realPropertyAccountId}";

            var result = await connection.QueryAsync<RealPropertySiteAddress>(sql).ConfigureAwait(false);

            return result;
        }
    }
}
