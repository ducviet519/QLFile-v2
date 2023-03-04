using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTools.Models.Entities
{
    public class FileSignPDF
    {
        public string fileName { get; set; }
        public string filePath { get; set; }
    }

    public class TextCoordinates
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int pageIndex { get; set; }
    }
}
