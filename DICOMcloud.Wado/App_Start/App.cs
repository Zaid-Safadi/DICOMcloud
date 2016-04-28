using System.Configuration;
using System.Web.Http;
using DICOMcloud.Core.Azure.Storage;
using DICOMcloud.Core.Storage;
using DICOMcloud.Dicom.DataAccess;
using DICOMcloud.Pacs;
using DICOMcloud.Pacs.Commands;
using DICOMcloud.Wado.Core;
using Microsoft.Azure;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.Storage;
using Unity.WebApi;

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