using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class ContactAccount : BaseTableClass
    {
        #region Fields
        public int? CONTACT_ID { get; set; }
        public string ACCT_NO { get; set; }
        public string ACCT_TYPE { get; set; }
        public string CONTACT_TYPE { get; set; }
        public DateTime? BEGIN_DT { get; set; }
        public DateTime? ACCT_INACTIVE_DT { get; set; }
        public char? MAIL_TS_FLAG { get; set; }
        public char? MAIL_NOTICE_FLAG { get; set; }
        #endregion

        public static async Task<IEnumerable<ContactAccount>> GetAsync(int contactId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: Verify this works in MS-SQL.
                var sql = $@"SELECT CONTACT_ID,
                                      ACCT_NO,
                                      ACCT_TYPE,
                                      CONTACT_TYPE,
                                      BEGIN_DT,
                                      ACCT_INACTIVE_DT,
                                      MAIL_TS_FLAG,
                                      MAIL_NOTICE_FLAG
                                    FROM CONTACT_ACCTS_VW
                                    WHERE CONTACT_ID = {contactId}";

                return await connection.QueryAsync<ContactAccount>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT CONTACT_ID,
                                      ACCT_NO,
                                      ACCT_TYPE,
                                      CONTACT_TYPE,
                                      BEGIN_DT,
                                      ACCT_INACTIVE_DT,
                                      MAIL_TS_FLAG,
                                      MAIL_NOTICE_FLAG
                                    FROM CONTACT_ACCTS_VW
                                    WHERE CONTACT_ID = {contactId}";

                return await connection.QueryAsync<ContactAccount>(sql).ConfigureAwait(false);
            }
        }
    }
}
