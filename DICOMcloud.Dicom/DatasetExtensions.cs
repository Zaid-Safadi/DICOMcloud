using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearCanvas.Dicom;

namespace DICOMcloud.Dicom
{
    public static class DatasetExtensions
    {
        public static void Merge ( this DicomAttributeCollection source, DicomAttributeCollection destination )
        { 
            foreach ( var element in source )
            { 
                destination[element.Tag] = element ;
            }
        }
    }
}
