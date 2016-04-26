using System ;


namespace DICOMcloud.Core.Messaging
{
    public interface IMessageSender
    {
        void SendMessage ( ITransportMessage message, TimeSpan? delay = default (TimeSpan?) ) ;
    }
}