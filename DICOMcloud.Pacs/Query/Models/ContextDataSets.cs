using ClearCanvas.Dicom;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Pacs
{
    public partial class ObjectArchieveResponseBuilder
    { 
        class KeyToDataSetCollection : ConcurrentDictionary<string,DicomAttributeCollection>{}

        class ResultSetCollection : ConcurrentDictionary<string, KeyToDataSetCollection> { }
    }
}
