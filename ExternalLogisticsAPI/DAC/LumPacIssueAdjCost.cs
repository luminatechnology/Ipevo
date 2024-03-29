﻿using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.IN;

namespace ExternalLogisticsAPI
{
    [Serializable]
    [PXCacheName("LumPacIssueAdjCost")]
    public class LumPacIssueAdjCost : IBqlTable
    {
        public static class FK
        {
            public class InventoryItem : PX.Objects.IN.InventoryItem.PK.ForeignKeyOf<LumPacIssueAdjCost>.By<inventoryID> { }
            public class Site : INSite.PK.ForeignKeyOf<LumPacIssueAdjCost>.By<siteid> { }
        }

        #region Selected
        [PXBool()]
        [PXUIField(DisplayName = "Selected")]
        public virtual bool? Selected { get; set; }
        public abstract class selected : PX.Data.BQL.BqlBool.Field<selected> { }
        #endregion

        #region FinPeriodID
        [PXDBString(6, IsUnicode = true, InputMask = "",IsKey = true)]
        [PXUIField(DisplayName = "Fin Period ID")]
        public virtual string FinPeriodID { get; set; }
        public abstract class finPeriodID : PX.Data.BQL.BqlString.Field<finPeriodID> { }
        #endregion

        #region RefNbr
        [PXDBString(15, IsKey = true)]
        [PXUIField(DisplayName = "RefNbr")]
        public virtual string RefNbr { get; set; }
        public abstract class refNbr : PX.Data.BQL.BqlString.Field<refNbr> { }
        #endregion

        #region LineNbr
        [PXDBInt(IsKey = true)]
        [PXUIField(DisplayName = "Fin Period ID")]
        public virtual int? LineNbr { get; set; }
        public abstract class lineNbr : PX.Data.BQL.BqlInt.Field<lineNbr> { }
        #endregion

        #region FinPtdCostIssued
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Fin Ptd Cost Issued")]
        public virtual Decimal? FinPtdCostIssued { get; set; }
        public abstract class finPtdCostIssued : PX.Data.BQL.BqlDecimal.Field<finPtdCostIssued> { }
        #endregion

        #region FinPtdQtyIssued
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Fin Ptd Qty Issued")]
        public virtual Decimal? FinPtdQtyIssued { get; set; }
        public abstract class finPtdQtyIssued : PX.Data.BQL.BqlDecimal.Field<finPtdQtyIssued> { }
        #endregion

        #region PACUnitCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "PACUnit Cost")]
        public virtual Decimal? PACUnitCost { get; set; }
        public abstract class pACUnitCost : PX.Data.BQL.BqlDecimal.Field<pACUnitCost> { }
        #endregion

        #region InventoryID
        [StockItem(IsKey = true)]
        [PXUIField(DisplayName = "Inventory ID")]
        public virtual int? InventoryID { get; set; }
        public abstract class inventoryID : PX.Data.BQL.BqlInt.Field<inventoryID> { }
        #endregion

        #region ItemClassID
        [PXDBInt()]
        [PXUIField(DisplayName = "Item Class")]
        [PXDimensionSelector(INItemClass.Dimension, typeof(Search<INItemClass.itemClassID>), typeof(INItemClass.itemClassCD), DescriptionField = typeof(INItemClass.descr), CacheGlobal = true)]
        public virtual int? ItemClassID { get; set; }
        public abstract class itemClassID : PX.Data.BQL.BqlInt.Field<itemClassID> { }
        #endregion

        #region PACIssueCost
        [PXDBDecimal()]
        [PXUIField(DisplayName = "PACIssue Cost")]
        public virtual Decimal? PACIssueCost { get; set; }
        public abstract class pACIssueCost : PX.Data.BQL.BqlDecimal.Field<pACIssueCost> { }
        #endregion

        #region Siteid
        [PX.Objects.IN.Site(DisplayName = "Warehouse ID", DescriptionField = typeof(INSite.descr), IsKey = true)]
        [PXUIField(DisplayName = "Warehouse")]
        [PXForeignReference(typeof(FK.Site))]
        public virtual int? Siteid { get; set; }
        public abstract class siteid : PX.Data.BQL.BqlInt.Field<siteid> { }
        #endregion

        #region IssueAdjAmount
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Issue Adj Amount")]
        public virtual Decimal? IssueAdjAmount { get; set; }
        public abstract class issueAdjAmount : PX.Data.BQL.BqlDecimal.Field<issueAdjAmount> { }
        #endregion

        #region ReasonCode
        [PXDBString(PX.Objects.CS.ReasonCode.reasonCodeID.Length, IsUnicode = true)]
        [PXUIField(DisplayName = "Reason Code")]
        public virtual String ReasonCode { get; set; }
        public abstract class reasonCode : PX.Data.BQL.BqlString.Field<reasonCode> { }
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

        #region Tstamp
        [PXDBTimestamp()]
        [PXUIField(DisplayName = "Tstamp")]
        public virtual byte[] Tstamp { get; set; }
        public abstract class tstamp : PX.Data.BQL.BqlByteArray.Field<tstamp> { }
        #endregion
    }
}