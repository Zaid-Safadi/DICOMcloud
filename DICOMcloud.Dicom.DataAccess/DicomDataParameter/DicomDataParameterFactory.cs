using ClearCanvas.Dicom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Dicom.DataAccess
{
    public class DicomDataParameterFactory<T> : IDicomDataParameterFactory <T>
        where T : IDicomDataParameter 
    {
        public List<IDicomDataParameter> ParametersTemplate { get; protected set ; }
        protected List<IDicomDataParameter> ProcessingList { get; set; }
        protected List<T> InternalResult { get; set; }


        protected virtual void PupolateTemplate ( List<IDicomDataParameter> parametersTemplate )
        {
            parametersTemplate.Add ( new DicomDataParameter ( ) ) ;
        }

        public DicomDataParameterFactory ( )
        {
            ParametersTemplate = new List<IDicomDataParameter> ( ) ;
            ProcessingList     = new List<IDicomDataParameter> ( ) ;
            InternalResult     = new List<T> ( ) ;

            PupolateTemplate ( ParametersTemplate ) ;
        }

        public IEnumerable<T> ProcessDataSet ( DicomAttributeCollection dataset )
        {
            BeginProcessingElements ( ) ;
            
            foreach ( var element in dataset )
            {
                ProcessElement ( element ) ;
            }

            return EndProcessingElements () ;
        }

        public virtual void BeginProcessingElements ( )
        { 
            InternalResult = new List<T> ( ) ;
            ProcessingList = new List<IDicomDataParameter> ( ) ;
        }

        public virtual void ProcessElement(DicomAttribute element) 
        {
            for ( int index = ProcessingList.Count -1; index >= 0; index-- )
            { 
                IDicomDataParameter currentCondition = ProcessingList[index] ;
                
                
                if ( currentCondition.IsSupported ( element ) )
                { 
                    currentCondition.SetElement ( element ) ;

                    if ( !currentCondition.AllowExtraElement)
                    { 
                        ProcessingList.RemoveAt(index);
                    }

                    return ;
                }
            }

            foreach ( var condition in ParametersTemplate )
            {
                if ( condition.IsSupported ( element ) )
                {
                    IDicomDataParameter dedicatedCondition = condition.CreateParameter ( ) ;


                    dedicatedCondition.SetElement ( element ) ;

                    InternalResult.Add ( (T) dedicatedCondition ) ;

                    if ( dedicatedCondition.AllowExtraElement )
                    { 
                        ProcessingList.Add ( dedicatedCondition ) ;
                    }

                    return ;
                }
            }
        }

        public virtual IEnumerable<T> EndProcessingElements ( ) 
        { 
            return (ICollection<T>) InternalResult ;
        }

        //public virtual IEnumerable<IDicomDataParameter> End ( ) 
        //{ 
        //    return InternalResult.OfType <IDicomDataParameter> ( ) ;
        //}
    }

    public class DicomStoreParameterFactory : DicomDataParameterFactory <StoreParameter>
    {
        private static StoreParameter studyDateTime ; 
        
        public DicomStoreParameterFactory ( ) : base ( )
        { 
        }

        protected override void PupolateTemplate(List<IDicomDataParameter> parametersTemplate)
        {
            List<uint> supportedDateTime = new List<uint> ( ) ;
            supportedDateTime.Add ( DicomTags.StudyDate ) ;
            supportedDateTime.Add ( DicomTags.StudyTime ) ;

            studyDateTime = new StoreParameter ( supportedDateTime ) ; 
            studyDateTime.KeyTag = DicomTags.StudyDate ;

            parametersTemplate.Add ( studyDateTime ) ;
            parametersTemplate.Add ( new StoreParameter ( ) ) ;
            //base.PupolateTemplate(parametersTemplate);
        }
    }
}
