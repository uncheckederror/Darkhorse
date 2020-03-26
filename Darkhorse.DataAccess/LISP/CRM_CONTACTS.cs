using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class CrmContact
    {
        public string CRM_EMAIL { get; set; }
        public string CRM_LASTNAME { get; set; }
        public string CRM_FIRSTNAME { get; set; }
        public char CRM_NOTIFY_FLAG { get; set; }

        public static async Task<IEnumerable<CrmContact>> GetAsync(int realPropertyAccountId, string connectionString)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = $@"SELECT   CC.CRM_EMAIL, CC.CRM_LASTNAME, CC.CRM_FIRSTNAME, CC.CRM_NOTIFY_FLAG
                            FROM     CRM_CONTACTS CC
                            WHERE    CC.ACCOUNT_ID = {realPropertyAccountId}";

            var result = await connection.QueryAsync<CrmContact>(sql).ConfigureAwait(false);

            return result;
        }
    }
}
