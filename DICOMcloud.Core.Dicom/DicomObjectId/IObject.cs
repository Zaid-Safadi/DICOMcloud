using System;

namespace DICOMcloud.Dicom.Data
{
    public interface IObjectID : ISeriesID
    {
        string SopInstanceUID {  get; set; }

        int? Frame { get; set; }
    }
}
