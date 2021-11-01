using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.CS;
using eGUICustomizations.DAC;
using eGUICustomizations.Descriptor;

namespace PX.Objects.AP
{
    public class APInvoiceEntry_Extension : PXGraphExtension<APInvoiceEntry>
    {
        #region Selects        
        [PXCopyPasteHiddenFields(typeof(TWNManualGUIAPBill.docType),
                                 typeof(TWNManualGUIAPBill.refNbr),
                                 typeof(TWNManualGUIAPBill.gUINbr))]
        public SelectFrom<TWNManualGUIAPBill>.Where<TWNManualGUIAPBill.docType.IsEqual<APInvoice.docType.FromCurrent>
                                                    .And<TWNManualGUIAPBill.refNbr.IsEqual<APInvoice.refNbr.FromCurrent>>>.View ManualAPBill;

        public SelectFrom<TWNWHT>.Where<TWNWHT.docType.IsEqual<APInvoice.docType.FromCurrent>
                                        .And<TWNWHT.refNbr.IsEqual<APInvoice.refNbr.FromCurrent>>>.View WHTView;

        public SelectFrom<TWNGUIPreferences>.View GUISetup;
        #endregion

        #region Delegate Methods
        public delegate void PersistDelegate();
        [PXOverride]
        public void Persist(PersistDelegate baseMethod)
        {
            var invoice = Base.CurrentDocument.Current;

            if (invoice != null && string.IsNullOrEmpty(invoice.InvoiceNbr))
            {
                Base.CurrentDocument.Cache.SetValue<APInvoice.invoiceNbr>(invoice, ManualAPBill.Select().TopFirst?.GUINbr);
                Base.CurrentDocument.UpdateCurrent();
            }

            baseMethod();
        }
        #endregion

        #region Event Handlers
        public bool activateGUI = TWNGUIValidation.ActivateTWGUI(new PXGraph());

        protected void _(Events.RowPersisting<APInvoice> e, PXRowPersisting baseHandler)
        {
            baseHandler?.Invoke(e.Cache, e.Args);

            var row = e.Row as APInvoice;

            if (row != null && row.DocType.IsIn(APDocType.Invoice, APDocType.DebitAdj) && string.IsNullOrEmpty(row.OrigModule))
            {
                if (ManualAPBill.Select().Count == 0 && Base.Taxes.Select().Count > 0)
                {
                    throw new PXException(TWMessages.NoGUIWithTax);
                }
                else
                {
                    decimal? taxSum = 0;

                    foreach (TWNManualGUIAPBill line in ManualAPBill.Select())
                    {
                        tWNGUIValidation.CheckCorrespondingInv(Base, line.GUINbr, line.VATInCode);

                        if (tWNGUIValidation.errorOccurred == true)
                        {
                            e.Cache.RaiseExceptionHandling<TWNManualGUIAPBill.gUINbr>(e.Row, line.GUINbr, new PXSetPropertyException(tWNGUIValidation.errorMessage, PXErrorLevel.RowError));
                        }

                        taxSum += line.TaxAmt.Value;
                    }

                    if (taxSum != row.TaxTotal && e.Operation != PXDBOperation.Delete)
                    {
                        throw new PXException(TWMessages.ChkTotalGUIAmt);
                    }
                }
            }
        }

        protected void _(Events.RowSelected<APInvoice> e, PXRowSelected baseHandler)
        {
            baseHandler?.Invoke(e.Cache, e.Args);

            var row = e.Row as APInvoice;

            if (row != null)
            {
                ManualAPBill.Cache.AllowSelect = TWNGUIValidation.ActivateTWGUI(Base);
                ManualAPBill.Cache.AllowDelete = ManualAPBill.Cache.AllowInsert = ManualAPBill.Cache.AllowUpdate = row.Status.IsIn(APDocStatus.Hold, APDocStatus.Balanced);
            }

            WHTView.AllowSelect = GUISetup.Select().TopFirst?.EnableWHT == true;

            //PXUIFieldAttribute.SetVisible<APRegisterExt.usrDeduction>(e.Cache, null, activateGUI);
            //PXUIFieldAttribute.SetVisible<APRegisterExt.usrGUIDate>(e.Cache, null, activateGUI);
            //PXUIFieldAttribute.SetVisible<APRegisterExt.usrGUINbr>(e.Cache, null, activateGUI);
            //PXUIFieldAttribute.SetVisible<APRegisterExt.usrOurTaxNbr>(e.Cache, null, activateGUI);
            //PXUIFieldAttribute.SetVisible<APRegisterExt.usrTaxNbr>(e.Cache, null, activateGUI);
            //PXUIFieldAttribute.SetVisible<APRegisterExt.usrVATInCode>(e.Cache, null, activateGUI);
        }

        protected void _(Events.RowInserting<APInvoice> e)
        {
            if (e.Row == null || Base.vendor.Current == null || activateGUI.Equals(false)) { return; }

            APRegisterExt regisExt = PXCache<APRegister>.GetExtension<APRegisterExt>(e.Row);

            string vATIncode = regisExt.UsrVATInCode ?? string.Empty;

            if (string.IsNullOrEmpty(vATIncode))
            {
                CSAnswers cSAnswers = SelectCSAnswers(Base, Base.vendor.Current.NoteID);

                vATIncode = (e.Row.IsRetainageDocument == true || cSAnswers == null) ? null : cSAnswers.Value;
            }

            regisExt.UsrVATInCode = e.Row.DocType.Equals(APDocType.DebitAdj, System.StringComparison.CurrentCulture) && 
                                    !string.IsNullOrEmpty(vATIncode) ? TWGUIFormatCode.vATInCode23 /*(int.Parse(vATIncode) + 2).ToString()*/ : vATIncode;
        }

        protected void _(Events.FieldUpdated<APInvoice.vendorID> e)
        {
            var row    = e.Row as APInvoice;
            var vendor = Base.vendor.Current;

            if (vendor == null || activateGUI == false) { return; }

            switch (row.DocType)
            {
                case APDocType.DebitAdj:
                    PXCache<APRegister>.GetExtension<APRegisterExt>(row).UsrVATInCode = TWGUIFormatCode.vATInCode23;
                    break;

                case APDocType.Invoice:
                    CSAnswers cSAnswers = SelectCSAnswers(Base, vendor.NoteID);

                    PXCache<APRegister>.GetExtension<APRegisterExt>(row).UsrVATInCode = cSAnswers?.Value;
                    break;
            }

            if (GUISetup.Select().TopFirst?.EnableWHT == true &&
                Base.Document.Current != null && Base.Document.Current.DocType.Equals(APDocType.Invoice))
            {
                TWNWHT wNWHT = new TWNWHT()
                {
                    DocType = Base.Document.Current.DocType
                };

                WHTView.Cache.SetDefaultExt<TWNWHT.personalID>(wNWHT);
                WHTView.Cache.SetDefaultExt<TWNWHT.propertyID>(wNWHT);
                WHTView.Cache.SetDefaultExt<TWNWHT.typeOfIn>(wNWHT);
                WHTView.Cache.SetDefaultExt<TWNWHT.wHTFmtCode>(wNWHT);
                WHTView.Cache.SetDefaultExt<TWNWHT.wHTFmtSub>(wNWHT);
                WHTView.Cache.SetDefaultExt<TWNWHT.wHTTaxPct>(wNWHT);
                WHTView.Cache.SetDefaultExt<TWNWHT.secNHICode>(wNWHT);

                WHTView.Cache.Insert(wNWHT);
            }
        }

        #region TWNManualGUIAPBill
        TWNGUIValidation tWNGUIValidation = new TWNGUIValidation();

        protected void _(Events.FieldDefaulting<TWNManualGUIAPBill.deduction> e)
        {
            var row = e.Row as TWNManualGUIAPBill;

            /// If user doesn't choose a vendor then bring the fixed default value from Attribure "DEDUCTCODE" first record.
            e.NewValue = row.VendorID == null ? new string1() : e.NewValue;
        }

        protected void _(Events.FieldDefaulting<TWNManualGUIAPBill.ourTaxNbr> e)
        {
            var row = e.Row as TWNManualGUIAPBill;

            TWNGUIPreferences preferences = SelectFrom<TWNGUIPreferences>.View.Select(Base);

            e.NewValue = row.VendorID == null ? preferences.OurTaxNbr : e.NewValue;
        }
        #endregion

        #endregion

        #region Static Method
        public static CSAnswers SelectCSAnswers(PXGraph graph, System.Guid? noteID)
        {
            return SelectFrom<CSAnswers>.Where<CSAnswers.refNoteID.IsEqual<@P.AsGuid>
                                               .And<CSAnswers.attributeID.IsEqual<APRegisterExt.VATINFRMTNameAtt>>>.View.Select(graph, noteID).TopFirst;
        }
        #endregion
    }
}