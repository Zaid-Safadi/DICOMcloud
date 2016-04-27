using ClearCanvas.Dicom;

namespace DICOMcloud.Dicom
{
    public interface IDicomConverter<T>
    {
        
        T Convert ( DicomAttributeCollection dicom ) ;
    }
}