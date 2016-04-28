using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DICOMcloud.Dicom.Data;
using DICOMcloud.Core.Storage;

namespace DICOMcloud.Pacs
{
    public interface IObjectRetrieveDataService
    { 
        IStorageLocation RetrieveSopInstance ( IObjectID query, string mimeType ) ;
    
        IEnumerable<IStorageLocation> RetrieveSopInstances ( IObjectID query, string mimeType ) ;
    }
}
