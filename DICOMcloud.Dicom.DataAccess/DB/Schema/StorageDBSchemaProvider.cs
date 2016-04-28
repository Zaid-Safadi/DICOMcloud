
using ClearCanvas.Dicom;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Dicom.DataAccess.DB.Schema
{
    public class StorageDbSchemaProvider : DbSchemaProvider
    {
        public StorageDbSchemaProvider ( )
        {
            PatientTable        = GetTableInfo ( PatientTableName) ;
            StudyTable          = GetTableInfo ( StudyTableName ) ;
            SeriesTable         = GetTableInfo ( SeriesTableName ) ;
            ObjectInstanceTable = GetTableInfo ( ObjectInstanceTableName ) ;
        }

        public  readonly TableKey PatientTable ;
        public  readonly TableKey StudyTable ;
        public  readonly TableKey SeriesTable ;
        public  readonly TableKey ObjectInstanceTable ;


        //TODO: replace access to this with a QueryBase Key that corresponds to a table defined in the schema (key=name or new attribute)
        public const string PatientTableName        = "Patient" ;
        public const string StudyTableName          = "Study" ;
        public const string SeriesTableName         = "Series" ;
        public const string ObjectInstanceTableName ="ObjectInstance" ;

        public class MetadataTable
        {
            public static string TableName         = "ObjectInstance" ;
            public static string SopInstanceColumn = "SopInstanceUid" ;
            public static string MetadataColumn    = "Metadata" ;
        }
        
    }
}
