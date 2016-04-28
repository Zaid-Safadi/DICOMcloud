using DICOMcloud.Dicom.Data;
using System.Collections.Generic;

namespace DICOMcloud.Dicom.DataAccess
{
    public interface IDicomInstnaceStorageDataAccess
    {
        void DeleteInstance ( string instance ) ;
        void StoreInstance(IEnumerable<IDicomDataParameter> conditions, int offset, int limit ) ;
        void StoreInstanceMetadata( IObjectID instance, string metadata ) ;
        string GetInstanceMetadata( IObjectID instance ) ;
    }
}