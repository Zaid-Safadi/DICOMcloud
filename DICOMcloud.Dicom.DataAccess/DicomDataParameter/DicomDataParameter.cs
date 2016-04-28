using ClearCanvas.Dicom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DICOMcloud.Dicom.DataAccess
{
    public class DicomDataParameter : IDicomDataParameter
    {
        public virtual uint        KeyTag            { get;  set ; }
        public virtual DicomVr     VR                { get; set ; }
        public virtual bool        AllowExtraElement {  get; set ;}
        public         IList<uint> SupportedTags     { get ; protected set; }

        public DicomDataParameter() : this (null ) {
        
        }

        public DicomDataParameter(IList<uint> supportedTags ) {
            Elements = new List<DicomAttribute> ( ) ;
            SupportedTags = supportedTags ?? new List<uint> ( ) ;
        }

        public virtual string[] GetValues ( ) 
        {
            if ( Elements.Count == 1 )
            { 
                return Elements.First ( ).ToString ( ).Split ( '/' ) ;
            }

            return null ;
        }
        public virtual void SetElement ( DicomAttribute element )
        {
            Elements.Add ( element ) ;

            if ( KeyTag == 0 )
            { 
                KeyTag = element.Tag.TagValue ;
            }

            if ( VR == null )
            { 
                VR = element.Tag.VR ;
            }
        }

        public virtual bool IsSupported(DicomAttribute element) {

            if ( null != SupportedTags && SupportedTags.Count > 0 )
            {
                return SupportedTags.Contains ( element.Tag.TagValue ) ;
            }

            return true ;
        }
        
        public virtual IDicomDataParameter CreateParameter ( )
        { 
            IDicomDataParameter dicomParam = (IDicomDataParameter) Activator.CreateInstance (this.GetType ( ) ) ;

            dicomParam.KeyTag            = KeyTag ;
            dicomParam.VR                = VR ;
            dicomParam.AllowExtraElement = AllowExtraElement ;

            foreach (var tag in SupportedTags)
            {
                dicomParam.SupportedTags.Add ( tag ) ;    
            }
            
            return dicomParam ;
        }
        
        public virtual List<PersonNameData> GetPNValues ( )
        {
            if ( VR != DicomVr.PNvr) {return null ; }

            List<PersonNameData> result   = new List<PersonNameData> ( ) ;
            string[]             pnValues = GetValues ( ) ;

            foreach ( string pnValue in pnValues )
            { 
                PersonNameData pnData = new PersonNameData ( ) ;
                string[] pnParts = pnValue.Split ( '^') ;
                int length = pnParts.Length ;

                if ( length > 0 )
                { 
                    pnData.LastName = pnParts [0] ;
                }
                if ( length > 1 )
                { 
                    pnData.GivenName = pnParts [ 1 ] ;
                }
                if ( length > 2 )
                { 
                    pnData.MiddleName = pnParts [ 2 ] ;
                }
                if ( length > 3 )
                { 
                    pnData.Prefix = pnParts [ 3 ] ;
                }
                if ( length > 4 )
                { 
                    pnData.Suffix = pnParts [ 4 ] ;
                }

                result.Add ( pnData ) ;
            }

            return result ;
        }

        public IList<DicomAttribute> Elements { get; set ; }
    }

    public class StoreParameter : DicomDataParameter
    {
        public StoreParameter() : base ( ){ }  
        public StoreParameter(IList<uint> supportedTags) : base ( supportedTags ) { }
        
        public override string[] GetValues()
        {
            if ( Elements.Count == 0 )
            { 
                return base.GetValues();
            }

            if ( Elements.Count > 1 )
            {
                if ( VR == DicomVr.TMvr || VR == DicomVr.DAvr )
                {
                    List<string> values = new List<string> ( ) ;

                    for ( int index = 0; index < Elements.Count; index+=2)
                    {
                        DicomAttribute dateElement = null ;
                        DicomAttribute timeElement = null ;
                        string value = null ;

                        for ( int localIndex = index; localIndex < index + 2 && localIndex < Elements.Count; localIndex++)
                        {
                            var element = Elements[localIndex] ;

                            if ( element.Tag.VR == DicomVr.DAvr )
                            {
                                dateElement = element ;
                            }
                            else if ( element.Tag.VR == DicomVr.TMvr )
                            {
                                timeElement = element ;
                            }
                        }

                        if ( null != dateElement )
                        {
                            
                        }


                    }
                }
            }

            return base.GetValues ( ) ;
        }

        public override bool AllowExtraElement
        {
            get
            {
                if ( SupportedTags.Count > 0 )
                { 
                    return Elements.Count < SupportedTags.Count ;
                }

                return base.AllowExtraElement ;
            }
            set
            {
                base.AllowExtraElement = value;
            }
        }
    }
}
