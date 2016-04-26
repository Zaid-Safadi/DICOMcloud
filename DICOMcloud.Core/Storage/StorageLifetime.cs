using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Core.Storage
{
    public enum StorageLifetime
    {
        Persistent, //always there
        Context, //within current call
        Container, //as long as the container object is alive
        Closed, //delete when underlying stream is closed
        //Application, //as long as the application is running
    }
}
