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
    //public abstract class BulkDataMediaReader : DicomMediaReaderBase<IEnumerable<Stream>>
    //{
    //    public BulkDataMediaReader() : base ( )
    //    { }

    //    public BulkDataMediaReader ( IMediaStorageService mediaStorage ) : base ( mediaStorage )
    //    { }

    //    public override string MediaType
    //    {
    //        get { return MimeMediaTypes.UncompressedData ; }
    //    }

    //    protected override IEnumerable<Stream> DoReadMedia ( DicomMediaId media, IEnumerable<IStorageLocation> locations )
    //    {
    //        foreach ( var location in locations )
    //        {
    //            yield return location.GetReadStream ( ) ;
    //        }
    //    }

    //    protected override bool ReadMultiMedia 
    //    { 
    //        get
    //        {
    //            return true ;
    //        }
    //    }
    //}
}
