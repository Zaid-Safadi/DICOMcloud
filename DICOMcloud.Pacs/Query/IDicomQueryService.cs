using System.Collections.Generic;
using ClearCanvas.Dicom;
using DICOMcloud.Dicom.DataAccess;

namespace DICOMcloud.Pacs.Query
{
    public interface IDicomQueryService
    {
        ICollection<DicomAttributeCollection> Find 
        ( 
            DicomAttributeCollection request, 
            QueryOptions options 
        ) ;
    }
}