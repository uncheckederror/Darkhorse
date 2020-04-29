using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class ReceiptRefund : BaseTableClass
    {
        #region Fields
        public int? RECEIPT_BATCH_ID { get; set; }
        public string RECEIPT_NO { get; set; }
        public int? RECEIPT_ID { get; set; }
        public int? REFUND_ID { get; set; }
        public int? RP_ACCT_OWNER_ID { get; set; }
        public int? PP_ACCT_OWNER_ID { get; set; }
        public int? LID_ACCT_ID { get; set; }
        public DateTime? RECEIPT_REFUND_DT { get; set; }
        public DateTime? CANCEL_DT { get; set; }
        public int? CASHIER_ID { get; set; }
        public string REFUND_TYPE { get; set; }
        public string REFUND_TYPE_SUB { get; set; }
        public string REFUND_PETITION_NO { get; set; }
        public DateTime? PETITION_SENT_DT { get; set; }
        public DateTime? PETITION_RECEIVED_DT { get; set; }
        public DateTime? SENT_TO_QB_DT { get; set; }
        public int RECEIPT_REFUND_AMT { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DT { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DT { get; set; }
        public int CANCEL_CASHIER_ID { get; set; }
        #endregion

        public static async Task<IEnumerable<ReceiptRefund>> GetAsync(int realAccountOwnerId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: Verify that this works in MS-SQL.
                var sql = $@"SELECT RECEIPT_BATCH_ID,
                                      RECEIPT_NO,
                                      RECEIPT_ID,
                                      REFUND_ID,
                                      RP_ACCT_OWNER_ID,
                                      PP_ACCT_OWNER_ID,
                                      LID_ACCT_ID,
                                      RECEIPT_REFUND_DT,
                                      CANCEL_DT,
                                      CASHIER_ID,
                                      REFUND_TYPE,
                                      REFUND_TYPE_SUB,
                                      REFUND_PETITION_NO,
                                      PETITION_SENT_DT,
                                      PETITION_RECEIVED_DT,
                                      SENT_TO_QB_DT,
                                      RECEIPT_REFUND_AMT,
                                      CREATED_BY,
                                      CREATED_DT,
                                      MODIFIED_BY,
                                      MODIFIED_DT,
                                      CANCEL_CASHIER_ID
                                    FROM RECEIPT_REFUND_VW
                                    WHERE RP_ACCT_OWNER_ID = {realAccountOwnerId}
                                    ORDER BY CREATED_DT DESC";

                return await connection.QueryAsync<ReceiptRefund>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT RECEIPT_BATCH_ID,
                                      RECEIPT_NO,
                                      RECEIPT_ID,
                                      REFUND_ID,
                                      RP_ACCT_OWNER_ID,
                                      PP_ACCT_OWNER_ID,
                                      LID_ACCT_ID,
                                      RECEIPT_REFUND_DT,
                                      CANCEL_DT,
                                      CASHIER_ID,
                                      REFUND_TYPE,
                                      REFUND_TYPE_SUB,
                                      REFUND_PETITION_NO,
                                      PETITION_SENT_DT,
                                      PETITION_RECEIVED_DT,
                                      SENT_TO_QB_DT,
                                      RECEIPT_REFUND_AMT,
                                      CREATED_BY,
                                      CREATED_DT,
                                      MODIFIED_BY,
                                      MODIFIED_DT,
                                      CANCEL_CASHIER_ID
                                    FROM RECEIPT_REFUND_VW
                                    WHERE RP_ACCT_OWNER_ID = {realAccountOwnerId}
                                    ORDER BY CREATED_DT DESC";

                return await connection.QueryAsync<ReceiptRefund>(sql).ConfigureAwait(false);
            }
        }
    }
}