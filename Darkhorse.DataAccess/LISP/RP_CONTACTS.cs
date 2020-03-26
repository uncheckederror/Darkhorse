using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class RealPropertyContacts
    {
        public int RP_CONTACT_ID { get; set; }
        public int RP_ACCT_OWNER_ID { get; set; }
        public int CONTACT_ID { get; set; }
        public string CHG_CODE { get; set; }
        public string CONTACT_TYPE { get; set; }
        public DateTime BEGIN_DT { get; set; }
        public DateTime END_DT { get; set; }
        public char MAIL_TS_FLAG { get; set; }
        public char MAIL_NOTICE_FLAG { get; set; }
        public string CREATED_BY { get; set; }
        public DateTime CREATED_DT { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DT { get; set; }
        public char MAIL_COPY_FLAG { get; set; }
        public static async Task<IEnumerable<RealPropertyContacts>> GetAsync(int contactId, string connectionString)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = $@"SELECT  ZIP_EXTENSION, ZIP_CODE, STATE, CITY, STREET_ADDR, NAME, MISC_LINE1, MISC_LINE2
                            FROM    CONTACTS
                            WHERE   CONTACT_ID = {contactId}";

            var result = await connection.QueryAsync<RealPropertyContacts>(sql).ConfigureAwait(false);

            return result;
        }
    }
}
