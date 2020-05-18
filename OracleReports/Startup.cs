using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using ServiceReference;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace OracleReports
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {

                var configuration = new ConfigurationBuilder()
                                            .AddJsonFile("appsettings.json")
                                            .AddUserSecrets("fcb4eb5f-a4e9-49af-9bb0-72b3a44ebda8")
                                            .Build();

                // This documentation was endlessly helpful:
                // https://docs.oracle.com/cd/B10464_05/bi.904/b13673/pbr_webservice.htm#i1006927
                // This will show you all the jobs subitted to the server.
                // http://kclis3.co.kitsap.local:9002/reports/rwservlet/showjobs?server=reportservertest

                var client = new RWWebServiceClient();
                client.ClientCredentials.UserName.UserName = configuration.GetConnectionString("ReportsUsername");
                client.ClientCredentials.UserName.Password = configuration.GetConnectionString("ReportsPassword");
                var serverName = configuration.GetConnectionString("ReportsServer");
                // The format is username/password/database
                var credentialsString = $"{configuration.GetConnectionString("ReportsUsername")}/{configuration.GetConnectionString("ReportsPassword")}@";
                if (serverName.Contains("test"))
                {
                    // The test reports on the test reports server are complied to run against the LISS database.
                    credentialsString += "liss";
                }


                //var response = await client.getAPIVersionAsync();
                //var response = await client.getServerInfoAsync(serverName, string.Empty);
                //var response = await client.getJobInfoAsync(0, string.Empty, string.Empty);

                // Reports Tested working
                //var reportName = "rckpendingrpt".ToLower();
                //var reportName = "LISRPARDELQACCTS".ToLower();

                //var reportName = "LISRPARLTROFDMD".ToLower();
                //var parameters = new List<FormParameter>
                //{
                //    new FormParameter
                //    {
                //        Name = "PF_foreclose_month_day",
                //        Value = "1201"
                //    },
                //    new FormParameter
                //    {
                //        Name = "PF_rp_acct_id",
                //        Value = "2385011"
                //    }
                //};

                var reportName = "LISRPARLTROFDMD";
                var parameters = new List<FormParameter>
                {
                    new FormParameter
                    {
                        Name = "PF_foreclose_month_day",
                        Value = "1201"
                    },
                    new FormParameter
                    {
                        Name = "PF_rp_acct_id",
                        Value = "2385011"
                    }
                };

                var job = await Job.StartJobAsync(client, serverName, reportName, credentialsString, parameters);

                bool jobFinished = false;
                var responseStatus = await client.getJobInfoAsync(job.JobId, serverName, string.Empty);
                while (!jobFinished)
                {
                    await Task.Delay(1000);
                    job = Job.ParseFromStatusResponse(responseStatus);
                    if (job.StatusCode == 4)
                    {
                        // Sucess
                        jobFinished = true;
                    }
                    if (job.StatusCode == 5)
                    {
                        // Failure
                        jobFinished = true;
                    }
                    if (job.StatusCode == 2)
                    {
                        jobFinished = false;
                    }
                    responseStatus = await client.getJobInfoAsync(job.JobId, serverName, string.Empty);
                }
                //var response = await client.getServerInfoAsync("rep_wls_reports_kclis3_frinst", string.Empty);
                //await context.Response.WriteAsync(responseStatus.Body.@return);
                context.Response.Redirect($"http://kclis3.co.kitsap.local:9002/reports/rwservlet/getjobid{job.JobId}?server=reportservertest");
            });
        }
    }
}
