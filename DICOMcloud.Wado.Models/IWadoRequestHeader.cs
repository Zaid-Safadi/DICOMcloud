using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Wado.Models
{
   public interface IWadoRequestHeader
   {
      HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> AcceptHeader { get; set; }
      HttpHeaderValueCollection<StringWithQualityHeaderValue> AcceptCharsetHeader { get; set; }
   
   }
}
