using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class CalculatePrepaymentAmounts : BaseTableClass
    {
        #region Fields
        // Yearly
        public int RP_ACCT_YR_ID { get; set; }
        public decimal TOTAL_BILLED { get; set; }
        public decimal TOTAL_PAID { get; set; }
        public decimal TOTAL_DUE { get; set; }

        // Yearly Other
        public decimal? TOTAL_OTHER_BILLED { get; set; }
        public decimal? TOTAL_OTHER_PAID { get; set; }
        public decimal? TOTAL_OTHER_REFUND { get; set; }

        // Fee
        public decimal FEE { get; set; }

        // Display Values
        public decimal SignUpDue { get; set; }
        public decimal MontlyDue { get; set; }
        #endregion

        public static async Task<CalculatePrepaymentAmounts> GetAsync(string accountNumber, IDbConnection dbConnection)
        {
            var yearly = await GetYearlyAsync(accountNumber, dbConnection);
            var other = await GetYearlyOtherAsync(yearly.RP_ACCT_YR_ID, dbConnection);
            var fee = await GetFeeAsync(dbConnection);

            return new CalculatePrepaymentAmounts
            {
                RP_ACCT_YR_ID = yearly.RP_ACCT_YR_ID,
                TOTAL_BILLED = yearly.TOTAL_BILLED,
                TOTAL_PAID = yearly.TOTAL_PAID,
                TOTAL_DUE = yearly.TOTAL_DUE,
                TOTAL_OTHER_BILLED = other.TOTAL_OTHER_BILLED,
                TOTAL_OTHER_PAID = other.TOTAL_OTHER_PAID,
                TOTAL_OTHER_REFUND = other.TOTAL_OTHER_REFUND,
                FEE = fee.FEE
            };
        }

        public void Calculate(int month)
        {
            var totalBilled = TOTAL_BILLED + (TOTAL_OTHER_BILLED ?? 0M);
            var totalPaid = TOTAL_PAID + (TOTAL_OTHER_PAID ?? 0M);
            var totalDue = TOTAL_DUE + (TOTAL_OTHER_BILLED ?? 0M) + (TOTAL_OTHER_REFUND ?? 0M) - TOTAL_PAID;
            var monthlyDue = 0M;
            var signupDue = 0M;

            if (month < 5)
            {
                monthlyDue = Math.Round(totalBilled / 8, 2);
                signupDue = monthlyDue * month;
            }
            else
            {
                monthlyDue = Math.Round(totalBilled / 12, 2);
                if (month < 11)
                {
                    signupDue = monthlyDue * (month - 4);
                }
                else
                {
                    signupDue = monthlyDue * (month - 10);
                }
            }

            SignUpDue = signupDue + FEE;
            MontlyDue = monthlyDue;
        }

        public static async Task<CalculatePrepaymentAmounts> GetYearlyAsync(string accountNumber, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT rp_acct_yr_id,
                                      (adj_tax_billed + adj_sswm_billed + adj_ffp_billed + adj_nox_weed_billed) total_billed,
                                      (rp_tax_paid    + sswm_asmt_paid + ffp_asmt_paid + nox_weed_asmt_paid) total_paid,
                                      (adj_tax_billed + rp_tax_refund - rp_tax_paid + adj_sswm_billed + sswm_asmt_refund - sswm_asmt_paid + adj_ffp_billed + ffp_asmt_refund - ffp_asmt_paid + adj_nox_weed_billed + nox_weed_asmt_refund - nox_weed_asmt_paid) total_due
                                    FROM rp_accts ra,
                                      rp_acct_owners ro,
                                      rp_acct_yrs ry
                                    WHERE ra.acct_no        = '{accountNumber}'
                                    AND ra.rp_acct_id       = ro.rp_acct_id
                                    AND ro.end_dt          IS NULL
                                    AND ro.rp_acct_owner_id = ry.rp_acct_owner_id
                                    AND ry.tax_yr           = to_number(TO_CHAR(sysdate,'YYYY'))
                                    AND ry.prorate_end_dt  IS NULL";

                return await connection.QueryFirstOrDefaultAsync<CalculatePrepaymentAmounts>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT rp_acct_yr_id,
                                      (adj_tax_billed + adj_sswm_billed + adj_ffp_billed + adj_nox_weed_billed) total_billed,
                                      (rp_tax_paid    + sswm_asmt_paid + ffp_asmt_paid + nox_weed_asmt_paid) total_paid,
                                      (adj_tax_billed + rp_tax_refund - rp_tax_paid + adj_sswm_billed + sswm_asmt_refund - sswm_asmt_paid + adj_ffp_billed + ffp_asmt_refund - ffp_asmt_paid + adj_nox_weed_billed + nox_weed_asmt_refund - nox_weed_asmt_paid) total_due
                                    FROM rp_accts ra,
                                      rp_acct_owners ro,
                                      rp_acct_yrs ry
                                    WHERE ra.acct_no        = '{accountNumber}'
                                    AND ra.rp_acct_id       = ro.rp_acct_id
                                    AND ro.end_dt          IS NULL
                                    AND ro.rp_acct_owner_id = ry.rp_acct_owner_id
                                    AND ry.tax_yr           = to_number(TO_CHAR(sysdate,'YYYY'))
                                    AND ry.prorate_end_dt  IS NULL";

                return await connection.QueryFirstOrDefaultAsync<CalculatePrepaymentAmounts>(sql).ConfigureAwait(false);
            }
        }

        public static async Task<CalculatePrepaymentAmounts> GetYearlyOtherAsync(int realPropertyAccountYearId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT SUM(other_asmt_billed) AS total_other_billed,
                                      SUM(other_asmt_paid) AS total_other_paid,
                                      SUM(other_asmt_refund) AS total_other_refund
                                    FROM other_asmts
                                    WHERE rp_acct_yr_id = {realPropertyAccountYearId}";

                return await connection.QueryFirstOrDefaultAsync<CalculatePrepaymentAmounts>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT SUM(other_asmt_billed) AS total_other_billed,
                                      SUM(other_asmt_paid) AS total_other_paid,
                                      SUM(other_asmt_refund) AS total_other_refund
                                    FROM other_asmts
                                    WHERE rp_acct_yr_id = {realPropertyAccountYearId}";

                return await connection.QueryFirstOrDefaultAsync<CalculatePrepaymentAmounts>(sql).ConfigureAwait(false);
            }
        }

        public static async Task<CalculatePrepaymentAmounts> GetFeeAsync(IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT value_text AS fee
                                FROM global_values v
                                WHERE v.code_text = 'PREPAYMENT FEE'";

                return await connection.QueryFirstOrDefaultAsync<CalculatePrepaymentAmounts>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT value_text AS fee
                                FROM global_values v
                                WHERE v.code_text = 'PREPAYMENT FEE'";

                return await connection.QueryFirstOrDefaultAsync<CalculatePrepaymentAmounts>(sql).ConfigureAwait(false);
            }
        }
    }
}
