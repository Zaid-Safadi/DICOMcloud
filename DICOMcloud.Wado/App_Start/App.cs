using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using DICOMcloud.Core.Azure.Messaging;
using DICOMcloud.Dicom.Data;
using DICOMcloud.Dicom.DataAccess;
using DICOMcloud.Pacs;
using DICOMcloud.Dicom.Media;
using DICOMcloud.Core.Messaging;
using DICOMcloud.Core.Storage;
using Microsoft.Azure;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.Storage;
using Unity.WebApi;
using DICOMcloud.Pacs.Commands;
using DICOMcloud.Wado.Core;
using DICOMcloud.Core.Azure.Storage;

namespace DICOMcloud.Wado
{
    public partial class App
    {
        private App ()
        {
            RegisterComponents ( ) ; 
        }

        public static void Config ( ) 
        {
            Instance = new App ( ) ;
        }

        private static App Instance {get; set; }

        public void RegisterComponents()
        {
            var container        = new UnityContainer();
            var connectionString = ConfigurationManager.ConnectionStrings["app:PacsDataArchieve"].ConnectionString;
            var storageConection = CloudConfigurationManager.GetSetting("app:PacsStorageConnection");
            var dataAccess       = new DicomInstanceArchieveDataAccess(connectionString);


            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
            
            container.RegisterType<IObjectArchieveQueryService, ObjectArchieveQueryService>();
            container.RegisterType<IObjectStoreService, ObjectStoreService>();
            container.RegisterType<IObjectRetrieveDataService, ObjectRetrieveDataService>();

            container.RegisterType<IStoreCommand, StoreCommand>();
            container.RegisterType<IWadoRsSerivce, WadoRsSerivce>();
            
            container.RegisterInstance<IDicomInstnaceStorageDataAccess>(dataAccess);
            container.RegisterInstance<IDicomStorageQueryDataAccess>(dataAccess);

            if ( System.IO.Path.IsPathRooted( storageConection ) )
            {
                container.RegisterType<IMediaStorageService, FileStorageService> ( new InjectionConstructor ( storageConection ) ) ;
            }
            else
            {
                var storageAccount = CloudStorageAccount.Parse( storageConection ) ;

                
                container.RegisterInstance<CloudStorageAccount>(storageAccount);
                
                container.RegisterType<IMediaStorageService, AzureStorageService> ( new InjectionConstructor ( storageAccount ) ) ;
            }

            RegisterMediaWriters(container);
        }
    }
}