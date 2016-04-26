using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DICOMcloud.Core.Extensions.Helpers
{
  public class ClassHelper 
  {

    public static string PathToBin ( ) 
    {
        return Path.GetDirectoryName ( Assembly.GetExecutingAssembly().GetName().CodeBase ) ;
    }

    public static XmlReader GetAsXml ( string path )
    { 
        return XmlReader.Create (path) ;
    }
  
  }
}
