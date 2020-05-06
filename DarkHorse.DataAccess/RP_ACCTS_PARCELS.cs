using Dapper;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class RealAccountParcel : BaseTableClass
    {
        #region Fields

        public int RP_ACCT_ID { get; set; }
        public string Z_CODE { get; set; }
        public string Z_TYPE { get; set; }
        public string GMA_JURISDICTION { get; set; }
        public string RNC_CODE { get; set; }
        public string CNC_CODE { get; set; }
        public string NEW_REC { get; set; }
        public DateTime? MODIFIED_DT { get; set; }

        #endregion

        public static async Task<IEnumerable<RealAccountParcel>> GetAsync(int realAccountId, IDbConnection dbConnection)
        {
            var sql = (dbConnection is SqlConnection)
                ? $@"SELECT RP_ACCT_ID, Z_CODE, Z_TYPE, GMA_JURISDICTION, RNC_CODE, CNC_CODE, NEW_REC, MODIFIED_DT
                            FROM RP_ACCTS_PARCELS
                            WHERE RP_ACCT_ID = {realAccountId}"
                : $@"SELECT RP_ACCT_ID, Z_CODE, Z_TYPE, GMA_JURISDICTION, RNC_CODE, CNC_CODE, NEW_REC, MODIFIED_DT
                            FROM RP_ACCTS_PARCELS
                            WHERE RP_ACCT_ID = {realAccountId}";

            return await Connection(dbConnection).QueryAsync<RealAccountParcel>(sql).ConfigureAwait(false);
        }
    }
}
