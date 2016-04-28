using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DICOMcloud.Core.Storage;
using System.Collections;
using System.IO;
using Microsoft.Azure;

namespace DICOMcloud.Core.Azure.Storage
{
    public class AzureContainer : IStorageContainer
    {
        private CloudBlobContainer __Container { get; set; }
        
        public AzureContainer ( CloudBlobContainer container )
        {
            __Container = container ;
        }

        public string Connection
        {
            get
            {
                return __Container.Uri.ToString();
            }
        }

        public void DeleteLocation(IStorageLocation location)
        {
            throw new NotImplementedException();
        }

        public IStorageLocation GetLocation(string key = null, IMediaId id = null )
        {
            var blob = __Container.GetBlockBlobReference ( (key == null ) ? Guid.NewGuid().ToString() : key ) ;
            
            return new AzureLocation ( blob, id ) ;
        }

        public IEnumerable<IStorageLocation> GetLocations (string key )
        {
            foreach (var blob in __Container.ListBlobs(key, true, BlobListingDetails.None).OfType<CloudBlockBlob>())
            {
                yield return new AzureLocation(blob);
            }
        }
    }
}
