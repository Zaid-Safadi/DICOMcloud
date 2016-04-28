using ClearCanvas.Dicom;
using DICOMcloud.Dicom.DataAccess;
using DICOMcloud.Dicom.DataAccess.DB;
using DICOMcloud.Dicom.DataAccess.DB.Schema;
using DICOMcloud.Dicom.DataAccess.Matching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DICOMcloud.Pacs.Query;

namespace DICOMcloud.Pacs
{
    public class ObjectArchieveQueryService : DicomQueryServiceBase, IObjectArchieveQueryService
    {
        public ObjectArchieveQueryService ( IDicomStorageQueryDataAccess dataAccess ) : base ( dataAccess )
        {}
         
        public ICollection<DicomAttributeCollection> FindStudies 
        ( 
            DicomAttributeCollection request, 
            int? limit,
            int? offset    
        )
        {
            return Find ( request, new QueryOptions() { Limit = limit, Offset = offset, QueryLevel = StorageDbSchemaProvider.StudyTableName } ) ;
        }

        public ICollection<DicomAttributeCollection> FindObjectInstances
        (
            DicomAttributeCollection request,
            int? limit,
            int? offset
        )
        {
            return Find ( request, new QueryOptions() { Limit = limit, Offset = offset, QueryLevel = StorageDbSchemaProvider.ObjectInstanceTableName }) ;
        }

        public ICollection<DicomAttributeCollection> FindSeries
        (
            DicomAttributeCollection request,
            int? limit,
            int? offset
        )
        {
            return Find ( request, new QueryOptions() { Limit = limit, Offset = offset, QueryLevel = StorageDbSchemaProvider.SeriesTableName }) ;
        }

        protected override void DoFind
        (
           DicomAttributeCollection request,
           QueryOptions options,
           IEnumerable<IMatchingCondition> conditions,
           ObjectArchieveResponseBuilder responseBuilder
        )
        {
            QueryDataAccess.Search ( conditions, responseBuilder, options ) ;
        }



        //private List<IMatchingCondition> BuildQueryCondition(DicomAttribute element)
        //{
        //    List<IMatchingCondition> conditions = new List<IMatchingCondition> ( ) ;

        //    conditions.Add ( ConditionFactory.Process ( element )  ) ;

        //    return conditions ;
        //}
    }
}
