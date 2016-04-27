using ClearCanvas.Dicom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Dicom.Common
{
    public interface IJsonDicomConverter : IDicomConverter<string>
    { }

    public class JsonDicomConverter : IJsonDicomConverter
    {
        private int _minValueIndex ;

        public JsonDicomConverter ( ) 
        {
            IncludeEmptyElements = false ;
        }

        public bool IncludeEmptyElements
        { 
            get
            { 
                return ( _minValueIndex == -1 ) ;
            }
            set
            { 
                _minValueIndex = ( value ? -1 : 0 ) ;
            }
        }

        public string Convert(DicomAttributeCollection ds)
        {
            StringBuilder sb = new StringBuilder ( ) ;
            StringWriter sw = new StringWriter(sb) ;
            
            using ( JsonWriter writer = new JsonTextWriter (sw))
            { 
                writer.Formatting = Formatting.Indented ;

                writer.WriteStartObject( ) ;
            
                ConvertChildren ( ds, writer ) ;
            
                writer.WriteEndObject ( ) ;
            }

            return sb.ToString ( ) ;
        }

        private void ConvertChildren ( DicomAttributeCollection ds, JsonWriter writer ) 
        {
            //WriteDicomAttribute ( ds, ds[DicomTags.FileMetaInformationVersion], writer ) ;
            //WriteDicomAttribute ( ds, ds[DicomTags.MediaStorageSopClassUid], writer ) ;
            //WriteDicomAttribute ( ds, ds[DicomTags.MediaStorageSopInstanceUid], writer ) ;
            //WriteDicomAttribute ( ds, ds[DicomTags.TransferSyntaxUid], writer ) ;
            //WriteDicomAttribute ( ds, ds[DicomTags.ImplementationClassUid], writer ) ;
            //WriteDicomAttribute ( ds, ds[DicomTags.ImplementationVersionName], writer ) ;
            
            foreach ( var element in ds.Where ( n=>n.Count > _minValueIndex ) )
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
            JsonWriter writer 
        )
        {
            DicomVr dicomVr = element.Tag.VR ;
            
            writer.WritePropertyName(element.Tag.HexString, false);
            //writer.WritePropertyName(element.Tag.Group.ToString("D4") + element.Tag.Element.ToString("D4"), false);
            writer.WriteStartObject();

            
            writer.WritePropertyName("temp");
            writer.WriteValue(element.Tag.VariableName);
            
            writer.WritePropertyName("vr");
            writer.WriteValue(element.Tag.VR.Name);


            //VR should at least support a switch!
            if ( dicomVr.Name == DicomVr.SQvr.Name ) 
            {
                ConvertSequence ( element, writer ) ;
            }
            else if ( dicomVr.Equals (DicomVr.PNvr) )
            {
                
                writer.WritePropertyName (JsonConstants.ValueField);
                writer.WriteStartArray();
                    writer.WriteStartObject();
                        writer.WritePropertyName ( JsonConstants.Alphabetic) ;
                        writer.WriteValue ( element.ToString().TrimEnd()); //TODO: not sure if PN need to be trimmed
                    writer.WriteEndObject();
                writer.WriteEndArray();
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
                    WriteStringValue ( writer, System.Convert.ToBase64String ( data));
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

            writer.WriteEndObject ( ) ; 
        }


        private void ConvertSequence(DicomAttribute element, JsonWriter writer )
        {
            for ( int index = 0; index < element.Count; index++ )
            {
                StringBuilder sqBuilder = new StringBuilder ( ) ;
                StringWriter sw = new StringWriter ( sqBuilder) ;

                using ( JsonWriter sqWriter = new JsonTextWriter (sw))
                {
                    sqWriter.Formatting = Formatting.Indented ;

                    sqWriter.WriteStartArray ( ) ;

                        var item = element.GetSequenceItem ( index ) ;
                        sqWriter.WriteStartObject ( ) ;
                            if ( null != item)
                            { 
                                ConvertChildren ( item, sqWriter) ;
                            }
                        sqWriter.WriteEndObject ( ) ;
                    sqWriter.WriteEndArray ( ) ;
                
                }
                
                WriteSequence ( writer, sqBuilder.ToString ( ) ) ;
            }
        }

        private void WriteSequence(JsonWriter writer,string data)
        {
            writer.WritePropertyName(JsonConstants.ValueField);
            writer.WriteRawValue(data) ;
        }

        private void WriteStringValue(JsonWriter writer,string data)
        {
            writer.WritePropertyName(JsonConstants.ValueField);
            writer.WriteStartArray ( ) ;
                    writer.WriteValue ( data )  ;
            writer.WriteEndArray ( ) ;
                
        }

        private void WriteNumberValue(JsonWriter writer,string data)
        {
            writer.WritePropertyName(JsonConstants.ValueField);
            writer.WriteStartArray ( ) ;
                    writer.WriteValue ( data )  ; //TODO: handle numbers to be with no ""
            writer.WriteEndArray ( ) ;
        }

        private void ConvertValue(DicomAttribute element, JsonWriter writer)
        {
            if ( _numberBasedVrs.Contains (element.Tag.VR.Name))
            {
                WriteNumberValue ( writer, element.ToString().TrimEnd());
            }
            else
            {
                //TODO: NOT ALL VRS CAN BE TRIMMED, CHECK THE STANDARD     
                WriteStringValue ( writer, element.ToString().TrimEnd());
            }
        }
    
        private static List<string> _numberBasedVrs = new List<string> ( ) ;
        private const string QuoutedStringFormat = "\"{0}\"" ;
        private const string QuoutedKeyValueStringFormat = "\"{0}\":\"{1}\"" ;
        private const string QuoutedKeyValueArrayFormat = "\"Value\":[\"{0}\"]" ;
        private const string SequenceValueFormatted     = "\"Value\":[{\"{0}\"}]" ;
        private const string NumberValueFormatted       = "\"Value\":[{1}]" ;

        private abstract class JsonConstants
        {
            public const string ValueField = "Value" ;
            public const string Alphabetic = "Alphabetic" ;
        }
    }
}
