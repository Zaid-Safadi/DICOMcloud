using System ;

namespace DICOMcloud.Core.Messaging
{
    public interface ITransportMessage
    {
        string Name { get; }
        string ID   { get; set; }
    }
}