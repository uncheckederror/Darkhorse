using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class CrmContact : BaseTableClass
    {
        #region Fields

        public string CRM_EMAIL { get; set; }
        public string CRM_LASTNAME { get; set; }
        public string CRM_FIRSTNAME { get; set; }
        public char CRM_NOTIFY_FLAG { get; set; }

        #endregion

        public static async Task<IEnumerable<CrmContact>> GetAsync(int realPropertyAccountId, IDbConnection dbConnection)
        {
            if (dbConnection.GetType()?.Name == "SqlConnection")
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT CRM_EMAIL, CRM_LASTNAME, CRM_FIRSTNAME, CRM_NOTIFY_FLAG
                             FROM   LIS.CRM_CONTACTS
                             WHERE  ACCOUNT_ID = {realPropertyAccountId}";

                return await connection.QueryAsync<CrmContact>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT   CC.CRM_EMAIL, CC.CRM_LASTNAME, CC.CRM_FIRSTNAME, CC.CRM_NOTIFY_FLAG
                             FROM     CRM_CONTACTS CC
                             WHERE    CC.ACCOUNT_ID = {realPropertyAccountId}";

                return await connection.QueryAsync<CrmContact>(sql).ConfigureAwait(false);
            }
        }
    }
}
