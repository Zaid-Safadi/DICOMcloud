using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearCanvas.Dicom;
using DICOMcloud.Core.Storage;

namespace DICOMcloud.Dicom.Media
{
    public class UncompressedMediaWriter : DicomMediaWriterBase
    {
        public UncompressedMediaWriter ( ) : base ( ) {}
         
        public UncompressedMediaWriter ( IMediaStorageService mediaStorage ) : base ( mediaStorage ) {}

        public override string MediaType
        {
            get
            {
                return MimeMediaTypes.UncompressedData ;
            }
        }

        protected override bool StoreMultiFrames
        {
            get
            {
                return true ;
            }
        }

        protected override void Upload ( DicomFile dicomObject, int frame, IStorageLocation storeLocation)
        {
            DicomPixelData pd = null ;
            byte[] buffer     = null ;


            pd     = DicomPixelData.CreateFrom ( dicomObject ) ;
            buffer = pd.GetFrame               ( frame - 1 ) ;
        
            storeLocation.Upload ( buffer ) ;    
        }
    }
}
