using ClearCanvas.Dicom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;


namespace DICOMcloud.Dicom.Common
{
    
    public interface IXmlDicomConverter : IDicomConverter<string>
    {}

    public class XmlDicomConverter : IXmlDicomConverter
    {
        protected XmlDicomWriterService WriterService {get; set; }
        public XmlDicomConverter()
        {
            WriterService = new XmlDicomWriterService ( ) ;
        }

        public string Convert ( DicomAttributeCollection ds )
        {
            StringBuilder sb = new StringBuilder ( ) ;
            XmlWriter writer = XmlTextWriter.Create (sb);
            
            WriteHeaders(writer);

            writer.WriteStartElement("NativeDicomModel") ;
            
            ConvertChildren ( ds, writer ) ;
            
            writer.WriteEndElement ( ) ;

            writer.Close ( ) ;

            return sb.ToString ( ) ;
        }

        private void WriteHeaders(XmlWriter writer)
        {
            writer.WriteStartDocument ();
        }

        private void ConvertChildren ( DicomAttributeCollection ds, XmlWriter writer ) 
        {
            //WriteDicomAttribute ( ds, ds[DicomTags.FileMetaInformationVersion], writer ) ;
            //WriteDicomAttribute ( ds, ds[DicomTags.MediaStorageSopClassUid], writer ) ;
            //WriteDicomAttribute ( ds, ds[DicomTags.MediaStorageSopInstanceUid], writer ) ;
            //WriteDicomAttribute ( ds, ds[DicomTags.TransferSyntaxUid], writer ) ;
            //WriteDicomAttribute ( ds, ds[DicomTags.ImplementationClassUid], writer ) ;
            //WriteDicomAttribute ( ds, ds[DicomTags.ImplementationVersionName], writer ) ;

            foreach ( var element in ds.Where ( n=>n.Count > 0 ) )
            {
                //TODO:
                //WriterService.WriteElement (element,writer);
                WriteDicomAttribute ( ds, element, writer ) ;
            }
        }

        private void WriteDicomAttribute 
        ( 
            DicomAttributeCollection ds, 
            DicomAttribute element, 
            XmlWriter writer 
        )
        {
            if (null == element ) {return ; }

            DicomVr dicomVr = element.Tag.VR ;
            
            writer.WriteStartElement ("DiocomAttribute") ;

            writer.WriteAttributeString ("keyword", element.Tag.VariableName ) ;
            writer.WriteAttributeString ("tag", element.Tag.Group.ToString("D4") + element.Tag.Element.ToString("D4") ) ;
            writer.WriteAttributeString ("vr", element.Tag.VR.Name ) ;            

            //VR should at least support a switch!
            if ( dicomVr.Name == DicomVr.SQvr.Name ) 
            {
                ConvertSequence ( element, writer ) ;
            }
            else if ( dicomVr.Equals (DicomVr.PNvr) )
            {
                for (int index = 0; index < element.Count; index++)
                {
                    writer.WriteStartElement ( "PersonName");
                    WriteNumberAttrib(writer, index) ;

                    writer.WriteElementString("Alphabetic", element.ToString()) ; //TODO: check the standard
                    writer.WriteEndElement ( ) ;
                }
            }
            else if ( dicomVr.Equals (DicomVr.OBvr) || dicomVr.Equals(DicomVr.ODvr) ||
                      dicomVr.Equals (DicomVr.OFvr) || dicomVr.Equals(DicomVr.OWvr) ||
                      dicomVr.Equals (DicomVr.UNvr) ) //TODO inline bulk
            {
                if ( element.Tag.TagValue == DicomTags.PixelData )
                { }
                else
                { 
                    byte[] data = (byte[]) element.Values;
                    writer.WriteBase64 ( data, 0, data.Length ) ;
                }
            }
            //else if ( dicomVr.Equals (DicomVr.PNvr) ) //TODO bulk reference
            //{
                
            //}
            else 
            {
                ConvertValue(element, writer);
            }
            
            if ( element.Tag.IsPrivate )
            { 
                //TODO:
                //writer.WriteAttributeString ("privateCreator", ds[DicomTags.privatecreatro. ) ;                        
            }

            writer.WriteEndElement ( ) ;
        }

        private static void ConvertValue(DicomAttribute element, XmlWriter writer )
        {
            DicomVr dicomVr = element.Tag.VR ;


            for ( int index = 0; index < element.Count; index++ )
            {
                writer.WriteStartElement ( "Value");
                WriteNumberAttrib(writer, index);
                    
                if ( dicomVr.Equals(DicomVr.ATvr))
                {
                    writer.WriteString(element.GetString(index, string.Empty)); //TODO: check standard
                }
                else
                {
                    writer.WriteString(element.GetString(index,string.Empty)); 
                }

                writer.WriteEndElement ( );
            }
        }

        private static void WriteNumberAttrib(XmlWriter writer, int index)
        {
            writer.WriteAttributeString("number", (index + 1).ToString());
        }

        private void ConvertSequence(DicomAttribute element, XmlWriter writer )
        {
            for ( int index = 0; index < element.Count; index++ )
            {
                var item = element.GetSequenceItem ( index ) ;

                writer.WriteStartElement ( "Item");
                WriteNumberAttrib(writer, index);
                
                ConvertChildren(item, writer);

                writer.WriteEndElement () ;
            }
        }
    }
}
