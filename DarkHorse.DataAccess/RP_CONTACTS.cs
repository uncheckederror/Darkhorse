using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class RealPropertyContact : BaseTableClass
    {
        #region Fields
        public int RP_CONTACT_ID { get; set; }
        public int RP_ACCT_OWNER_ID { get; set; }
        public int CONTACT_ID { get; set; }
        public string CHG_CODE { get; set; }
        public string CONTACT_TYPE { get; set; }
        public DateTime BEGIN_DT { get; set; }
        public DateTime? END_DT { get; set; }
        public char MAIL_TS_FLAG { get; set; }
        public char MAIL_NOTICE_FLAG { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DT { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DT { get; set; }
        public char MAIL_COPY_FLAG { get; set; }
        #endregion

        public static async Task<IEnumerable<RealPropertyContact>> GetAsync(int contactId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT RP_CONTACT_ID,
                                      RP_ACCT_OWNER_ID,
                                      CONTACT_ID,
                                      CHG_CODE,
                                      CONTACT_TYPE,
                                      BEGIN_DT,
                                      END_DT,
                                      MAIL_TS_FLAG,
                                      MAIL_NOTICE_FLAG,
                                      CREATED_BY,
                                      CREATED_DT,
                                      MODIFIED_BY,
                                      MODIFIED_DT,
                                      MAIL_COPY_FLAG
                                    FROM RP_CONTACTS
                                    WHERE CONTACT_ID = {contactId}";

                return await connection.QueryAsync<RealPropertyContact>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT RP_CONTACT_ID,
                                      RP_ACCT_OWNER_ID,
                                      CONTACT_ID,
                                      CHG_CODE,
                                      CONTACT_TYPE,
                                      BEGIN_DT,
                                      END_DT,
                                      MAIL_TS_FLAG,
                                      MAIL_NOTICE_FLAG,
                                      CREATED_BY,
                                      CREATED_DT,
                                      MODIFIED_BY,
                                      MODIFIED_DT,
                                      MAIL_COPY_FLAG
                                    FROM RP_CONTACTS
                                    WHERE CONTACT_ID = {contactId}";

                return await connection.QueryAsync<RealPropertyContact>(sql).ConfigureAwait(false);
            }
        }
    }
}
