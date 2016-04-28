using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DICOMcloud.Core.Storage;

namespace DICOMcloud.Wado.Core
{
    public class WadoResponseProcessorFactory
    {
        public IMediaStorageService MediaStorage { get; private set; }
    
        public WadoResponseProcessorFactory ( IMediaStorageService stroageService )
        {
            MediaStorage = stroageService ;
        }
        
        public IMimeResponseHandler GetHandler(List<MediaTypeHeaderValue> mimeType)
        {
            return new ImageObjectHandler ( MediaStorage ) ;
        }
    }
}
