﻿using APILibrary;
using PX.Data;
using PX.Objects.AR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PX.Data.PXAccess;

namespace ExternalLogisticsAPI.Graph_Extensions
{
    public class CustomerMaint_Extension : PXGraphExtension<CustomerMaint>
    {
        #region Override
        public override void Initialize()
        {
            base.Initialize();
            var curCoutry = (PXSelect<Branch>.Select(Base, PX.Data.Update.PXInstanceHelper.CurrentCompany)).TopFirst;
            if (curCoutry?.BranchCD.Trim() == "IPEVOUK" || curCoutry?.BranchCD.Trim() == "IPEVONL")
            {
                this.lumValidEUVAT.SetVisible(true);
                Base.action.AddMenuAction(lumValidEUVAT);
            }
        }
        #endregion

        #region Delegate
        public delegate void PersistDelegate();

        [PXOverride]
        public void Persist(PersistDelegate baseMethod)
        {
            var curCoutry = (PXSelect<Branch>.Select(Base, PX.Data.Update.PXInstanceHelper.CurrentCompany)).TopFirst;
            var locationRow = Base.BaseLocations.Current;
            if (locationRow != null && curCoutry?.BranchCD.Trim() == "IPEVONL" && locationRow.CTaxZoneID == "OTB2B" && string.IsNullOrEmpty(locationRow?.TaxRegistrationID))
            {
                Base.BaseLocations.Cache.RaiseExceptionHandling<PX.Objects.CR.Standalone.Location.taxRegistrationID>(
                  locationRow,
                  locationRow.TaxRegistrationID,
                  new PXSetPropertyException<PX.Objects.CR.Standalone.Location.taxRegistrationID>("Tax Registration ID not validated", PXErrorLevel.Error));
            }
            else if (locationRow != null && curCoutry?.BranchCD.Trim() == "IPEVOUK" && locationRow.CTaxZoneID == "TAXEXEMPT" && string.IsNullOrEmpty(locationRow?.TaxRegistrationID))
            {
                Base.BaseLocations.Cache.RaiseExceptionHandling<PX.Objects.CR.Standalone.Location.taxRegistrationID>(
                 locationRow,
                 locationRow.TaxRegistrationID,
                 new PXSetPropertyException<PX.Objects.CR.Standalone.Location.taxRegistrationID>("Tax Registration ID not validated", PXErrorLevel.Error));
            }
            baseMethod();
        }
        #endregion

        #region Action
        public PXAction<Customer> lumValidEUVAT;
        [PXButton]
        [PXUIField(DisplayName = "Validate Tax ID", Enabled = true, MapEnableRights = PXCacheRights.Select, Visible = false)]
        protected IEnumerable LumValidEUVAT(PXAdapter adapter)
        {
            var row = Base.BaseLocations.Current;
            if (row != null && !string.IsNullOrEmpty(row.TaxRegistrationID))
            {
                var vaildResult = new SOAPHelper().ValidEUVat(row.TaxRegistrationID);
                if (!vaildResult)
                {
                    Base.BaseLocations.Cache.RaiseExceptionHandling<PX.Objects.CR.Standalone.Location.taxRegistrationID>(
                    row,
                    row.TaxRegistrationID,
                    new PXSetPropertyException<PX.Objects.CR.Standalone.Location.taxRegistrationID>("Tax Registration ID not validated", PXErrorLevel.Warning));
                    Base.BaseLocations.Cache.SetValue<PX.Objects.CR.Standalone.LocationExt.usrVATIsValid>(row, false);
                }
                else
                    Base.BaseLocations.Cache.SetValue<PX.Objects.CR.Standalone.LocationExt.usrVATIsValid>(row, true);
            }
            else if (row != null && string.IsNullOrEmpty(row.TaxRegistrationID))
                Base.BaseLocations.Cache.RaiseExceptionHandling<PX.Objects.CR.Standalone.Location.taxRegistrationID>(
                    row,
                    row.TaxRegistrationID,
                    new PXSetPropertyException<PX.Objects.CR.Standalone.Location.taxRegistrationID>("Tax Registration ID can not be empty", PXErrorLevel.Warning));
            Base.BaseLocations.Cache.MarkUpdated(row);
            Base.Save.Press();
            return adapter.Get();
        }
        #endregion
    }
}
