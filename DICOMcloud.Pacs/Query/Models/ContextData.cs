using ClearCanvas.Dicom;
using DICOMcloud.Dicom.DataAccess;
using DICOMcloud.Dicom.DataAccess.DB.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Pacs
{
    public partial class ObjectArchieveResponseBuilder
    { 
        private class EntityReadData
        { 
            public DicomAttributeCollection         CurrentDs                 = new DicomAttributeCollection ( ) ;
            public Dictionary<uint, PersonNameData> CurrentPersonNames        = new Dictionary<uint,PersonNameData> ( )  ; 
            public PersonNameData                   CurrentPersonNameData     = null ;
            public uint                             CurrentPersonNameTagValue = 0 ;
            public string                           KeyValue                  = null ;

        
            public bool  IsCurrentDsSequence   = false ;
            public bool  IsCurrentDsMultiValue = false ;
            public uint  ForeignTagValue       = 0 ;
            public DicomAttributeCollection ForeignDs = null ;             
            
            //public void ResetCurrentRead ( )
            //{
            //    CurrentDs                 = null ;
            //    CurrentPersonNames        = new Dictionary<uint,PersonNameData> ( )  ; 
            //    CurrentPersonNameData     = null ;
            //    CurrentPersonNameTagValue = 0 ;
            //    KeyValue                  = null ;
            //    IsCurrentDsSequence   = false ;
            //    IsCurrentDsMultiValue = false ;
            //    ForeignTagValue       = 0 ;
            //    ForeignDs              = null ; 

            //}
        }
    }
}
