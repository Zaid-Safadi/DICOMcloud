using ClearCanvas.Dicom;
using DICOMcloud.Dicom.DataAccess.DB.Schema;
using DICOMcloud.Dicom.DataAccess.Matching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Dicom.DataAccess.DB
{
    public class ObjectArchieveBuilderBase
    {
        public ObjectArchieveBuilderBase ( DbSchemaProvider schemaProvider )
        {
            SchemaProvider = schemaProvider ;
        }

        public DbSchemaProvider SchemaProvider
        {
            get;
            protected set;
        }

        protected virtual IList<string> GetValues ( IDicomDataParameter condition )
        {
            if ( condition is RangeMatching )
            {
                RangeMatching  rangeCondition  = (RangeMatching) condition ;
                DicomAttribute dateElement     = rangeCondition.DateElement ;
                DicomAttribute timeElement     = rangeCondition.TimeElement ;
                
                
                return GetDateTimeValues ( dateElement, timeElement ) ;
            }
            else if ( condition.VR.Equals ( DicomVr.DAvr ) || condition.VR.Equals ( DicomVr.DTvr ) )
            {
                DicomAttribute dateElement = null ;
                DicomAttribute timeElement = null ;

                foreach ( var element in condition.Elements )
                {
                    if ( element.Tag.VR.Equals ( DicomVr.DAvr ) )
                    {
                        dateElement = element ;
                        continue ;
                    }

                    if ( element.Tag.VR.Equals ( DicomVr.TMvr ) )
                    { 
                        timeElement = element ;
                    }
                }

                return GetDateTimeValues ( dateElement, timeElement ) ;
            }
            else
            { 
                return condition.GetValues ( ) ;
            }
        }

        private IList<string> GetDateTimeValues ( DicomAttribute dateElement, DicomAttribute timeElement )
        {
            List<string> values = new List<string> ( ) ; 
            int dateValuesCount = dateElement == null ? 0 : (int)dateElement.Count;
            int timeValuesCount = timeElement == null ? 0 : (int)timeElement.Count;
            int dateTimeIndex = 0;

            for (; dateTimeIndex < dateValuesCount || dateTimeIndex < timeValuesCount; dateTimeIndex++)
            {
                string dateString = null;
                string timeString = null;

                if (dateTimeIndex < dateValuesCount)
                {
                    dateString = dateElement == null ? null : dateElement.GetString(0, "");
                }

                if (dateTimeIndex < dateValuesCount)
                {
                    timeString = timeElement == null ? null : timeElement.GetString(0, "");
                }

                values.AddRange(GetDateTimeValues(dateString, timeString));
            }

            return values;
        }

        protected virtual IList<string> GetDateTimeValues ( string dateString, string timeString )
        {
            string date1String = null ;
            string time1String = null ;
            string date2String = null ;
            string time2String = null ;

            if ( dateString != null )
            { 
                dateString = dateString.Trim ( ) ;

                if ( !string.IsNullOrWhiteSpace ( dateString ) )
                { 
                    string[] dateRange = dateString.Split ( '-' ) ;

                    if ( dateRange.Length > 0 )
                    { 
                        date1String = dateRange [0 ] ;
                        time1String = "" ;
                    }

                    if ( dateRange.Length == 2 )
                    { 
                        date2String = dateRange [ 1 ] ;
                        time2String = "" ;
                    }
                }
            }
        

            if ( timeString != null )
            { 
                timeString = timeString.Trim ( ) ;

                if ( !string.IsNullOrWhiteSpace ( timeString ) )
                { 
                    string[] timeRange = timeString.Split ( '-' ) ;

                    if ( timeRange.Length > 0 )
                    { 
                        date1String = date1String ?? "" ;
                        time1String = timeRange [0 ] ; 
                    }

                    if ( timeRange.Length == 2 )
                    { 
                        date2String = date2String ?? "" ;
                        time2String = timeRange [ 1 ] ;
                    }
                }
            }
        
            return GetDateTimeQueryValues ( date1String, time1String, dateString, time2String ) ;
        }

        protected virtual IList<string> GetDateTimeQueryValues
        (
            string date1String, 
            string time1String, 
            string date2String, 
            string time2String
        )
        {
            List<string> values = new List<string> ( ) ;
            
            
            SanitizeDate ( ref date1String ) ;
            SanitizeDate ( ref date2String ) ;
            SanitizeTime ( ref time1String, true ) ;
            SanitizeTime ( ref time2String, false ) ;

            if ( string.IsNullOrEmpty (date1String) && string.IsNullOrEmpty(date2String) &&
                 string.IsNullOrEmpty (time1String) && string.IsNullOrEmpty(time2String) )
            {
                return values ;
            }

            if ( string.IsNullOrEmpty(date1String) ) 
            {
                //date should be either min or same as second
                date1String = string.IsNullOrEmpty ( date2String ) ? SqlConstants.MinDate : date2String  ;
            }

            if ( string.IsNullOrEmpty (time1String) )
            {
                time1String = string.IsNullOrEmpty ( time2String ) ? SqlConstants.MinTime : time2String ;
            }

            if ( string.IsNullOrEmpty(date2String) ) 
            {
                //date should be either min or same as second
                date2String = ( SqlConstants.MinDate == date1String ) ? SqlConstants.MaxDate : date1String ;
            }

            if ( string.IsNullOrEmpty (time2String) )
            {
                time2String = ( SqlConstants.MinTime == time1String ) ? SqlConstants.MaxTime : time1String ;
            } 

            values.Add ( date1String + " " + time1String ) ;
            values.Add ( date2String + " " + time2String ) ;
            
            return values ;
        }

        
        //TODO: currently not used any more
        protected virtual string CombineDateTime(string dateString, string timeString, bool secondInRange )
        {
            if ( string.IsNullOrWhiteSpace ( timeString ) && string.IsNullOrWhiteSpace ( dateString ) )
            {
                return ( secondInRange ) ? SqlConstants.MaxDateTime : SqlConstants.MinDateTime ;
            }

            if ( string.IsNullOrEmpty ( timeString ) )
            {
                timeString = ( secondInRange ) ? SqlConstants.MaxTime : SqlConstants.MinTime ;
            }

            if ( string.IsNullOrEmpty ( dateString ) )
            {
                dateString = ( secondInRange ) ? SqlConstants.MaxDate : SqlConstants.MinDate ;
            }
            

            return dateString + " " + timeString ;
        }

        protected virtual void SanitizeTime(ref string timeString, bool startTime )
        {
            if (null == timeString) { return ;}

            if ( string.IsNullOrEmpty ( timeString ) )
            { 
                timeString = "" ;

                return ;
            }

            if ( true )//TODO: add to config
            {
                timeString = timeString.Replace (":", "");
            }

            int length = timeString.Length ;

            if (length > "hhmm".Length) 
            {  
                timeString = timeString.Insert (4, ":") ; 
            }
            else if ( length == 4 )
            { 
                if ( startTime )
                {
                    timeString   += ":00" ;
                }
                else
                { 
                    timeString += ":59" ;
                }
            }
            
            if (timeString.Length > "hh".Length) 
            {  
                timeString = timeString.Insert (2, ":") ; 
            }
            else //it must equal
            { 
                if ( startTime )
                {
                    timeString   += ":00:00" ;
                }
                else
                { 
                    timeString += ":59:59" ;
                }
            }
            
            {//TODO: no support for fractions 
                int fractionsIndex ;

                if( ( fractionsIndex= timeString.LastIndexOf (".") ) > -1 )
                {
                    timeString = timeString.Substring ( 0, fractionsIndex ) ;
                }
            } 
        }

        protected virtual void SanitizeDate(ref string dateString )
        {
            if (null == dateString) { return ;}
            
            if ( string.IsNullOrEmpty ( dateString) )
            { 
                dateString = "" ;

                return ;
            }

            //TODO: make it a configuration option
            //a lot of dataset samples do not follow dicom standard
            if (true)
            {   
                dateString = dateString.Replace ( ".", "" ).Replace ( "-", "") ;
            }

            int length = dateString.Length ;

            if (length != 8) {  throw new ArgumentException ( "Invalid date value") ; }
            
            dateString = dateString.Insert ( 6, "-") ;

            dateString = dateString.Insert ( 4, "-") ;
        }
    
        public static class SqlConstants
        {
            public static string MinDate = "1753/1/1" ;
            public static string MaxDate = "9999/12/31" ;
            public static string MinTime = "00:00:00" ;
            public static string MaxTime = "23:59:59" ;

            public static string MaxDateTime = "9999/12/31 11:59:59"   ;
            public static string MinDateTime = "1753/1/1 00:00:00" ;
        }
    }
}
