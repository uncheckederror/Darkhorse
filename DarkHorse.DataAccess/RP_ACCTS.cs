using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Dapper;
using Oracle.ManagedDataAccess.Client;

namespace DarkHorse.DataAccess
{
    public class RealPropertyAccount : BaseTableClass
    {
        #region Fields

        public int RP_ACCT_ID { get; set; }
        public string ACCT_NO { get; set; }
        public string MAP_NO { get; set; }
        public string QUARTER_SECTION { get; set; }
        public char? PP_AS_RP_FLAG { get; set; }
        public string SEC_TWN_RNG { get; set; }
        public string WORK_GROUP { get; set; }
        public DateTime? INACTIVE_DT { get; set; }
        public DateTime? REFERENCE_DT { get; set; }
        public string NEIGHBORHOOD_CODE { get; set; }

        #endregion

        public static async Task<IEnumerable<RealPropertyAccount>> GetAsync(int realAccountId, IDbConnection dbConnection)
        {
            var sql = (dbConnection is SqlConnection)
                ? $"SELECT * FROM LIS.RP_ACCTS WHERE RP_ACCT_ID = {realAccountId}"
                : $@"SELECT  MAP_NO, QUARTER_SECTION, PP_AS_RP_FLAG, SEC_TWN_RNG, WORK_GROUP, INACTIVE_DT, REFERENCE_DT, CREATED_BY, CREATED_DT, ACCT_NO, RP_ACCT_ID, MODIFIED_DT, MODIFIED_BY, NEIGHBORHOOD_CODE
                     FROM    RP_ACCTS
                     WHERE   RP_ACCT_ID = {realAccountId}";

            return await Connection(dbConnection).QueryAsync<RealPropertyAccount>(sql).ConfigureAwait(false);
        }

        public static async Task<IEnumerable<RealPropertyAccount>> GetAsync(string accountNumber, IDbConnection dbConnection)
        {
            var sql = (dbConnection is SqlConnection)
                ? $@"SELECT * FROM LIS.RP_ACCTS WHERE ACCT_NO = '{accountNumber}'"
                : $@"SELECT * FROM LIS.RP_ACCTS WHERE ACCT_NO = '{accountNumber}'";

            return await Connection(dbConnection).QueryAsync<RealPropertyAccount>(sql).ConfigureAwait(false);
        }

        public static async Task<IEnumerable<RealPropertyAccount>> GetAsync(Regex accountRegEx, IDbConnection dbConnection)
        {
            var sql = (dbConnection is SqlConnection)
                ? $@"SELECT * FROM LIS.RP_ACCTS WHERE ACCT_NO LIKE '{accountRegEx}%' ORDER BY ACCT_NO"
                : $@"SELECT * FROM LIS.RP_ACCTS WHERE REGEXP_LIKE(ACCT_NO, '{accountRegEx}') ORDER BY ACCT_NO";

            return await Connection(dbConnection).QueryAsync<RealPropertyAccount>(sql).ConfigureAwait(false);
        }
    }
}
