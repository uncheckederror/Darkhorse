using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class RealPropertyYearPayment : BaseTableClass
    {
        #region Fields
        public int RP_ACCT_YR_PMT_ID { get; set; }
        public int RP_ACCT_YR_ID { get; set; }
        public DateTime LOCK_DT { get; set; }
        public DateTime? APPROVED_DT { get; set; }
        public string APPROVED_BY { get; set; }
        public DateTime? CANCELLED_DT { get; set; }
        public DateTime? COMPLETE_DT { get; set; }
        public decimal LOCK_TAX_DUE { get; set; }
        public decimal LOCK_FFP_DUE { get; set; }
        public decimal LOCK_SSWM_DUE { get; set; }
        public decimal LOCK_NW_DUE { get; set; }
        public decimal LOCK_ASMT_DUE { get; set; }
        public decimal LOCK_PENALTY_DUE { get; set; }
        public decimal LOCK_INT_TAX_DUE { get; set; }
        public decimal LOCK_INT_FFP_DUE { get; set; }
        public decimal LOCK_INT_SSWM_DUE { get; set; }
        public decimal LOCK_INT_NW_DUE { get; set; }
        public decimal LOCK_INT_ASMT_DUE { get; set; }
        public int PAYMENT_MONTHS { get; set; }
        public decimal PAYMENT_AMT { get; set; }
        public decimal CURR_TAX_DUE { get; set; }
        public decimal CURR_FFP_DUE { get; set; }
        public decimal CURR_SSWM_DUE { get; set; }
        public decimal CURR_NW_DUE { get; set; }
        public decimal CURR_ASMT_DUE { get; set; }
        public decimal CURR_PENALTY_DUE { get; set; }
        public decimal CURR_INT_TAX_DUE { get; set; }
        public decimal CURR_INT_FFP_DUE { get; set; }
        public decimal CURR_INT_SSWM_DUE { get; set; }
        public decimal CURR_INT_NW_DUE { get; set; }
        public decimal CURR_INT_ASMT_DUE { get; set; }
        public DateTime? LAST_PAID_DT { get; set; }
        public decimal INTEREST_RATE { get; set; }
        //public string CREATED_BY { get; set; }
        //public DateTime CREATED_DT { get; set; }
        //public string MODIFIED_BY { get; set; }
        //public DateTime MODIFIED_DT { get; set; }
        #endregion

        public static async Task<RealPropertyYearPayment> GetAsync(int realAccountTaxYearId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: Verify that this works in MS-SQL.
                var sql = $@"SELECT RP_ACCT_YR_PMT_ID, RP_ACCT_YR_ID, LOCK_DT, APPROVED_DT, APPROVED_BY, CANCELLED_DT, COMPLETE_DT, LOCK_TAX_DUE, LOCK_FFP_DUE, LOCK_SSWM_DUE, LOCK_NW_DUE, LOCK_ASMT_DUE, LOCK_PENALTY_DUE, LOCK_INT_TAX_DUE, LOCK_INT_FFP_DUE, LOCK_INT_SSWM_DUE, LOCK_INT_NW_DUE, LOCK_INT_ASMT_DUE, PAYMENT_MONTHS, PAYMENT_AMT, CURR_TAX_DUE, CURR_FFP_DUE, CURR_SSWM_DUE, CURR_NW_DUE, CURR_ASMT_DUE, CURR_PENALTY_DUE, CURR_INT_TAX_DUE, CURR_INT_FFP_DUE, CURR_INT_SSWM_DUE, CURR_INT_NW_DUE, CURR_INT_ASMT_DUE, LAST_PAID_DT, INTEREST_RATE, CREATED_BY, CREATED_DT, MODIFIED_BY, MODIFIED_DT FROM RP_ACCT_YR_PMTS WHERE RP_ACCT_YR_ID = {realAccountTaxYearId}";

                return await connection.QueryFirstOrDefaultAsync<RealPropertyYearPayment>(sql).ConfigureAwait(false) ?? new RealPropertyYearPayment();
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT RP_ACCT_YR_PMT_ID, RP_ACCT_YR_ID, LOCK_DT, APPROVED_DT, APPROVED_BY, CANCELLED_DT, COMPLETE_DT, LOCK_TAX_DUE, LOCK_FFP_DUE, LOCK_SSWM_DUE, LOCK_NW_DUE, LOCK_ASMT_DUE, LOCK_PENALTY_DUE, LOCK_INT_TAX_DUE, LOCK_INT_FFP_DUE, LOCK_INT_SSWM_DUE, LOCK_INT_NW_DUE, LOCK_INT_ASMT_DUE, PAYMENT_MONTHS, PAYMENT_AMT, CURR_TAX_DUE, CURR_FFP_DUE, CURR_SSWM_DUE, CURR_NW_DUE, CURR_ASMT_DUE, CURR_PENALTY_DUE, CURR_INT_TAX_DUE, CURR_INT_FFP_DUE, CURR_INT_SSWM_DUE, CURR_INT_NW_DUE, CURR_INT_ASMT_DUE, LAST_PAID_DT, INTEREST_RATE, CREATED_BY, CREATED_DT, MODIFIED_BY, MODIFIED_DT FROM RP_ACCT_YR_PMTS WHERE RP_ACCT_YR_ID = {realAccountTaxYearId}";

                return await connection.QueryFirstOrDefaultAsync<RealPropertyYearPayment>(sql).ConfigureAwait(false) ?? new RealPropertyYearPayment();
            }
        }
    }
}
