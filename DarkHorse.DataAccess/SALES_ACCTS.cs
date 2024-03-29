using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class SalesAccount : BaseTableClass
    {
        #region Fields
        public string EXCISE_NO { get; set; }
        public DateTime? DOCUMENT_DT { get; set; }
        public string INVALID_CODE { get; set; }
        public DateTime DATA_ENTRY_DT { get; set; }
        public int PRICE { get; set; }
        public int SALES_ID { get; set; }
        public string PRIMARY_PARCEL { get; set; }
        #endregion

        public static async Task<IEnumerable<SalesAccount>> GetAsync(int realAccountId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT     S.EXCISE_NO, S.DOCUMENT_DT, S.INVALID_CODE, S.DATA_ENTRY_DT, S.PRICE, SA.SALES_ID, SA.PRIMARY_PARCEL
                             FROM       LIS.SALES S
                             INNER JOIN LIS.SALES_ACCTS SA ON (S.SALES_ID = SA.SALES_ID)
                             WHERE      SA.RP_ACCT_ID = {realAccountId}                             
                             ORDER BY   S.DOCUMENT_DT DESC";

                return await connection.QueryAsync<SalesAccount>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT  S.EXCISE_NO, S.DOCUMENT_DT, S.INVALID_CODE, S.DATA_ENTRY_DT, S.PRICE, SA.SALES_ID, SA.PRIMARY_PARCEL
                             FROM    SALES_ACCTS SA, SALES S
                             WHERE   RP_ACCT_ID = {realAccountId}
                             AND     SA.SALES_ID = S.SALES_ID
                             ORDER BY S.DOCUMENT_DT DESC";

                return await connection.QueryAsync<SalesAccount>(sql).ConfigureAwait(false);
            }
        }
    }
}
