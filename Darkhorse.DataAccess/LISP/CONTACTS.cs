using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Darkhorse.DataAccess
{
    public class Contacts
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

        public static async Task<IEnumerable<Contacts>> GetAsync(int contactId, string connectionString)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = $@"SELECT  CONTACTS.NAME, RP_CONTACTS.CONTACT_TYPE, RP_CONTACTS.CHG_CODE, RP_CONTACTS.BEGIN_DT, RP_CONTACTS.END_DT, CONTACTS.MISC_LINE1, CONTACTS.STREET_ADDR, CONTACTS.MISC_LINE2, CONTACTS.CITY, CONTACTS.STATE, CONTACTS.ZIP_CODE, CONTACTS.ZIP_EXTENSION, RP_CONTACTS.MAIL_NOTICE_FLAG, RP_CONTACTS.MAIL_TS_FLAG, RP_CONTACTS.MAIL_COPY_FLAG
                            FROM    CONTACTS,
                                    RP_CONTACTS
                            WHERE   CONTACTS.CONTACT_ID         = RP_CONTACTS.CONTACT_ID
                            AND     RP_CONTACTS.CONTACT_ID = {contactId}";

            var result = await connection.QueryAsync<Contacts>(sql).ConfigureAwait(false);

            return result;
        }
    }
}
