using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearCanvas.Dicom;
using DICOMcloud.Dicom.Data;
using DICOMcloud.Core.Storage;

namespace DICOMcloud.Dicom.Media
{
    //TODO: can be extended to support other formats by passing different 
    //types in the constructor (e.g. json, XML...)
    public class DicomMediaId : IMediaId
    {
        public virtual string MediaType { get; set ; }
        public virtual IObjectID DicomObject { get; set; }
        
        public DicomMediaId ( ) {}

        public DicomMediaId ( string [] parts ) 
        {
            if ( parts == null || parts.Length != 5 ) { throw new ArgumentOutOfRangeException ( "parts array must be 5" ) ; }

            MediaType   = parts[0] ;
            DicomObject = new ObjectID ( ) { StudyInstanceUID = parts[1], SeriesInstanceUID = parts[2], SopInstanceUID = parts[3], Frame = int.Parse(parts[4]) };
        }

        public DicomMediaId ( DicomAttributeCollection dataset, string mediaType ) : this (dataset, 0, mediaType )
        {}

        public DicomMediaId ( DicomAttributeCollection dataset, int frame, string mediaType )
        {
            DicomObject       = new ObjectID ( ) {
            StudyInstanceUID  = dataset[DicomTags.StudyInstanceUid].GetString(0,""),
            SeriesInstanceUID = dataset[DicomTags.SeriesInstanceUid].GetString(0,""), 
            SopInstanceUID    = dataset[DicomTags.SopInstanceUid].GetString(0,""),
            Frame             = frame } ;
            
            MediaType = mediaType ;
        }

        public DicomMediaId ( IStudyID studyId, string mediaType ) : this ( new ObjectID ( ) { StudyInstanceUID = studyId.StudyInstanceUID }, mediaType  )
        {}

        public DicomMediaId ( ISeriesID seriesId, string mediaType ) : this ( new ObjectID ( ) { StudyInstanceUID = seriesId.StudyInstanceUID,
                                                                                                 SeriesInstanceUID = seriesId.SeriesInstanceUID }, 
                                                                              mediaType  )
        {}
        
        public DicomMediaId ( IObjectID objectId, string mediaType )
        {
            DicomObject = objectId ;
            MediaType   = mediaType ;
        }

        public string[] GetIdParts()
        {
            //TODO: sanitize all parts..... on storage NOT here!
            List<string> parts = new List<string> ( )  ;


            parts.Add ( MediaType.Replace ("/", "-").Replace ( "+", "" ) ) ;

            if( string.IsNullOrWhiteSpace ( DicomObject.StudyInstanceUID ) )
            {
                return parts.ToArray ( ) ;
            }

            parts.Add ( DicomObject.StudyInstanceUID ) ;

            if( string.IsNullOrWhiteSpace ( DicomObject.SeriesInstanceUID ) )
            {
                return parts.ToArray ( ) ;
            }

            parts.Add ( DicomObject.SeriesInstanceUID ) ;

            if( string.IsNullOrWhiteSpace ( DicomObject.SopInstanceUID ) )
            {
                return parts.ToArray ( ) ;
            }

            parts.Add ( DicomObject.SopInstanceUID ) ;

            if( DicomObject.Frame == null )
            {
                parts.Add ( FIRST_FRAME_NUMER ) ;
            }
            else
            {
                parts.Add ( DicomObject.Frame.ToString ( ) ) ;
            }

            return parts.ToArray ( ) ;
            
        }
        
        private const string FIRST_FRAME_NUMER = "1" ;
    }
}
