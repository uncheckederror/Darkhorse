﻿using Dapper;

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

        // From CADASTRAL_ACCTS
        public int CADASTRAL_ACCT_ID { get; set; }
        public int RP_ACCT_ID { get; set; }
        public char ORIG_NEW { get; set; }
        public char? COPY_CHAR_FLAG { get; set; }

        // From RP_ACCTS
        public string ACCT_NO { get; set; }

        public static async Task<IEnumerable<CadastralAction>> GetAllAsync(DateTime minimumTaxYear, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: Not sure if this works in MS-SQL
                var sql = $@"SELECT CADAC.CADASTRAL_ACTN_ID,
                                      CADAC.CAD_ACTN_TYPE,
                                      CADAC.CAD_ACTN_RSN,
                                      CADAC.PLAT_ID,
                                      CADAC.CAD_ACTN_NO,
                                      CADAC.FINALIZED_DT,
                                      CADAC.CANCEL_DT,
                                      CADAC.CREATED_BY,
                                      CADAC.CREATED_DT,
                                      CADAC.MODIFIED_BY,
                                      CADAC.MODIFIED_DT,
                                      CADAC.CAD_ACTN_EFF_YR,
                                      CADAC.NO_STMT,
                                      CADAC.ACCTS_LOCKED,
                                      CADAC.COMPLETED_DT,
                                      CATS.CADASTRAL_ACCT_ID,
                                      CATS.RP_ACCT_ID,
                                      CATS.ORIG_NEW,
                                      CATS.COPY_CHAR_FLAG,
                                      RA.ACCT_NO
                                    FROM CADASTRAL_ACTNS CADAC
                                    INNER JOIN CADASTRAL_ACCTS CATS
                                    ON (CADAC.CADASTRAL_ACTN_ID = CATS.CADASTRAL_ACTN_ID)
                                    INNER JOIN RP_ACCTS RA
                                    ON (CATS.RP_ACCT_ID = RA.RP_ACCT_ID)
                                    WHERE CAD_ACTN_EFF_YR >= {minimumTaxYear.Year}
                                    ORDER BY CATS.CADASTRAL_ACCT_ID DESC";

                return await connection.QueryAsync<CadastralAction>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT CADAC.CADASTRAL_ACTN_ID,
                                      CADAC.CAD_ACTN_TYPE,
                                      CADAC.CAD_ACTN_RSN,
                                      CADAC.PLAT_ID,
                                      CADAC.CAD_ACTN_NO,
                                      CADAC.FINALIZED_DT,
                                      CADAC.CANCEL_DT,
                                      CADAC.CREATED_BY,
                                      CADAC.CREATED_DT,
                                      CADAC.MODIFIED_BY,
                                      CADAC.MODIFIED_DT,
                                      CADAC.CAD_ACTN_EFF_YR,
                                      CADAC.NO_STMT,
                                      CADAC.ACCTS_LOCKED,
                                      CADAC.COMPLETED_DT,
                                      CATS.CADASTRAL_ACCT_ID,
                                      CATS.RP_ACCT_ID,
                                      CATS.ORIG_NEW,
                                      CATS.COPY_CHAR_FLAG,
                                      RA.ACCT_NO
                                    FROM CADASTRAL_ACTNS CADAC
                                    INNER JOIN CADASTRAL_ACCTS CATS
                                    ON (CADAC.CADASTRAL_ACTN_ID = CATS.CADASTRAL_ACTN_ID)
                                    INNER JOIN RP_ACCTS RA
                                    ON (CATS.RP_ACCT_ID = RA.RP_ACCT_ID)
                                    WHERE CAD_ACTN_EFF_YR >= {minimumTaxYear.Year}
                                    ORDER BY CATS.CADASTRAL_ACCT_ID DESC";

                return await connection.QueryAsync<CadastralAction>(sql).ConfigureAwait(false);
            }
        }

        public static async Task<IEnumerable<CadastralAction>> GetByIdAsync(DateTime minimumTaxYear, int cadastralActionNumber, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: Not sure if this works in MS-SQL
                var sql = $@"SELECT CADAC.CADASTRAL_ACTN_ID,
                                      CADAC.CAD_ACTN_TYPE,
                                      CADAC.CAD_ACTN_RSN,
                                      CADAC.PLAT_ID,
                                      CADAC.CAD_ACTN_NO,
                                      CADAC.FINALIZED_DT,
                                      CADAC.CANCEL_DT,
                                      CADAC.CREATED_BY,
                                      CADAC.CREATED_DT,
                                      CADAC.MODIFIED_BY,
                                      CADAC.MODIFIED_DT,
                                      CADAC.CAD_ACTN_EFF_YR,
                                      CADAC.NO_STMT,
                                      CADAC.ACCTS_LOCKED,
                                      CADAC.COMPLETED_DT,
                                      CATS.CADASTRAL_ACCT_ID,
                                      CATS.RP_ACCT_ID,
                                      CATS.ORIG_NEW,
                                      CATS.COPY_CHAR_FLAG,
                                      RA.ACCT_NO
                                    FROM CADASTRAL_ACTNS CADAC
                                    INNER JOIN CADASTRAL_ACCTS CATS
                                    ON (CADAC.CADASTRAL_ACTN_ID = CATS.CADASTRAL_ACTN_ID)
                                    INNER JOIN RP_ACCTS RA
                                    ON (CATS.RP_ACCT_ID = RA.RP_ACCT_ID)
                                    WHERE CADAC.CAD_ACTN_NO LIKE '{cadastralActionNumber}%'
                                    AND CAD_ACTN_EFF_YR >= {minimumTaxYear.Year}";

                return await connection.QueryAsync<CadastralAction>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT CADAC.CADASTRAL_ACTN_ID,
                                      CADAC.CAD_ACTN_TYPE,
                                      CADAC.CAD_ACTN_RSN,
                                      CADAC.PLAT_ID,
                                      CADAC.CAD_ACTN_NO,
                                      CADAC.FINALIZED_DT,
                                      CADAC.CANCEL_DT,
                                      CADAC.CREATED_BY,
                                      CADAC.CREATED_DT,
                                      CADAC.MODIFIED_BY,
                                      CADAC.MODIFIED_DT,
                                      CADAC.CAD_ACTN_EFF_YR,
                                      CADAC.NO_STMT,
                                      CADAC.ACCTS_LOCKED,
                                      CADAC.COMPLETED_DT,
                                      CATS.CADASTRAL_ACCT_ID,
                                      CATS.RP_ACCT_ID,
                                      CATS.ORIG_NEW,
                                      CATS.COPY_CHAR_FLAG,
                                      RA.ACCT_NO
                                    FROM CADASTRAL_ACTNS CADAC
                                    INNER JOIN CADASTRAL_ACCTS CATS
                                    ON (CADAC.CADASTRAL_ACTN_ID = CATS.CADASTRAL_ACTN_ID)
                                    INNER JOIN RP_ACCTS RA
                                    ON (CATS.RP_ACCT_ID = RA.RP_ACCT_ID)
                                    WHERE CADAC.CAD_ACTN_NO LIKE '{cadastralActionNumber}%'
                                    AND CAD_ACTN_EFF_YR >= {minimumTaxYear.Year}";

                return await connection.QueryAsync<CadastralAction>(sql).ConfigureAwait(false);
            }
        }

    }
}
