using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class Building : BaseTableClass
    {
        #region Fields

        public int LRSN { get; set; }
        public string BLDGNO { get; set; }
        public string IMPROVEMENT_ID { get; set; }
        public int YRBUILT { get; set; }
        public decimal DECKSF { get; set; }
        public string BLDGTYPE { get; set; }
        public decimal BSMTAREA { get; set; }
        public int STORIES { get; set; }
        public decimal BSMTFIN { get; set; }
        public string ROOFMATD { get; set; }
        public decimal DETGARSF { get; set; }
        public string WALL_COVER { get; set; }
        public decimal ATTGARSF { get; set; }
        public decimal FINSIZE { get; set; }
        public int NUMBDRMS { get; set; }
        public string WELL { get; set; }
        public int NUM2BATHS { get; set; }
        public string SEPTIC { get; set; }
        public int FBATHS { get; set; }
        public string WATER { get; set; }
        public string HEATDESC { get; set; }
        public string SEWER { get; set; }
        public string FIREPLACES { get; set; }
        public int YEAR_BUILT { get; set; }
        public int TAX_VALUE { get; set; }
        public int BLDGS { get; set; }

        #endregion

        public static async Task<IEnumerable<Building>> GetAsync(int realPropertyAccountId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT  *
                             FROM    LIS.CAMA_MH_IMPROV_VW
                             WHERE   LRSN = {realPropertyAccountId}";

                return await connection.QueryAsync<Building>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT  *
                             FROM    CAMA_MH_IMPROV_VW CA
                             WHERE   CA.LRSN = {realPropertyAccountId}";

                return await connection.QueryAsync<Building>(sql).ConfigureAwait(false);
            }
        }
    }
}
