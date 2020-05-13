using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class CadastralDoc : BaseTableClass
    {
        #region Fields
        public int CADASTRAL_DOC_ID { get; set; }
        public int CADASTRAL_ACTN_ID { get; set; }
        public string DOC_NO { get; set; }
        public string DOC_TYPE { get; set; }
        public string AUDITOR_FILE_NO { get; set; }
        public string DESCRIPTION { get; set; }
        #endregion

        public static async Task<IEnumerable<CadastralDoc>> GetAsync(int cadastralActionId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: Verify this works in MS-SQL.
                var sql = $@"SELECT CD.CADASTRAL_DOC_ID,
                                      CD.CADASTRAL_ACTN_ID,
                                      CD.DOC_NO,
                                      CD.DOC_TYPE,
                                      CD.AUDITOR_FILE_NO,
                                      CDT.DESCRIPTION
                                    FROM CADASTRAL_DOCS CD
                                    INNER JOIN CADASTRAL_DOC_TYPES CDT
                                    ON (CD.DOC_TYPE         = CDT.CODE_TEXT)
                                    WHERE CADASTRAL_ACTN_ID = {cadastralActionId}";

                return await connection.QueryAsync<CadastralDoc>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT CD.CADASTRAL_DOC_ID,
                                      CD.CADASTRAL_ACTN_ID,
                                      CD.DOC_NO,
                                      CD.DOC_TYPE,
                                      CD.AUDITOR_FILE_NO,
                                      CDT.DESCRIPTION
                                    FROM CADASTRAL_DOCS CD
                                    INNER JOIN CADASTRAL_DOC_TYPES CDT
                                    ON (CD.DOC_TYPE         = CDT.CODE_TEXT)
                                    WHERE CADASTRAL_ACTN_ID = {cadastralActionId}";

                return await connection.QueryAsync<CadastralDoc>(sql).ConfigureAwait(false);
            }
        }
    }
}
