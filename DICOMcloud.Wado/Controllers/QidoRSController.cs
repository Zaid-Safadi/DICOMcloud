using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using DICOMcloud.Wado.Core;
using DICOMcloud.Wado.Core.Services;
using DICOMcloud.Wado.Models;

namespace DICOMcloud.Wado.Controllers
{
    //[Authorize]
    public class QidoRSController : ApiController
    {
        protected QidoRsService QidoService {get; set;}

        public QidoRSController ( QidoRsService qidoService )
        {
            QidoService = qidoService ;
        }

        [Route("qidors/studies")]
        [HttpGet]
        public HttpResponseMessage SearchForStudies
        (
            [ModelBinder(typeof(QidoRequestModelBinder))] 
            IQidoRequestModel request
        )
        {
            return QidoService.SearchForStudies ( request ) ;
        }

        [Route("qidors/studies/{StudyInstanceUID}/series")]
        [Route("qidors/series")]
        [HttpGet]
        public HttpResponseMessage SearchForSeries 
        ( 
            [ModelBinder(typeof(QidoRequestModelBinder))] 
            IQidoRequestModel request 
        ) 
        {
            return QidoService.SearchForSeries ( request ) ;
        }


        [Route("qidors/studies/{StudyInstanceUID}/series/{SeriesInstanceUID}/instances")]
        [Route("qidors/studies/{StudyInstanceUID}/instances")]
        [Route("qidors/instances")]
        [HttpGet]
        public HttpResponseMessage SearchForInstances 
        (
            [ModelBinder(typeof(QidoRequestModelBinder))] 
            IQidoRequestModel request  
        ) 
        {
            return QidoService.SearchForInstances ( request ) ;
        }
    }
}