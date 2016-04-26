using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearCanvas.Dicom;
using DICOMcloud.Core.Storage;

namespace DICOMcloud.Dicom.Media
{
    //TODO: what is the purpose of this class again? just return streams and locations or formatted jsonSSSSS and XMLssss
    //public abstract class DicomMediaReaderBase<T> : IDicomMediaReader
    //{
    //    public IMediaStorageService MediaStorage { get; set; }

    //    public DicomMediaReaderBase() : this(new FileStorageService())
    //    { }

    //    public DicomMediaReaderBase ( IMediaStorageService mediaStorage )
    //    {
    //        MediaStorage = mediaStorage ;
    //    }

    //    public abstract string MediaType
    //    {
    //        get ;
    //    }

    //    public IEnumerable<IStorageLocation> ReadMedia ( DicomMediaId media )
    //    {
    //        IEnumerable<IStorageLocation> locations ;
            

    //        if ( ReadMultiMedia )
    //        {
    //            yield return MediaStorage.EnumerateLocation ( media ) ;
    //        }
    //        else
    //        {
    //            var temp = new List<IStorageLocation> ( ) ;


    //            temp.Add ( MediaStorage.GetLocation ( media ) ) ;
            
    //            locations = temp ;    
    //        }

    //        foreach ( var location in locations )
    //        {
    //            yield return location.GetReadStream ( ) ;
    //        }


    //        return DoReadMedia ( media, locations ) ;
    //    }
    //    //    if (string.Compare(mimeType, MimeMediaTypes.DICOM, true) == 0)
    //    //    {
    //    //        return new WadoResponse(Location, mimeType);
    //    //    }

    //    //    DicomFile file = new DicomFile ( ) ;

    //    //    file.Load ( dcmLocation.GetReadStream() ) ;

    //    //    if (string.Compare(mimeType, MimeMediaTypes.Jpeg, true) == 0)
    //    //    {
    //    //        WadoResponse response = new WadoResponse();


    //    //        if (file.TransferSyntax == TransferSyntax.JpegBaselineProcess1)
    //    //        {
    //    //            //ClearCanvas.Dicom.Codec.Jpeg.Jpeg8Codec codec = new ClearCanvas.Dicom.Codec.Jpeg.Jpeg8Codec (ClearCanvas.Dicom.Codec.Jpeg.JpegMode.Baseline, 0, 0 )
                
    //    //            //codec.Encode ()

    //    //            DicomCompressedPixelData pd = DicomPixelData.CreateFrom(file) as DicomCompressedPixelData ;

    //    //            byte[] buffer = pd.GetFrameFragmentData(request.ImageRequestInfo.FrameNumber ?? 0);

    //    //            response.Content = new MemoryStream(buffer);
    //    //            response.MimeType = mimeType ;

    //    //            return response ;
    //    //        }
    //    //        else
    //    //        {
    //    //        }
    //    //    }

    //    //    if ( string.Compare(mimeType, MimeMediaTypes.UncompressedData) == 0)
    //    //    {
    //    //        WadoResponse response = null ;
    //    //        DicomPixelData pd     = null ;
    //    //        byte[] buffer         = null ;


    //    //        response = new WadoResponse          ( ) ;
    //    //        pd       = DicomPixelData.CreateFrom ( file ) ;
    //    //        buffer   = pd.GetFrame                ( request.ImageRequestInfo.FrameNumber ?? 0) ;
            
            
    //    //        //********* TEST CODE***************
    //    //            //System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap (pd.ImageWidth, pd.ImageHeight, System.Drawing.Imaging.PixelFormat.Format8bppIndexed ) ;

    //    //            //System.Drawing.Imaging.ColorPalette ncp = bitmap.Palette;
    //    //            //    for (int i = 0; i < 256; i++)
    //    //            //        ncp.Entries[i] = System.Drawing.Color.FromArgb(255, i, i, i);
    //    //            //    bitmap.Palette = ncp;
    //    //            // System.Drawing.Imaging.BitmapData data =  bitmap.LockBits (new System.Drawing.Rectangle ( 0,0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

    //    //            //IntPtr ptr = data.Scan0 ;

    //    //            //System.Runtime.InteropServices.Marshal.Copy (buffer, 0, ptr, buffer.Length ) ;
    //    //            //string fileName = @"C:\Users\zalsafadi_p.SPX\Downloads\libwebp-master\Output\release-static\x86\bin\Samples\uncompressed.raw" ;
    //    //            //bitmap.UnlockBits (data);
    //    //            //bitmap.Save ( fileName);

    //    //            //File.WriteAllBytes(fileName, buffer) ;

    //    //        //********* TEST CODE***************

    //    //        response.Content  = new MemoryStream(buffer);
    //    //        response.MimeType = mimeType ;

    //    //        return response ;
    //    //    }

    //    ////if ( string.Compare(mimeType, MimeMediaTypes.WebP) == 0)
    //    ////{
    //    ////    WadoResponse response = new WadoResponse ( ) ;
                
    //    ////    byte[] buffer = File.ReadAllBytes(Location) ;

    //    ////    response.Content  = new MemoryStream(buffer);
    //    ////    response.MimeType = mimeType ;

    //    ////    return response ;
    //    ////}

    //    // return null;
      


    //    //    if (null != MediaStorage)
    //    //    {
    //    //        string                 key          = null;
    //    //        int                    framesCount  = 1;
    //    //        List<IStorageLocation> locations    = new List<IStorageLocation> ( ) ;

    //    //        if ( StoreMultiFrames )
    //    //        {
    //    //            DicomPixelData pd ;


    //    //            pd          = DicomPixelData.CreateFrom ( data ) ;
    //    //            framesCount = pd.NumberOfFrames ;
    //    //        }
                
    //    //        for ( int frame = 0; frame < framesCount; frame++ )
    //    //        {
    //    //            key = MediaStorage.KeyProvider.GetStorageKey ( new DicomMediaId ( data.DataSet, frame, MediaType) );

    //    //            var storeLocation = MediaStorage.GetLocation(key);

    //    //            Upload ( data, frame, storeLocation ) ;
                
    //    //            locations.Add ( storeLocation ) ;
    //    //        }

    //    //        return locations ;
    //    //    }

    //    //    throw new InvalidOperationException ( "No MediaStorage service found") ;
    //    //}

    //    protected abstract T DoReadMedia (DicomMediaId media, IEnumerable<IStorageLocation> locations );
    
    //    protected abstract bool ReadMultiMedia { get; }
    //}
}
