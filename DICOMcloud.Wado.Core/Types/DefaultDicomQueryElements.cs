using ClearCanvas.Dicom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Wado.Core
{
    public class DefaultDicomQueryElements
    {
        static DicomAttributeCollection _studyDs = new DicomAttributeCollection ( ) ;
        static DicomAttributeCollection _seriesDs = new DicomAttributeCollection ( ) ;
        static DicomAttributeCollection _instanceDs = new DicomAttributeCollection ( ) ;
            
    
        public virtual DicomAttributeCollection GetStudyQuery ( ) 
        {
            return GetDefaultStudyQuery ( ) ;
        }

        public virtual DicomAttributeCollection GetSeriesQuery ( ) 
        {
            return GetDefaultSeriesQuery ( ) ;
        }

        public virtual DicomAttributeCollection GetInstanceQuery ( ) 
        {
            return GetDefaultInstanceQuery ( ) ;
        }


        public static DicomAttributeCollection GetDefaultStudyQuery ( ) 
        {
            return _studyDs.Copy ( false, true, true ) ;
        }

        public static DicomAttributeCollection GetDefaultSeriesQuery ( ) 
        {
            return _seriesDs.Copy ( false, true, true ) ;
        }

        public static DicomAttributeCollection GetDefaultInstanceQuery ( ) 
        {
            return _instanceDs.Copy ( false, true, true ) ;
        }

        static DefaultDicomQueryElements ( ) 
        {
            
            FillStudyLevel     ( _studyDs ) ;
            FillSeriesLevel    ( _seriesDs ) ;
            FillInstsanceLevel ( _instanceDs ) ;

        }

        private static void FillStudyLevel(DicomAttributeCollection studyDs)
        {
            var temp = studyDs[DicomTags.SpecificCharacterSet] ;
            temp = studyDs[DicomTags.StudyDate] ;
            temp = studyDs[DicomTags.StudyTime] ;
            temp = studyDs[DicomTags.AccessionNumber] ;
            temp = studyDs[DicomTags.InstanceAvailability] ;
            temp = studyDs[DicomTags.ModalitiesInStudy] ;
            temp = studyDs[DicomTags.ReferringPhysiciansName] ;
            temp = studyDs[DicomTags.TimezoneOffsetFromUtc] ;
            temp = studyDs[DicomTags.RetrieveUri] ;
            temp = studyDs[DicomTags.PatientsName] ;
            temp = studyDs[DicomTags.PatientId] ;
            temp = studyDs[DicomTags.PatientsBirthDate] ;
            temp = studyDs[DicomTags.PatientsSex] ;
            temp = studyDs[DicomTags.StudyInstanceUid] ;
            temp = studyDs[DicomTags.StudyId] ;
            temp = studyDs[DicomTags.NumberOfStudyRelatedSeries] ;
            temp = studyDs[DicomTags.NumberOfStudyRelatedInstances] ;
        }

        private static void FillSeriesLevel(DicomAttributeCollection seriesDs)
        {
            var temp = seriesDs[DicomTags.SpecificCharacterSet] ;
            temp = seriesDs[DicomTags.Modality] ;
            temp = seriesDs[DicomTags.TimezoneOffsetFromUtc] ;
            temp = seriesDs[DicomTags.SeriesDescription] ;
            temp = seriesDs[DicomTags.RetrieveUri] ;
            temp = seriesDs[DicomTags.SeriesInstanceUid] ;
            temp = seriesDs[DicomTags.SeriesNumber] ;
            temp = seriesDs[DicomTags.NumberOfSeriesRelatedInstances] ;
            temp = seriesDs[DicomTags.PerformedProcedureStepStartDate] ;
            temp = seriesDs[DicomTags.PerformedProcedureStepStartTime] ;
            //temp = seriesDs[DicomTags.RequestAttributesSequence] ; //Not supported yet


        }

        private static void FillInstsanceLevel(DicomAttributeCollection instanceDs)
        {
            var temp = instanceDs[DicomTags.SpecificCharacterSet] ;
            temp = instanceDs[DicomTags.SopClassUid] ;
            temp = instanceDs[DicomTags.SopInstanceUid] ;
            temp = instanceDs[DicomTags.InstanceNumber] ;
            //temp = instanceDs[DicomTags.SopClassUid] ;
        }
    }
}
