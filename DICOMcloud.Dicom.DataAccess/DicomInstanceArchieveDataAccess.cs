using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DICOMcloud.Dicom.DataAccess.Matching;
using DICOMcloud.Dicom.DataAccess.DB;
using System.Data.SqlClient;
using DICOMcloud.Dicom.DataAccess.DB.Schema;
using System.Data;
using DICOMcloud.Dicom.Data;

namespace DICOMcloud.Dicom.DataAccess
{
    public interface IDicomInstanceArchieveDataAccess : IDicomInstnaceStorageDataAccess, IDicomStorageQueryDataAccess
    {}

    public class DicomInstanceArchieveDataAccess : IDicomInstanceArchieveDataAccess
    {
        public string ConnectionString { get; set ; }

        public DicomInstanceArchieveDataAccess() : this("") { }
        public DicomInstanceArchieveDataAccess ( string connectionString )
        { 
            ConnectionString = connectionString ;
        }

        public void Search
        (
            IEnumerable<IMatchingCondition> conditions, 
            IStorageDataReader responseBuilder,
            QueryOptions options
        )
        {
            DicomSqlDataAdapter dbAdapter = new DicomSqlDataAdapter ( ConnectionString ) ;
            string queryLevel             = responseBuilder.QueryLevel ;
            var cmd                       = dbAdapter.CreateSelectCommand ( queryLevel, conditions ) ;

            cmd.Connection.Open ( ) ;

            try
            {
                List<IDicomDataParameter> parameters = new List<IDicomDataParameter> ( ) ;

                using ( var reader = cmd.ExecuteReader ( ) )
                { 
                    var tables = dbAdapter.GetCurrentQueryTables ( ) ;
                    int currentTableIndex = -1 ;
                    
                    
                    do
                    {
                        currentTableIndex ++ ;
                        
                        responseBuilder.BeginResultSet ( tables[currentTableIndex] ) ;

                        while ( reader.Read ( ) )
                        {
                            responseBuilder.BeginRead ( ) ;
                            
                            for ( int columnIndex = 0; columnIndex < reader.FieldCount; columnIndex++ )
                            {
                                string columnName = reader.GetName ( columnIndex ) ;
                                string tableName  = tables [currentTableIndex];

                                object value = reader.GetValue ( columnIndex ) ;
                                
                                responseBuilder.ReadData ( tableName, columnName, value ) ;
                            }
                        
                            responseBuilder.EndRead ( ) ;
                        }

                        responseBuilder.EndResultSet ( ) ;

                    } while ( reader.NextResult ( ) ) ;
                }
            }
            finally
            { 
                if ( cmd.Connection.State == System.Data.ConnectionState.Open )
                { 
                    cmd.Connection.Close ( ) ;
                }
            }

            //System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection ()
        }

        public void StoreInstance ( IEnumerable<IDicomDataParameter> parameters, int offset, int limit )
        {
            DicomSqlDataAdapter dbAdapter = new DicomSqlDataAdapter ( ConnectionString ) ;
            //TODO: use transation
            //dbAdapter.CreateTransation ( ) 
            
            var cmd = dbAdapter.CreateInsertCommand( parameters ) ;
        
            cmd.Connection.Open ( ) ;

            try
            { 
                int rowsInserted = cmd.ExecuteNonQuery ( ) ;
                
                if ( rowsInserted <= 0 )
                { 
                    //return duplicate instance?!!!
                }
            }
            finally
            { 
                cmd.Connection.Close ( ) ;
            }
        }

        public void StoreInstanceMetadata( IObjectID instance, string metadata ) 
        {
            DicomSqlDataAdapter dbAdapter = new DicomSqlDataAdapter ( ConnectionString ) ;
            //TODO: use transaction
            //dbAdapter.CreateTransaction ( ) 

            var cmd = dbAdapter.CreateUpdateMetadataCommand ( instance, metadata ) ;
        
            cmd.Connection.Open ( ) ;

            try
            { 
                int rowsInserted = cmd.ExecuteNonQuery ( ) ;
                
                if ( rowsInserted <= 0 )
                { 
                    //TODO: return duplicate instance?!!!
                }
            }
            finally
            { 
                cmd.Connection.Close ( ) ;
            }        
        }

        public string GetInstanceMetadata( IObjectID instance ) 
        {
            DicomSqlDataAdapter dbAdapter = new DicomSqlDataAdapter ( ConnectionString ) ;
            
            
            var cmd = dbAdapter.CreateGetMetadataCommand ( instance ) ;
        
            cmd.Connection.Open ( ) ;

            try
            { 
                object result = cmd.ExecuteScalar ( ) ;
                
                return result as string ;
            }
            finally
            { 
                cmd.Connection.Close ( ) ;
            }
        }

        public void DeleteInstance ( string instance )
        {
            DicomSqlDataAdapter dbAdapter = new DicomSqlDataAdapter ( ConnectionString ) ;
            IDbCommand cmd ;
            
            dbAdapter.CreateConnection ( ) ;
            
            cmd = dbAdapter.CreateDeleteInstanceCommand ( instance ) ;
        
            cmd.Connection.Open ( ) ;

            try
            { 
                cmd.ExecuteScalar ( ) ;
            }
            finally
            { 
                cmd.Connection.Close ( ) ;
            }
        }


    }
}
