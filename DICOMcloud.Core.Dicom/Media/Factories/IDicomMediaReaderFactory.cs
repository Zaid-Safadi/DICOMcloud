namespace DICOMcloud.Dicom.Media
{
    public interface IDicomMediaReaderFactory
    {
        IDicomMediaReader GetMediaReader ( string mimeType ) ;
    }
}