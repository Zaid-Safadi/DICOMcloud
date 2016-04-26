using ClearCanvas.Dicom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DICOMcloud.Dicom.Common
{
    public interface IDicomVrWriter<T,N>
    {
        T WriteElement ( DicomAttribute element, N writer ) ;
    }

    public interface IDicomXmlVrWriter : IDicomVrWriter<string,XmlWriter>
    {
//        string WriteElement ( DicomAttribute element, XmlWriter writer ) ;
    }
}
