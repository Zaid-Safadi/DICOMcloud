using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DICOMcloud.Dicom.Data;

namespace DICOMcloud.Wado.Models
{
    public interface IWadoRsStudiesRequest : IWadoRequestHeader, IStudyID
    {
    
    }

    public interface IWadoRsSeriesRequest : IWadoRsStudiesRequest, ISeriesID
    {
        
    }

    public interface IWadoRSInstanceRequest : IWadoRsSeriesRequest, IObjectID
    {

    }

    public interface IWadoRSFramesRequest :IWadoRSInstanceRequest
    {
        int[] Frames { get; set; }
    }
}
