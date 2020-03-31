using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class Inspection : BaseTableClass
    {
        #region Fields

        public int ASSESSOR_INSPECTION_ID { get; set; }
        public int NEW_CONSTRUCTION_ID { get; set; }
        public string NC_STATUS { get; set; }
        public int NC_VALUE { get; set; }
        public DateTime INSPECTION_DT { get; set; }
        public string INSPECTOR { get; set; }
        public DateTime POSTED_DT { get; set; }
        public char POSTED_FLAG { get; set; }

        #endregion

        public static async Task<IEnumerable<Inspection>> GetAsync(int newConstructionId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT ASSESSOR_INSPECTION_ID, NEW_CONSTRUCTION_ID, NC_STATUS, NC_VALUE, CREATED_BY, CREATED_DT, INSPECTION_DT, INSPECTOR, MODIFIED_BY, MODIFIED_DT, POSTED_DT, POSTED_FLAG
                             FROM LIS.ASSESSOR_INSPECTIONS
                             WHERE NEW_CONSTRUCTION_ID = {newConstructionId}
                             ORDER BY INSPECTION_DT DESC";

                return await connection.QueryAsync<Inspection>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT ASSESSOR_INSPECTION_ID, NEW_CONSTRUCTION_ID, NC_STATUS, NC_VALUE, CREATED_BY, CREATED_DT, INSPECTION_DT, INSPECTOR, MODIFIED_BY, MODIFIED_DT, POSTED_DT, POSTED_FLAG
                             FROM ASSESSOR_INSPECTIONS
                             WHERE NEW_CONSTRUCTION_ID = {newConstructionId}
                             ORDER BY INSPECTION_DT DESC";

                return await connection.QueryAsync<Inspection>(sql).ConfigureAwait(false);
            }
        }
    }
}
