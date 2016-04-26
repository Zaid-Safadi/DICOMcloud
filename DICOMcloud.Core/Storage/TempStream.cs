using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Core.Storage
{
    public class TempStream : FileStream
    {
        TempFile _tmpFile ;
        public TempStream ( string filePath ) : this ( new TempFile ( filePath ) )
        {
        }

        public TempStream ( TempFile file ) : base ( file.Path, FileMode.Create )
        {
            _tmpFile = file ;
        }


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _tmpFile.Dispose ( ) ;
        }
    }
}
