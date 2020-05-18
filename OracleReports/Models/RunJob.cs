using ServiceReference;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace OracleReports
{
    public class Destination
    {
        public string Type { get; set; }
        public string Format { get; set; }
    }

    public class TimingInfo
    {
        public DateTime Queued { get; set; }
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }
    }

    public class FormParameter
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class Job
    {
        public int JobId { get; set; }
        public string QueueType { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int StatusCode { get; set; }
        public string Status { get; set; }
        public string Owner { get; set; }
        public string Server { get; set; }
        public Destination Destination { get; set; }
        public TimingInfo TimingInfo { get; set; }
        public XmlDocument RawResponse { get; set; }

        /// <summary>
        /// This is a custom XML parser because the WDSL file doesn't contain enough info on the response format to autogenerate this code.
        /// </summary>
        /// <param name="response"></param>
        /// <returns> A staticly-typed POCO. </returns>
        public static Job ParseFromResponse(runJobResponse response)
        {
            var xmlFromResponse = new XmlDocument();
            xmlFromResponse.LoadXml(response.Body.@return);

            var jobInfo = new Job
            {
                RawResponse = xmlFromResponse
            };

            var jobs = xmlFromResponse.GetElementsByTagName("job");
            foreach (XmlNode job in jobs)
            {
                if (job.Attributes != null)
                {
                    foreach (XmlAttribute item in job.Attributes)
                    {
                        string itemName = item.Name;

                        if (itemName == "id")
                        {
                            var checkid = int.TryParse(item.Value, out int value);
                            if (checkid)
                            {
                                jobInfo.JobId = value;
                            }
                        }

                        if (itemName == "queueType")
                        {
                            jobInfo.QueueType = item.Value;
                        }
                    }
                }

                if (job.ChildNodes != null)
                {
                    foreach (XmlNode node in job.ChildNodes)
                    {
                        if (node.Name == "status")
                        {
                            if (node.Attributes != null)
                            {
                                foreach (XmlAttribute item in node.Attributes)
                                {
                                    if (item.Name == "code")
                                    {
                                        var checkCode = int.TryParse(item.Value, out int value);
                                        if (checkCode)
                                        {
                                            jobInfo.StatusCode = value;
                                        }
                                    }
                                }
                            }

                            jobInfo.Status = node.FirstChild != null ? node.FirstChild.Value : string.Empty;
                        }

                        if (node.Name == "destination")
                        {
                            if (node.ChildNodes != null)
                            {
                                var type = string.Empty;
                                var format = string.Empty;
                                foreach (XmlNode subNode in node.ChildNodes)
                                {
                                    if (subNode.Name == "desType")
                                    {
                                        type = subNode.FirstChild != null ? subNode.FirstChild.Value : string.Empty;
                                    }

                                    if (subNode.Name == "desFormat")
                                    {
                                        format = subNode.FirstChild != null ? subNode.FirstChild.Value : string.Empty;
                                    }
                                }

                                jobInfo.Destination = new Destination
                                {
                                    Type = type,
                                    Format = format
                                };
                            }
                        }

                        if (node.Name == "timingInfo")
                        {
                            if (node.ChildNodes != null)
                            {
                                var queued = new DateTime();
                                var started = new DateTime();
                                var finished = new DateTime();

                                foreach (XmlNode subNode in node.ChildNodes)
                                {
                                    if (subNode.Name == "queued")
                                    {
                                        var check = DateTime.TryParse(subNode.FirstChild.Value, out DateTime parsed);
                                        if (check)
                                        {
                                            queued = parsed;
                                        }
                                    }

                                    if (subNode.Name == "started")
                                    {
                                        var check = DateTime.TryParse(subNode.FirstChild.Value, out DateTime parsed);
                                        if (check)
                                        {
                                            started = parsed;
                                        }
                                    }

                                    if (subNode.Name == "finished")
                                    {
                                        var check = DateTime.TryParse(subNode.FirstChild.Value, out DateTime parsed);
                                        if (check)
                                        {
                                            finished = parsed;
                                        }
                                    }
                                }

                                jobInfo.TimingInfo = new TimingInfo
                                {
                                    Queued = queued,
                                    Started = started,
                                    Finished = finished
                                };
                            }
                        }

                        switch (node.Name)
                        {
                            case "name":
                                jobInfo.Name = node.FirstChild != null ? node.FirstChild.Value : string.Empty;
                                break;
                            case "type":
                                jobInfo.Type = node.FirstChild != null ? node.FirstChild.Value : string.Empty;
                                break;
                            case "owner":
                                jobInfo.Owner = node.FirstChild != null ? node.FirstChild.Value : string.Empty;
                                break;
                            case "server":
                                jobInfo.Server = node.FirstChild != null ? node.FirstChild.Value : string.Empty;
                                break;
                        }
                    }
                }
            }

            return jobInfo;
        }

        /// <summary>
        /// This is a custom XML parser because the WDSL file doesn't contain enough info on the response format to autogenerate this code.
        /// </summary>
        /// <param name="response"></param>
        /// <returns> A staticly-typed POCO. </returns>
        public static Job ParseFromStatusResponse(getJobInfoResponse response)
        {
            var xmlFromResponse = new XmlDocument();
            xmlFromResponse.LoadXml(response.Body.@return);

            var jobInfo = new Job
            {
                RawResponse = xmlFromResponse
            };

            var jobs = xmlFromResponse.GetElementsByTagName("job");
            foreach (XmlNode job in jobs)
            {
                if (job.Attributes != null)
                {
                    foreach (XmlAttribute item in job.Attributes)
                    {
                        string itemName = item.Name;

                        if (itemName == "id")
                        {
                            var checkid = int.TryParse(item.Value, out int value);
                            if (checkid)
                            {
                                jobInfo.JobId = value;
                            }
                        }

                        if (itemName == "queueType")
                        {
                            jobInfo.QueueType = item.Value;
                        }
                    }
                }

                if (job.ChildNodes != null)
                {
                    foreach (XmlNode node in job.ChildNodes)
                    {
                        if (node.Name == "status")
                        {
                            if (node.Attributes != null)
                            {
                                foreach (XmlAttribute item in node.Attributes)
                                {
                                    if (item.Name == "code")
                                    {
                                        var checkCode = int.TryParse(item.Value, out int value);
                                        if (checkCode)
                                        {
                                            jobInfo.StatusCode = value;
                                        }
                                    }
                                }
                            }

                            jobInfo.Status = node.FirstChild != null ? node.FirstChild.Value : string.Empty;
                        }

                        if (node.Name == "destination")
                        {
                            if (node.ChildNodes != null)
                            {
                                var type = string.Empty;
                                var format = string.Empty;
                                foreach (XmlNode subNode in node.ChildNodes)
                                {
                                    if (subNode.Name == "desType")
                                    {
                                        type = subNode.FirstChild != null ? subNode.FirstChild.Value : string.Empty;
                                    }

                                    if (subNode.Name == "desFormat")
                                    {
                                        format = subNode.FirstChild != null ? subNode.FirstChild.Value : string.Empty;
                                    }
                                }

                                jobInfo.Destination = new Destination
                                {
                                    Type = type,
                                    Format = format
                                };
                            }
                        }

                        if (node.Name == "timingInfo")
                        {
                            if (node.ChildNodes != null)
                            {
                                var queued = new DateTime();
                                var started = new DateTime();
                                var finished = new DateTime();

                                foreach (XmlNode subNode in node.ChildNodes)
                                {
                                    if (subNode.Name == "queued")
                                    {
                                        var check = DateTime.TryParse(subNode.FirstChild.Value, out DateTime parsed);
                                        if (check)
                                        {
                                            queued = parsed;
                                        }
                                    }

                                    if (subNode.Name == "started")
                                    {
                                        var check = DateTime.TryParse(subNode.FirstChild.Value, out DateTime parsed);
                                        if (check)
                                        {
                                            started = parsed;
                                        }
                                    }

                                    if (subNode.Name == "finished")
                                    {
                                        var check = DateTime.TryParse(subNode.FirstChild.Value, out DateTime parsed);
                                        if (check)
                                        {
                                            finished = parsed;
                                        }
                                    }
                                }

                                jobInfo.TimingInfo = new TimingInfo
                                {
                                    Queued = queued,
                                    Started = started,
                                    Finished = finished
                                };
                            }
                        }

                        switch (node.Name)
                        {
                            case "name":
                                jobInfo.Name = node.FirstChild != null ? node.FirstChild.Value : string.Empty;
                                break;
                            case "type":
                                jobInfo.Type = node.FirstChild != null ? node.FirstChild.Value : string.Empty;
                                break;
                            case "owner":
                                jobInfo.Owner = node.FirstChild != null ? node.FirstChild.Value : string.Empty;
                                break;
                            case "server":
                                jobInfo.Server = node.FirstChild != null ? node.FirstChild.Value : string.Empty;
                                break;
                        }
                    }
                }
            }

            return jobInfo;
        }

        /// <summary>
        /// Submits a report to the report server to run.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="serverName"></param>
        /// <param name="reportName"></param>
        /// <param name="userid"></param>
        /// <param name="formParameters"></param>
        /// <returns> The status of the report. </returns>
        public static async Task<Job> StartJobAsync(RWWebServiceClient client, string serverName, string reportName, string userid, IEnumerable<FormParameter>? formParameters)
        {
            // Build the command and set the parameters. The report name must be lower case. Typically the report name is the same as the module name.
            var command = $"server={serverName} report={reportName.ToLower()}.rdf destype=cache desname=gonowhere desformat=pdf userid={userid}";
            if (formParameters != null && formParameters.Any())
            {
                foreach (var param in formParameters)
                {
                    command += $" {param.Name}={param.Value}";
                }
            }

            // Submit the job.
            var startJobResponse = await client.runJobAsync(command, false);

            // Parse the response.
            return ParseFromResponse(startJobResponse);
        }
    }
}
