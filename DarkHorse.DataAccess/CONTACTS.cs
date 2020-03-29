using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class Contact : BaseTableClass
    {
        public string NAME { get; set; }
        public string CONTACT_TYPE { get; set; }
        public string CHG_CODE { get; set; }
        public DateTime BEGIN_DT { get; set; }
        public DateTime END_DT { get; set; }
        public string MISC_LINE1 { get; set; }
        public string STREET_ADDR { get; set; }
        public string MISC_LINE2 { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIP_CODE { get; set; }
        public string ZIP_EXTENSION { get; set; }
        public char MAIL_NOTICE_FLAG { get; set; }
        public char MAIL_TS_FLAG { get; set; }
        public char MAIL_COPY_FLAG { get; set; }

        public static async Task<IEnumerable<Contact>> GetAsync(int realPropertyAccountOwnersId, IDbConnection dbConnection)
        {
            if (dbConnection.GetType()?.Name == "SqlConnection")
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                string sql = $@"";

                var result = await connection.QueryAsync<Contact>(sql).ConfigureAwait(false);

                return result;
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                string sql = $@"SELECT  CONTACTS.NAME, RP_CONTACTS.CONTACT_TYPE, RP_CONTACTS.CHG_CODE, RP_CONTACTS.BEGIN_DT, RP_CONTACTS.END_DT, CONTACTS.MISC_LINE1, CONTACTS.STREET_ADDR, CONTACTS.MISC_LINE2, CONTACTS.CITY, CONTACTS.STATE, CONTACTS.ZIP_CODE, CONTACTS.ZIP_EXTENSION, RP_CONTACTS.MAIL_NOTICE_FLAG, RP_CONTACTS.MAIL_TS_FLAG, RP_CONTACTS.MAIL_COPY_FLAG
                            FROM    RP_CONTACTS, CONTACTS
                            WHERE   RP_CONTACTS.RP_ACCT_OWNER_ID = {realPropertyAccountOwnersId}
                            AND     RP_CONTACTS.CONTACT_ID = CONTACTS.CONTACT_ID
                            ORDER BY RP_CONTACTS.BEGIN_DT DESC";

                var result = await connection.QueryAsync<Contact>(sql).ConfigureAwait(false);

                return result;
            }
        }
    }
}
