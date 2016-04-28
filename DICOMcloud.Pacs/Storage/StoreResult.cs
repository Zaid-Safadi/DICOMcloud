using System;
using ClearCanvas.Dicom;
using DICOMcloud.Pacs.Commands;

namespace DICOMcloud.Pacs
{
    public class StoreResult
    {
        public Exception Error { get; set; }
        public string Message { get; set; }
        public CommandStatus Status { get; set; }
    
        public DicomAttributeCollection DataSet { get; set; }    
    }
}