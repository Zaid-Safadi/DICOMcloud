using ClearCanvas.Dicom;
using DICOMcloud.Core.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;


namespace DICOMcloud.Dicom.Common
{
    
    public interface IUncompressedPixelDataConverter : IDicomConverter<Stream>
    {}

    public class UncompressedPixelDataConverter : IUncompressedPixelDataConverter
    {
        public UncompressedPixelDataConverter()
        {
        }

        public Stream Convert ( DicomAttributeCollection ds )
        {
            DicomAttributeCollection command = new DicomAttributeCollection () ;

            command[DicomTags.TransferSyntaxUid] = ds[DicomTags.TransferSyntaxUid] ;
            DicomMessage message = new DicomMessage (command, ds) ;

            
            DicomPixelData pd = DicomPixelData.CreateFrom ( message ) ;
            string tempFile = System.IO.Path.GetTempFileName ( ) ; 
            System.IO.File.WriteAllBytes ( tempFile, pd.GetFrame (0));
        
            return new DICOMcloud.Core.Storage.TempStream (new TempFile(tempFile));
        }
    }
}
