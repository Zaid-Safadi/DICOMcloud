using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearCanvas.Dicom;
using DICOMcloud.Dicom.Data;
using DICOMcloud.Dicom.DataAccess;
using DICOMcloud.Dicom.Media;
using DICOMcloud.Core.Extensions;
using DICOMcloud.Pacs;
using DICOMcloud.Core.Storage;

namespace DICOMcloud.Pacs.Commands
{
    public class DeleteCommand : IDicomCommand<IObjectID, DicomCommandResult>
    {
        public IMediaStorageService            StorageService { get; set; }
        public IDicomInstnaceStorageDataAccess DataAccess { get; set; }
        
        public DeleteCommand
        ( 
            IMediaStorageService storageService,    
            IDicomInstnaceStorageDataAccess dataAccess
        )
        {
            StorageService = storageService ;
            DataAccess     = dataAccess ;
        }
        
        public DicomCommandResult Execute ( IObjectID instance ) 
        {
            string objectMetaRaw = DataAccess.GetInstanceMetadata ( instance ) ;
            dynamic objectMeta   = objectMetaRaw.FromJson ( ) ; 
            string media         = objectMeta.Media ;


            foreach ( string mediaType in media.Split ( ';' ) )
            { 
                IStorageLocation location ;
                DicomMediaId mediaId = new DicomMediaId ( instance, mediaType ) ;

                
                location = StorageService.GetLocation ( mediaId ) ;
                location.Delete ( ) ;
            }

            DataAccess.DeleteInstance ( instance.SopInstanceUID ) ;
        
            return new DicomCommandResult ( ) ;//TODO: currently nothing to return    
        }
         
    }
}
