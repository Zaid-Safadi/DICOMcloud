using ClearCanvas.Dicom;
using DICOMcloud.Dicom.DataAccess.DB.Schema;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DICOMcloud.Dicom.Data;

namespace DICOMcloud.Dicom.DataAccess.DB
{
    public class ObjectArchieveStorageBuilder : ObjectArchieveBuilderBase
    {
        public IList<System.Data.IDbDataParameter> Parameters { get; protected set; }
        public string InsertString {  get ; protected set ; }

        public ObjectArchieveStorageBuilder ( ) : this ( new DbSchemaProvider ( ) )
        {}
        public ObjectArchieveStorageBuilder ( DbSchemaProvider schemaprovider ) : base ( schemaprovider )
        { 
            Parameters = new List<System.Data.IDbDataParameter> ( ) ;
        }

        public void BuildInsert ( IEnumerable<IDicomDataParameter> conditions ) { BuildInsert ( conditions, null ) ; } 
        public void BuildInsert ( IEnumerable<IDicomDataParameter> conditions, IDbCommand insertCommand )
        {
            if ( null == conditions ) throw new ArgumentNullException ( ) ;

            Parameters = new List<System.Data.IDbDataParameter> ( ) ;
            
            FillParameters ( conditions, insertCommand ) ;
            
            InsertString = GetInsertText ( ) ;

            if ( null != insertCommand )
            { 
                insertCommand.CommandText = InsertString ;
            }
        }

        protected virtual string GetInsertText()
        {
            StringBuilder result = new StringBuilder ( SqlInsertStatments.BeginTransaction ) ;
            
            result.AppendLine ( ) ;

            foreach ( var insertKeyValue in _tableToInsertStatments )
            {
                var insert = insertKeyValue.Value ;

                string columns = string.Join ( ", ", insert.ColumnNames ) ;
                string values  = string.Join ( ", ", insert.ParametersValueNames ) ;
            
                result.AppendLine ( SqlInsertStatments.GetTablesKey ( insertKeyValue.Key ) ) ;
                result.AppendFormat ( insert.InsertTemplate, columns, values ) ;
                result.AppendLine ( ) ;
            }

            result.AppendLine ( SqlInsertStatments.CommitTransaction ) ;

            return result.ToString ( ) ;
        }

        protected virtual void FillParameters
        (
            IEnumerable<IDicomDataParameter> dicomParameters,
            IDbCommand insertCommad
        )
        {
            foreach ( var dicomParam in dicomParameters )
            {
                if ( dicomParam.VR == DicomVr.PNvr )
                { 
                    List<PersonNameData> pnValues ;

                         
                    pnValues = dicomParam.GetPNValues ( ) ;
                        
                    foreach ( var values in pnValues )
                    {
                        string[] stringValues = values.ToArray ( ) ;
                        int index = -1 ;
                        List<string> pnConditions = new List<string> ( ) ;

                        foreach ( var column in SchemaProvider.GetColumnInfo ( dicomParam.KeyTag ) )
                        { 
                            column.Values = new string [] { stringValues[++index]} ;
                                
                            InsertColumnValue ( column, dicomParam, insertCommad ) ;
                        }
                    }
                    
                    continue ;
                }

                
                foreach ( var column in SchemaProvider.GetColumnInfo ( dicomParam.KeyTag ) )
                { 
                    column.Values = GetValues ( dicomParam ) ;
                        
                    InsertColumnValue ( column, dicomParam, insertCommad ) ;
                }
            }
        }

        public void BuildInsertOrUpdateMetadata ( ObjectID instance, IDbCommand insertCommand )
        {
            
        }

        public void BuildDelete(string sopInstanceUID, IDbCommand command)
        {
            string delete = SqlDeleteStatments.GetDeleteInstanceCommandText (sopInstanceUID ) ;
            
             command.CommandText = delete ;

        }

        protected virtual void InsertColumnValue
        (
            ColumnInfo column, 
            IDicomDataParameter dicomParam, 
            IDbCommand insertCommand 
        )
        {
            InsertSections insert = GetTableInsert ( column.Table ) ;

            insert.ColumnNames.Add ( column.Name ) ;
            insert.ParametersValueNames.Add ( "@" + column.Name ) ; //TODO: add a parameter name to the column class
            //insert.ParametersValueNames.Add ( column.Values[0] ) ;


            System.Data.SqlClient.SqlParameter param ;
            
            object value = DBNull.Value ;
            
            if ( null != column.Values && column.Values.Count != 0 )
            {
                value = column.Values [0] ;
            
                if ( null == value )
                { 
                    value = DBNull.Value ;
                }
            }
            
            param = new System.Data.SqlClient.SqlParameter ( "@" + column.Name, value ) ;
            
            Parameters.Add ( param ) ;
            
            if ( null != insertCommand )
            { 
                insertCommand.Parameters.Add ( param ) ;
            }
        }

        protected InsertSections GetTableInsert ( TableKey table )
        { 
            InsertSections result = null ;

            if ( _tableToInsertStatments.TryGetValue ( table, out result) )
            { 
                return result ;
            }


            result = new InsertSections ( ) ;

            result.InsertTemplate = SqlInsertStatments.GetInsertIntoTable ( table ) ;
            result.TableName      = table.Name ;

            _tableToInsertStatments.Add ( table, result ) ;
        
            return result ;
        }


        SortedDictionary<TableKey,InsertSections> _tableToInsertStatments = new SortedDictionary<TableKey,InsertSections> ( ) ;
        

    }

    public class InsertSections
    {
        public InsertSections ( )
        { 
            ColumnNames = new List<string> (  ) ;
            ParametersValueNames = new List<string> ( ) ;
        }

        public string TableName                  { get; set;  }
        public List<string> ColumnNames          { get; set; }
        public List<string> ParametersValueNames { get; set; }
        public string       InsertTemplate       { get; set; }
    }
}
