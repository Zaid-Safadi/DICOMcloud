using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Core.Storage
{
    public class FileStorageService : MediaStorageService 
    {
        //List<IStorageContainer> _containers = new List<IStorageContainer> ( ) ;
        
        //public override IStorageLocation GetTempLocation ( ) 
        //{
        //    return new LocalStorageLocation ( Path.GetTempFileName () ) ;
        //}

        public FileStorageService ( ) 
        {
            BaseStorePath = ".//" ;
        }

        public FileStorageService ( string storePath ) 
        {
            BaseStorePath = storePath ;
        }

        protected override IStorageContainer GetContainer ( string containerKey ) 
        {
            LocalStorageContainer storage = new LocalStorageContainer ( GetStoragePath (containerKey ) ) ;

            if ( !Directory.Exists ( storage.FolderPath ))
            {
                Directory.CreateDirectory ( storage.FolderPath )  ;
            }

            return storage ;
        }

        protected override IKeyProvider CreateKeyProvider ( ) 
        {
            return new FileKeyProvider ( ) ;
        }

        protected override IEnumerable<IStorageContainer> GetContainers ( string containerKey ) 
        {
            foreach ( string folder in Directory.EnumerateDirectories ( BaseStorePath, containerKey, SearchOption.TopDirectoryOnly ))
            {
                yield return GetContainer ( folder ) ;
            }
        }

        protected virtual string GetStoragePath ( string folderName )
        {
            return Path.Combine (BaseStorePath, folderName ) ; 
        }

        public string BaseStorePath { get; set; }
    }
}
