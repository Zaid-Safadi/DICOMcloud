using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearCanvas.Dicom;

namespace DICOMcloud.Pacs.Commands
{
    public interface IStoreCommand : IDicomCommand<DicomFile,StoreCommandResult>
    {

    }
}
