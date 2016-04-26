using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearCanvas.Dicom;
using DICOMcloud.Dicom.Common;
using DICOMcloud.Core.Storage;

namespace DICOMcloud.Dicom.Media
{
    public class DicomMediaWriter : DicomMediaWriterBase
    {
        private string _mediaType ;

        public IDicomConverter<string> Converter { get; set; }
        
        public DicomMediaWriter (  IDicomConverter<string> converter, string mediaType ) : this ( new FileStorageService ( ), converter, mediaType )
        {}

        public DicomMediaWriter ( IMediaStorageService mediaStorage, IDicomConverter<string> converter, string mediaType ) : base ( mediaStorage )
        {
            Converter  = converter ;
            _mediaType = mediaType ;
        }

        public override string MediaType
        {
            get 
            {
                return _mediaType ;
            }
        }

        protected override bool StoreMultiFrames
        {
            get
            {
                return false ;
            }
        }


        protected override void Upload ( DicomFile data, int frame, IStorageLocation location )
        {
            location.Upload ( System.Text.Encoding.UTF8.GetBytes (Converter.Convert(data.DataSet)) ) ;
        }
    }
}
