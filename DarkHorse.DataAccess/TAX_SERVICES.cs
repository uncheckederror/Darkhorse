using Dapper;

using Oracle.ManagedDataAccess.Client;

using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class TaxService : BaseTableClass
    {
        #region Fields
        public string PHONE { get; set; }
        public string ZIP_CODE { get; set; }
        public string STATE { get; set; }
        public string CITY { get; set; }
        public string MISC_LINE2 { get; set; }
        public string STREET_ADDR { get; set; }
        public string MISC_LINE1 { get; set; }
        public string CONTACT_NAME { get; set; }
        public string TAX_SERVICE_NAME { get; set; }
        public string TAX_SERVICE_CODE { get; set; }
        #endregion

        public static async Task<TaxService> GetAsync(string taxServiceId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                // TODO: Verify that this works in MS-SQL.
                var sql = $@"SELECT L_TXSER.PHONE ,
                                  L_TXSER.ZIP_CODE ,
                                  L_TXSER.STATE ,
                                  L_TXSER.CITY ,
                                  L_TXSER.MISC_LINE2 ,
                                  L_TXSER.STREET_ADDR ,
                                  L_TXSER.MISC_LINE1 ,
                                  L_TXSER.CONTACT_NAME ,
                                  L_TXSER.TAX_SERVICE_NAME ,
                                  L_TXSER.TAX_SERVICE_CODE
                                FROM TAX_SERVICES L_TXSER
                                WHERE L_TXSER.TAX_SERVICE_ID = {taxServiceId}";

                return await connection.QueryFirstOrDefaultAsync<TaxService>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT L_TXSER.PHONE ,
                                  L_TXSER.ZIP_CODE ,
                                  L_TXSER.STATE ,
                                  L_TXSER.CITY ,
                                  L_TXSER.MISC_LINE2 ,
                                  L_TXSER.STREET_ADDR ,
                                  L_TXSER.MISC_LINE1 ,
                                  L_TXSER.CONTACT_NAME ,
                                  L_TXSER.TAX_SERVICE_NAME ,
                                  L_TXSER.TAX_SERVICE_CODE
                                FROM TAX_SERVICES L_TXSER
                                WHERE L_TXSER.TAX_SERVICE_ID = {taxServiceId}";

                return await connection.QueryFirstOrDefaultAsync<TaxService>(sql).ConfigureAwait(false);
            }
        }
    }
}
