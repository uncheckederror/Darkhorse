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
        public int CONTACT_ID { get; set; }
        public string STREET_NO { get; set; }
        public string STREET_EXTENSION { get; set; }
        public string PRE_DIRECTIONAL { get; set; }
        public string STREET_NAME { get; set; }
        public string DESIGNATOR { get; set; }
        public string POST_DIRECTIONAL { get; set; }
        public string SUITE_APT { get; set; }
        public string UNIT_NO { get; set; }
        public string CARRIER { get; set; }
        public string HOME_PHONE { get; set; }
        public string WORK_PHONE { get; set; }
        public string CELL_PHONE { get; set; }
        public string FAX_PHONE { get; set; }
        public string PAGER_PHONE { get; set; }
        public string EMAIL_ADDRESS { get; set; }
        public DateTime CREATED_DT { get; set; }
        public string CREATED_BY { get; set; }
        public string MODIFIED_BY { get; set; }
        public DateTime? MODIFIED_DT { get; set; }
        public char ROW_SOURCE { get; set; }
        public char INVALID_FLAG { get; set; }
        public string COUNTRY { get; set; }
        public char HALF_YEAR_REMINDER_FLAG { get; set; }
        public string L_NAME { get; set; }
        public string F_NAME { get; set; }
        public string CHECK_SUM { get; set; }

        #endregion

        public static async Task<IEnumerable<Contact>> GetAsync(int realPropertyAccountOwnersId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT     C.CONTACT_ID, C.NAME, RC.CONTACT_TYPE, RC.CHG_CODE, RC.BEGIN_DT, RC.END_DT, C.MISC_LINE1, C.STREET_ADDR, C.MISC_LINE2, C.CITY, C.STATE, C.ZIP_CODE, C.ZIP_EXTENSION, RC.MAIL_NOTICE_FLAG, RC.MAIL_TS_FLAG, RC.MAIL_COPY_FLAG
                             FROM       LIS.RP_CONTACTS RC
                             INNER JOIN LIS.CONTACTS C ON (RC.CONTACT_ID = C.CONTACT_ID)
                             WHERE      RC.RP_ACCT_OWNER_ID = {realPropertyAccountOwnersId}
                             ORDER BY   RC.BEGIN_DT DESC";

                return await connection.QueryAsync<Contact>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT  CONTACTS.CONTACT_ID, CONTACTS.NAME, RP_CONTACTS.CONTACT_TYPE, RP_CONTACTS.CHG_CODE, RP_CONTACTS.BEGIN_DT, RP_CONTACTS.END_DT, CONTACTS.MISC_LINE1, CONTACTS.STREET_ADDR, CONTACTS.MISC_LINE2, CONTACTS.CITY, CONTACTS.STATE, CONTACTS.ZIP_CODE, CONTACTS.ZIP_EXTENSION, RP_CONTACTS.MAIL_NOTICE_FLAG, RP_CONTACTS.MAIL_TS_FLAG, RP_CONTACTS.MAIL_COPY_FLAG
                             FROM    RP_CONTACTS, CONTACTS
                             WHERE   RP_CONTACTS.RP_ACCT_OWNER_ID = {realPropertyAccountOwnersId}
                             AND     RP_CONTACTS.CONTACT_ID = CONTACTS.CONTACT_ID
                             ORDER BY RP_CONTACTS.BEGIN_DT DESC";

                return await connection.QueryAsync<Contact>(sql).ConfigureAwait(false);
            }
        }

        public static async Task<IEnumerable<Contact>> GetByIdAsync(int contactId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT CONTACT_ID,
                                      NAME,
                                      STREET_ADDR,
                                      MISC_LINE1,
                                      MISC_LINE2,
                                      STREET_NO,
                                      STREET_EXTENSION,
                                      PRE_DIRECTIONAL,
                                      STREET_NAME,
                                      DESIGNATOR,
                                      POST_DIRECTIONAL,
                                      SUITE_APT,
                                      UNIT_NO,
                                      CITY,
                                      STATE,
                                      ZIP_CODE,
                                      ZIP_EXTENSION,
                                      CARRIER,
                                      HOME_PHONE,
                                      WORK_PHONE,
                                      CELL_PHONE,
                                      FAX_PHONE,
                                      PAGER_PHONE,
                                      EMAIL_ADDRESS,
                                      CREATED_DT,
                                      CREATED_BY,
                                      MODIFIED_BY,
                                      MODIFIED_DT,
                                      ROW_SOURCE,
                                      INVALID_FLAG,
                                      COUNTRY,
                                      HALF_YEAR_REMINDER_FLAG,
                                      L_NAME,
                                      F_NAME,
                                      CHECK_SUM
                                    FROM CONTACTS
                                    WHERE CONTACT_ID = {contactId}";

                return await connection.QueryAsync<Contact>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT CONTACT_ID,
                                      NAME,
                                      STREET_ADDR,
                                      MISC_LINE1,
                                      MISC_LINE2,
                                      STREET_NO,
                                      STREET_EXTENSION,
                                      PRE_DIRECTIONAL,
                                      STREET_NAME,
                                      DESIGNATOR,
                                      POST_DIRECTIONAL,
                                      SUITE_APT,
                                      UNIT_NO,
                                      CITY,
                                      STATE,
                                      ZIP_CODE,
                                      ZIP_EXTENSION,
                                      CARRIER,
                                      HOME_PHONE,
                                      WORK_PHONE,
                                      CELL_PHONE,
                                      FAX_PHONE,
                                      PAGER_PHONE,
                                      EMAIL_ADDRESS,
                                      CREATED_DT,
                                      CREATED_BY,
                                      MODIFIED_BY,
                                      MODIFIED_DT,
                                      ROW_SOURCE,
                                      INVALID_FLAG,
                                      COUNTRY,
                                      HALF_YEAR_REMINDER_FLAG,
                                      L_NAME,
                                      F_NAME,
                                      CHECK_SUM
                                    FROM CONTACTS
                                    WHERE CONTACT_ID = {contactId}";

                return await connection.QueryAsync<Contact>(sql).ConfigureAwait(false);
            }
        }

    }
}
