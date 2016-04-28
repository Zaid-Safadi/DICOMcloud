using System;
using System.Collections.Generic;
using ClearCanvas.Dicom;
using DICOMcloud.Dicom;
using DICOMcloud.Dicom.DataAccess;
using DICOMcloud.Dicom.DataAccess.DB.Schema;

namespace DICOMcloud.Pacs
{
    public partial class ObjectArchieveResponseBuilder : IStorageDataReader
    {
        private EntityReadData  CurrentData            = null ;
        private KeyToDataSetCollection CurrentResultSet = null ;
        private ResultSetCollection    ResultSets       = new ResultSetCollection ( ) ;
        private string CurrentResultSetName = "" ;

        public string QueryLevel { get; set; }

        public DbSchemaProvider SchemaProvider
        {
            get;
            protected set;
        }

        public ObjectArchieveResponseBuilder ( DbSchemaProvider schemaProvider, string queryLevel )
        {
            SchemaProvider = schemaProvider ;
            QueryLevel     = queryLevel ;
        }

        public virtual void BeginResultSet( string name )
        { 
            CurrentResultSet = new KeyToDataSetCollection ( ) ;
            CurrentResultSetName = name ;
        }

        public virtual void EndResultSet ( )
        { 
            ResultSets[CurrentResultSetName] = CurrentResultSet ;
        }

        public virtual void BeginRead( )
        {
            CurrentData = new EntityReadData ( ) ;
        }

        public virtual void EndRead ( )
        {
            UpdateDsPersonName ( ) ;
        
            CurrentResultSet[CurrentData.KeyValue] = CurrentData.CurrentDs ;

            CurrentData = null ;
        }

        public virtual void ReadData ( string tableName, string columnName, object value )
        { 
            var column = SchemaProvider.GetColumn( tableName, columnName ) ;
            var dicomTags = column.Tags ;

            if ( column.IsKey )
            { 
                CurrentData.KeyValue = value.ToString ( ) ;
            }
            
            if ( column.IsForeign )
            { 
                string keyString = value.ToString ( ) ;

                KeyToDataSetCollection resultSet = null ;
                
                if ( ResultSets.TryGetValue ( column.Table.Parent, out resultSet ) )
                {             
                    DicomAttributeCollection foreignDs = resultSet[keyString] ;

                    if ( QueryLevel == column.Table.Name )
                    { 
                        foreignDs.Merge ( CurrentData.CurrentDs ) ;

                        //resultSet[keyString] = CurrentData.CurrentDs ;
                    }
                    else
                    { 
                        if ( column.Table.IsSequence )
                        { 
                            DicomAttributeSQ sq = (DicomAttributeSQ) CurrentData.ForeignDs [CurrentData.ForeignTagValue] ;
                            DicomSequenceItem item = new DicomSequenceItem ( ) ;

                            sq.AddSequenceItem ( item ) ;

                            CurrentData.CurrentDs.Merge ( item ) ;

                            CurrentData.CurrentDs = item ; 
                        }
                        else if ( column.Table.IsMultiValue )
                        { 
                            CurrentData.CurrentDs = foreignDs ;
                        }
                        else
                        {
                            CurrentData.CurrentDs.Merge ( foreignDs ) ;

                            CurrentData.CurrentDs = foreignDs.Copy ( ) ;
                        }
                    }
                }
            }

            if (null == dicomTags) { return;}

            ReadTags(columnName, value, dicomTags);
        }

        private void ReadTags(string columnName, object value, uint[] dicomTags)
        {
            foreach ( var dicomTag in dicomTags )
            {
                var dicomElement = CurrentData.CurrentDs[dicomTag] ;
            

                if ( null != dicomElement && DBNull.Value != value && value != null )
                {
                    var vr = dicomElement.Tag.VR ;
                    Type valueType = value.GetType ( ) ;

                    if ( vr == DicomVr.PNvr )
                    { 
                        PersonNameParts currentPart = SchemaProvider.GetPNColumnPart ( columnName ) ;

                        if ( CurrentData.CurrentPersonNameData == null )
                        { 
                            CurrentData.CurrentPersonNameData = new PersonNameData ( ) ;
                            CurrentData.CurrentPersonNameTagValue  = dicomElement.Tag.TagValue ;
                            CurrentData.CurrentPersonNames.Add ( CurrentData.CurrentPersonNameTagValue , CurrentData.CurrentPersonNameData ) ;
                        }
                        else
                        { 
                            if ( dicomElement.Tag.TagValue != CurrentData.CurrentPersonNameTagValue )
                            { 
                                if ( CurrentData.CurrentPersonNames.TryGetValue ( dicomElement.Tag.TagValue, out CurrentData.CurrentPersonNameData ) )
                                {
                                    CurrentData.CurrentPersonNameTagValue = dicomElement.Tag.TagValue ;
                                }
                                else
                                { 
                                    CurrentData.CurrentPersonNameData = new PersonNameData ( ) ;
                                    CurrentData.CurrentPersonNameTagValue  = dicomElement.Tag.TagValue ;
                                    CurrentData.CurrentPersonNames.Add ( CurrentData.CurrentPersonNameTagValue , CurrentData.CurrentPersonNameData ) ;
                                }
                            }
                        }

                        CurrentData.CurrentPersonNameData.SetPart ( currentPart, (string) value ) ;
                    }
                    else if ( valueType == typeof(String) ) //shortcut
                    { 
                        dicomElement.SetStringValue ( (string)value ) ;
                    }
                    else if (  valueType == typeof(DateTime) ) 
                    { 
                        dicomElement.SetDateTime ( (int) dicomElement.Count, (DateTime) value ) ;
                    }

                    else if ( valueType == typeof(Int32))
                    { 
                        dicomElement.SetInt32 ((int) dicomElement.Count, (Int32) value ) ;
                    }
                    else if ( valueType == typeof(Int64))
                    { 
                        dicomElement.SetInt64 ((int) dicomElement.Count, (Int64) value ) ;
                    }
                    else
                    { 
                        dicomElement.SetStringValue ( (string)value ) ;                        
                        
                        System.Diagnostics.Debug.Assert ( false, "Unknown element db value" ) ;
                    }
                }
            }
        }
        
        public virtual ICollection<DicomAttributeCollection> GetResponse ( )
        { 
            return ResultSets[QueryLevel].Values ;
        }

        private void UpdateDsPersonName()
        {
            if (null != CurrentData.CurrentPersonNames)
            {
                foreach (var personName in CurrentData.CurrentPersonNames)
                {
                    var pnElement = CurrentData.CurrentDs[personName.Key];

                    pnElement.SetStringValue(personName.Value.ToString());
                }
            }

            CurrentData.CurrentPersonNames.Clear();

            CurrentData.CurrentPersonNames = new Dictionary<uint, PersonNameData>();
        
            CurrentData.CurrentPersonNameData = null ;
        }
    }
}
