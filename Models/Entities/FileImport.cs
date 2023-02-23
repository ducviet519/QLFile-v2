using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models.Entities
{
    public class FileImport
    {
        public string status { get; set; }
        public string filepath { get; set; }
        public DataTable dataExcels { get; set; }
    }

}
