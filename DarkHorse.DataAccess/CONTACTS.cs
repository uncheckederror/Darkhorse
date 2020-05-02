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
        #region Fields
        public string NAME { get; set; }
        public string CONTACT_TYPE { get; set; }
        public string CHG_CODE { get; set; }
        public DateTime BEGIN_DT { get; set; }
        public DateTime? END_DT { get; set; }
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
        #endregion

        public static async Task<IEnumerable<Contact>> GetAsync(int realPropertyAccountOwnersId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT     C.NAME, RC.CONTACT_TYPE, RC.CHG_CODE, RC.BEGIN_DT, RC.END_DT, C.MISC_LINE1, C.STREET_ADDR, C.MISC_LINE2, C.CITY, C.STATE, C.ZIP_CODE, C.ZIP_EXTENSION, RC.MAIL_NOTICE_FLAG, RC.MAIL_TS_FLAG, RC.MAIL_COPY_FLAG
                             FROM       LIS.RP_CONTACTS RC
                             INNER JOIN LIS.CONTACTS C ON (RC.CONTACT_ID = C.CONTACT_ID)
                             WHERE      RC.RP_ACCT_OWNER_ID = {realPropertyAccountOwnersId}
                             ORDER BY   RC.BEGIN_DT DESC";

                return await connection.QueryAsync<Contact>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT  CONTACTS.NAME, RP_CONTACTS.CONTACT_TYPE, RP_CONTACTS.CHG_CODE, RP_CONTACTS.BEGIN_DT, RP_CONTACTS.END_DT, CONTACTS.MISC_LINE1, CONTACTS.STREET_ADDR, CONTACTS.MISC_LINE2, CONTACTS.CITY, CONTACTS.STATE, CONTACTS.ZIP_CODE, CONTACTS.ZIP_EXTENSION, RP_CONTACTS.MAIL_NOTICE_FLAG, RP_CONTACTS.MAIL_TS_FLAG, RP_CONTACTS.MAIL_COPY_FLAG
                             FROM    RP_CONTACTS, CONTACTS
                             WHERE   RP_CONTACTS.RP_ACCT_OWNER_ID = {realPropertyAccountOwnersId}
                             AND     RP_CONTACTS.CONTACT_ID = CONTACTS.CONTACT_ID
                             ORDER BY RP_CONTACTS.BEGIN_DT DESC";

                return await connection.QueryAsync<Contact>(sql).ConfigureAwait(false);
            }
        }
    }
}
