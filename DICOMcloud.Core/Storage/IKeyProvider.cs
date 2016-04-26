namespace DICOMcloud.Core.Storage
{
    public interface IKeyProvider
    {
        string GetStorageKey (IMediaId key ) ;
    
        string GetLogicalSeparator ( ) ;

        string GetContainerName ( string key ) ;

        string GetLocationName ( string key ) ;    
    }
}
