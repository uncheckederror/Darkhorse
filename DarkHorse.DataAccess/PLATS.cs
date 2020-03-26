using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public class Plat
    {
        public string PLAT_NAME { get; set; }

        public static async Task<Plat> GetNameAsync(string accountNumber, string connectionString)
        {
            using var connection = new OracleConnection(connectionString);

            var platNumber = accountNumber.Substring(0, 4);

            string sql = $@"SELECT  plat_name 
	                        FROM    plats
	                        WHERE   {platNumber} = plat_no";

            var result = await connection.QueryFirstOrDefaultAsync<Plat>(sql).ConfigureAwait(false) ?? new Plat();

            return result;
        }
    }
}
