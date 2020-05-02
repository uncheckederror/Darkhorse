using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class RealAccount : BaseTableClass
    {
        #region Fields
        public int RP_ACCT_ID { get; set; }
        public string ACCT_NO { get; set; }
        public string WORK_GROUP { get; set; }
        public string QUARTER_SECTION { get; set; }
        public string MAP_NO { get; set; }
        public DateTime? REFERENCE_DT { get; set; }
        public DateTime? INACTIVE_DT { get; set; }
        public string SEC_TWN_RNG { get; set; }
        public string SUB_ACCT_TYPE { get; set; }
        public int? LAND_FOR_BLDG_ID { get; set; }
        public char? PP_AS_RP_FLAG { get; set; }
        public int? BILLING_ID { get; set; }
        public int? EXEMPTION_ID { get; set; }
        public string NEIGHBORHOOD_CODE { get; set; }
        public string SUBDIVISION { get; set; }
        public string BLOCK_LOT { get; set; }
        public string BLOCK_TYPE_ID { get; set; }
        public string LOT_TYPE_ID { get; set; }
        public string LOT { get; set; }
        public string MESSAGE_ID { get; set; }
        public DateTime? LAST_UPDATE_FOR_EXPORT { get; set; }

        #endregion

        /// <summary>
        /// Gets a Real Property account by its id.
        /// </summary>
        /// <param name="rpAcctId"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static async Task<RealAccount> GetAsync(int rpAcctId, IDbConnection dbConnection)
        {
            var result = default(RealAccount);

            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT  RP_ACCT_ID, ACCT_NO, WORK_GROUP, QUARTER_SECTION,
                                     MAP_NO, REFERENCE_DT, INACTIVE_DT, CREATED_BY, CREATED_DT,
                                     MODIFIED_BY, MODIFIED_DT, SEC_TWN_RNG, SUB_ACCT_TYPE,
                                     LAND_FOR_BLDG_ID, PP_AS_RP_FLAG, BILLING_ID, EXEMPTION_ID,
                                     NEIGHBORHOOD_CODE, ROAD_SERVICE_AREA, SUBDIVISION, BLOCK_LOT,
                                     BLOCK_TYPE_ID, LOT_TYPE_ID, LOT, MESSAGE_ID,
                                     LAST_UPDATE_FOR_EXPORT
                             FROM    LIS.RP_ACCTS
                             WHERE   RP_ACCT_ID = {rpAcctId}";

                result = await connection.QuerySingleOrDefaultAsync<RealAccount>(sql).ConfigureAwait(false) ?? new RealAccount();
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT  RP_ACCT_ID, ACCT_NO, WORK_GROUP, QUARTER_SECTION, 
                                     MAP_NO, REFERENCE_DT, INACTIVE_DT, CREATED_BY, CREATED_DT, 
                                     MODIFIED_BY, MODIFIED_DT, SEC_TWN_RNG, SUB_ACCT_TYPE, 
                                     LAND_FOR_BLDG_ID, PP_AS_RP_FLAG, BILLING_ID, EXEMPTION_ID, 
                                     NEIGHBORHOOD_CODE, ROAD_SERVICE_AREA, SUBDIVISION, BLOCK_LOT, 
                                     BLOCK_TYPE_ID, LOT_TYPE_ID, LOT, MESSAGE_ID, 
                                     LAST_UPDATE_FOR_EXPORT 
                             FROM    LIS.RP_ACCTS 
                             WHERE   RP_ACCT_ID = {rpAcctId}";

                result = await connection.QuerySingleOrDefaultAsync<RealAccount>(sql).ConfigureAwait(false) ?? new RealAccount();
            }

            result.SEC_TWN_RNG = string.IsNullOrWhiteSpace(result.SEC_TWN_RNG) ? "Not Found" : result.SEC_TWN_RNG.Trim();

            return result;
        }

        /// <summary>
        /// Gets a Real Property account by the parcel number that's associated with it.
        /// </summary>
        /// <param name="parcelNumber"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<RealAccount>> GetAsync(string parcelNumber, IDbConnection dbConnection)
        {
            var results = new List<RealAccount>();

            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT  RP_ACCT_ID, ACCT_NO, WORK_GROUP, QUARTER_SECTION, 
                                     MAP_NO, REFERENCE_DT, INACTIVE_DT, CREATED_BY, CREATED_DT, 
                                     MODIFIED_BY, MODIFIED_DT, SEC_TWN_RNG, SUB_ACCT_TYPE, 
                                     LAND_FOR_BLDG_ID, PP_AS_RP_FLAG, BILLING_ID, EXEMPTION_ID, 
                                     NEIGHBORHOOD_CODE, ROAD_SERVICE_AREA, SUBDIVISION, BLOCK_LOT, 
                                     BLOCK_TYPE_ID, LOT_TYPE_ID, LOT, MESSAGE_ID, 
                                     LAST_UPDATE_FOR_EXPORT 
                             FROM    LIS.RP_ACCTS 
                             WHERE   ACCT_NO LIKE '{parcelNumber}%'";

                results.AddRange(await connection.QueryAsync<RealAccount>(sql).ConfigureAwait(false));
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT  RP_ACCT_ID, ACCT_NO, WORK_GROUP, QUARTER_SECTION, 
                                     MAP_NO, REFERENCE_DT, INACTIVE_DT, CREATED_BY, CREATED_DT, 
                                     MODIFIED_BY, MODIFIED_DT, SEC_TWN_RNG, SUB_ACCT_TYPE, 
                                     LAND_FOR_BLDG_ID, PP_AS_RP_FLAG, BILLING_ID, EXEMPTION_ID, 
                                     NEIGHBORHOOD_CODE, ROAD_SERVICE_AREA, SUBDIVISION, BLOCK_LOT, 
                                     BLOCK_TYPE_ID, LOT_TYPE_ID, LOT, MESSAGE_ID, 
                                     LAST_UPDATE_FOR_EXPORT 
                             FROM    LIS.RP_ACCTS 
                             WHERE   ACCT_NO LIKE '{parcelNumber}%'";

                results.AddRange(await connection.QueryAsync<RealAccount>(sql).ConfigureAwait(false));
            }

            foreach (var result in results)
            {
                result.SEC_TWN_RNG = string.IsNullOrWhiteSpace(result.SEC_TWN_RNG) ? "Not Found" : result.SEC_TWN_RNG.Trim();
            }

            return results;
        }
    }
}
