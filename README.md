# DICOMcloud

DICOMcloud is an open source DICOMweb server that implements RESTful services in DICOM part 13

The code is designed to be a complete DICOM web server implementation with storage, query and retrieve capabilities.

   - STOW –RS (partially supported)
   - QIDO-RS (partially supported)
   - WADO-RS (partially supported)
   - WADO URI (partially supported)
   - UPS-RS (not yet supported)
   - RS Capabilities (not yet supported)

The code is built in .NET (currently V4.5) and Visual Studio 2015. The web services are built as ASP.NET MVC REST API Controllers.

Currently the project uses ClearCanvas DICOM dll for reading and writing DICOM information and image data. The plan is to replace this by another better supported "open source" library (fo-DICOM or EvilDICOM)

Physical DICOM storage is supported on both, either Windows File System or Azure Blob Storage.
Query is currently implemeneted against a SQL database

Implementation natively support JSON and XML DICOM format.

Current support is only for storing/returning un-compressed Images due to challenges working with ClearCanvas with decoding/encoding. 
The new libraryt which will replace ClearCanvas will hopefully have support for other image compressions J2K, JPG... (Lossy and Lossless)


