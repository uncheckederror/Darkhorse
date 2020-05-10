using Dapper;

using Oracle.ManagedDataAccess.Client;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class CadastralAction : BaseTableClass
    {
        public int CADASTRAL_ACTN_ID { get; set; }
        public string CAD_ACTN_TYPE { get; set; }
        public string CAD_ACTN_RSN { get; set; }
        public int? PLAT_ID { get; set; }
        public string CAD_ACTN_NO { get; set; }
        public DateTime? FINALIZED_DT { get; set; }
        public DateTime? CANCEL_DT { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DT { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DT { get; set; }
        public int CAD_ACTN_EFF_YR { get; set; }
        public char NO_STMT { get; set; }
        public char ACCTS_LOCKED { get; set; }
        public DateTime? COMPLETED_DT { get; set; }

        public static async Task<IEnumerable<CadastralAction>> GetAllAsync(DateTime minimumTaxYear, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: Not sure if this works in MS-SQL
                var sql = $@"SELECT CADASTRAL_ACTN_ID,
                                      CAD_ACTN_TYPE,
                                      CAD_ACTN_RSN,
                                      PLAT_ID,
                                      CAD_ACTN_NO,
                                      FINALIZED_DT,
                                      CANCEL_DT,
                                      CREATED_BY,
                                      CREATED_DT,
                                      MODIFIED_BY,
                                      MODIFIED_DT,
                                      CAD_ACTN_EFF_YR,
                                      NO_STMT,
                                      ACCTS_LOCKED,
                                      COMPLETED_DT
                                    FROM CADASTRAL_ACTNS CADAC
                                    WHERE CAD_ACTN_EFF_YR >= {minimumTaxYear.Year}";

                return await connection.QueryAsync<CadastralAction>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT CADASTRAL_ACTN_ID,
                                      CAD_ACTN_TYPE,
                                      CAD_ACTN_RSN,
                                      PLAT_ID,
                                      CAD_ACTN_NO,
                                      FINALIZED_DT,
                                      CANCEL_DT,
                                      CREATED_BY,
                                      CREATED_DT,
                                      MODIFIED_BY,
                                      MODIFIED_DT,
                                      CAD_ACTN_EFF_YR,
                                      NO_STMT,
                                      ACCTS_LOCKED,
                                      COMPLETED_DT
                                    FROM CADASTRAL_ACTNS CADAC
                                    WHERE CAD_ACTN_EFF_YR >= {minimumTaxYear.Year}";

                return await connection.QueryAsync<CadastralAction>(sql).ConfigureAwait(false);
            }
        }

        public static async Task<IEnumerable<CadastralAction>> GetByIdAsync(DateTime minimumTaxYear, int cadastralActionNumber, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: Not sure if this works in MS-SQL
                var sql = $@"SELECT CADASTRAL_ACTN_ID,
                                      CAD_ACTN_TYPE,
                                      CAD_ACTN_RSN,
                                      PLAT_ID,
                                      CAD_ACTN_NO,
                                      FINALIZED_DT,
                                      CANCEL_DT,
                                      CREATED_BY,
                                      CREATED_DT,
                                      MODIFIED_BY,
                                      MODIFIED_DT,
                                      CAD_ACTN_EFF_YR,
                                      NO_STMT,
                                      ACCTS_LOCKED,
                                      COMPLETED_DT
                                    FROM CADASTRAL_ACTNS CADAC
                                    WHERE CADAC.CAD_ACTN_NO LIKE '{cadastralActionNumber}%'
                                    AND CAD_ACTN_EFF_YR >= {minimumTaxYear.Year}";

                return await connection.QueryAsync<CadastralAction>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT CADASTRAL_ACTN_ID,
                                      CAD_ACTN_TYPE,
                                      CAD_ACTN_RSN,
                                      PLAT_ID,
                                      CAD_ACTN_NO,
                                      FINALIZED_DT,
                                      CANCEL_DT,
                                      CREATED_BY,
                                      CREATED_DT,
                                      MODIFIED_BY,
                                      MODIFIED_DT,
                                      CAD_ACTN_EFF_YR,
                                      NO_STMT,
                                      ACCTS_LOCKED,
                                      COMPLETED_DT
                                    FROM CADASTRAL_ACTNS CADAC
                                    WHERE CADAC.CAD_ACTN_NO LIKE '{cadastralActionNumber}%'
                                    AND CAD_ACTN_EFF_YR >= {minimumTaxYear.Year}";

                return await connection.QueryAsync<CadastralAction>(sql).ConfigureAwait(false);
            }
        }

    }
}
