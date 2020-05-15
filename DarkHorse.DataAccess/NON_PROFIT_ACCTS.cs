using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class NonprofitAccount : BaseTableClass
    {
        #region Fields
        public int NON_PROFIT_ACCT_ID { get; set; }
        public int NON_PROFIT_ID { get; set; }
        public int RP_ACCT_OWNER_ID { get; set; }
        public string DOR_PROPERTY_NO { get; set; }
        public string REGISTRATION_NO { get; set; }
        public string APPLICANT { get; set; }
        public string OCCUPANT { get; set; }
        public string ACCT_NO { get; set; }
        // Account Year query
        public int NON_PROFIT_ACCT_YR_ID { get; set; }
        public int TAX_YR { get; set; }
        public DateTime? DETERMINATION_DT { get; set; }
        public char EXEMPT_STATUS_FLAG { get; set; }
        // Flag query
        public char NON_PROFIT_FLAG { get; set; }
        #endregion

        public static async Task<IEnumerable<NonprofitAccount>> GetAllAsync(IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT NON_PROFIT_ID, REGISTRATION_NO, APPLICANT, OCCUPANT, CREATED_BY, CREATED_DT, MODIFIED_BY, MODIFIED_DT FROM NON_PROFIT_APPLS NPA";

                return await connection.QueryAsync<NonprofitAccount>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT NON_PROFIT_ID, REGISTRATION_NO, APPLICANT, OCCUPANT, CREATED_BY, CREATED_DT, MODIFIED_BY, MODIFIED_DT FROM NON_PROFIT_APPLS NPA";

                return await connection.QueryAsync<NonprofitAccount>(sql).ConfigureAwait(false);
            }
        }

        public static async Task<IEnumerable<NonprofitAccount>> GetAsync(int nonProfitId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT NPA.NON_PROFIT_ACCT_ID,
                                      NPA.NON_PROFIT_ID,
                                      NPA.RP_ACCT_OWNER_ID,
                                      NPA.DOR_PROPERTY_NO,
                                      NPAL.REGISTRATION_NO,
                                      NPAL.APPLICANT,
                                      NPAL.OCCUPANT,
                                      RA.ACCT_NO
                                    FROM NON_PROFIT_ACCTS NPA
                                    INNER JOIN NON_PROFIT_APPLS NPAL
                                    ON (NPA.NON_PROFIT_ID = NPAL.NON_PROFIT_ID)
                                    INNER JOIN RP_ACCT_OWNERS RAO
                                    ON (NPA.RP_ACCT_OWNER_ID = RAO.RP_ACCT_OWNER_ID)
                                    INNER JOIN RP_ACCTS RA
                                    ON (RAO.RP_ACCT_ID = RA.RP_ACCT_ID)
                                    WHERE NPA.NON_PROFIT_ID = {nonProfitId}";

                return await connection.QueryAsync<NonprofitAccount>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT NPA.NON_PROFIT_ACCT_ID,
                                      NPA.NON_PROFIT_ID,
                                      NPA.RP_ACCT_OWNER_ID,
                                      NPA.DOR_PROPERTY_NO,
                                      NPAL.REGISTRATION_NO,
                                      NPAL.APPLICANT,
                                      NPAL.OCCUPANT,
                                      RA.ACCT_NO
                                    FROM NON_PROFIT_ACCTS NPA
                                    INNER JOIN NON_PROFIT_APPLS NPAL
                                    ON (NPA.NON_PROFIT_ID = NPAL.NON_PROFIT_ID)
                                    INNER JOIN RP_ACCT_OWNERS RAO
                                    ON (NPA.RP_ACCT_OWNER_ID = RAO.RP_ACCT_OWNER_ID)
                                    INNER JOIN RP_ACCTS RA
                                    ON (RAO.RP_ACCT_ID = RA.RP_ACCT_ID)
                                    WHERE NPA.NON_PROFIT_ID = {nonProfitId}";

                return await connection.QueryAsync<NonprofitAccount>(sql).ConfigureAwait(false);
            }
        }

        public static async Task<IEnumerable<NonprofitAccount>> GetAccountYearAsync(int nonProfitAccountId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT
                              NPAY.NON_PROFIT_ACCT_YR_ID,
                              NPAY.NON_PROFIT_ACCT_ID,
                              NPAY.TAX_YR,
                              NPAY.DETERMINATION_DT,
                              NPAY.EXEMPT_STATUS_FLAG
                            FROM NON_PROFIT_ACCT_YRS NPAY
                            WHERE NPAY.NON_PROFIT_ACCT_ID  = {nonProfitAccountId}";

                return await connection.QueryAsync<NonprofitAccount>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);


                var sql = $@"SELECT
                              NPAY.NON_PROFIT_ACCT_YR_ID,
                              NPAY.NON_PROFIT_ACCT_ID,
                              NPAY.TAX_YR,
                              NPAY.DETERMINATION_DT,
                              NPAY.EXEMPT_STATUS_FLAG
                            FROM NON_PROFIT_ACCT_YRS NPAY
                            WHERE NPAY.NON_PROFIT_ACCT_ID  = {nonProfitAccountId}";

                return await connection.QueryAsync<NonprofitAccount>(sql).ConfigureAwait(false);
            }
        }

        public static async Task<NonprofitAccount> GetAppliedExemptionAsync(int nonProfitAccountYearId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT RAY.NON_PROFIT_FLAG
                                    FROM RP_ACCT_YRS RAY
                                    INNER JOIN RP_ACCT_OWNERS RAO
                                    ON (RAO.RP_ACCT_OWNER_ID = RAY.RP_ACCT_OWNER_ID)
                                    INNER JOIN NON_PROFIT_ACCTS NPA
                                    ON (NPA.RP_ACCT_OWNER_ID = RAO.RP_ACCT_OWNER_ID)
                                    INNER JOIN NON_PROFIT_ACCT_YRS NPAY
                                    ON (NPAY.NON_PROFIT_ACCT_ID = NPA.NON_PROFIT_ACCT_ID)
                                    WHERE NPAY.NON_PROFIT_ACCT_YR_ID = {nonProfitAccountYearId}
                                    AND RAY.TAX_YR = NPAY.TAX_YR";

                return await connection.QueryFirstOrDefaultAsync<NonprofitAccount>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT RAY.NON_PROFIT_FLAG
                                    FROM RP_ACCT_YRS RAY
                                    INNER JOIN RP_ACCT_OWNERS RAO
                                    ON (RAO.RP_ACCT_OWNER_ID = RAY.RP_ACCT_OWNER_ID)
                                    INNER JOIN NON_PROFIT_ACCTS NPA
                                    ON (NPA.RP_ACCT_OWNER_ID = RAO.RP_ACCT_OWNER_ID)
                                    INNER JOIN NON_PROFIT_ACCT_YRS NPAY
                                    ON (NPAY.NON_PROFIT_ACCT_ID = NPA.NON_PROFIT_ACCT_ID)
                                    WHERE NPAY.NON_PROFIT_ACCT_YR_ID = {nonProfitAccountYearId}
                                    AND RAY.TAX_YR = NPAY.TAX_YR";

                return await connection.QueryFirstOrDefaultAsync<NonprofitAccount>(sql).ConfigureAwait(false);
            }
        }
    }
}
