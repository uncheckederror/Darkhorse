using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class PaymentReceipt : BaseTableClass
    {
        #region Fields
        public int PAYMENT_RECEIPT_ID { get; set; }
        public string PAYMENT_METHOD { get; set; }
        public decimal PAYMENT_AMT { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DT { get; set; }
        public string CHECK_NO { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DT { get; set; }
        public int RECEIPT_BATCH_ID { get; set; }
        public int REFUND_ID { get; set; }
        public int CANCELLED_RECEIPT_ID { get; set; }
        #endregion

        public static async Task<IEnumerable<PaymentReceipt>> GetAsync(int receiptBatchId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: Verify that this works in MS-SQL.
                var sql = $@"SELECT PR.PAYMENT_RECEIPT_ID,
                                      PR.PAYMENT_METHOD,
                                      PR.PAYMENT_AMT,
                                      PR.CREATED_BY,
                                      PR.CREATED_DT,
                                      PR.CHECK_NO,
                                      PR.MODIFIED_BY,
                                      PR.MODIFIED_DT,
                                      PR.RECEIPT_BATCH_ID,
                                      PR.REFUND_ID,
                                      PR.CANCELLED_RECEIPT_ID
                                    FROM PAYMENT_RECEIPTS PR
                                    WHERE PR.RECEIPT_BATCH_ID = {receiptBatchId}";

                return await connection.QueryAsync<PaymentReceipt>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT PR.PAYMENT_RECEIPT_ID,
                                      PR.PAYMENT_METHOD,
                                      PR.PAYMENT_AMT,
                                      PR.CREATED_BY,
                                      PR.CREATED_DT,
                                      PR.CHECK_NO,
                                      PR.MODIFIED_BY,
                                      PR.MODIFIED_DT,
                                      PR.RECEIPT_BATCH_ID,
                                      PR.REFUND_ID,
                                      PR.CANCELLED_RECEIPT_ID
                                    FROM PAYMENT_RECEIPTS PR
                                    WHERE PR.RECEIPT_BATCH_ID = {receiptBatchId}";

                return await connection.QueryAsync<PaymentReceipt>(sql).ConfigureAwait(false);
            }
        }
    }
}