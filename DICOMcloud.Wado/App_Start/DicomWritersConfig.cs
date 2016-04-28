using System;
using System.Configuration;
using DICOMcloud.Core.Azure.Storage;
using DICOMcloud.Core.Storage;
using DICOMcloud.Dicom.Common;
using DICOMcloud.Dicom.DataAccess;
using DICOMcloud.Dicom.Media;
using DICOMcloud.Pacs;
using DICOMcloud.Pacs.Commands;
using DICOMcloud.Wado.Core;
using Microsoft.Azure;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.Storage;

namespace DICOMcloud.Wado
{
    public partial class App
    {
        public static void RegisterMediaWriters ( IUnityContainer container ) 
        {
            var factory = new InjectionFactory(c => new Func<string, IDicomMediaWriter> (name => c.Resolve<IDicomMediaWriter>(name))) ;

            container.RegisterType <IDicomMediaWriter, NativeMediaWriter>       ( MimeMediaTypes.DICOM ) ;
            container.RegisterType <IDicomMediaWriter, JsonMediaWriter>         ( MimeMediaTypes.Json ) ; 
            container.RegisterType <IDicomMediaWriter, XmlMediaWriter>          ( MimeMediaTypes.xmlDicom ) ; 
            container.RegisterType <IDicomMediaWriter, UncompressedMediaWriter> ( MimeMediaTypes.UncompressedData) ;
            container.RegisterType<IDicomMediaWriter, JpegMediaWriter>          ( MimeMediaTypes.Jpeg) ;
            container.RegisterType <Func<string, IDicomMediaWriter>> (factory) ;
            container.RegisterType <IDicomMediaWriterFactory,DicomMediaWriterFactory> ( ) ;
            //TODO: should not be needed when we find out why unity select a constructor that it can't build when there is a defualt
            container.RegisterType<IJsonDicomConverter, JsonDicomConverter> ( ) ;                
        }
    }
}