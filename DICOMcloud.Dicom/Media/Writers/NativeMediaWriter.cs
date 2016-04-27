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
    public class NativeMediaWriter : DicomMediaWriterBase
    {
        public NativeMediaWriter ( ) : base ( ) {}
         
        public NativeMediaWriter ( IMediaStorageService mediaStorage ) : base ( mediaStorage ) {}

        public override string MediaType 
        { 
            get 
            {
                return MimeMediaTypes.DICOM ;
            }
        }

        protected override bool StoreMultiFrames
        {
            get
            {
                return false ;
            }
        }

        protected override void Upload(DicomFile dicomObject, int frame, IStorageLocation location )
        {
            if (!string.IsNullOrWhiteSpace(dicomObject.Filename))
            {
                location.Upload(dicomObject.Filename);
            }
            else
            {
                using (Stream stream = new MemoryStream())
                {
                    dicomObject.Save(stream, DicomWriteOptions.Default);
                    stream.Position = 0;

                    location.Upload(stream);
                }
            }
        }
    }
}
