
using DICOMcloud.Wado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using DICOMcloud.Pacs;

namespace DICOMcloud.Wado.Core
{
    //TODO: this service should be unified with the RetrieveService
    public class WadoUriService : IWadoUriService
    {
        public IObjectRetrieveDataService RetrieveService { get; private set ; }
        
        //public WadoUriService ( WadoResponseProcessorFactory mediaResponseHandlerFactory )
        public WadoUriService ( IObjectRetrieveDataService retrieveService )
        {
            RetrieveService = retrieveService ;
        }

        public HttpResponseMessage GetInstance ( IWadoUriRequest request )
        {
            //validation code should go in here
            if (null == request || string.Compare(request.RequestType, "WADO", true ) != 0 )
            {
                throw new Exception("Request Type must be set to WADO");//TODO
            }

            List<MediaTypeHeaderValue> mimeType = GetRequestedMimeType ( request );


            foreach (MediaTypeHeaderValue mediaType in mimeType)
            {
                var dcmLocation = RetrieveService.RetrieveSopInstance ( request, mediaType.MediaType ) ;


                if ( null != dcmLocation && dcmLocation.Exists ( ) )
                {
                    //TODO: use this for on the fly generation
                    //IWadoRsResponse response = imageHandler.Process ( request, mediaType.MediaType ) ;

                
                    StreamContent sc        = new StreamContent        (  dcmLocation.GetReadStream ( ) ) ;
                    sc.Headers.ContentType  = new MediaTypeHeaderValue ( mediaType.MediaType ) ;
                    HttpResponseMessage msg = new HttpResponseMessage  ( HttpStatusCode.OK ) ;

                    msg.Content = sc;

                    return msg;
                }
                
                return new HttpResponseMessage ( HttpStatusCode.NotFound ) ;
            }

            return null;
        }

        protected virtual List<MediaTypeHeaderValue> GetRequestedMimeType(IWadoUriRequest request)
        {
            List<MediaTypeHeaderValue> acceptTypes = new List<MediaTypeHeaderValue>();
            bool acceptAll = request.AcceptHeader.Contains(AllMimeType, new MediaTypeHeaderComparer ( ) ); 

            if (!string.IsNullOrEmpty(request.ContentType))
            {
            string[] mimeTypes = request.ContentType.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string mime in mimeTypes)
            {
                MediaTypeWithQualityHeaderValue mediaType;

                if (MediaTypeWithQualityHeaderValue.TryParse(mime, out mediaType))
                {
                    if (acceptAll || request.AcceptHeader.Contains(mediaType, new MediaTypeHeaderComparer()))
                    {
                        acceptTypes.Add(mediaType);
                    }
                }
                else
                { 
                    //TODO: throw excpetion?
                }
            }
            }

            return acceptTypes;
        }

        private readonly MediaTypeHeaderValue AllMimeType = MediaTypeHeaderValue.Parse ("*/*");

        //public WadoResponseProcessorFactory MediaResponseHandlerFactory { get; private set; }
        
    }

    public class MediaTypeHeaderComparer : IEqualityComparer<MediaTypeHeaderValue>
    {
        public bool Equals(MediaTypeHeaderValue x, MediaTypeHeaderValue y)
        {
            return string.Compare(x.MediaType, y.MediaType, true) == 0;
        }

        public int GetHashCode(MediaTypeHeaderValue obj)
        {
            return obj.MediaType.GetHashCode();
        }
    }
}
