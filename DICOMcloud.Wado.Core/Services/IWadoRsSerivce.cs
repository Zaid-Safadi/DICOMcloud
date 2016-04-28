using System.Net.Http;
using DICOMcloud.Wado.Models;

namespace DICOMcloud.Wado.Core
{
    public interface IWadoRsSerivce
    {
        HttpResponseMessage RetrieveBulkData(IWadoRsStudiesRequest request);
        HttpResponseMessage RetrieveFrames(IWadoRSFramesRequest request);
        HttpResponseMessage RetrieveInstance(IWadoRSInstanceRequest request);
        HttpResponseMessage RetrieveInstanceMetadata(IWadoRSInstanceRequest request);
        HttpResponseMessage RetrieveSeries(IWadoRsSeriesRequest request);
        HttpResponseMessage RetrieveSeriesMetadata(IWadoRsSeriesRequest request);
        HttpResponseMessage RetrieveStudy(IWadoRsStudiesRequest request);
        HttpResponseMessage RetrieveStudyMetadata(IWadoRsStudiesRequest request);
    }
}