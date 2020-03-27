using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class LegalDescription : BaseTableClass
    {
        public int LEGAL_DESCR_ID { get; set; }
        public int RP_ACCT_ID { get; set; }
        public string LEGAL_TEXT { get; set; }
        public DateTime BEGIN_DT { get; set; }
        public DateTime END_DT { get; set; }

        public static async Task<IEnumerable<LegalDescription>> GetAsync(int realAccountId, string connectionString)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = $@"SELECT  LEGAL_DESCR_ID,
                                    RP_ACCT_ID,
                                    LEGAL_TEXT,
                                    BEGIN_DT,
                                    END_DT,
                                    CREATED_BY,
                                    CREATED_DT,
                                    MODIFIED_BY,
                                    MODIFIED_DT
                            FROM    LEGAL_DESCRS
                            WHERE   RP_ACCT_ID = {realAccountId}";

            var result = await connection.QueryAsync<LegalDescription>(sql).ConfigureAwait(false);

            return result;
        }
    }
}