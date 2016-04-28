using DICOMcloud.Wado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClearCanvas.Dicom;
using System.IO;
using DICOMcloud.Dicom.Media;
using DICOMcloud.Core.Storage;

namespace DICOMcloud.Wado.Core
{
    public class TextObjectHandler : ObjectHandlerBase
    {
        public TextObjectHandler ( IMediaStorageService mediaStorage ) : base ( mediaStorage )
        {}

        public override bool CanProcess(string mimeType)
        {
            return string.Compare (mimeType , MimeMediaTypes.PlainText, true ) == 0 ;
        }

        protected override WadoResponse DoProcess(IWadoUriRequest request, string mimeType)
        {
            DicomFile df = new DicomFile ( ) ;
            WadoResponse response = new WadoResponse ( ) ;


            df.Load ( Location.GetReadStream ( ), null, DicomReadOptions.DoNotStorePixelDataInDataSet);

            response.Content = GenerateStreamFromString ( df.DataSet.Dump() );
            response.MimeType = mimeType ;

            return response ;
        }

        public Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
   }
}
