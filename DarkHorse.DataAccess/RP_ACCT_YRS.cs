using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class RealPropertyAccountYear : BaseTableClass
    {
        #region Fields

        public string PROPERTY_CLASS { get; set; }
        public decimal? PARCEL_ACREAGE { get; set; }
        public string TAX_CODE { get; set; }
        public int RP_ACCT_YR_ID { get; set; }
        public int RP_ACCT_OWNER_ID { get; set; }
        public int? SSWM_ASMT_ID { get; set; }
        public int? FFP_ASMT_ID { get; set; }
        public string TAX_STATUS { get; set; }
        public int TAX_YR { get; set; }
        public int TAXABLE_AV { get; set; }
        public int ASSESSED_VALUE { get; set; }
        public int MARKET_VALUE { get; set; }
        public decimal RP_TAX_PAID { get; set; }
        public decimal RP_TAX_REFUND { get; set; }
        public decimal RP_INTEREST_PAID { get; set; }
        public decimal RP_PENALTY_PAID { get; set; }
        public string IMPERVIOUS_SURF_BLDG { get; set; }
        public string IMPERVIOUS_SURF_OTHER { get; set; }
        public DateTime? MEASURED_DT { get; set; }
        public decimal SSWM_ASMT_BILLED { get; set; }
        public decimal SSWM_ASMT_PAID { get; set; }
        public decimal? FFP_ACRES { get; set; }
        public decimal FFP_ASMT_BILLED { get; set; }
        public decimal FFP_ASMT_PAID { get; set; }
        public int NEW_CONSTRUCTION_AMT { get; set; }
        public char FFP_AGRMT_FLAG { get; set; }
        public char PUBLIC_OWNED_FLAG { get; set; }
        public char NON_PROFIT_FLAG { get; set; }
        public DateTime PRORATE_START_DT { get; set; }
        public DateTime? PRORATE_END_DT { get; set; }
        public int LAND_AV { get; set; }
        public int LAND_MKT_AV { get; set; }
        public int IMPR_AV { get; set; }
        public char STATE_ASSESSED_FLAG { get; set; }
        public decimal SSWM_ASMT_REFUND { get; set; }
        public decimal FFP_ASMT_REFUND { get; set; }
        public char OVERRIDE_FFP_FLAG { get; set; }
        public char OVERRIDE_SSWM_FLAG { get; set; }
        public decimal RP_TAX_BILLED { get; set; }
        public decimal? ADJ_TAX_BILLED { get; set; }
        public decimal ADJ_SSWM_BILLED { get; set; }
        public decimal ADJ_FFP_BILLED { get; set; }
        public char OVERRIDE_ASSESSED_FLAG { get; set; }
        public char OVERRIDE_CAMA_FLAG { get; set; }
        public char OVERRIDE_TAX_BILLED_FLAG { get; set; }
        public decimal PRORATE_CURR_BILLED { get; set; }
        public decimal PRORATE_PREV_BILLED { get; set; }
        public int? FROZEN_VALUE { get; set; }
        public char FED_OWNED_FLAG { get; set; }
        public decimal RP_ADVANCE_PAID { get; set; }
        public int NOX_WEED_ASMT_ID { get; set; }
        public decimal NOX_WEED_ASMT_BILLED { get; set; }
        public decimal NOX_WEED_ASMT_PAID { get; set; }
        public decimal NOX_WEED_ASMT_REFUND { get; set; }
        public decimal ADJ_NOX_WEED_BILLED { get; set; }
        public char OVERRIDE_NOX_WEED_FLAG { get; set; }

        // This is for convience.
        public decimal TaxRate { get; set; }

        #endregion

        /// <summary>
        /// Get the 
        /// </summary>
        /// <param name="realAccountOwnerId"></param>
        /// <param name="taxYear"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static async Task<RealPropertyAccountYear> GetRealAccountFiltersAsync(int realAccountOwnerId, DateTime taxYear, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT PROPERTY_CLASS, PARCEL_ACREAGE, TAX_CODE
	                         FROM   LIS.RP_ACCT_YRS
	                         WHERE  RP_ACCT_OWNER_ID = {realAccountOwnerId} AND TAX_YR = {taxYear.Year}";

                return await connection.QueryFirstOrDefaultAsync<RealPropertyAccountYear>(sql).ConfigureAwait(false) ?? new RealPropertyAccountYear();
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT property_class, parcel_acreage, tax_code
	                         FROM   rp_acct_yrs
	                         WHERE  rp_acct_owner_id = {realAccountOwnerId}
	                         AND    tax_yr = {taxYear.Year}";

                return await connection.QueryFirstOrDefaultAsync<RealPropertyAccountYear>(sql).ConfigureAwait(false) ?? new RealPropertyAccountYear();
            }
        }

        public static async Task<IEnumerable<RealPropertyAccountYear>> GetAsync(int realAccountId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT Y.RP_ACCT_YR_ID,
                          Y.RP_ACCT_OWNER_ID,
                          Y.SSWM_ASMT_ID,
                          Y.FFP_ASMT_ID,
                          Y.TAX_STATUS,
                          Y.TAX_CODE,
                          Y.TAX_YR,
                          Y.PROPERTY_CLASS,
                          Y.TAXABLE_AV,
                          Y.ASSESSED_VALUE,
                          Y.MARKET_VALUE,
                          Y.RP_TAX_PAID,
                          Y.RP_TAX_REFUND,
                          Y.RP_INTEREST_PAID,
                          Y.RP_PENALTY_PAID,
                          Y.IMPERVIOUS_SURF_BLDG,
                          Y.IMPERVIOUS_SURF_OTHER,
                          Y.MEASURED_DT,
                          Y.SSWM_ASMT_BILLED,
                          Y.SSWM_ASMT_PAID,
                          Y.FFP_ACRES,
                          Y.FFP_ASMT_BILLED,
                          Y.FFP_ASMT_PAID,
                          Y.NEW_CONSTRUCTION_AMT,
                          Y.FFP_AGRMT_FLAG,
                          Y.PUBLIC_OWNED_FLAG,
                          Y.NON_PROFIT_FLAG,
                          Y.PRORATE_START_DT,
                          Y.PRORATE_END_DT,
                          Y.CREATED_BY,
                          Y.CREATED_DT,
                          Y.MODIFIED_BY,
                          Y.MODIFIED_DT,
                          Y.LAND_AV,
                          Y.LAND_MKT_AV,
                          Y.IMPR_AV,
                          Y.STATE_ASSESSED_FLAG,
                          Y.PARCEL_ACREAGE,
                          Y.SSWM_ASMT_REFUND,
                          Y.FFP_ASMT_REFUND,
                          Y.OVERRIDE_FFP_FLAG,
                          Y.OVERRIDE_SSWM_FLAG,
                          Y.RP_TAX_BILLED,
                          Y.ADJ_TAX_BILLED,
                          Y.ADJ_SSWM_BILLED,
                          Y.ADJ_FFP_BILLED,
                          Y.OVERRIDE_ASSESSED_FLAG,
                          Y.OVERRIDE_CAMA_FLAG,
                          Y.OVERRIDE_TAX_BILLED_FLAG,
                          Y.PRORATE_CURR_BILLED,
                          Y.PRORATE_PREV_BILLED,
                          Y.FROZEN_VALUE,
                          Y.FED_OWNED_FLAG,
                          Y.RP_ADVANCE_PAID,
                          Y.PARCEL_ACRES,
                          Y.NOX_WEED_ASMT_ID,
                          Y.NOX_WEED_ASMT_BILLED,
                          Y.NOX_WEED_ASMT_PAID,
                          Y.NOX_WEED_ASMT_REFUND,
                          Y.ADJ_NOX_WEED_BILLED,
                          Y.OVERRIDE_NOX_WEED_FLAG
                        FROM RP_ACCT_YRS Y,
                          RP_ACCT_OWNERS
                        WHERE Y.RP_ACCT_OWNER_ID       = RP_ACCT_OWNERS.RP_ACCT_OWNER_ID
                        AND (RP_ACCT_OWNERS.RP_ACCT_ID = {realAccountId})
                        AND (Y.PRORATE_END_DT         IS NULL)
                        ORDER BY Y.TAX_YR DESC";

                return await connection.QueryAsync<RealPropertyAccountYear>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT Y.RP_ACCT_YR_ID,
                          Y.RP_ACCT_OWNER_ID,
                          Y.SSWM_ASMT_ID,
                          Y.FFP_ASMT_ID,
                          Y.TAX_STATUS,
                          Y.TAX_CODE,
                          Y.TAX_YR,
                          Y.PROPERTY_CLASS,
                          Y.TAXABLE_AV,
                          Y.ASSESSED_VALUE,
                          Y.MARKET_VALUE,
                          Y.RP_TAX_PAID,
                          Y.RP_TAX_REFUND,
                          Y.RP_INTEREST_PAID,
                          Y.RP_PENALTY_PAID,
                          Y.IMPERVIOUS_SURF_BLDG,
                          Y.IMPERVIOUS_SURF_OTHER,
                          Y.MEASURED_DT,
                          Y.SSWM_ASMT_BILLED,
                          Y.SSWM_ASMT_PAID,
                          Y.FFP_ACRES,
                          Y.FFP_ASMT_BILLED,
                          Y.FFP_ASMT_PAID,
                          Y.NEW_CONSTRUCTION_AMT,
                          Y.FFP_AGRMT_FLAG,
                          Y.PUBLIC_OWNED_FLAG,
                          Y.NON_PROFIT_FLAG,
                          Y.PRORATE_START_DT,
                          Y.PRORATE_END_DT,
                          Y.CREATED_BY,
                          Y.CREATED_DT,
                          Y.MODIFIED_BY,
                          Y.MODIFIED_DT,
                          Y.LAND_AV,
                          Y.LAND_MKT_AV,
                          Y.IMPR_AV,
                          Y.STATE_ASSESSED_FLAG,
                          Y.PARCEL_ACREAGE,
                          Y.SSWM_ASMT_REFUND,
                          Y.FFP_ASMT_REFUND,
                          Y.OVERRIDE_FFP_FLAG,
                          Y.OVERRIDE_SSWM_FLAG,
                          Y.RP_TAX_BILLED,
                          Y.ADJ_TAX_BILLED,
                          Y.ADJ_SSWM_BILLED,
                          Y.ADJ_FFP_BILLED,
                          Y.OVERRIDE_ASSESSED_FLAG,
                          Y.OVERRIDE_CAMA_FLAG,
                          Y.OVERRIDE_TAX_BILLED_FLAG,
                          Y.PRORATE_CURR_BILLED,
                          Y.PRORATE_PREV_BILLED,
                          Y.FROZEN_VALUE,
                          Y.FED_OWNED_FLAG,
                          Y.RP_ADVANCE_PAID,
                          Y.PARCEL_ACRES,
                          Y.NOX_WEED_ASMT_ID,
                          Y.NOX_WEED_ASMT_BILLED,
                          Y.NOX_WEED_ASMT_PAID,
                          Y.NOX_WEED_ASMT_REFUND,
                          Y.ADJ_NOX_WEED_BILLED,
                          Y.OVERRIDE_NOX_WEED_FLAG
                        FROM RP_ACCT_YRS Y,
                          RP_ACCT_OWNERS
                        WHERE Y.RP_ACCT_OWNER_ID       = RP_ACCT_OWNERS.RP_ACCT_OWNER_ID
                        AND (RP_ACCT_OWNERS.RP_ACCT_ID = {realAccountId})
                        AND (Y.PRORATE_END_DT         IS NULL)
                        ORDER BY Y.TAX_YR DESC";

                return await connection.QueryAsync<RealPropertyAccountYear>(sql).ConfigureAwait(false);
            }
        }
    }
}
