using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Dicom.DataAccess.DB.Query
{
    public partial class ObjectArchieveQueryBuilder
    { 
        class JoinInfo
        {
            public string SourceTable { get; set; }
            public string DestinationTable { get; set; }

            public string JoinText { get; set; }
        }
    }
}
