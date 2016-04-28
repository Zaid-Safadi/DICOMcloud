using ClearCanvas.Dicom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Dicom.DataAccess
{
    public interface IDicomDataParameterFactory <T>
        where T : IDicomDataParameter 
    {
        void BeginProcessingElements ( ) ;

        void ProcessElement(DicomAttribute element) ;

        IEnumerable<T> EndProcessingElements ( ) ;

        IEnumerable<T> ProcessDataSet ( DicomAttributeCollection dataset );
    }
}
