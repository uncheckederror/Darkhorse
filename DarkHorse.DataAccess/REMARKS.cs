using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class Remark : BaseTableClass
    {
        public int RMK_ID { get; set; }
        public string RMK_CD { get; set; }
        public string TABLE_NAME { get; set; }
        public int TABLE_PRIMARY_ID { get; set; }
        public string FORM_NAME { get; set; }
        public char PRINT_NOTE_FLAG { get; set; }
        public string REMARKS { get; set; }
        public char ACTIVE { get; set; }
        public DateTime BEGIN_DT { get; set; }
        public int PP_ACCT_ID { get; set; }
        public int APPLICATION_ID { get; set; }
        public int RP_ACCT_ID { get; set; }
        public string LINK_TEXT { get; set; }
        public string LINK_OBJECT { get; set; }
        public DateTime END_DT { get; set; }
        public int HOLD_CODE_ID { get; set; }
        public string COLUMN_NAME { get; set; }
        public string LINK_FILE { get; set; }

        public static async Task<IEnumerable<Remark>> GetAsync(int realPropertyAccountId, string connectionString)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = $@"SELECT  RMK_ID, RMK_CD, TABLE_NAME, TABLE_PRIMARY_ID, FORM_NAME, PRINT_NOTE_FLAG, REMARKS, ACTIVE, BEGIN_DT, CREATED_BY, CREATED_DT, PP_ACCT_ID, APPLICATION_ID, RP_ACCT_ID, LINK_TEXT, LINK_OBJECT, END_DT, MODIFIED_BY, MODIFIED_DT, HOLD_CODE_ID, COLUMN_NAME, LINK_FILE
                            FROM    REMARKS R
                            WHERE   R.RP_ACCT_ID = {realPropertyAccountId}
                            ORDER BY R.BEGIN_DT DESC";

            var result = await connection.QueryAsync<Remark>(sql).ConfigureAwait(false);

            return result;
        }
    }
}
