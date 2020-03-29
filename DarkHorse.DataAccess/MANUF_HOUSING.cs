using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class MobileHome : BaseTableClass
    {
        public string MH_MAKE { get; set; }
        public string MH_MODEL { get; set; }
        public string MH_SERIAL_NUM { get; set; }
        public string IMP_LENGTH { get; set; }
        public string IMP_WIDTH { get; set; }

        public static async Task<IEnumerable<MobileHome>> GetAsync(int realPropertyAccountId, string buildingExtension, IDbConnection dbConnection)
        {
            if (dbConnection.GetType()?.Name == "SqlConnection")
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                string sql = $@"";

                var result = await connection.QueryAsync<MobileHome>(sql).ConfigureAwait(false);

                return result;
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                string sql = $@"SELECT  cmh.mh_make,
                                    cmh.mh_model,
                                    cmh.mh_serial_num,
                                    ci.imp_length,
                                    ci.imp_width
                            FROM    cama.manuf_housing cmh,
                                    cama.improvements ci
                            WHERE   cmh.lrsn                          = {realPropertyAccountId}
                            AND     (cmh.mh_make                       IS NOT NULL
                            OR      cmh.mh_model                        IS NOT NULL
                            OR      cmh.mh_serial_num                   IS NOT NULL)
                            AND     cmh.lrsn                            = ci.lrsn
                            AND     cmh.MH_EXTRA_CHAR20A                = ci.extension
                            AND     to_number(SUBSTR(ci.extension,2,2)) = to_number('{buildingExtension}')
                            AND (ci.improvement_id                  = 'C'
                            OR ci.improvement_id                    = 'D'
                            OR ci.improvement_id                    = 'M')
                            AND ci.status                           = 'A'
                            AND ci.extension                       IN
                            (SELECT ce.extension
                            FROM cama.extensions ce
                            WHERE ce.lrsn = cmh.lrsn
                            AND ce.status = 'A'
                            )";

                var result = await connection.QueryAsync<MobileHome>(sql).ConfigureAwait(false);

                return result;
            }
        }
    }
}
