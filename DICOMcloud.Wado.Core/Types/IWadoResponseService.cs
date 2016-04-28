using DICOMcloud.Wado.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using DICOMcloud.Dicom.Media;
using DICOMcloud.Core.Extensions;


namespace DICOMcloud.Wado.Core
{
    public interface IWadoResponseService 
    {
        HttpResponseMessage CreateWadoRsResponse  
        ( 
            IWadoRequestHeader request, 
            IEnumerable<IWadoRsResponse> instancesFilePath, 
            MimeMediaType mimeType 
        ) ; 
    }

    public class WadoResponseService : IWadoResponseService
    {
        public HttpResponseMessage CreateRetrieveInstancesResponse 
        ( 
            IWadoRequestHeader request, 
            IEnumerable<IWadoRsResponse>  wadoResponses, 
            MimeMediaType mimeType 
        )
        { 
            HttpResponseMessage response = null ;

            if ( !IsValidResponse ( wadoResponses, out response ))
            { 
                return response ;
            }

            //if ( request.IsMultipart ( ) )
            {
                return GetMultipartResponse(wadoResponses, mimeType );
            }
        }

        public HttpResponseMessage CreateWadoRsResponse
        ( 
            IWadoRequestHeader request, 
            IEnumerable<IWadoRsResponse> wadoResponses,
            MimeMediaType mimeType 
        )
        {
            HttpResponseMessage response = new HttpResponseMessage ( ) ;
            
            
            if ( wadoResponses == null || wadoResponses.FirstOrDefault ( ) == null )
            {
                response.StatusCode = System.Net.HttpStatusCode.NotAcceptable ;

                return response ;            
            }
            if ( !IsValidResponse ( wadoResponses, out response ))
            { 
                return response ;
            }


            if ( mimeType == MimeMediaTypes.Json )
            {
                return GetJsonResponse (wadoResponses ) ;
            }
            else
            {
                return GetMultipartResponse ( wadoResponses, mimeType ) ;
            }
        }

        private static bool IsValidResponse(IEnumerable<IWadoRsResponse> wadoResponses, out HttpResponseMessage response)
        {
            response = new HttpResponseMessage ( ) ;

            if ( null == wadoResponses ){ response.StatusCode = System.Net.HttpStatusCode.BadRequest ; return false ; }
            if ( wadoResponses.IsNullOrEmpty() ) { response.StatusCode =  System.Net.HttpStatusCode.NotFound ; return false ; }
            
            return true ;
        }

        private static HttpResponseMessage GetMultipartResponse(IEnumerable<IWadoRsResponse>  wadoResponses, string mimeType )
        {
            HttpResponseMessage response = new HttpResponseMessage ( ) ;
            MultipartContent multiContent = new MultipartContent("related", "DICOM DATA BOUNDARY");
            multiContent.Headers.ContentType.Parameters.Add(new System.Net.Http.Headers.NameValueHeaderValue("type", "\"" + mimeType + "\""));

            response.Content = multiContent;

            foreach (var wadoResponse in wadoResponses)
            {
                StreamContent sContent = new StreamContent(wadoResponse.Content);
                sContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(wadoResponse.MimeType);

                multiContent.Add(sContent);
            }

            return response;
        }

        private static HttpResponseMessage GetJsonResponse ( IEnumerable<IWadoRsResponse>  wadoResponses )
        {
            IWadoRsResponse       wadoResponse ;
            HttpResponseMessage response ;
            StreamContent       content  ;


            wadoResponse = wadoResponses.FirstOrDefault ( ) ;

            if ( wadoResponses.IsNullOrEmpty() )
            { 
                throw new ArgumentException ("invalid wado responses") ;
            }

            response = new HttpResponseMessage ( ) ;
            content  = new StreamContent(wadoResponse.Content);
            
            content.Headers.ContentType= new System.Net.Http.Headers.MediaTypeHeaderValue(wadoResponse.MimeType);
            
            content.Headers.ContentType.Parameters.Add ( new NameValueHeaderValue ( "transfer-syntax", "\"" + wadoResponse.TransferSyntax + "\""));

            response.Content = content ;

            return response;
        }
    }
}
