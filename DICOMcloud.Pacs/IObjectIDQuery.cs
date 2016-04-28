
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DICOMcloud.Dicom.Data;

namespace DICOMcloud.Pacs
{
    public interface IObjectQuery : IObjectID
    {
        ObjectQueryLevel QueryLevel     { get; set; }
    }
}
