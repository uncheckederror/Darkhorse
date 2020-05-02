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
        public string ZIP_EXTENSION { get; set; }
        public string ZIP_CODE { get; set; }
        public string STATE { get; set; }
        public string CITY { get; set; }
        public string STREET_ADDR { get; set; }
        public string NAME { get; set; }
        public string MISC_LINE1 { get; set; }
        public string MISC_LINE2 { get; set; }
        #endregion

        public static async Task<IEnumerable<RealPropertyContact>> GetAsync(int contactId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT  ZIP_EXTENSION, ZIP_CODE, STATE, CITY, STREET_ADDR, NAME, MISC_LINE1, MISC_LINE2
                             FROM    LIS.CONTACTS
                             WHERE   CONTACT_ID = {contactId}";

                return await connection.QueryAsync<RealPropertyContact>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT  ZIP_EXTENSION, ZIP_CODE, STATE, CITY, STREET_ADDR, NAME, MISC_LINE1, MISC_LINE2
                             FROM    CONTACTS
                             WHERE   CONTACT_ID = {contactId}";

                return await connection.QueryAsync<RealPropertyContact>(sql).ConfigureAwait(false);
            }
        }
    }
}
