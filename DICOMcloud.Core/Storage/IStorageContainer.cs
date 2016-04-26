using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Core.Storage
{
    public interface IStorageContainer
    { 
        string Connection
        {
            get;
        }

        IStorageLocation GetLocation     ( string name = null, IMediaId id = null ) ;

        void   DeleteLocation ( IStorageLocation location );
        IEnumerable<IStorageLocation> GetLocations(string v);
    }
}
