using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Core.Storage
{
    public class LocalStorageContainer : IStorageContainer
    {
        private List<IStorageLocation> _tempLocations = new List<IStorageLocation> ( ) ;
        
        public LocalStorageContainer () 
        {
            FolderPath = GetDefaultStoragePath ( ) ;
        }

        protected virtual string GetDefaultStoragePath()
        {
            return Environment.CurrentDirectory ;
        }

        public string FolderPath
        {
            get;
            private set;
        }
        

        public LocalStorageContainer ( string folderPath )
        {
            FolderPath = folderPath ;
        }

        public string Connection
        {
            get
            {
                return FolderPath ;
            }
        }
        

        //public IStorageLocation GetTempLocation ( )
        //{
        //    IStorageLocation location = new LocalStorageLocation ( Path.GetTempFileName ( ) ) ;

        //    return location ;
        //}

        public IStorageLocation GetLocation ( string name = null, IMediaId id = null )
        {
            if ( string.IsNullOrWhiteSpace(name))
            {
                name = Guid.NewGuid ( ).ToString ( ) ;
            }

            return new LocalStorageLocation ( Path.Combine ( FolderPath, name), id ) ;
        }

        public void DeleteLocation ( IStorageLocation location )
        {
            location.Delete ( ) ;
        }

        public IEnumerable<IStorageLocation> GetLocations ( string name )
        {
            //check if name is really a file 
            string path = Path.Combine ( FolderPath, name ) ;

            if ( !Directory.Exists ( FolderPath ))
            {
                yield return null ;
            }

            if ( File.Exists (path))
            {
                yield return GetLocation ( path ) ;
            }

            if ( Directory.Exists ( path ))
            {
                foreach ( string file in Directory.EnumerateFiles ( path , "*", SearchOption.AllDirectories ) )
                {
                    yield return GetLocation ( file ) ;
                }
            }
        }
    }
}
