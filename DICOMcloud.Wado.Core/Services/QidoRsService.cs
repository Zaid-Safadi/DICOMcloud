using ClearCanvas.Dicom;
using DICOMcloud.Dicom.Common;
using DICOMcloud.Pacs;
using DICOMcloud.Wado.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DICOMcloud.Dicom.Media;


namespace DICOMcloud.Wado.Core.Services
{
    public class QidoRsService
    {
        protected IObjectArchieveQueryService QueryService { get; set; }

        public QidoRsService ( IObjectArchieveQueryService queryService )
        {
            QueryService = queryService ;
        }

        public HttpResponseMessage SearchForStudies
        (
            IQidoRequestModel request
        )
        {
            return SearchForDicomEntity ( request, 
            DefaultDicomQueryElements.GetDefaultStudyQuery(),
            delegate 
            ( 
                IObjectArchieveQueryService queryService, 
                DicomAttributeCollection dicomRequest, 
                int? limit, 
                int? offset  
            )
            {
                return queryService.FindStudies ( dicomRequest, offset, limit ) ;
            }  ) ;
        }

        public HttpResponseMessage SearchForSeries(IQidoRequestModel request)
        {
            return SearchForDicomEntity ( request, 
            DefaultDicomQueryElements.GetDefaultSeriesQuery ( ),
            delegate 
            ( 
                IObjectArchieveQueryService queryService, 
                DicomAttributeCollection dicomRequest, 
                int? limit, 
                int? offset  
            )
            {
                return queryService.FindSeries ( dicomRequest, offset, limit ) ;
            }  ) ;
        }

        public HttpResponseMessage SearchForInstances(IQidoRequestModel request)
        {
            return SearchForDicomEntity ( request,
            DefaultDicomQueryElements.GetDefaultInstanceQuery ( ),
            delegate 
            ( 
                IObjectArchieveQueryService queryService, 
                DicomAttributeCollection dicomRequest, 
                int? limit, 
                int? offset  
            )
            {
                return queryService.FindObjectInstances ( dicomRequest, offset, limit ) ;
            }  ) ;
        }

        private HttpResponseMessage SearchForDicomEntity 
        ( 
            IQidoRequestModel request, 
            DicomAttributeCollection dicomSource,
            DoQueryDelegate doQuery 
        )
        {
            if ( null != request.Query )
            {
                var matchingParams = request.Query.MatchingElements ;
                var includeParams = request.Query.IncludeElements ;

                foreach ( var queryParam in  matchingParams )
                {
                    string paramValue = queryParam.Value;

                    InsertDicomElement(dicomSource, queryParam.Key, paramValue);
                }

                foreach ( var returnParam in includeParams )
                {
                    InsertDicomElement ( dicomSource,  returnParam, "" );
                }

                ICollection<DicomAttributeCollection> results = doQuery (QueryService, dicomSource, request.Limit, request.Offset ) ; //TODO: move configuration params into their own object

                StringBuilder jsonReturn = new StringBuilder ( "[" ) ;

                JsonDicomConverter converter = new JsonDicomConverter ( ) { IncludeEmptyElements = true } ;

                foreach ( var response in results )
                {
                    
                    jsonReturn.AppendLine (converter.Convert ( response )) ;

                    jsonReturn.Append(",") ;
                }

                if ( results.Count > 0 )
                {
                    jsonReturn.Remove ( jsonReturn.Length -1, 1 ) ;
                }

                jsonReturn.Append("]") ;

                return new HttpResponseMessage (System.Net.HttpStatusCode.OK ) { Content = new StringContent ( jsonReturn.ToString ( ), Encoding.UTF8, MimeMediaTypes.Json) } ;
            }

            return null;
        }

        private void InsertDicomElement(DicomAttributeCollection dicomRequest, string paramKey, string paramValue)
        {
            List<string> elements = new List<string>();

            elements.AddRange(paramKey.Split('.'));

            if(elements.Count > 1)
            {
                CreateSequence(elements, 0, dicomRequest, paramValue);
            }
            else
            {
                CreateElement(elements[0], dicomRequest, paramValue);
            }
        }

        private void CreateElement(string tagString, DicomAttributeCollection dicomRequest, string value)
        {
            uint tag = uint.Parse (tagString, System.Globalization.NumberStyles.HexNumber) ;

            dicomRequest[tag].SetStringValue (value ) ;
        }

        private void CreateSequence(List<string> elements, int currentElementIndex, DicomAttributeCollection dicomRequest, string value)
        {
            uint tag = uint.Parse ( elements[currentElementIndex], System.Globalization.NumberStyles.HexNumber) ;//TODO: need to handle the case of keywords

            DicomAttributeSQ sequence = dicomRequest[tag] as DicomAttributeSQ ;
            DicomSequenceItem item ;


            if ( sequence.Count > 0 )
            {
                item = sequence[0] ;
            }
            else
            {
                item = new DicomSequenceItem ( ) ;

                sequence.AddSequenceItem ( item ) ;
            }
            
            for ( int index = (currentElementIndex+1); index < elements.Count; index++  )
            {
                tag = uint.Parse ( elements[index], System.Globalization.NumberStyles.HexNumber) ;
                
                DicomAttribute childElement = item[tag] ; //TODO: if item.Contains(tag)??? throw error?

                if (  childElement.Tag.VR.Equals (DicomVr.SQvr) )
                {
                    CreateSequence ( elements, index, item, value) ;

                    break ;
                }
                else
                {
                    childElement.SetStringValue ( value ) ;
                }
            }
        }
    
        private delegate ICollection<DicomAttributeCollection> DoQueryDelegate 
        ( 
            IObjectArchieveQueryService queryService, 
            DicomAttributeCollection dicomRequest, 
            int? limit, 
            int? offset  
        ) ;
    }
}
