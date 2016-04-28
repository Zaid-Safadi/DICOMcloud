using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearCanvas.Dicom;

namespace DICOMcloud.Pacs.Commands
{
    public interface IStoreCommandResult
    {
    }

    public class StoreCommandResult
    {
        public CommandStatus Status  { get; set; }
        public Exception     Error   { get; set; }
        public string        Message { get; set; }

        public DicomAttributeCollection ReferencedSopInstance { get; set; }
    }

    public enum CommandStatus
    {
        Success,
        Failed
    }
}
