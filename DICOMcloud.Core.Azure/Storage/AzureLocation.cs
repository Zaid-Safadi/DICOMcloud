using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DICOMcloud.Core.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace DICOMcloud.Core.Azure.Storage
{
    public class AzureLocation : IStorageLocation
    {
        public AzureLocation ( ICloudBlob blob, IMediaId id = null )
        {
            __Blob  = blob;
            MediaId = id;        
        }

        public void Delete()
        {
            __Blob.Delete ();
        }

        public string Name
        {
            get {  return __Blob.Name ; }
        }

        public string ID
        {
            get {  return __Blob.Uri.AbsolutePath ; }
        }

        //public Stream GetWriteStream (  )
        //{
        //    stream = 
            
        //    return __Blob.Uri.ToString();
        //}

        public Stream Download()
        {
            return __Blob.OpenRead();
        }

        public void Upload(Stream stream)
        {
            __Blob.Properties.ContentType = ContentType ;
            __Blob.UploadFromStream (stream);
            
            WriteMetadata ( ) ;
        }

        private void WriteMetadata ( )
        {
            __Blob.SetMetadata ( ) ;
            //__Blob.SetProperties ( ) ;
        }

        public void Download(Stream stream)
        {
            __Blob.DownloadToStream ( stream ) ;
        }

        public virtual void Upload ( byte[] buffer )
        {
            __Blob.UploadFromByteArray ( buffer, 0, buffer.Length ) ;
            WriteMetadata ( ) ;
        }

        public void Upload(string filename)
        {
            __Blob.UploadFromFile (filename ) ;
            WriteMetadata( ) ;
         }

        public Stream GetReadStream()
        {
            return __Blob.OpenRead ( ) ;
        }

        public bool Exists()
        {
            return __Blob.Exists ( ) ;
        }

        public string ContentType 
        { 
            get
            {
                return __Blob.Properties.ContentType ;
            } 
        }

        public IMediaId MediaId { get; private set; }

        private ICloudBlob __Blob
        {
            get; 
            set; 
        }

        public string Metadata
        {
            get
            {
                return __Blob.Metadata["meta"] ;
            }

            set
            {
                __Blob.Metadata["meta"] = value ;
            }
        }
    }
}
