using DICOMcloud.Core.Storage;
using DICOMcloud.Dicom.Data;
using DICOMcloud.Dicom.Media;
using System.Collections.Generic;

namespace DICOMcloud.Pacs
{
    public class ObjectRetrieveDataService : IObjectRetrieveDataService
    {
        private IMediaStorageService __Storage { get; set; }

        public ObjectRetrieveDataService ( IMediaStorageService mediaStorage )
        {
            __Storage = mediaStorage ;
        }
        
        public IStorageLocation RetrieveSopInstance ( IObjectID query, string mimeType = null ) 
        {
            return __Storage.GetLocation ( new DicomMediaId ( query, mimeType )) ;
        }
        
        public IEnumerable<IStorageLocation> RetrieveSopInstances ( IObjectID query, string mimeType = null ) 
        {
            return __Storage.EnumerateLocation ( new DicomMediaId ( query, mimeType )) ;
        }
    }
}
