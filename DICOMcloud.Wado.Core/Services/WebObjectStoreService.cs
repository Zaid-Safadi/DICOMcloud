using ClearCanvas.Dicom;
using DICOMcloud.Wado.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using DICOMcloud.Pacs;
using DICOMcloud.Dicom.Media;
using DICOMcloud.Dicom.Common;

namespace DICOMcloud.Wado.Core
{
    public class WebObjectStoreService
    {
        private IObjectStoreService _storageService;

        //public WebObjectStoreService ( ) : this ( new ObjectStoreDataService ( ) ) {}
        public WebObjectStoreService ( IObjectStoreService storage ) 
        {
            _storageService = storage ;
        }

        public async Task<HttpResponseMessage> ProcessRequest 
        (
            IWebStoreRequest request, 
            string studyInstanceUID 
        )
        {
            DicomAttributeCollection bodyContent = null ;
            
            
            switch ( request.MediaType )
            {
                //TODO: build the response here, { Successes.Add(objectMetadata), Failures.Add(objectMetadata), Create
                case MimeMediaTypes.DICOM:
                {
                    bodyContent = await GetResponseDataset (request, studyInstanceUID );
                }
                break ;

                case MimeMediaTypes.xmlDicom:
                {

                }
                break ;

                case MimeMediaTypes.Json:
                {

                }
                break ;

                default:
                {
                    throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                }
            }

            if ( null != bodyContent )
            {
                var result = new HttpResponseMessage (HttpStatusCode.OK) ;

                
                if ( new MimeMediaType ( MimeMediaTypes.Json ).IsIn ( request.AcceptHeader ) ) //this is not taking the "q" parameter
                {
                    JsonDicomConverter converter = new JsonDicomConverter ( ) ;
                    
                    
                    result.Content = new StringContent (  converter.Convert ( bodyContent ), System.Text.Encoding.UTF8, MimeMediaTypes.Json ) ;
                }
                else
                {
                    XmlDicomConverter xmlConverter = new XmlDicomConverter ( ) ;
                    
                    result.Content = new StringContent (  xmlConverter.Convert ( bodyContent ), System.Text.Encoding.UTF8, MimeMediaTypes.xmlDicom ) ;
                }

                return result ;    
            }
            else
            {
                return new HttpResponseMessage ( HttpStatusCode.BadRequest ) ;
            }
        }

        private async Task<DicomAttributeCollection> GetResponseDataset ( IWebStoreRequest request, string studyInstanceUID )
        {
            DicomAttributeCollection bodyContent = null ;
            WadoStoreResponse response = new WadoStoreResponse(studyInstanceUID);

            foreach (var mediaContent in request.Contents)
            {
                Stream dicomStream = await mediaContent.ReadAsStreamAsync();

                try
                {
                    var result = _storageService.StoreDicom(dicomStream);//TODO:make this ASync??

                    response.AddResult(result);
                }
                catch (Exception ex)
                {
                    response.AddResult(ex, dicomStream);
                }

            }

            bodyContent = response.GetResponseContent();
            return bodyContent;
        }
    }
}
