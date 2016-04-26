using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Core.Storage
{
    public abstract class MediaStorageService : IMediaStorageService 
    {
        public virtual Stream Read ( IMediaId key ) 
        {
            var location = GetLocation ( key ) ;
        
            return location.Download ( ) ;    
        }
        
        public virtual void Write ( Stream stream, IMediaId id )
        {
            var location = GetLocation ( id ) ;
        
            location.Upload ( stream ) ;
        }

        //public virtual Stream GetWriteStream ( string key)
        //{
        //    IStorageLocation location = GetLocation ( key ) ;

        //    return location.GetWriteStream();
        //}

        
        public virtual IStorageLocation GetLocation ( IMediaId id )
        {
            string            key       = KeyProvider.GetStorageKey ( id ) ;
            IStorageContainer container = GetContainer              ( KeyProvider.GetContainerName ( key ) ) ;
            var               location  = container.GetLocation     ( KeyProvider.GetLocationName  ( key ), id ) ; 
            

            return location ;
        }

        public IEnumerable<IStorageLocation> EnumerateLocation ( IMediaId id )
        {
            string  key = KeyProvider.GetStorageKey ( id ) ;
            string containerName = KeyProvider.GetContainerName ( key) ;
            
            
            foreach ( IStorageContainer container in GetContainers ( containerName ) )
            {
                foreach ( IStorageLocation location in container.GetLocations ( KeyProvider.GetLocationName (key) ) )
                {
                    yield return location ;
                }
            }

        }

        //public abstract IStorageLocation GetTempLocation ( ) ;

        protected virtual IKeyProvider KeyProvider 
        { 
            get
            {
                return GetKeyProvider ( ) ;
            }
        }

        protected abstract IKeyProvider                   CreateKeyProvider ( ) ;
        protected abstract IStorageContainer              GetContainer      ( string containerKey ) ;
        protected abstract IEnumerable<IStorageContainer> GetContainers     ( string containerKey ) ;
        
        private IKeyProvider GetKeyProvider ( ) 
        {
            if ( null != _keyProvider )
            {
                return _keyProvider ;
            }

            lock ( _keyLock )
            {
                if (null == _keyProvider )
                {
                    _keyProvider = CreateKeyProvider ( ) ;
                }
            }

            return _keyProvider ;
        }
    
        private IKeyProvider _keyProvider = null ;
        private object       _keyLock     = new object ( ) ;
    }
}
