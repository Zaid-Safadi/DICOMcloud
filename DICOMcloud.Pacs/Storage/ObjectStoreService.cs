using ClearCanvas.Dicom;
using DICOMcloud.Core.Storage;
using DICOMcloud.Dicom.Common;
using DICOMcloud.Dicom.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DICOMcloud.Dicom;
using DICOMcloud.Pacs.Commands;

namespace DICOMcloud.Pacs
{
    public class ObjectStoreService : IObjectStoreService
    {
        public IDicomInstnaceStorageDataAccess DataAccess   { get; set; }
        public IStoreCommand                    StoreCommand { get; set; }
        //public ObjectStoreDataService ( ) {}
        
        public ObjectStoreService 
        ( 
            IDicomInstnaceStorageDataAccess dataAccess,
            IStoreCommand                   storeCommand
        )
        {
            DataAccess   = dataAccess ;
            StoreCommand = storeCommand ;
        }
        
        public StoreResult StoreDicom
        ( 
            Stream dicomStream
        )
        {
            StoreResult storeResult  = new StoreResult ( ) ;
            DicomFile   dicomObject  = null ;


            try
            {
                //currently not used
                StoreCommandResult result = new StoreCommandResult ( ) ;


                dicomObject = GetDicom(dicomStream);
                
                result = StoreCommand.Execute ( dicomObject );

                storeResult.DataSet = dicomObject.DataSet ;
                storeResult.Status  = CommandStatus.Success ;
            }
            catch ( Exception ex )
            {
                storeResult.Status = CommandStatus.Failed ;

                //TODO: must catch specific exception types and set status, message and "code" accoringely
                storeResult.DataSet = dicomObject.DataSet ;
                storeResult.Status  = CommandStatus.Failed ;
                storeResult.Error   = ex ;
                storeResult.Message = ex.Message ;
            }
            finally
            {
                if ( null != dicomObject && !string.IsNullOrEmpty ( dicomObject.Filename ) && File.Exists ( dicomObject.Filename ) ) 
                {
                    try
                    {
                        File.Delete ( dicomObject.Filename ) ;
                    }catch {}
                }
            }
        
            return storeResult ;    
        }

        protected virtual DicomFile GetDicom ( Stream dicomStream )
        {
            //TODO: try this!
            DicomFile dicom;
            dicom = new DicomFile();
            dicom.Load(dicomStream);
            return dicom;

            string tempFile = Path.GetTempFileName();

            var reader = File.Open(tempFile, FileMode.OpenOrCreate, FileAccess.ReadWrite);//new MemoryStream () ;

            dicomStream.CopyTo(reader);

            reader.Close();

            //byte[] buffer =  reader.ToArray();

            //File.WriteAllBytes(tempFile, buffer);

            DicomFile df = new DicomFile(tempFile);
            df.Load();
            return df;
        }
    }
}
