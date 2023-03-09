using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTools.Services.Interface;

namespace WebTools.Services.Repositories
{
    public class ReportsAPIServices : IReportsAPIServices, IDisposable
    {
        private readonly RestClient _client;
        //public


        public void Dispose()
        {
            _client?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
