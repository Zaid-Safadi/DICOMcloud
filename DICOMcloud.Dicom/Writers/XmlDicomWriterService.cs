using ClearCanvas.Dicom;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DICOMcloud.Dicom.Common
{
    public class XmlDicomWriterService
    {
        static XmlDicomWriterService ( )
        {

            _vrWriters = new ConcurrentDictionary<string,IDicomXmlVrWriter> ( ) ;
        }
        public XmlDicomWriterService ( )
        { }

        public XmlDicomWriterService ( DicomAttribute dicomElement )
        { 
            DicomElement = dicomElement ;
        }

        internal string WriteElement(DicomAttributeCollection ds, DicomAttribute element, XmlWriter writer)
        {
            IDicomXmlVrWriter vrWriter = GetVrWriter ( element ) ;

            return vrWriter.WriteElement ( element, writer ) ;
        }

        private IDicomXmlVrWriter GetVrWriter(DicomAttribute element)
        {
            return _vrWriters.GetOrAdd ( element.Tag.VR.Name, CreateDefualtVrWriter(element.Tag.VR));
        }

        protected virtual IDicomXmlVrWriter CreateDefualtVrWriter(DicomVr dicomVr)
        {
            IDicomXmlVrWriter writer = null ;

            if ( !_defaultVrWriters.TryGetValue ( dicomVr.Name, out writer) )
            { 
                throw new ApplicationException ( "Default VR writer not registered!") ;
            }

            return writer ;
        }

    
    
    
        public DicomAttribute DicomElement { get; set; }
    


        private static ConcurrentDictionary<string,IDicomXmlVrWriter> _vrWriters ;
        private static ConcurrentDictionary<string,IDicomXmlVrWriter> _defaultVrWriters ;
    }
}
