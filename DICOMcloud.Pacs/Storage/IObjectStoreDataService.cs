using System.IO;

namespace DICOMcloud.Pacs
{
    public interface IObjectStoreService
    {
        StoreResult StoreDicom ( Stream dicomStream ) ;
    }
}