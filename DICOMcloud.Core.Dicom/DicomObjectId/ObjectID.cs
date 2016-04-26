using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearCanvas.Dicom;

namespace DICOMcloud.Dicom.Data
{
    public class ObjectID : IObjectID
    {

        public ObjectID ( ) 
        {}

        public ObjectID ( DicomAttributeCollection dataset )
        {
            StudyInstanceUID  = dataset[DicomTags.StudyInstanceUid].GetString   ( 0, "" ) ;
            SeriesInstanceUID = dataset[DicomTags.SeriesInstanceUid].GetString  ( 0, "" ) ;
            SopInstanceUID    = dataset[DicomTags.SopInstanceUid].GetString     ( 0, "" ) ;
        }

        public string SeriesInstanceUID
        {
            get ;
            set ;
        }

        public string SopInstanceUID
        {
            get ;
            set ;
        }

        public string StudyInstanceUID
        {
            get ;
            set ;
        }
    
        public int? Frame { get; set; }    
    }
}
