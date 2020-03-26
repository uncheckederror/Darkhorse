using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Darkhorse.DataAccess
{
    public class RealPropertyAccountYear
    {
        public int PROPERTY_CLASS { get; set; }
        public decimal PARCEL_ACREAGE { get; set; }
        public int TAX_CODE { get; set; }
        public int RP_ACCT_YR_ID { get; set; }
        public int RP_ACCT_OWNER_ID { get; set; }
        public int SSWM_ASMT_ID { get; set; }
        public int FFP_ASMT_ID { get; set; }
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
        public DateTime MEASURED_DT { get; set; }
        public decimal SSWM_ASMT_BILLED { get; set; }
        public decimal SSWM_ASMT_PAID { get; set; }
        public decimal FFP_ACRES { get; set; }
        public decimal FFP_ASMT_BILLED { get; set; }
        public decimal FFP_ASMT_PAID { get; set; }
        public int NEW_CONSTRUCTION_AMT { get; set; }
        public char FFP_AGRMT_FLAG { get; set; }
        public char PUBLIC_OWNED_FLAG { get; set; }
        public char NON_PROFIT_FLAG { get; set; }
        public DateTime PRORATE_START_DT { get; set; }
        public DateTime PRORATE_END_DT { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DT { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DT { get; set; }
        public int LAND_AV { get; set; }
        public int LAND_MKT_AV { get; set; }
        public int IMPR_AV { get; set; }
        public char STATE_ASSESSED_FLAG { get; set; }
        public decimal SSWM_ASMT_REFUND { get; set; }
        public decimal FFP_ASMT_REFUND { get; set; }
        public char OVERRIDE_FFP_FLAG { get; set; }
        public char OVERRIDE_SSWM_FLAG { get; set; }
        public decimal RP_TAX_BILLED { get; set; }
        public decimal ADJ_TAX_BILLED { get; set; }
        public decimal ADJ_SSWM_BILLED { get; set; }
        public decimal ADJ_FFP_BILLED { get; set; }
        public char OVERRIDE_ASSESSED_FLAG { get; set; }
        public char OVERRIDE_CAMA_FLAG { get; set; }
        public char OVERRIDE_TAX_BILLED_FLAG { get; set; }
        public decimal PRORATE_CURR_BILLED { get; set; }
        public decimal PRORATE_PREV_BILLED { get; set; }
        public int FROZEN_VALUE { get; set; }
        public char FED_OWNED_FLAG { get; set; }
        public decimal RP_ADVANCE_PAID { get; set; }
        public int NOX_WEED_ASMT_ID { get; set; }
        public decimal NOX_WEED_ASMT_BILLED { get; set; }
        public decimal NOX_WEED_ASMT_PAID { get; set; }
        public decimal NOX_WEED_ASMT_REFUND { get; set; }
        public decimal ADJ_NOX_WEED_BILLED { get; set; }
        public char OVERRIDE_NOX_WEED_FLAG { get; set; }

        /// <summary>
        /// Get the 
        /// </summary>
        /// <param name="realAccountOwnerId"></param>
        /// <param name="taxYear"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static async Task<RealPropertyAccountYear> GetRealAccountFiltersAsync(int realAccountOwnerId, DateTime taxYear, string connectionString)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = $@"SELECT property_class, parcel_acreage, tax_code
	                        FROM   rp_acct_yrs
	                        WHERE  rp_acct_owner_id = {realAccountOwnerId}
	                        AND    tax_yr = {taxYear.Year}";

            var result = await connection.QueryFirstOrDefaultAsync<RealPropertyAccountYear>(sql).ConfigureAwait(false) ?? new RealPropertyAccountYear();

            return result;
        }

        public static async Task<IEnumerable<RealPropertyAccountYear>> GetAsync(int realAccountOwnerId, string connectionString)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = $@"SELECT  RP_ACCT_YR_ID, RP_ACCT_OWNER_ID, SSWM_ASMT_ID, FFP_ASMT_ID, TAX_STATUS, TAX_CODE, TAX_YR, PROPERTY_CLASS, TAXABLE_AV, ASSESSED_VALUE, MARKET_VALUE, RP_TAX_PAID, RP_TAX_REFUND, RP_INTEREST_PAID, RP_PENALTY_PAID, IMPERVIOUS_SURF_BLDG, IMPERVIOUS_SURF_OTHER, MEASURED_DT, SSWM_ASMT_BILLED, SSWM_ASMT_PAID, FFP_ACRES, FFP_ASMT_BILLED, FFP_ASMT_PAID, NEW_CONSTRUCTION_AMT, FFP_AGRMT_FLAG, PUBLIC_OWNED_FLAG, NON_PROFIT_FLAG, PRORATE_START_DT, PRORATE_END_DT, CREATED_BY, CREATED_DT, MODIFIED_BY, MODIFIED_DT, LAND_AV, LAND_MKT_AV, IMPR_AV, STATE_ASSESSED_FLAG, PARCEL_ACREAGE, SSWM_ASMT_REFUND, FFP_ASMT_REFUND, OVERRIDE_FFP_FLAG, OVERRIDE_SSWM_FLAG, RP_TAX_BILLED, ADJ_TAX_BILLED, ADJ_SSWM_BILLED, ADJ_FFP_BILLED, OVERRIDE_ASSESSED_FLAG, OVERRIDE_CAMA_FLAG, OVERRIDE_TAX_BILLED_FLAG, PRORATE_CURR_BILLED, PRORATE_PREV_BILLED, FROZEN_VALUE, FED_OWNED_FLAG, RP_ADVANCE_PAID, PARCEL_ACRES, NOX_WEED_ASMT_ID, NOX_WEED_ASMT_BILLED, NOX_WEED_ASMT_PAID, NOX_WEED_ASMT_REFUND, ADJ_NOX_WEED_BILLED, OVERRIDE_NOX_WEED_FLAG
                            FROM    RP_ACCT_YRS Y
                            WHERE   Y.RP_ACCT_OWNER_ID = {realAccountOwnerId}
                            ORDER   BY Y.TAX_YR DESC";

            var result = await connection.QueryAsync<RealPropertyAccountYear>(sql).ConfigureAwait(false);

            return result;
        }
    }
}
