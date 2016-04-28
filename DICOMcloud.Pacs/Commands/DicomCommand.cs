using ClearCanvas.Dicom;
using DICOMcloud.Core.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DICOMcloud.Dicom.DataAccess;
using DICOMcloud.Dicom.Media;
using System.Diagnostics;

namespace DICOMcloud.Pacs.Commands
{
    public abstract class DicomCommand<T,R> : IDicomCommand<T,R>
    {
        public IDicomInstnaceStorageDataAccess DataAccess   { get; set; }
        
        public DicomCommand ( ) : this ( null ) 
        {}

        public DicomCommand
        ( 
            IDicomInstnaceStorageDataAccess dataStorage
            //, 
//            IDicomMediaWriterFactory mediaFactory
        )
        {
            DataAccess   = dataStorage ;
            //MediaFactory = mediaFactory ; //?? new DicomMediaWriterFactory ( ) ;
        }

        public abstract R Execute(T commandData ) ;
    }
}
