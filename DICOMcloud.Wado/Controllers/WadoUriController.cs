using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using DICOMcloud.Wado.Core;
using DICOMcloud.Wado.Models;

namespace DICOMcloud.Wado.Controllers
{
    //[Authorize]
    public class WadoUriController : ApiController
    {
        WadoUriService ServiceHandler { get; set; }

        public WadoUriController ( WadoUriService serviceHandler )
        {
            ServiceHandler = serviceHandler ;
        }

        public HttpResponseMessage Get
        (
            [ModelBinder(typeof(UriRequestModelBinder))]
            IWadoUriRequest request
        )
        {
            if (null == request) { return new HttpResponseMessage(HttpStatusCode.BadRequest ); }

            return ServiceHandler.GetInstance(request);
      }
   }
}
