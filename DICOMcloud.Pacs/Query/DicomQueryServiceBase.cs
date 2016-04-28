using ClearCanvas.Dicom;
using DICOMcloud.Dicom.DataAccess;
using DICOMcloud.Dicom.DataAccess.DB.Schema;
using DICOMcloud.Dicom.DataAccess.Matching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Pacs.Query
{

    //TODO: base class for query services
    public abstract class DicomQueryServiceBase : IDicomQueryService
    {
        protected IDicomStorageQueryDataAccess QueryDataAccess { get; set; }
        
        public DicomQueryServiceBase ( IDicomStorageQueryDataAccess queryDataAccess )
        {
            QueryDataAccess = queryDataAccess ;
        }

        public ICollection<DicomAttributeCollection> Find 
        ( 
            DicomAttributeCollection request, 
            QueryOptions options 
        )
        {
            StorageDbSchemaProvider         dbSchema        = new StorageDbSchemaProvider ( ) ;
            IEnumerable<IMatchingCondition> conditions      = null ;
            ObjectArchieveResponseBuilder   responseBuilder = new ObjectArchieveResponseBuilder ( dbSchema, options.QueryLevel ) ;


            conditions = BuildConditions ( request ) ;

            DoFind ( request, options, conditions, responseBuilder ) ;

            return responseBuilder.GetResponse ( ) ;
        }

        protected virtual IEnumerable<IMatchingCondition> BuildConditions
        (
            DicomAttributeCollection request
        )
        {
            ConditionFactory condFactory = new ConditionFactory ( ) ;
            IEnumerable<IMatchingCondition> conditions ;
            
            condFactory.BeginProcessingElements ( ) ;

            foreach ( var element in request )
            {
                condFactory.ProcessElement ( element ) ;
            }

            conditions = condFactory.EndProcessingElements ( ) ;

            return conditions ;
        }

        protected abstract void DoFind
        (
            DicomAttributeCollection request,
            QueryOptions options, 
            IEnumerable<IMatchingCondition> conditions, 
            ObjectArchieveResponseBuilder responseBuilder
        ) ;

        //protected virtual DicomInstanceArchieveDataAccess CreateDataAccess()
        //{
        //    DicomInstanceArchieveDataAccess dataAccess = new DicomInstanceArchieveDataAccess(connectionString);
        //    return dataAccess;
        //}

        protected virtual ObjectArchieveResponseBuilder CreateResponseBuilder ( string queryLevel )
        {
            StorageDbSchemaProvider         dbSchema        = new StorageDbSchemaProvider ( ) ;
            ObjectArchieveResponseBuilder responseBuilder = new ObjectArchieveResponseBuilder( dbSchema, queryLevel);

            responseBuilder.QueryLevel = queryLevel ;

            return responseBuilder;
        }
    }
}
