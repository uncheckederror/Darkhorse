using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class RealPropertyAccountsFilter : BaseTableClass
    {
        #region Fields

        public char? ACCT_STATUS { get; set; }
        public int RP_ACCT_ID { get; set; }
        public string ACCT_NO { get; set; }
        public int? CONTACT_ID { get; set; }
        public string CONTACT_NAME { get; set; }
        public string MISC_LINE1 { get; set; }
        public string CONTACT_TYPE { get; set; }
        public string STREET_NO { get; set; }
        public string STREET_NAME { get; set; }
        public string STREET_ADDR { get; set; }
        public string SEC_TWN_RNG { get; set; }
        public string QUARTER_SECTION { get; set; }
        public int RP_ACCT_OWNER_ID { get; set; }

        #endregion

        /// <summary>
        /// Get a list of Real Accounts by their Account Numbers.
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<RealPropertyAccountsFilter>> GetByAccountNumberAsync(string accountNumber, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT  ACCT_STATUS, RP_ACCT_ID, ACCT_NO, CONTACT_ID, CONTACT_NAME, MISC_LINE1, CONTACT_TYPE, STREET_NO, STREET_NAME, STREET_ADDR, SEC_TWN_RNG, QUARTER_SECTION, RP_ACCT_OWNER_ID
                             FROM    LIS.REAL_PROP_ACCT_FILTER_VW 
                             WHERE   ACCT_NO LIKE '{accountNumber}%'";

                return await connection.QueryAsync<RealPropertyAccountsFilter>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT  ACCT_STATUS, RP_ACCT_ID, ACCT_NO, CONTACT_ID, CONTACT_NAME, MISC_LINE1, CONTACT_TYPE, STREET_NO, STREET_NAME, STREET_ADDR, SEC_TWN_RNG, QUARTER_SECTION, RP_ACCT_OWNER_ID
                             FROM    REAL_PROP_ACCT_FILTER_VW 
                             WHERE   REAL_PROP_ACCT_FILTER_VW.ACCT_NO LIKE '{accountNumber}%'";

                return await connection.QueryAsync<RealPropertyAccountsFilter>(sql).ConfigureAwait(false);
            }
        }

        public static async Task<IEnumerable<RealPropertyAccountsFilter>> GetByRpAcctIdAsync(int realPropertyAccountId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT  ACCT_STATUS, RP_ACCT_ID, ACCT_NO, CONTACT_ID, CONTACT_NAME, MISC_LINE1, CONTACT_TYPE, STREET_NO, STREET_NAME, STREET_ADDR, SEC_TWN_RNG, QUARTER_SECTION, RP_ACCT_OWNER_ID
                             FROM    LIS.REAL_PROP_ACCT_FILTER_VW 
                             WHERE   RP_ACCT_ID = {realPropertyAccountId}";

                return await connection.QueryAsync<RealPropertyAccountsFilter>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT  ACCT_STATUS, RP_ACCT_ID, ACCT_NO, CONTACT_ID, CONTACT_NAME, MISC_LINE1, CONTACT_TYPE, STREET_NO, STREET_NAME, STREET_ADDR, SEC_TWN_RNG, QUARTER_SECTION, RP_ACCT_OWNER_ID
                             FROM    REAL_PROP_ACCT_FILTER_VW 
                             WHERE   REAL_PROP_ACCT_FILTER_VW.RP_ACCT_ID = {realPropertyAccountId}";

                return await connection.QueryAsync<RealPropertyAccountsFilter>(sql).ConfigureAwait(false);
            }
        }

        public static async Task<IEnumerable<RealPropertyAccountsFilter>> GetByNameAsync(string contact, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT ACCT_STATUS,
                                      RP_ACCT_ID,
                                      ACCT_NO,
                                      CONTACT_ID,
                                      CONTACT_NAME,
                                      MISC_LINE1,
                                      CONTACT_TYPE,
                                      STREET_NO,
                                      STREET_NAME,
                                      STREET_ADDR,
                                      SEC_TWN_RNG,
                                      QUARTER_SECTION,
                                      RP_ACCT_OWNER_ID
                                    FROM LIS.REAL_PROP_ACCT_FILTER_VW
                                    WHERE CONTACT_NAME LIKE '{contact}%'";

                return await connection.QueryAsync<RealPropertyAccountsFilter>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT ACCT_STATUS,
                                      RP_ACCT_ID,
                                      ACCT_NO,
                                      CONTACT_ID,
                                      CONTACT_NAME,
                                      MISC_LINE1,
                                      CONTACT_TYPE,
                                      STREET_NO,
                                      STREET_NAME,
                                      STREET_ADDR,
                                      SEC_TWN_RNG,
                                      QUARTER_SECTION,
                                      RP_ACCT_OWNER_ID
                                    FROM LIS.REAL_PROP_ACCT_FILTER_VW
                                    WHERE CONTACT_NAME LIKE '{contact}%'";

                return await connection.QueryAsync<RealPropertyAccountsFilter>(sql).ConfigureAwait(false);
            }
        }

        public static async Task<IEnumerable<RealPropertyAccountsFilter>> GetByAddressAsync(string streetNumber, string streetName, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT ACCT_STATUS,
                                      RP_ACCT_ID,
                                      ACCT_NO,
                                      CONTACT_ID,
                                      CONTACT_NAME,
                                      MISC_LINE1,
                                      CONTACT_TYPE,
                                      STREET_NO,
                                      STREET_NAME,
                                      STREET_ADDR,
                                      SEC_TWN_RNG,
                                      QUARTER_SECTION,
                                      RP_ACCT_OWNER_ID
                                    FROM LIS.REAL_PROP_ACCT_FILTER_VW
                                    WHERE STREET_NO LIKE '{streetNumber}%'
                                    AND STREET_NAME LIKE '{streetName}%'";

                return await connection.QueryAsync<RealPropertyAccountsFilter>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT ACCT_STATUS,
                                      RP_ACCT_ID,
                                      ACCT_NO,
                                      CONTACT_ID,
                                      CONTACT_NAME,
                                      MISC_LINE1,
                                      CONTACT_TYPE,
                                      STREET_NO,
                                      STREET_NAME,
                                      STREET_ADDR,
                                      SEC_TWN_RNG,
                                      QUARTER_SECTION,
                                      RP_ACCT_OWNER_ID
                                    FROM LIS.REAL_PROP_ACCT_FILTER_VW
                                    WHERE STREET_NO LIKE '{streetNumber}%'
                                    AND STREET_NAME LIKE '{streetName}%'";

                return await connection.QueryAsync<RealPropertyAccountsFilter>(sql).ConfigureAwait(false);
            }
        }
    }
}
