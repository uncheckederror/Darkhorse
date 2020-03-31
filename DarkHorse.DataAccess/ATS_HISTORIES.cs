using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class ATSHistory : BaseTableClass
    {
        #region Fields

        public int ATS_HIST_ID { get; set; }
        public int RP_ACCT_ID { get; set; }
        public int PP_ACCT_ID { get; set; }
        public string AHST_NUM { get; set; }
        public string AHST_TYPE { get; set; }
        public DateTime AHST_DT { get; set; }
        public string AHST_USER { get; set; }
        public int LID_ACCT_ID { get; set; }
        public int ACCT_GROUP_ID { get; set; }

        #endregion

        public static async Task<IEnumerable<ATSHistory>> GetAsync(int realPropertyAccountId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT  ATS_HIST_ID, RP_ACCT_ID, PP_ACCT_ID, AHST_NUM, AHST_TYPE, AHST_DT, AHST_USER, CREATED_BY, CREATED_DT, MODIFIED_BY, MODIFIED_DT, LID_ACCT_ID, ACCT_GROUP_ID
                             FROM    LIS.ATS_HISTORIES
                             WHERE   RP_ACCT_ID = {realPropertyAccountId}
                             ORDER BY AHST_DT DESC";

                return await connection.QueryAsync<ATSHistory>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT  ATS_HIST_ID, RP_ACCT_ID, PP_ACCT_ID, AHST_NUM, AHST_TYPE, AHST_DT, AHST_USER, CREATED_BY, CREATED_DT, MODIFIED_BY, MODIFIED_DT, LID_ACCT_ID, ACCT_GROUP_ID
                             FROM    ATS_HISTORIES H
                             WHERE   H.RP_ACCT_ID = {realPropertyAccountId}
                             ORDER BY H.AHST_DT DESC";

                return await connection.QueryAsync<ATSHistory>(sql).ConfigureAwait(false);
            }
        }
    }
}
