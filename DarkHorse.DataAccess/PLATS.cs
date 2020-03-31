using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class Plat : BaseTableClass
    {
        #region Fields

        public int PLAT_ID { get; set; }
        public string PLAT_NO { get; set; }
        public string PLAT_NAME { get; set; }
        public short TAX_YR { get; set; }
        public string AUDITOR_FILE_NO { get; set; }
        public DateTime RECORDED_DT { get; set; }
        public short VOLUME { get; set; }
        public string PAGE_NOS { get; set; }

        #endregion

        public static async Task<Plat> GetNameAsync(string accountNumber, IDbConnection dbConnection)
        {
            var platNumber = accountNumber.Substring(0, 4);

            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT PLAT_NAME FROM LIS.PLATS WHERE PLAT_NO = {platNumber}";

                return await connection.QueryFirstOrDefaultAsync<Plat>(sql).ConfigureAwait(false) ?? new Plat();
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT  plat_name 
	                         FROM    plats
	                         WHERE   {platNumber} = plat_no";

                return await connection.QueryFirstOrDefaultAsync<Plat>(sql).ConfigureAwait(false) ?? new Plat();
            }
        }
    }
}
