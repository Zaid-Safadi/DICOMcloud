using ClearCanvas.Dicom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Dicom.DataAccess.Matching
{
    public class ConditionFactory : DicomDataParameterFactory<IMatchingCondition>
    {
        private static RangeMatching      studyDateTime = new RangeMatching ( new List<uint> ( ), DicomTags.StudyDate ) ;
        private static IMatchingCondition seqMatching = new SequenceMatching ( ) ;
        private static IMatchingCondition uidMatching = new ListofUIDMatching ( ) ;
        private static IMatchingCondition rngMatching = new RangeMatching ( ) ;
        private static IMatchingCondition wicMatching = new WildCardMatching ( ) ;
        private static IMatchingCondition sivMatching = new SingleValueMatching ( ) ;
        private static IMatchingCondition uniMatching = new UniversalMatching ( ) ;
        
        static ConditionFactory ( )
        {
            IList<uint> supportedTags = studyDateTime.SupportedTags ;

            supportedTags.Add ( DicomTags.StudyDate);
            supportedTags.Add ( DicomTags.StudyTime);
        }

        public ConditionFactory ( )
        {
        }

        protected override void PupolateTemplate ( List<IDicomDataParameter> parametersTemplate )
        {
            parametersTemplate.Add ( studyDateTime ) ;
            parametersTemplate.Add ( seqMatching ) ;
            parametersTemplate.Add ( uidMatching ) ;
            parametersTemplate.Add ( rngMatching ) ;
            parametersTemplate.Add ( wicMatching ) ;
            parametersTemplate.Add ( sivMatching ) ;
            parametersTemplate.Add ( uniMatching ) ;

            //Stupid C# is not accepting this
            //R matchingParam = new SequenceMatching ( ) ;
            //or this:
            //parametersTemplate.Add ( new SequenceMatching ( ) ) ;
        }

        //public virtual IEnumerable<IMatchingCondition> End ( )
        //{ 
        //    return (ICollection<IMatchingCondition>) InternalResult ;
        //}
    }
}
