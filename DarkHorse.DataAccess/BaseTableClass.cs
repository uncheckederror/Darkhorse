using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public abstract class BaseTableClass
    {
        #region Fields

        public string CREATED_BY { get; set; }
        public DateTime CREATED_DT { get; set; }

        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DT { get; set; }

        #endregion

        /// <summary>
        /// Override the ToString() method to dump the model object's properties as JSON
        /// </summary>
        /// <returns>String</returns>
        public override string ToString()
        {
            try
            {
                // Example of handling errors within NewtonSoft vs. catching.
                // https://www.newtonsoft.com/json/help/html/SerializationErrorHandling.htm

                var errors = new List<string>();

                var settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    MaxDepth = 10, /* EA: arbitrary */
                    DateFormatString = "MM/dd/yyyy hh:mm:ss tt",
                    Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs ea)
                    {
                        errors.Add(ea.ErrorContext.Error.Message);
                        ea.ErrorContext.Handled = true;
                    }
                };

                if (Debugger.IsAttached)
                {
                    errors.ForEach(s => Debug.WriteLine(s));
                }

                return JsonConvert.SerializeObject(this, settings);
            }
            catch (Exception ex)
            {
                return JsonConvert.SerializeObject(ex);
            }
        }

        public static async Task<List<string>> CompareSqlSatatements<T>(string originalSql, string revisedSql, IDbConnection dbConnection)
        {
            var originalResult = await dbConnection.QueryAsync<T>(originalSql).ConfigureAwait(false);
            var revisedResult = await dbConnection.QueryAsync<T>(revisedSql).ConfigureAwait(false);

            //TODO: compare the two objects, creating a list of mismatches.
            //If the count is 0, the SQL query results are the same.

            var mismatch = new List<string>();

            return mismatch;
        }
    }
}
