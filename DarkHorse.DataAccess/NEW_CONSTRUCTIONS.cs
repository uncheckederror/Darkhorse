using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class NewConstruction : BaseTableClass
    {
        public int NEW_CONSTRUCTION_ID { get; set; }
        public int RP_ACCT_ID { get; set; }
        public string ROW_SOURCE { get; set; }
        public string JURISDICTION { get; set; }
        public string PERMIT_TYPE_CODE { get; set; }
        public string APPLICATION_ID { get; set; }
        public string PERMIT_ID { get; set; }

        public static async Task<IEnumerable<NewConstruction>> GetAsync(int realPropertyAccountId, string connectionString)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = $@"SELECT NEW_CONSTRUCTION_ID, RP_ACCT_ID, ROW_SOURCE, JURISDICTION, PERMIT_TYPE_CODE, CREATED_BY, CREATED_DT, APPLICATION_ID, MODIFIED_BY, MODIFIED_DT, PERMIT_ID
                            FROM NEW_CONSTRUCTIONS
                            WHERE RP_ACCT_ID = {realPropertyAccountId}
                            ORDER BY CREATED_DT DESC";

            var results = await connection.QueryAsync<NewConstruction>(sql).ConfigureAwait(false);

            return results;
        }
    }
}
