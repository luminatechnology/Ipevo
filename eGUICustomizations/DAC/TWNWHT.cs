using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.AP;
using PX.Objects.CS;

namespace eGUICustomizations.DAC
{
    [Serializable]
    [PXCacheName("Withholding Tax")]
    public class TWNWHT : IBqlTable
    {
        #region String Classes
        public const string PersonalName = "PERSONALID";
        public class PersonalAtt : PX.Data.BQL.BqlString.Constant<PersonalAtt>
        {
            public PersonalAtt() : base(PersonalName) { }
        }

        public const string PropertyName = "PROPERTYID";
        public class PropertyAtt : PX.Data.BQL.BqlString.Constant<PropertyAtt>
        {
            public PropertyAtt() : base(PropertyName) { }
        }

        public const string TypeOfInName = "TYPEOFIN";
        public class TypeOfInAtt : PX.Data.BQL.BqlString.Constant<TypeOfInAtt>
        {
            public TypeOfInAtt() : base(TypeOfInName) { }
        }

        public const string WHTFmtCodeName = "WHTFMTCODE";
        public class WHTFmtCodeAtt : PX.Data.BQL.BqlString.Constant<WHTFmtCodeAtt>
        {
            public WHTFmtCodeAtt() : base(WHTFmtCodeName) { }
        }

        public const string WHTFmtSubName = "WHTFMTSUB";
        public class WHTFmtSubAtt : PX.Data.BQL.BqlString.Constant<WHTFmtSubAtt>
        {
            public WHTFmtSubAtt() : base(WHTFmtSubName) { }
        }

        public const string WHTTaxPctName = "WHTTAXPCT";
        public class WHTTaxPctAtt : PX.Data.BQL.BqlString.Constant<WHTTaxPctAtt>
        {
            public WHTTaxPctAtt() : base(WHTTaxPctName) { }
        }

        public const string ToGNHICodeName = "TOGNHICODE";
        public class ToGNHICodeAtt : PX.Data.BQL.BqlString.Constant<ToGNHICodeAtt>
        {
            public ToGNHICodeAtt() : base(ToGNHICodeName) { }
        }
        #endregion

        #region PersonalID
        [PXDBString(10, IsKey = true, IsUnicode = true)]
        [PXUIField(DisplayName = "Personal ID")]
        [PXDefault(typeof(Search<CSAnswers.value, Where<CSAnswers.refNoteID, Equal<Current<Vendor.noteID>>,
                                                        And<CSAnswers.attributeID, Equal<PersonalAtt>>>>),
                   PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual string PersonalID { get; set; }
        public abstract class personalID : PX.Data.BQL.BqlString.Field<personalID> { }
        #endregion

        #region DocType
        [PXDBString(3, IsFixed = true)]
        [PXUIField(DisplayName = "Doc. Type", Visible = false)]
        [PXDBDefault(typeof(APInvoice.docType))]
        public virtual string DocType { get; set; }
        public abstract class docType : PX.Data.BQL.BqlString.Field<docType> { }
        #endregion

        #region RefNbr
        [PXDBString(15, IsUnicode = true)]
        [PXUIField(DisplayName = "Ref. Nbr", Visible = false)]
        [PXDBDefault(typeof(APInvoice.refNbr))]
        [PXParent(typeof(SelectFrom<APInvoice>.Where<APInvoice.docType.IsEqual<TWNWHT.docType.FromCurrent>
                                                     .And<APInvoice.refNbr.IsEqual<TWNWHT.refNbr.FromCurrent>>>))]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : PX.Data.BQL.BqlString.Field<refNbr> { }
        #endregion

        #region PropertyID
        [PXDBString(12, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Property ID")]
        [PXDefault(typeof(Search<CSAnswers.value, Where<CSAnswers.refNoteID, Equal<Current<Vendor.noteID>>,
                                                        And<CSAnswers.attributeID, Equal<PropertyAtt>>>>),
                   PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual string PropertyID { get; set; }
        public abstract class propertyID : PX.Data.BQL.BqlString.Field<propertyID> { }
        #endregion
    
        #region TypeOfIn
        [PXDBString(1, IsFixed = true, InputMask = "")]
        [PXUIField(DisplayName = "Type Of Income")]
        [PXDefault(typeof(Search<CSAnswers.value, Where<CSAnswers.refNoteID, Equal<Current<Vendor.noteID>>,
                                                        And<CSAnswers.attributeID, Equal<TypeOfInAtt>>>>),
                   PersistingCheck = PXPersistingCheck.Nothing)]
        [TypeOfInSelector]
        public virtual string TypeOfIn { get; set; }
        public abstract class typeOfIn : PX.Data.BQL.BqlString.Field<typeOfIn> { }
        #endregion
    
        #region WHTFmtCode
        [PXDBString(2, IsFixed = true, InputMask = "")]
        [PXUIField(DisplayName = "Format Code")]
        [PXDefault(typeof(Search<CSAnswers.value, Where<CSAnswers.refNoteID, Equal<Current<Vendor.noteID>>,
                                                        And<CSAnswers.attributeID, Equal<WHTFmtCodeAtt>>>>),
                   PersistingCheck = PXPersistingCheck.Nothing)]
        [WHTFmtCodeSelector]
        public virtual string WHTFmtCode { get; set; }
        public abstract class wHTFmtCode : PX.Data.BQL.BqlString.Field<wHTFmtCode> { }
        #endregion
    
        #region WHTFmtSub
        [PXDBString(2, IsFixed = true, InputMask = "")]
        [PXUIField(DisplayName = "Format Sub Code")]
        [PXDefault(typeof(Search<CSAnswers.value, Where<CSAnswers.refNoteID, Equal<Current<Vendor.noteID>>,
                                                        And<CSAnswers.attributeID, Equal<WHTFmtSubAtt>>>>),
                   PersistingCheck = PXPersistingCheck.Nothing)]
        [WHTFmtSubSelector]
        public virtual string WHTFmtSub { get; set; }
        public abstract class wHTFmtSub : PX.Data.BQL.BqlString.Field<wHTFmtSub> { }
        #endregion
    
        #region WHTTaxPct
        [PXDBDecimal()]
        [PXUIField(DisplayName = "WHT Tax %")]
        public virtual decimal? WHTTaxPct { get; set; }
        public abstract class wHTTaxPct : PX.Data.BQL.BqlDecimal.Field<wHTTaxPct> { }
        #endregion

        #region SecNHIPct
        [PXDBDecimal()]
        [PXUIField(DisplayName = "2GNHI %")]
        [PXDefault(typeof(Search<TWNGUIPreferences.secGenerationNHIPct>), PersistingCheck = PXPersistingCheck.Nothing)]
        public virtual decimal? SecNHIPct { get; set; }
        public abstract class secNHIPct : PX.Data.BQL.BqlDecimal.Field<secNHIPct> { }
        #endregion

        #region SecNHICode
        [PXDBString(2, IsFixed = true, InputMask = "")]
        [PXUIField(DisplayName = "2GNHI code")]
        [PXDefault(typeof(Search<CSAnswers.value, Where<CSAnswers.refNoteID, Equal<Current<Vendor.noteID>>,
                                                        And<CSAnswers.attributeID, Equal<ToGNHICodeAtt>>>>),
                   PersistingCheck = PXPersistingCheck.Nothing)]
        [GNHI2CodeSelector]
        public virtual string SecNHICode { get; set; }
        public abstract class secNHICode : PX.Data.BQL.BqlString.Field<secNHICode> { }
        #endregion
    
        #region CreatedByID
        [PXDBCreatedByID()]
        public virtual Guid? CreatedByID { get; set; }
        public abstract class createdByID : PX.Data.BQL.BqlGuid.Field<createdByID> { }
        #endregion
    
        #region CreatedByScreenID
        [PXDBCreatedByScreenID()]
        public virtual string CreatedByScreenID { get; set; }
        public abstract class createdByScreenID : PX.Data.BQL.BqlString.Field<createdByScreenID> { }
        #endregion
    
        #region CreatedDateTime
        [PXDBCreatedDateTime()]
        public virtual DateTime? CreatedDateTime { get; set; }
        public abstract class createdDateTime : PX.Data.BQL.BqlDateTime.Field<createdDateTime> { }
        #endregion
    
        #region LastModifiedByID
        [PXDBLastModifiedByID()]
        public virtual Guid? LastModifiedByID { get; set; }
        public abstract class lastModifiedByID : PX.Data.BQL.BqlGuid.Field<lastModifiedByID> { }
        #endregion
    
        #region LastModifiedByScreenID
        [PXDBLastModifiedByScreenID()]
        public virtual string LastModifiedByScreenID { get; set; }
        public abstract class lastModifiedByScreenID : PX.Data.BQL.BqlString.Field<lastModifiedByScreenID> { }
        #endregion
    
        #region LastModifiedDateTime
        [PXDBLastModifiedDateTime()]
        public virtual DateTime? LastModifiedDateTime { get; set; }
        public abstract class lastModifiedDateTime : PX.Data.BQL.BqlDateTime.Field<lastModifiedDateTime> { }
        #endregion
    
        #region NoteID
        [PXNote()]
        public virtual Guid? NoteID { get; set; }
        public abstract class noteID : PX.Data.BQL.BqlGuid.Field<noteID> { }
        #endregion
    
        #region Tstamp
        [PXDBTimestamp()]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion
    }

    #region Custom Attributes
    public class TypeOfInSelectorAttribute : PXSelectorAttribute
    {
        public TypeOfInSelectorAttribute() : base(typeof(Search<CSAttributeDetail.valueID,
                                                                  Where<CSAttributeDetail.attributeID, Equal<TWNWHT.TypeOfInAtt>>>),
                                                  typeof(CSAttributeDetail.description))
        {
            Filterable = true;
            DirtyRead = true;
            DescriptionField = typeof(CSAttributeDetail.description);
        }
    }

    public class WHTFmtCodeSelectorAttribute : PXSelectorAttribute
    {
        public WHTFmtCodeSelectorAttribute() : base(typeof(Search<CSAttributeDetail.valueID,
                                                                  Where<CSAttributeDetail.attributeID, Equal<TWNWHT.WHTFmtCodeAtt>>>),
                                                    typeof(CSAttributeDetail.description))
        {
            Filterable = true;
            DirtyRead = true;
            DescriptionField = typeof(CSAttributeDetail.description);
        }
    }

    public class WHTFmtSubSelectorAttribute : PXSelectorAttribute
    {
        public WHTFmtSubSelectorAttribute() : base(typeof(Search<CSAttributeDetail.valueID,
                                                                 Where<CSAttributeDetail.attributeID, Equal<TWNWHT.WHTFmtSubAtt>>>),
                                                   typeof(CSAttributeDetail.description))
        {
            Filterable = true;
            DirtyRead = true;
            DescriptionField = typeof(CSAttributeDetail.description);
        }
    }

    public class GNHI2CodeSelectorAttribute : PXSelectorAttribute
    {
        public GNHI2CodeSelectorAttribute() : base(typeof(Search<CSAttributeDetail.valueID,
                                                                 Where<CSAttributeDetail.attributeID, Equal<TWNWHT.ToGNHICodeAtt>>>),
                                                  typeof(CSAttributeDetail.description))
        {
            Filterable = true;
            DirtyRead = true;
            DescriptionField = typeof(CSAttributeDetail.description);
        }
    }
    #endregion
}