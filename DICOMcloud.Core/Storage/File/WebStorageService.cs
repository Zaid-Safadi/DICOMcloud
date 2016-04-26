using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Core.Storage
{
    public class WebStorageService : FileStorageService 
    {
        public WebStorageService ( ) 
        {
            BaseStorePath = System.Web.Hosting.HostingEnvironment.MapPath ( "~/") ;
        }
    }
}
