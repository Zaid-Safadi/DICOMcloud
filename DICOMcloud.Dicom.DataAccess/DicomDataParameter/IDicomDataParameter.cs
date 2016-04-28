using ClearCanvas.Dicom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Dicom.DataAccess
{
    public interface IDicomDataParameter
    {
        uint        KeyTag            { get; set ; }
        DicomVr     VR                { get; set ; }
        bool        AllowExtraElement { get; set ; }
        IList<uint> SupportedTags     { get ; }

        bool                  IsSupported     ( DicomAttribute element );
        void                  SetElement      ( DicomAttribute element ) ;
        string[]              GetValues       ( ) ;
        List<PersonNameData>  GetPNValues     ( ) ;
        IDicomDataParameter   CreateParameter ( ) ;
        IList<DicomAttribute> Elements { get; set ; }
    
    }
}
