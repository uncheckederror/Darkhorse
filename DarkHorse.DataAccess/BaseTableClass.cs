using Dapper;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DarkHorse.DataAccess
{
    public abstract class BaseTableClass : IComparable<BaseTableClass>
    {
        #region Fields

        public string CREATED_BY { get; set; }
        public DateTime CREATED_DT { get; set; }

        public string MODIFIED_BY { get; set; }
        public DateTime MODIFIED_DT { get; set; }

        #endregion

        /// <summary>
        /// Helper method to create the correct database connection object
        /// based on the object type of the prototype parameter.
        /// </summary>
        /// <param name="prototype">The object to base the new database connection on.</param>
        /// <returns>New database connection object of the appropriate type.</returns>
        /// <remarks>Do not use the prototype for connecting to the database.</remarks>
        protected static IDbConnection Connection(IDbConnection prototype) =>
            (prototype is SqlConnection)
                ? new SqlConnection(prototype.ConnectionString) as IDbConnection
                : new OracleConnection(prototype.ConnectionString) as IDbConnection;

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

        /// <summary>
        /// Compare the properties of one object to another of the same type.
        /// </summary>
        /// <param name="other">The "other" object to compare to.</param>
        /// <returns>0 if equal, non-zero if not.</returns>
        /// <remarks>If the created or modified dates do not match, the comparison automatically fails.</remarks>
        public int CompareTo(BaseTableClass other)
        {
            if (other == null)
            {
                throw new ArgumentException("Attempt to compare to null object.");
            }

            if (GetType() != other.GetType())
            {
                throw new ArgumentException("Objects are not of the same type.");
            }

            if (CREATED_DT < other.CREATED_DT || MODIFIED_DT < other.MODIFIED_DT)
            {
                return -1;
            }

            if (CREATED_DT > other.CREATED_DT || MODIFIED_DT > other.MODIFIED_DT)
            {
                return 1;
            }

            foreach (PropertyInfo prop in GetType().GetProperties())
            {
                if (!prop.Name.StartsWith("CREATED_BY") && !prop.Name.StartsWith("MODIFIED_BY"))
                {
                    if (Debugger.IsAttached)
                    {
                        Debug.WriteLine($"Comparing {prop.Name}");
                    }

                    var thisValue = prop.GetValue(this);
                    var otherValue = prop.GetValue(other);

                    if (thisValue == null && otherValue != null)
                    {
                        return -1;
                    }

                    if (thisValue != null && otherValue == null)
                    {
                        return 1;
                    }

                    if (thisValue != null && otherValue != null && !thisValue.Equals(otherValue))
                    {
                        return -1;
                    }
                }
            }

            return 0;
        }
    }
}
