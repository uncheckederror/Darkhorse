using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class Transaction : BaseTableClass
    {
        #region Fields
        public int TRANS_ID { get; set; }
        public int? RECEIPT_ID { get; set; }
        public int? REFUND_ID { get; set; }
        public int? FEE_ID { get; set; }
        public int? RP_ACCT_YR_ID { get; set; }
        public int? PP_ACCT_YR_ID { get; set; }
        public int? LID_ACCT_YR_ID { get; set; }
        public string COLLECT_FUND { get; set; }
        public char? TAX_CLEARING { get; set; }
        public decimal TRANS_AMT_PAID { get; set; }
        public char OVERRIDE_PAYMENT_FLAG { get; set; }
        public char ADVANCE_PAYMENT_FLAG { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DT { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DT { get; set; }
        public char? PAYMENT_STATUS { get; set; }
        public string DEL_PMT_FUND { get; set; }
        public int TAX_YR { get; set; }
        // Stuffing this value in here to keep it simple.
        public string FundDescription { get; set; }
        #endregion

        public static async Task<IEnumerable<Transaction>> GetAsync(int receiptId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: Verify that this works in MS-SQL.
                var sql = $@"SELECT T.TRANS_ID,
                                      T.RECEIPT_ID,
                                      T.REFUND_ID,
                                      T.FEE_ID,
                                      T.RP_ACCT_YR_ID,
                                      T.PP_ACCT_YR_ID,
                                      T.LID_ACCT_YR_ID,
                                      T.COLLECT_FUND,
                                      T.TAX_CLEARING,
                                      T.TRANS_AMT_PAID,
                                      T.OVERRIDE_PAYMENT_FLAG,
                                      T.ADVANCE_PAYMENT_FLAG,
                                      T.CREATED_BY,
                                      T.CREATED_DT,
                                      T.MODIFIED_BY,
                                      T.MODIFIED_DT,
                                      T.PAYMENT_STATUS,
                                      T.DEL_PMT_FUND, 
                                      RPA.TAX_YR
                                    FROM TRANSACTIONS T
                                    INNER JOIN RP_ACCT_YRS RPA
                                    ON T.RP_ACCT_YR_ID = RPA.RP_ACCT_YR_ID
                                    WHERE RECEIPT_ID = {receiptId}";

                return await connection.QueryAsync<Transaction>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT T.TRANS_ID,
                                      T.RECEIPT_ID,
                                      T.REFUND_ID,
                                      T.FEE_ID,
                                      T.RP_ACCT_YR_ID,
                                      T.PP_ACCT_YR_ID,
                                      T.LID_ACCT_YR_ID,
                                      T.COLLECT_FUND,
                                      T.TAX_CLEARING,
                                      T.TRANS_AMT_PAID,
                                      T.OVERRIDE_PAYMENT_FLAG,
                                      T.ADVANCE_PAYMENT_FLAG,
                                      T.CREATED_BY,
                                      T.CREATED_DT,
                                      T.MODIFIED_BY,
                                      T.MODIFIED_DT,
                                      T.PAYMENT_STATUS,
                                      T.DEL_PMT_FUND, 
                                      RPA.TAX_YR
                                    FROM TRANSACTIONS T
                                    INNER JOIN RP_ACCT_YRS RPA
                                    ON T.RP_ACCT_YR_ID = RPA.RP_ACCT_YR_ID
                                    WHERE RECEIPT_ID = {receiptId}";

                return await connection.QueryAsync<Transaction>(sql).ConfigureAwait(false);
            }
        }
    }
}