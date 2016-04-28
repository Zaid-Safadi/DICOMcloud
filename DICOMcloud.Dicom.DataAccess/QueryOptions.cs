using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Dicom.DataAccess
{
    public class QueryOptions
    {
        public string QueryLevel { get; set; }

        public int? Limit { get; set; }

        public int? Offset { get; set; }
    }
}
