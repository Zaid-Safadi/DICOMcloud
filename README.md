# DICOMcloud

[![Join the chat at https://gitter.im/Zaid-Safadi/DICOMcloud](https://badges.gitter.im/Zaid-Safadi/DICOMcloud.svg)](https://gitter.im/Zaid-Safadi/DICOMcloud?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)
DICOMcloud is an open source DICOMweb server that implements RESTful services in DICOM part 18


# Demo
I'm maintining an online version of the server currently hosted at: 
[https://dicomcloud.azurewebsites.net/](https://dicomcloud.azurewebsites.net/)
Sure this is only a default ASP.NET landing page but the Web API can be accessed by using the URLs defined here:
[https://dicomcloud.azurewebsites.net/help/](https://dicomcloud.azurewebsites.net/help/)

There is a client Demo as well that I already open sourced and hosting:
[https://github.com/Zaid-Safadi/dicom-webJS](https://github.com/Zaid-Safadi/dicom-webJS/)

# Implementation
The code is built in .NET (currently V4.5) and Visual Studio 2015. The web services are built as ASP.NET MVC REST API Controllers.

Currently the project uses ClearCanvas DICOM dll for reading and writing DICOM information and image data. The plan is to replace this by another better supported "open source" library (fo-DICOM or EvilDICOM)

Physical DICOM storage is supported on both, either Windows File System or Azure Blob Storage.
Query is currently implemented against a SQL database

Implementation natively support JSON and XML DICOM format.



# Support
The code is designed to be a complete DICOM web server implementation with storage, query and retrieve capabilities.

   - STOW –RS (partially supported)
   - QIDO-RS (partially supported)
   - WADO-RS (partially supported)
   - WADO URI (partially supported)
   - UPS-RS (not yet supported)
   - RS Capabilities (not yet supported)

Current support is only for storing/returning un-compressed Images due to challenges working with ClearCanvas with decoding/encoding. 
The new library which will replace ClearCanvas will hopefully have support for other image compressions J2K, JPG... (Lossy and Lossless)

# License
 
    Copyright 2016 Zaid AL-Safadi

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
