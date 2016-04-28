using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClearCanvas.Dicom;
using DICOMcloud.Dicom;
using DICOMcloud.Dicom.Data;
using DICOMcloud.Pacs;
using DICOMcloud.Pacs.Commands;

namespace DICOMcloud.Wado.Core
{
    public class WadoStoreResponse
    {
        private DicomAttributeCollection _dataset ;
        public RetieveUrlProvider UrlProvider { get; set; }
        public string StudyInstanceUID { get; private set; }

        public WadoStoreResponse ( )
        : this ( "" )
        {

        }

        public WadoStoreResponse ( string studyInstanceUID )
        {
            _dataset         = new DicomAttributeCollection ( ) ;
            UrlProvider      = new RetieveUrlProvider ( ) ;
            StudyInstanceUID = studyInstanceUID ;
        }

        public void AddResult ( StoreResult result ) 
        {
            switch ( result.Status )
            {
                case CommandStatus.Success:
                {
                    AddSuccessItem ( result.DataSet ) ;
                }
                break;

                case CommandStatus.Failed:
                {
                    AddFailedItem ( result.DataSet ) ;
                }
                break;

                default:
                {}
                break;
            }
        }

        public DicomAttributeCollection GetResponseContent ( )
        {
            _dataset[DicomTags.RetrieveUri].SetStringValue ( UrlProvider.GetStudyUrl ( StudyInstanceUID ) ) ;
        
            return _dataset ;
        }

        public void AddResult ( Exception ex, Stream dicomStream )
        {
            DicomFile dataSet = new DicomFile ( ) ;
            
            dataSet.Load ( dicomStream ) ;
            
            AddFailedItem ( GetReferencedInstsance ( dataSet.DataSet ) ) ;
        }

        private void AddFailedItem ( DicomAttributeCollection ds )
        {
            var            referencedInstance = GetReferencedInstsance ( ds ) ;
            DicomAttribute failedSeq          = _dataset[DicomTags.FailedSopSequence];
            var            item = new DicomSequenceItem ( ) ;

            referencedInstance.Merge ( item ) ;

            failedSeq.AddSequenceItem ( item ) ;

            item [ DicomTags.FailureReason ].SetUInt16 ( 0, 272 ) ; //TODO: for now 272 == "0110 - Processing failure", must map proper result code from org. exception
        }

        private void AddSuccessItem ( DicomAttributeCollection ds )
        {
            var referencedInstance = GetReferencedInstsance ( ds ) ;
            var referencedSeq      = _dataset [DicomTags.ReferencedInstanceSequence];
            var item               = new DicomSequenceItem ( ) ;

            referencedInstance.Merge ( item ) ;

            item[DicomTags.RetrieveUri].SetStringValue ( UrlProvider.GetInstanceUrl ( new ObjectID ( ds ) ) ) ; 

            referencedSeq.AddSequenceItem ( item ) ;
        }

        private DicomAttributeCollection GetReferencedInstsance ( DicomAttributeCollection ds )
        {
            DicomAttributeCollection dataset = new DicomAttributeCollection ( ) ;

            dataset [DicomTags.SopClassUid]    = ds [DicomTags.SopClassUid] ;
            dataset [DicomTags.SopInstanceUid] = ds [DicomTags.SopInstanceUid] ;

            return dataset ;
        }
    }
}

//6.6.1.3.2.1.2 Failure Reason
//*****************************
//A7xx - Refused out ofResources
//The STOW-RS Service did not store the instance because it was out of resources.

//A9xx - Error: Data Set does notmatch SOP Class
//The STOW-RS Service did not store the instance because the instance does not conform to itsspecified SOP Class.

//Cxxx - Error: Cannotunderstand
//The STOW-RS Service did not store the instance because it cannot understand certain Data Ele-ments.

//C122 - Referenced TransferSyntax not supported
//The STOW-RS Service did not store the instance because it does not support the requestedTransfer Syntax for the instance.

//0110 - Processing failure
//The STOW-RS Service did not store the instance because of a general failure in processing theoperation.

//0122 - Referenced SOP Classnot supported
//The STOW-RS Service did not store the instance because it does not support the requested SOPClass.
//*********************************
