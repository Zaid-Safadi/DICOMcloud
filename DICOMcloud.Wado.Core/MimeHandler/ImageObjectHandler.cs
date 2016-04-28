using DICOMcloud.Wado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ClearCanvas.Dicom;
using DICOMcloud.Dicom.Media;
using DICOMcloud.Core.Storage;
using DICOMcloud.Pacs;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;

namespace DICOMcloud.Wado.Core
{
    public class ImageObjectHandler : ObjectHandlerBase
    {
        private List<string> _supportedMime = new List<string>();
        IObjectRetrieveDataService RetrieveService     { get; set;  }
      
        public ImageObjectHandler(IMediaStorageService mediaStorage ) : base ( mediaStorage )
        {
            _supportedMime.Add(MimeMediaTypes.DICOM);
            _supportedMime.Add(MimeMediaTypes.Jpeg);
            _supportedMime.Add(MimeMediaTypes.UncompressedData);
            _supportedMime.Add(MimeMediaTypes.WebP);
        }

        public override bool CanProcess(string mimeType)
        {
            return _supportedMime.Contains(mimeType, StringComparer.CurrentCultureIgnoreCase);
        }
      //TODO: I should be able to replace this with the media readers now
        protected override WadoResponse DoProcess(IWadoUriRequest request, string mimeType)
        {
            var dcmLocation = MediaStorage.GetLocation ( new DicomMediaId ( request, MimeMediaTypes.DICOM) ) ;
            //var dcmLocation = RetrieveService.RetrieveSopInstances ( request, mimeType ).FirstOrDefault();


            if ( !dcmLocation.Exists ( ) )
            {
                throw new ApplicationException ( "Object Not Found - return proper wado error ") ;
            }

            //if (string.Compare(mimeType, MimeMediaTypes.DICOM, true) == 0)
            {
                return new WadoResponse(Location, mimeType);
            }

            DicomFile file = new DicomFile ( ) ;
            var frameIndex = request.ImageRequestInfo.FrameNumber - 1 ?? 0  ;
                    
            frameIndex = Math.Max ( frameIndex, 0 ) ;

            file.Load ( dcmLocation.GetReadStream() ) ;

            if (string.Compare(mimeType, MimeMediaTypes.Jpeg, true) == 0)
            {
                WadoResponse response = new WadoResponse();


                if (file.TransferSyntax == TransferSyntax.JpegBaselineProcess1)
                {
                    //ClearCanvas.Dicom.Codec.Jpeg.Jpeg8Codec codec = new ClearCanvas.Dicom.Codec.Jpeg.Jpeg8Codec (ClearCanvas.Dicom.Codec.Jpeg.JpegMode.Baseline, 0, 0 )
                
                    //codec.Encode ()

                    DicomCompressedPixelData pd = DicomPixelData.CreateFrom(file) as DicomCompressedPixelData ;

                    byte[] buffer = pd.GetFrameFragmentData ( frameIndex );

                    response.Content = new MemoryStream(buffer);
                    response.MimeType = mimeType ;

                    return response ;
                }
                else
                {
                }
            }

            if ( string.Compare(mimeType, MimeMediaTypes.UncompressedData) == 0)
            {
                WadoResponse response = null ;
                DicomPixelData pd     = null ;
                byte[] buffer         = null ;


                response = new WadoResponse          ( ) ;
                pd       = DicomPixelData.CreateFrom ( file ) ;
                buffer   = pd.GetFrame                ( frameIndex) ;
            
            
                //********* TEST CODE***************
                    //System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap (pd.ImageWidth, pd.ImageHeight, System.Drawing.Imaging.PixelFormat.Format8bppIndexed ) ;

                    //System.Drawing.Imaging.ColorPalette ncp = bitmap.Palette;
                    //    for (int i = 0; i < 256; i++)
                    //        ncp.Entries[i] = System.Drawing.Color.FromArgb(255, i, i, i);
                    //    bitmap.Palette = ncp;
                    // System.Drawing.Imaging.BitmapData data =  bitmap.LockBits (new System.Drawing.Rectangle ( 0,0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

                    //IntPtr ptr = data.Scan0 ;

                    //System.Runtime.InteropServices.Marshal.Copy (buffer, 0, ptr, buffer.Length ) ;
                    //string fileName = @"C:\Users\zalsafadi_p.SPX\Downloads\libwebp-master\Output\release-static\x86\bin\Samples\uncompressed.raw" ;
                    //bitmap.UnlockBits (data);
                    //bitmap.Save ( fileName);

                    //File.WriteAllBytes(fileName, buffer) ;

                //********* TEST CODE***************

                response.Content  = new MemoryStream(buffer);
                response.MimeType = mimeType ;

                return response ;
            }

        //if ( string.Compare(mimeType, MimeMediaTypes.WebP) == 0)
        //{
        //    WadoResponse response = new WadoResponse ( ) ;
                
        //    byte[] buffer = File.ReadAllBytes(Location) ;

        //    response.Content  = new MemoryStream(buffer);
        //    response.MimeType = mimeType ;

        //    return response ;
        //}

         return null;
      }
   }
}
