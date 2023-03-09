using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Services.Interface;

namespace WebTools.Services.Repositories
{
    public class ReportsAPIServices : IReportsAPIServices
    {
        public static class Resource
        {
            public const string baseUrl = "https://localhost:44372";
            public const string testReport = "/api/Reports/GetReport";
        }
        public async Task<string> GetReport(string reportName, string jsonContent)
        {
            var client = new RestClient(Resource.baseUrl);
            var request = new RestRequest(Resource.testReport, Method.Post);
            request.AddParameter("reportName", reportName);
            request.AddParameter("jsonContent", jsonContent);
            var response = await client.GetAsync(request);
            return response.Content;
        }
    }
}
