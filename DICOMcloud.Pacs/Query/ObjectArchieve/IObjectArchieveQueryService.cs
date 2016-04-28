using System.Collections.Generic;
using ClearCanvas.Dicom;

namespace DICOMcloud.Pacs
{
    public interface IObjectArchieveQueryService
    {
        ICollection<DicomAttributeCollection> FindStudies 
        ( 
            DicomAttributeCollection request, 
            int? limit,
            int? offset    
        ) ;

        ICollection<DicomAttributeCollection> FindObjectInstances
        (
            DicomAttributeCollection request,
            int? limit,
            int? offset
        ) ;

        ICollection<DicomAttributeCollection> FindSeries
        (
            DicomAttributeCollection request,
            int? limit,
            int? offset
        ) ;
    }
}