using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class ContactNumber : BaseTableClass
    {
        #region Fields
        public string CONTACT_NUMBER_ID { get; set; }
        public string CONTACT_ID { get; set; }
        public string CONTACT_NUMBER_TYPE_ID { get; set; }
        public string VALUE { get; set; }
        public string ORDER_BY { get; set; }
        public string CREATED_BY { get; set; }
        public string CREATED_DT { get; set; }
        public string MODIFIED_BY { get; set; }
        public string MODIFIED_DT { get; set; }
        public string CONTACT_NUMBER_TYPE { get; set; }
        public string CODE_ID { get; set; }
        public string CODE_TEXT { get; set; }
        public string DESCRIPTION { get; set; }
        public string ACTIVE { get; set; }
        public string FORMAT { get; set; }
        #endregion

        public static async Task<IEnumerable<ContactNumber>> GetAsync(int contactId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: Verify this works in MS-SQL.
                var sql = $@"SELECT CN.CONTACT_NUMBER_ID,
                                      CN.CONTACT_ID,
                                      CN.CONTACT_NUMBER_TYPE_ID,
                                      CN.VALUE,
                                      CN.ORDER_BY,
                                      CN.CREATED_BY,
                                      CN.CREATED_DT,
                                      CN.MODIFIED_BY,
                                      CN.MODIFIED_DT,
                                      CN.CONTACT_NUMBER_TYPE,
                                      CNT.CODE_ID,
                                      CNT.CODE_TEXT,
                                      CNT.DESCRIPTION,
                                      CNT.ACTIVE,
                                      CNT.FORMAT
                                    FROM contact_numbers CN
                                    INNER JOIN contact_number_types CNT
                                    ON CN.CONTACT_NUMBER_TYPE_ID = CNT.CODE_ID
                                    WHERE CONTACT_ID             = {contactId}
                                    ";

                return await connection.QueryAsync<ContactNumber>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT CN.CONTACT_NUMBER_ID,
                                      CN.CONTACT_ID,
                                      CN.CONTACT_NUMBER_TYPE_ID,
                                      CN.VALUE,
                                      CN.ORDER_BY,
                                      CN.CREATED_BY,
                                      CN.CREATED_DT,
                                      CN.MODIFIED_BY,
                                      CN.MODIFIED_DT,
                                      CN.CONTACT_NUMBER_TYPE,
                                      CNT.CODE_ID,
                                      CNT.CODE_TEXT,
                                      CNT.DESCRIPTION,
                                      CNT.ACTIVE,
                                      CNT.FORMAT
                                    FROM contact_numbers CN
                                    INNER JOIN contact_number_types CNT
                                    ON CN.CONTACT_NUMBER_TYPE_ID = CNT.CODE_ID
                                    WHERE CONTACT_ID             = {contactId}
                                    ";

                return await connection.QueryAsync<ContactNumber>(sql).ConfigureAwait(false);
            }
        }
    }
}
