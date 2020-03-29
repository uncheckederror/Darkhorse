using Dapper;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

        public static async Task<List<string>> CompareSqlStatements<T>(string originalSql, string modifiedSql, string dbConnectionString) where T : BaseTableClass
        {
            var mismatches = new List<string>();

            using var dbConnection = new OracleConnection(dbConnectionString);
            var originalEnum = await dbConnection.QueryAsync<T>(originalSql).ConfigureAwait(false);
            var modifiedEnum = await dbConnection.QueryAsync<T>(modifiedSql).ConfigureAwait(false);

            var originalResult = originalEnum.ToList();
            var modifiedResult = modifiedEnum.ToList();

            // perform a single JSON value comparison
            // if the results are the same, we're done.
            if (JsonConvert.SerializeObject(originalResult) == JsonConvert.SerializeObject(modifiedResult))
            {
                return mismatches;
            }

            // perform a single JSON value comparison
            // if the results are the same, we're done.
            if (originalResult.Count != modifiedResult.Count)
            {
                mismatches.Add($"Count mismatch: Original SQL = {originalResult.Count:N0}, Modified SQL = {modifiedResult.Count:N0}");
            }

            var maxIterations = originalResult.Count <= modifiedResult.Count
                ? originalResult.Count
                : modifiedResult.Count;

            for (var i = 0; i < maxIterations; i++)
            {
                mismatches.AddRange(originalResult[i].CompareProperties(modifiedResult[i]));
            }

            return mismatches;
        }

        protected List<string> CompareProperties<T>(T compareTo)
        {
            var mismatched = new List<string>();

            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                if (prop.GetValue(this) != prop.GetValue(compareTo))
                {
                    mismatched.Add($"[{prop.Name}]: Original = {prop.GetValue(this)}, Modified = {prop.GetValue(compareTo)}");
                }
            }

            return mismatched;
        }
    }
}
