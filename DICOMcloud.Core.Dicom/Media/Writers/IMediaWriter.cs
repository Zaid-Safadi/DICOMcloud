using System.Collections.Generic;
using DICOMcloud.Core.Storage;

namespace DICOMcloud.Dicom.Media
{
    public interface IMediaWriter<T>
    {
        IList<IStorageLocation> CreateMedia ( T data ) ;

        string MediaType 
        { 
            get ;
        }
    }
}