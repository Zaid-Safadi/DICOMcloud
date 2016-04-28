using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DICOMcloud.Dicom.DataAccess.Matching;
using ClearCanvas.Dicom;
using DICOMcloud.Dicom.DataAccess.DB.Query;
using DICOMcloud.Dicom.Data;

namespace DICOMcloud.Dicom.DataAccess.DB
{
    public class DicomSqlDataAdapter
    {
        public DicomSqlDataAdapter ( string connectionString ) : this ( connectionString, new ObjectArchieveQueryBuilder ( ), new ObjectArchieveStorageBuilder ( ) )
        { 
        }

        public DicomSqlDataAdapter ( string connectionString, ObjectArchieveQueryBuilder queryBuilder, ObjectArchieveStorageBuilder storageBuilder )
        { 
            ConnectionString = connectionString ;
            QueryBuilder     = queryBuilder ;
            StorageBuilder   = storageBuilder ;
        }

        public IDbCommand CreateSelectCommand ( string sourceTable, IEnumerable<IMatchingCondition> conditions )
        {
            QueryBuilder.BuildQuery ( conditions, sourceTable ) ;

            string queryText = QueryBuilder.GetQueryText ( sourceTable ) ;
        
            var SelectCommand = CreateCommand ( ) ;

            SelectCommand.CommandText = queryText ;
            
            SetConnectionIfNull ( SelectCommand ) ;
        
            return SelectCommand ;
        }

        public IDbCommand CreateInsertCommand ( IEnumerable<IDicomDataParameter> conditions )
        {
            IDbCommand insertCommand = CreateCommand ( ) ;

            StorageBuilder.BuildInsert ( conditions, insertCommand ) ;

            SetConnectionIfNull ( insertCommand ) ;
            
            return insertCommand ;
        
        }

        public string[] GetCurrentQueryTables ( )
        { 
            return QueryBuilder.GetQueryResultTables ( ).ToArray ( ) ;
        }

        //TODO: thread safety??
        //TODO: replace this with passing the proper "Database" provider
        public virtual void CreateConnection()
        {
            if ( null == Connection )
            { 
                Connection = new System.Data.SqlClient.SqlConnection ( ) ;
                Connection.ConnectionString = ConnectionString ;
            }
        }


        public IDbCommand CreateUpdateMetadataCommand ( IObjectID instance, string metadata )
        {
            IDbCommand insertCommand = CreateCommand ( ) ;

            insertCommand = CreateCommand ( ) ;

            insertCommand.CommandText = string.Format ( @"
UPDATE {0} SET {2}=@{2} WHERE {1}=@{1}

IF @@ROWCOUNT = 0
   INSERT INTO {0} ({2}) VALUES (@{2})
", DB.Schema.StorageDbSchemaProvider.MetadataTable.TableName, DB.Schema.StorageDbSchemaProvider.MetadataTable.SopInstanceColumn, DB.Schema.StorageDbSchemaProvider.MetadataTable.MetadataColumn ) ;

             var sopParam  = new System.Data.SqlClient.SqlParameter ( "@" + DB.Schema.StorageDbSchemaProvider.MetadataTable.SopInstanceColumn, instance.SopInstanceUID ) ;
             var metaParam = new System.Data.SqlClient.SqlParameter ( "@" + DB.Schema.StorageDbSchemaProvider.MetadataTable.MetadataColumn, metadata ) ;
            
            insertCommand.Parameters.Add ( sopParam ) ;
            insertCommand.Parameters.Add ( metaParam ) ;

            SetConnectionIfNull ( insertCommand ) ;        
            
            return insertCommand ; 
        }

        public IDbCommand CreateGetMetadataCommand ( IObjectID instance )
        {
            IDbCommand command  = CreateCommand ( ) ;
             var       sopParam = new System.Data.SqlClient.SqlParameter ( "@" + DB.Schema.StorageDbSchemaProvider.MetadataTable.SopInstanceColumn, instance.SopInstanceUID ) ;
            
             
             command.CommandText = string.Format ( "SELECT {0} FROM {1} WHERE {2}=@{2}", 
                                                  DB.Schema.StorageDbSchemaProvider.MetadataTable.MetadataColumn,
                                                  DB.Schema.StorageDbSchemaProvider.MetadataTable.TableName,
                                                  DB.Schema.StorageDbSchemaProvider.MetadataTable.SopInstanceColumn ) ;

            command.Parameters.Add ( sopParam );

            SetConnectionIfNull ( command ) ;
            
            return command ;
        }

        public IDbCommand CreateDeleteInstanceCommand ( string sopInstanceUID )
        {
            IDbCommand command  = CreateCommand ( ) ;

            StorageBuilder.BuildDelete ( sopInstanceUID, command ) ;

            SetConnectionIfNull ( command ) ;

            return command ;
        }

        public IDbConnection Connection { get ; set ; }

        public string ConnectionString {  get; protected set ; }

        protected virtual ObjectArchieveQueryBuilder   QueryBuilder {  get; set ;}
        protected virtual ObjectArchieveStorageBuilder StorageBuilder { get; set; }

        protected virtual IDbCommand CreateCommand ( )
        { 
            return new System.Data.SqlClient.SqlCommand ( ) ;
        }

        protected virtual System.Data.SqlClient.SqlConnection CreateNewConnection ()
        {
            return new System.Data.SqlClient.SqlConnection ( ) ;
        }

        private void SetConnectionIfNull ( IDbCommand command )
        {
            if (command !=null && command.Connection == null)
            {
                if ( Connection == null )
                {
                    CreateConnection ( ) ;
                }

                command.Connection = Connection;
            }
        }

    }
}
