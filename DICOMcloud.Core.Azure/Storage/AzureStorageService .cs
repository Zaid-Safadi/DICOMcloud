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
    public class AzureStorageService : MediaStorageService//, IEnumerable<IStorageContainer>
    {
        public AzureStorageService ( string connectionName )
        : this ( CloudStorageAccount.Parse( CloudConfigurationManager.GetSetting(connectionName) ) )
        {}

        public AzureStorageService ( CloudStorageAccount storageAccount )
        {
            // Create a blob client for interacting with the blob service.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            Init(blobClient);
        }

        //        public IEnumerator<IStorageContainer> GetEnumerator()
        //        {
        //            foreach ( var container in __CloudClient.ListContainers() )
        //            {
        //                yield return new AzureContainer ( container ) ; 
        //            }
        //        }

        //        public override IStorageLocation GetTempLocation ( ) 
        //        {
        //            var container = CreateContainer (TempContainerName) ;

        //            return container.GetTempLocation ( ) ;    
        //        }

        //        public override string GetLogicalSeparator ( ) 
        //        {
        //            return "/" ;
        //        }

        protected override IStorageContainer GetContainer(string containerKey)
        {
            CloudBlobContainer cloudContainer = __CloudClient.GetContainerReference(containerKey);

            cloudContainer.CreateIfNotExists();

            return new AzureContainer(cloudContainer);
        }
        
        protected override IEnumerable<IStorageContainer> GetContainers ( string containerKey ) 
        {
            foreach ( var container in __CloudClient.ListContainers ( containerKey, ContainerListingDetails.None ) )
            {
                yield return GetContainer ( containerKey ) ;
            }
        }

        private void Init(CloudBlobClient blobClient)
        {
            __CloudClient = blobClient;
        }

        //        IEnumerator IEnumerable.GetEnumerator()
        //        {
        //            foreach ( var container in __CloudClient.ListContainers() )
        //            {
        //                yield return new AzureContainer ( container ) ; 
        //            }
        //        }

        private CloudBlobClient __CloudClient { get; set; }
        
        protected override IKeyProvider CreateKeyProvider()
        {
            return new AzureKeyProvider ( ) ;
        }
    }
}
