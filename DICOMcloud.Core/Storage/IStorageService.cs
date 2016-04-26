using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Core.Storage
{
    public interface IMediaStorageService
    {
        //IStorageContainer CreateContainer ( string containerKey ) ;

        //IStorageContainer GetStorageContainer ( string containerKey ) ;
        
        //TODO: 
        //1. create async methods
        //2. methods to search/enumerate keys 
        //4. methods to search/enumerate streams

        void   Write                     ( Stream stream, IMediaId key ) ;
        Stream Read                      ( IMediaId key ) ;
        IStorageLocation GetLocation     ( IMediaId key ) ;
        IEnumerable<IStorageLocation> EnumerateLocation ( IMediaId key ) ;
    }
}
