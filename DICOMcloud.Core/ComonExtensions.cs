using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Core.Extensions
{
    public static class CommonExtensions
    {
        public static string ToJson ( this object me )
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject (me) ;
        }

        public static dynamic FromJson ( this string me )
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic> ( me) ;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable) 
        {
            return enumerable == null || !enumerable.Any();
        }
    }
}
