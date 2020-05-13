using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class CadastralStep : BaseTableClass
    {
        #region Fields
        public int CADASTRAL_STEP_ID { get; set; }
        public int CADASTRAL_ACTN_ID { get; set; }
        public string CAD_STEP_TYPE { get; set; }
        public string CAD_STEP_ASSGN_TO { get; set; }
        public DateTime? CAD_STEP_ASSGN_DT { get; set; }
        public DateTime? CAD_STEP_COMPLETE_DT { get; set; }
        public string DESCRIPTION { get; set; }
        #endregion

        public static async Task<IEnumerable<CadastralStep>> GetAsync(int cadastralActionId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: Verify this works in MS-SQL.
                var sql = $@"SELECT CS.CADASTRAL_STEP_ID,
                                      CS.CADASTRAL_ACTN_ID,
                                      CS.CAD_STEP_TYPE,
                                      CS.CAD_STEP_ASSGN_TO,
                                      CS.CAD_STEP_ASSGN_DT,
                                      CS.CAD_STEP_COMPLETE_DT,
                                      CST.DESCRIPTION
                                    FROM CADASTRAL_STEPS CS
                                    INNER JOIN CADASTRAL_STEP_TYPES CST
                                    ON CS.CAD_STEP_TYPE     = CST.CODE_TEXT
                                    WHERE CADASTRAL_ACTN_ID = {cadastralActionId}";

                return await connection.QueryAsync<CadastralStep>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT CS.CADASTRAL_STEP_ID,
                                      CS.CADASTRAL_ACTN_ID,
                                      CS.CAD_STEP_TYPE,
                                      CS.CAD_STEP_ASSGN_TO,
                                      CS.CAD_STEP_ASSGN_DT,
                                      CS.CAD_STEP_COMPLETE_DT,
                                      CST.DESCRIPTION
                                    FROM CADASTRAL_STEPS CS
                                    INNER JOIN CADASTRAL_STEP_TYPES CST
                                    ON CS.CAD_STEP_TYPE     = CST.CODE_TEXT
                                    WHERE CADASTRAL_ACTN_ID = {cadastralActionId}";

                return await connection.QueryAsync<CadastralStep>(sql).ConfigureAwait(false);
            }
        }
    }
}
