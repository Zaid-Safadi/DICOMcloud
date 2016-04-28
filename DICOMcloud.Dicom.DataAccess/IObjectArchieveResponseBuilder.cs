using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearCanvas.Dicom;

namespace DICOMcloud.Dicom.DataAccess
{
    public interface IStorageDataReader
    {
        string QueryLevel { get; set; }
        
        void BeginResultSet ( string name ) ;
        void EndResultSet   ( ) ;

        void BeginRead  ( ) ;
        void EndRead    ( ) ;
        void ReadData   ( string tableName, string columnName, object value ) ;
    
        
        ICollection<DicomAttributeCollection> GetResponse ( ) ;
    }
}
