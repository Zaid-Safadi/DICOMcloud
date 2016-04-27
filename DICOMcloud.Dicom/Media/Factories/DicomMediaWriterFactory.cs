using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Dicom.Media
{
    public class DicomMediaWriterFactory : IDicomMediaWriterFactory
    {
        protected Func<string, IDicomMediaWriter> MediaFactory { get; private set; }

        public DicomMediaWriterFactory ( ) 
        {
            Init ( CreateDefualtWriters ) ;
        }

        public DicomMediaWriterFactory ( Func<string, IDicomMediaWriter> mediaFactory ) 
        {
            Init ( mediaFactory ) ;
        }

        private void Init ( Func<string, IDicomMediaWriter> mediaFactory )
        {
            MediaFactory = mediaFactory ;
        }

        public virtual IDicomMediaWriter GetMediaWriter ( string mimeType )
        {
            try
            {
                IDicomMediaWriter writer = null ;
            
                writer = MediaFactory ( mimeType ) ;
            
                if ( null == writer )
                {
                    Trace.TraceInformation ( "Requested media writer not registered: " + mimeType ) ;
                }
                
                return writer ;
            }
            catch
            {
                return null ;
            }
        }
        
        protected virtual IDicomMediaWriter CreateDefualtWriters ( string mimeType  ) 
        {
            if ( mimeType == MimeMediaTypes.DICOM )
            {
                return new NativeMediaWriter ( ) ;
            }

            return null ;
        }
    }
}
