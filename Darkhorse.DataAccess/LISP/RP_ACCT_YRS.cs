using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Threading.Tasks;

namespace Darkhorse.DataAccess
{
    public class RealPropertyAccountYears
    {
        public int PROPERTY_CLASS { get; set; }
        public decimal PARCEL_ACREAGE { get; set; }
        public int TAX_CODE { get; set; }

        /// <summary>
        /// Get the 
        /// </summary>
        /// <param name="realAccountOwnerId"></param>
        /// <param name="taxYear"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static async Task<RealPropertyAccountYears> GetRealAccountFiltersAsync(int realAccountOwnerId, DateTime taxYear, string connectionString)
        {
            using var connection = new OracleConnection(connectionString);

            string sql = $@"SELECT property_class, parcel_acreage, tax_code
	                        FROM   rp_acct_yrs
	                        WHERE  rp_acct_owner_id = {realAccountOwnerId}
	                        AND    tax_yr = {taxYear.Year}";

            var result = await connection.QueryFirstOrDefaultAsync<RealPropertyAccountYears>(sql).ConfigureAwait(false) ?? new RealPropertyAccountYears();

            return result;
        }
    }
}
