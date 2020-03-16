using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;

namespace Darkhorse.DataAccess
{
    public class MobileHome
    {
        public string MH_MAKE { get; set; }
        public string MH_MODEL { get; set; }
        public string MH_SERIAL_NUM { get; set; }
        public string IMP_LENGTH { get; set; }
        public string IMP_WIDTH { get; set; }
        public static async Task<MobileHome> GetAsync(int realPropertyAccountId, string buildingExtension, string connectionString)
        {
            using var connection = new OracleConnection(connectionString);

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

            var result = await connection.QueryFirstOrDefaultAsync<MobileHome>(sql).ConfigureAwait(false) ?? new MobileHome();

            return result;
        }
    }
}
