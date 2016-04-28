using ClearCanvas.Dicom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Dicom.DataAccess.Matching
{
    public class MatchingBase : DicomDataParameter, IMatchingCondition
    {
        public MatchingBase ( ) : this ( null, 0 )
        {
            
        }

        public MatchingBase ( IList<uint> supportedTags, uint keyTag )
        {
            Elements      = new List<DicomAttribute> ( ) ;
            SupportedTags = supportedTags ?? new List<uint> ( );
            KeyTag        = keyTag ;

            ExactMatch = true ;
        }

        public virtual bool IsCaseSensitive { get;  protected set ; }

        public virtual bool ExactMatch { get;  protected set ;}

        public virtual bool SupportFuzzy { get; protected set ;}

        public virtual bool CanMatch ( DicomAttribute element )
        {
            if ( SupportedTags.Count > 0 ) 
            {
                return SupportedTags.Contains(element.Tag.TagValue ) ;
            }

            return true ;
        }

        public override bool IsSupported(DicomAttribute element)
        {
            return CanMatch ( element ) ;
        }

        protected virtual bool HasWildcardMatching ( string elementValue )
        {
            return elementValue.Contains ( "*") || elementValue.Contains ( "?") ;
        }
    
    }

    public class SingleValueMatching : MatchingBase
    {
        public SingleValueMatching ( )
        {
            ExactMatch = true ;
            IsCaseSensitive = false ;
        }

        public override bool CanMatch ( DicomAttribute element )
        {
            if ( element.Tag.VR.Equals ( DicomVr.SQvr ) || element.Count == 0 ) { return false ;}

            string elementValue = element.ToString ( ) ;

            if ( element.Tag.VR.Equals ( DicomVr.DAvr) || 
                 element.Tag.VR.Equals ( DicomVr.DTvr ) || 
                 element.Tag.VR.Equals ( DicomVr.TMvr ) )
            {
                if ( elementValue.Contains ( "-")){return false ;}
            }
            else
            {
                if ( HasWildcardMatching (elementValue) )
                {
                    return false ;
                }
            }

            return base.CanMatch ( element ) ;
        }
    }

    public class ListofUIDMatching : MatchingBase
    {
        public override bool CanMatch(DicomAttribute element)
        {
            if ( !element.Tag.VR.Equals ( DicomVr.UIvr)) { return false ; }

            if ( element.Count <= 1 ) { return false ; }

            return base.CanMatch ( element ) ;
        }
    }

    public class UniversalMatching : MatchingBase
    {
        public override bool CanMatch(DicomAttribute element)
        {
            if ( element.Count > 0 ) { return false ; }

            return base.CanMatch ( element ) ;
        }
    }

    public class WildCardMatching : MatchingBase
    {

        private static List<DicomVr> _invliadVrs = new List<DicomVr> ( ) ;

        public WildCardMatching ( )
        {
            IsCaseSensitive = true ;
        }

        static WildCardMatching ( )
        {
            _invliadVrs.AddRange ( new DicomVr[] { DicomVr.SQvr, DicomVr.DAvr, DicomVr.TMvr, DicomVr.DTvr, DicomVr.SLvr, DicomVr.SSvr,
                                                   DicomVr.USvr, DicomVr.ULvr, DicomVr.FLvr, DicomVr.FDvr, DicomVr.OBvr, DicomVr.OWvr,
                                                   DicomVr.UNvr, DicomVr.ATvr, DicomVr.DSvr, DicomVr.ISvr, DicomVr.ASvr, DicomVr.UIvr } ) ;
        }

        public override bool ExactMatch
        {
            get
            {
                return false ;
            }

            protected set
            {
                base.ExactMatch = value;
            }
        }

        public override bool CanMatch(DicomAttribute element)
        {
            if ( _invliadVrs.Contains ( element.Tag.VR ) ) { return false ; }

            if ( !HasWildcardMatching (element.ToString ( )) ) { return false ; }

            return base.CanMatch ( element ) ;
        }
    }

    public class RangeMatching : MatchingBase
    {
        public RangeMatching ( ): base ()
        {

        }

        public RangeMatching ( IList<uint> supportedTags, uint keyTag ) : base(supportedTags, keyTag)
        {

        }

        public override bool CanMatch(DicomAttribute element)
        {
            //if ( IsRangeSupported && !DateTimeMatching.IsSupported(element.Tag.TagValue ) )
            //{
            //    return false ;
            //}

            if ( !MatchVr ( element ) )
            { 
                return false ;
            }

            return base.CanMatch ( element ) ;
        }

        private bool MatchVr(DicomAttribute element)
        {
            DicomVr elementVr = element.Tag.VR ;
            if ( !elementVr.Equals ( DicomVr.DAvr) && !elementVr.Equals ( DicomVr.TMvr ) && !elementVr.Equals ( DicomVr.DTvr)) { return false ; }

            if ( HasWildcardMatching (element.ToString ( ) )) { return false ; }

            return true ;
        }

        public override bool AllowExtraElement
        {
            get
            {
                if ( DateElement != null && TimeElement != null ) //we already have enough
                { 
                    return false ;
                }
                
                return true ;
            }
        }

        public override bool ExactMatch
        {
            get
            {
                return true ;
            }

            protected set
            {
                base.ExactMatch = value;
            }
        }

        public override void SetElement(DicomAttribute element)
        {
            base.SetElement ( element ) ;
        
            if ( element.Tag.VR == DicomVr.DAvr )
            { 
                DateElement = element ;
            }

            if ( element.Tag.VR == DicomVr.TMvr )
            { 
                TimeElement = element ;
            }
        }

        public DicomAttribute DateElement { get; protected set; }
        public DicomAttribute TimeElement { get; protected set; }

        //protected DateTimeElementsMatching DateTimeMatching { get; set; }
        
        //private bool IsRangeSupported
        //{
        //    get
        //    { 
        //        return ( DateTimeMatching != null ) ;
        //    }
        //}

        public override string[] GetValues()
        {
            //if ( DateElement != null )
            //{ 
            //    DateElement.GetDateTime()
            //}

            return base.GetValues();
        }

        public override IDicomDataParameter CreateParameter()
        {
            var rangeMatch = new RangeMatching ( SupportedTags, KeyTag ) ;

            rangeMatch.AllowExtraElement = AllowExtraElement ;
            rangeMatch.DateElement       = DateElement ;
            rangeMatch.TimeElement       = TimeElement ;
            rangeMatch.Elements          = Elements; 
            rangeMatch.ExactMatch        = ExactMatch ;
            rangeMatch.IsCaseSensitive   = IsCaseSensitive ;
            rangeMatch.SupportFuzzy      = SupportFuzzy ;
            rangeMatch.VR                = VR ;

            return rangeMatch ;
        }
    }

    public class SequenceMatching : MatchingBase
    {
        public override bool CanMatch(DicomAttribute element)
        {
            if ( !element.Tag.VR.Equals (DicomVr.SQvr)) { return false; }

            return base.CanMatch ( element ) ;
        }
    }

    //public class DateTimeElementsMatching
    //{ 
    //    public uint MatchingDateTag { get; set; }
    //    public uint MatchingTimeTag { get; set; }        
    
    //    public bool IsSupported ( uint tagValue )
    //    { 
    //        return tagValue == MatchingDateTag || tagValue == MatchingTimeTag ;
    //    }
    //}

}
