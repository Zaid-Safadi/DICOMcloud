namespace DICOMcloud.Core.Storage
{
    public interface IMediaWriter<T>
    {
        string MediaType { get; }

        IStorageLocation[] CreateMedia(T objectData);
    }
}