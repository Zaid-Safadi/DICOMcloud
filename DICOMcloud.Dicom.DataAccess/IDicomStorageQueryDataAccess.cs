using DICOMcloud.Dicom.DataAccess.DB;
using DICOMcloud.Dicom.DataAccess.Matching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Dicom.DataAccess
{
    public interface IDicomStorageQueryDataAccess
    {
        void Search
        ( 
            IEnumerable<IMatchingCondition> conditions, 
            IStorageDataReader responseBuilder,
            QueryOptions options
        ) ;
    }
}
