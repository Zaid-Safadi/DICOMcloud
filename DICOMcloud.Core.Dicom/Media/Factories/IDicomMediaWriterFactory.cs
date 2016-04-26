namespace DICOMcloud.Dicom.Media
{
    public interface IDicomMediaWriterFactory
    {
        IDicomMediaWriter GetMediaWriter(string mimeType);
    }
}