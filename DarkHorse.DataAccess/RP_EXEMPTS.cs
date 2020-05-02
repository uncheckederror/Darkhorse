using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

/// <summary>
/// Account 1081256 is an example.
/// </summary>

namespace DarkHorse.DataAccess
{
    public class RealPropertyExemptions : BaseTableClass
    {
        #region Fields
        public string EXEMPT_TYPE { get; set; }
        public string DESCRIPTION { get; set; }
        public int? EXEMPT_VALUE { get; set; }
        public int REVIEW_YR { get; set; }
        public char OVERRIDE_FLAG { get; set; }
        #endregion

        public static async Task<IEnumerable<RealPropertyExemptions>> GetAsync(int realAccountOwnerId, int realAccountYearId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: Verify that this works in MS-SQL
                var sql = $@"SELECT     RPE.EXEMPT_TYPE, ET.DESCRIPTION, RPE.EXEMPT_VALUE, SRA.REVIEW_YR, RPE.OVERRIDE_FLAG 
                             FROM       RP_EXEMPTS RPE, SRCIT_APPLS SRA, EXEMPT_TYPES ET
                             WHERE      RPE.RP_ACCT_YR_ID = {realAccountYearId}
                                        AND SRA.RP_ACCT_OWNER_ID = {realAccountOwnerId}
                                        AND RPE.EXEMPT_TYPE = ET.CODE_TEXT";

                return await connection.QueryAsync<RealPropertyExemptions>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT     RPE.EXEMPT_TYPE, ET.DESCRIPTION, RPE.EXEMPT_VALUE, SRA.REVIEW_YR, RPE.OVERRIDE_FLAG 
                             FROM       RP_EXEMPTS RPE, SRCIT_APPLS SRA, EXEMPT_TYPES ET
                             WHERE      RPE.RP_ACCT_YR_ID = {realAccountYearId}
                                        AND SRA.RP_ACCT_OWNER_ID = {realAccountOwnerId}
                                        AND RPE.EXEMPT_TYPE = ET.CODE_TEXT";

                return await connection.QueryAsync<RealPropertyExemptions>(sql).ConfigureAwait(false);
            }
        }
    }
}
