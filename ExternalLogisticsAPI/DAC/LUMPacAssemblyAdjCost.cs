﻿using System;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Objects.IN;

namespace ExternalLogisticsAPI
{
    [Serializable]
    [PXCacheName("LUMPacAssemblyAdjCost")]
    public class LUMPacAssemblyAdjCost : IBqlTable
    {

        public static class FK
        {
            public class InventoryItem : PX.Objects.IN.InventoryItem.PK.ForeignKeyOf<LUMPacAssemblyAdjCost>.By<inventoryID> { }
            public class Site : INSite.PK.ForeignKeyOf<LUMPacAssemblyAdjCost>.By<siteid> { }
            public class ProductSite : INSite.PK.ForeignKeyOf<LUMPacAssemblyAdjCost>.By<productSiteid> { }
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

        #region FinPtdCostAssemblyOut
        [PXDBDecimal(IsKey = true)]
        [PXUIField(DisplayName = "Fin Ptd Cost Assembly Out")]
        public virtual Decimal? FinPtdCostAssemblyOut { get; set; }
        public abstract class finPtdCostAssemblyOut : PX.Data.BQL.BqlDecimal.Field<finPtdCostAssemblyOut> { }
        #endregion

        #region FinPtdQtyAssemblyOut
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Fin Ptd Qty Assembly Out")]
        public virtual Decimal? FinPtdQtyAssemblyOut { get; set; }
        public abstract class finPtdQtyAssemblyOut : PX.Data.BQL.BqlDecimal.Field<finPtdQtyAssemblyOut> { }
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

        #region KitRefNbr
        [PXDBString(15, IsUnicode = true, IsKey = true)]
        [PXUIField(DisplayName = "Kit Ref Nbr.")]
        public virtual string KitRefNbr { get; set; }
        public abstract class kitRefNbr : PX.Data.BQL.BqlString.Field<kitRefNbr> { }
        #endregion

        #region Siteid
        [PX.Objects.IN.Site(DisplayName = "Warehouse ID", DescriptionField = typeof(INSite.descr), IsKey = true)]
        [PXUIField(DisplayName = "Warehouse")]
        [PXForeignReference(typeof(FK.Site))]
        public virtual int? Siteid { get; set; }
        public abstract class siteid : PX.Data.BQL.BqlInt.Field<siteid> { }
        #endregion

        #region AssembleAdjAmountJ
        [PXDBDecimal()]
        [PXUIField(DisplayName = "Assembly Adj Amount")]
        public virtual Decimal? AssemblyAdjAmount { get; set; }
        public abstract class assemblyAdjAmount : PX.Data.BQL.BqlDecimal.Field<assemblyAdjAmount> { }
        #endregion

        #region InventoryID
        [StockItem(IsKey = true)]
        [PXUIField(DisplayName = "Products Stock ID")]
        public virtual int? ProductInventoryID { get; set; }
        public abstract class productInventoryID : PX.Data.BQL.BqlInt.Field<productInventoryID> { }
        #endregion

        #region AssembleAdjAmountJ
        [PXDBDecimal()]
        [PXUIField(DisplayName = "PAC Cost ADJ - Products")]
        public virtual Decimal? ProductAdjAmount { get; set; }
        public abstract class productAdjAmount : PX.Data.BQL.BqlDecimal.Field<productAdjAmount> { }
        #endregion

        #region Siteid
        [PX.Objects.IN.Site(DisplayName = "Product Warehouse", DescriptionField = typeof(INSite.descr))]
        [PXUIField(DisplayName = "Product Warehouse")]
        [PXForeignReference(typeof(FK.ProductSite))]
        public virtual int? ProductSiteid { get; set; }
        public abstract class productSiteid : PX.Data.BQL.BqlInt.Field<productSiteid> { }
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