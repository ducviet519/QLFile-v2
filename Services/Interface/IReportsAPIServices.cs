using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Services.Interface
{
    public interface IReportsAPIServices
    {
        Task<string> GetReport(string reportName, string jsonContent);
    }
}
