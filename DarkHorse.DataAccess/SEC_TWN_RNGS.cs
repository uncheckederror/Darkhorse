using Dapper;

using Oracle.ManagedDataAccess.Client;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class SectionTownshipRange : BaseTableClass
    {
        #region Fields
        public int PLAT_SECTION_ID { get; set; }
        public int PLAT_ID { get; set; }
        public int SEC_TWN_RNG_ID { get; set; }
        public string SEC_TWN_RNG { get; set; }
        public int PLAT_NO { get; set; }
        public string PLAT_NAME { get; set; }
        public int TAX_YR { get; set; }
        public string AUDITOR_FILE_NO { get; set; }
        public DateTime? RECORDED_DT { get; set; }
        #endregion
        public static async Task<IEnumerable<SectionTownshipRange>> GetAsync(string sectionTownshipRange, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT SEC_TWN_RNG_ID, SEC_TWN_RNG FROM SEC_TWN_RNGS WHERE SEC_TWN_RNG LIKE '{sectionTownshipRange}%'";

                return await connection.QueryAsync<SectionTownshipRange>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT SEC_TWN_RNG_ID, SEC_TWN_RNG FROM SEC_TWN_RNGS WHERE SEC_TWN_RNG LIKE '{sectionTownshipRange}%'";

                return await connection.QueryAsync<SectionTownshipRange>(sql).ConfigureAwait(false);
            }
        }

        public static async Task<IEnumerable<SectionTownshipRange>> GetRelatedPlatsAsync(int sectionTownshipRangeId, IDbConnection dbConnection)
        {
            if (dbConnection is SqlConnection)
            {
                using var connection = new SqlConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT PS.PLAT_SECTION_ID,
                                      P.PLAT_ID,
                                      STR.SEC_TWN_RNG_ID,
                                      STR.SEC_TWN_RNG,
                                      P.PLAT_NO,
                                      P.PLAT_NAME,
                                      P.TAX_YR,
                                      P.AUDITOR_FILE_NO,
                                      P.RECORDED_DT
                                    FROM PLAT_SECTIONS PS
                                    INNER JOIN SEC_TWN_RNGS STR
                                    ON PS.SEC_TWN_RNG_ID = STR.SEC_TWN_RNG_ID
                                    INNER JOIN PLATS P
                                    ON P.PLAT_ID             = PS.PLAT_ID
                                    WHERE STR.SEC_TWN_RNG_ID = {sectionTownshipRangeId}";

                return await connection.QueryAsync<SectionTownshipRange>(sql).ConfigureAwait(false);
            }
            else
            {
                using var connection = new OracleConnection(dbConnection.ConnectionString);

                var sql = $@"SELECT PS.PLAT_SECTION_ID,
                                      P.PLAT_ID,
                                      STR.SEC_TWN_RNG_ID,
                                      STR.SEC_TWN_RNG,
                                      P.PLAT_NO,
                                      P.PLAT_NAME,
                                      P.TAX_YR,
                                      P.AUDITOR_FILE_NO,
                                      P.RECORDED_DT
                                    FROM PLAT_SECTIONS PS
                                    INNER JOIN SEC_TWN_RNGS STR
                                    ON PS.SEC_TWN_RNG_ID = STR.SEC_TWN_RNG_ID
                                    INNER JOIN PLATS P
                                    ON P.PLAT_ID             = PS.PLAT_ID
                                    WHERE STR.SEC_TWN_RNG_ID = {sectionTownshipRangeId}";


                return await connection.QueryAsync<SectionTownshipRange>(sql).ConfigureAwait(false);
            }
        }

    }
}
