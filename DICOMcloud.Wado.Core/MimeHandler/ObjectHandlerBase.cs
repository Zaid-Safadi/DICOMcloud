using DICOMcloud.Wado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using DICOMcloud.Dicom.Media;
using DICOMcloud.Core.Storage;

namespace DICOMcloud.Wado.Core
{
    public abstract class ObjectHandlerBase : IMimeResponseHandler
    {
        public virtual IMediaStorageService MediaStorage { get; protected set ; }
   
        public abstract bool CanProcess(string mimeType);
      
        public ObjectHandlerBase ( IMediaStorageService mediaStorage )
        {
            MediaStorage = mediaStorage ;
        }

        public IWadoRsResponse Process(IWadoUriRequest request, string mimeType)
        {
            //base class that has the logic to get the DICOM file

            Location = MediaStorage.GetLocation ( new DicomMediaId (request, mimeType) ) ;
         
            if ( Location.Exists ( ) )
            {
                WadoResponse response = new WadoResponse ( Location, mimeType ) ;
                
                return response ;
            }
            else
            {
                //TODO: in case mime not storedmethod to create on 
                return DoProcess(request, mimeType);
            }
        }

      protected abstract WadoResponse DoProcess(IWadoUriRequest request, string mimeType);

       protected IStorageLocation Location { get; set; }

   }
}
