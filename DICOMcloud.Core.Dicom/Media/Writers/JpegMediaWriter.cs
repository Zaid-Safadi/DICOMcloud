using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearCanvas.Dicom;
using DICOMcloud.Core.Storage;

namespace DICOMcloud.Dicom.Media
{
    public class JpegMediaWriter : DicomMediaWriterBase
    {
        public JpegMediaWriter ( ) : base ( ) {}
         
        public JpegMediaWriter ( IMediaStorageService mediaStorage ) : base ( mediaStorage ) {}

        public override string MediaType
        {
            get
            {
                return MimeMediaTypes.Jpeg ;
            }
        }

        protected override bool StoreMultiFrames
        {
            get
            {
                return true ;
            }
        }

        protected override void Upload ( DicomFile dicomObject, int frame, IStorageLocation storeLocation )
        {
            var frameIndex = frame - 1 ;
            
            
            if (dicomObject.TransferSyntax == TransferSyntax.JpegBaselineProcess1)
            {
                DicomCompressedPixelData pd = DicomPixelData.CreateFrom(dicomObject) as DicomCompressedPixelData ;

                
                byte[] buffer = pd.GetFrameFragmentData ( frameIndex ) ;

                storeLocation.Upload ( buffer) ;
            }
            else if ( false ) //TODO: handle compressed images properly!
            {
                DicomFile dcmJpeg = new DicomFile ( ) ;
                DicomUncompressedPixelData unCompressed = DicomPixelData.CreateFrom (dicomObject)as DicomUncompressedPixelData ;
                DicomCompressedPixelData compressed = new DicomCompressedPixelData (unCompressed) ;


                //compressed.ImageWidth = unCompressed.ImageWidth;
                //compressed.ImageHeight = unCompressed.HighBit;
                compressed.BitsStored = 8;
                compressed.BitsAllocated = 8;
                //compressed.HighBit = 7;
                compressed.SamplesPerPixel = 3;
                //compressed.PlanarConfiguration = 0;
                compressed.PhotometricInterpretation = "YBR_FULL_422";
                compressed.TransferSyntax = TransferSyntax.JpegBaselineProcess1 ;

                byte[] imageBuffer = unCompressed.GetFrame (frameIndex);
                compressed.AddFrameFragment(imageBuffer);

                compressed.UpdateMessage(dcmJpeg);

                storeLocation.Upload ( compressed.GetFrame(frameIndex) ) ;
                //ClearCanvas.Dicom.Codec.Jpeg.Jpeg8Codec codec = new ClearCanvas.Dicom.Codec.Jpeg.Jpeg8Codec (ClearCanvas.Dicom.Codec.Jpeg.JpegMode.Baseline, 0, 0 ) ;
                //ClearCanvas.Dicom.Codec.Jpeg.DicomJpegParameters jparam = new ClearCanvas.Dicom.Codec.Jpeg.DicomJpegParameters ( ) ;

                //jparam.
                //codec.
                //codec.Encode ( )
            }
        }
    }
}
