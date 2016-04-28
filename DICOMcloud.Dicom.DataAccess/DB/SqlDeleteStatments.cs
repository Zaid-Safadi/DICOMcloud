using DICOMcloud.Dicom.DataAccess.DB.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Dicom.DataAccess.DB
{
    static class SqlDeleteStatments
    {
        public static string GetDeleteInstanceCommandText 
        ( 
            //string sopColKeyName, //0
            //string seriesColKeyName, //1
            //string studyColKeyName, //2
            //string patientColKeyName,//3
            //string sopInstanceTableName, //4
            //string seriesTableName, //5
            //string studyTableName, //6
            //string patientTableName, //7
            //string sopInstanceRefSeriesColName, //8
            //string seriesRefStudyColName, //9
            //string studyRefPatientColName, //10
            //string sopInstanceUidColName, //11
            string sopInstanceUidValue //12
        ) 
        {
            StorageDbSchemaProvider schemaProvider = new StorageDbSchemaProvider ( ) ;

            return string.Format ( Delete.Delete_Instance_Command_Formatted,
                                   schemaProvider.ObjectInstanceTable.KeyColumn.Name, 
                                   schemaProvider.SeriesTable.KeyColumn.Name, 
                                   schemaProvider.StudyTable.KeyColumn.Name, 
                                   schemaProvider.PatientTable.KeyColumn.Name,
                                   schemaProvider.ObjectInstanceTable.Name, 
                                   schemaProvider.SeriesTable.Name, 
                                   schemaProvider.StudyTable.Name, 
                                   schemaProvider.PatientTable.Name,
                                   schemaProvider.ObjectInstanceTable.ForeignColumn.Name,
                                   schemaProvider.SeriesTable.ForeignColumn,
                                   schemaProvider.StudyTable.ForeignColumn,
                                   schemaProvider.ObjectInstanceTable.ModelKeyColumns.FirstOrDefault().Name, 
                                   sopInstanceUidValue) ;
        }

        public class Delete
        {
            public static readonly string Delete_Instance_Command_Formatted = 
            @"
declare @sop bigint
declare @series bigint
declare @study bigint
declare @patient bigint
declare @sopCount int
declare @seriesCount int
declare @studyCount int

SELECT @sop = instance.{0}, @series = ser.{1}, @study = stud.{2}, @patient = p.{3}
FROM {4} instance, {5} ser, {6} stud, {7} p
where 
	instance.{8} = ser.{1} AND 
	Ser.{9} = stud.{2} AND
	stud.{10} = p.{3} AND
	instance.{11} = '{12}'
	
Delete from {4} where {4}.{0} = @sop

SELECT @sopCount = COUNT(*)
FROM {4} instance
WHERE instance.{8} = @series

/* if 0 entries, remove orphaned' series record */
IF (@sopCount = 0) 
DELETE FROM {5} WHERE (@series = {5}.{1});

SELECT  @seriesCount = COUNT(*) FROM {5} ser
WHERE ser.{9} = @study

/* if 0 entries, remove orphaned study record */
IF (@seriesCount = 0) 
DELETE FROM {6} WHERE (@study = {6}.{2});

SELECT  @studyCount = COUNT(*) FROM {6} stud
WHERE stud.{10} = @patient

/* if 0 entries, remove orphaned patient record */
IF (@studyCount = 0) 
DELETE FROM {7} WHERE (@patient = {7}.{3});
            ";
        }
    }
}
