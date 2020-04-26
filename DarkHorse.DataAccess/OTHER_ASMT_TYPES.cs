using Dapper;

using Oracle.ManagedDataAccess.Client;

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class OtherAssessmentTypes : BaseTableClass
    {
        #region Fields
        public string OTHER_ASMT_TYPE { get; set; }
        public string FUND_NO { get; set; }
        public string FUND_TITLE { get; set; }
        public string DESCRIPTION { get; set; }
        public char ACTIVE { get; set; }
        #endregion

        public static async Task<IEnumerable<OtherAssessmentTypes>> GetAllAsync(IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: Verify that this works in MS-SQL.
                var sql = $@"SELECT OAT.OTHER_ASMT_TYPE, AF.FUND_NO, AFN.DESCRIPTION AS FUND_TITLE, OAT.DESCRIPTION, AFN.ACTIVE
                            FROM OTHER_ASMT_TYPES OAT
                            INNER JOIN ASMT_FUNDS AF
                            ON AF.ASMT_FUND_ID = OAT.OTHER_ASMT_TYPE_ID
                            INNER JOIN ASMT_FUND_NOS AFN
                            ON AFN.CODE_TEXT = AF.FUND_NO";

                return await connection.QueryAsync<OtherAssessmentTypes>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT OAT.OTHER_ASMT_TYPE, AF.FUND_NO, AFN.DESCRIPTION AS FUND_TITLE, OAT.DESCRIPTION, AFN.ACTIVE
                            FROM OTHER_ASMT_TYPES OAT
                            INNER JOIN ASMT_FUNDS AF
                            ON AF.ASMT_FUND_ID = OAT.OTHER_ASMT_TYPE_ID
                            INNER JOIN ASMT_FUND_NOS AFN
                            ON AFN.CODE_TEXT = AF.FUND_NO";

                return await connection.QueryAsync<OtherAssessmentTypes>(sql).ConfigureAwait(false);
            }
        }
    }
}
