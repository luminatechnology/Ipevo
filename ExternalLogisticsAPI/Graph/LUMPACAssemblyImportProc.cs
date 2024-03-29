﻿using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.IN;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalLogisticsAPI.Graph
{
    public class LUMPACAssemblyImportProc : PXGraph<LUMPACAssemblyImportProc>
    {
        public PXCancel<PACFilter> Cancel;
        public PXFilter<PACFilter> Filter;

        public LUMPACAssemblyImportProc()
        {
            this.ImportPACList.SetProcessVisible(false);
            this.ImportPACList.SetProcessAllVisible(false);
        }

        [PXFilterable]
        public PXFilteredProcessing<LUMPacAssemblyAdjCost,
                                    PACFilter,
                                    Where<LUMPacAssemblyAdjCost.finPeriodID, Equal<Current<PACFilter.finPeriod>>>> ImportPACList;

        public PXAction<PACFilter> loadData;
        [PXButton]
        [PXUIField(DisplayName = "Load Data", Enabled = true, MapEnableRights = PXCacheRights.Select)]
        protected virtual IEnumerable LoadData(PXAdapter adapter)
        {
            var filter = this.Filter.Current;
            var pacData = SelectFrom<vPACUnitCost>.View.Select(new PXGraph()).RowCast<vPACUnitCost>().ToList();
            var inComponentTranData = SelectFrom<INComponentTran>
                                      .Where<INComponentTran.finPeriodID.IsEqual<P.AsString>>.View.Select(new PXGraph(), filter.FinPeriod).RowCast<INComponentTran>().ToList();
            var inKitRegisterData = SelectFrom<INKitRegister>.Where<INKitRegister.docType.IsEqual<P.AsString>>.View.Select(new PXGraph(),"P").RowCast<INKitRegister>().ToList();
            var inventoryItemData = SelectFrom<InventoryItem>.View.Select(new PXGraph()).RowCast<InventoryItem>().ToList();
            var result = from kit in inComponentTranData 
                         join t in pacData on new { A = kit.InventoryID, B = kit.FinPeriodID } equals new { A = t.InventoryID, B = t.FinPeriodID }
                         join item in inventoryItemData on kit.InventoryID equals item.InventoryID
                         join kitItem in inKitRegisterData on new { A = kit.DocType, B = kit.RefNbr } equals new { A = kitItem.DocType, B = kitItem.RefNbr }
                         where item.ItemClassID == filter.ItemClassID
                         select new 
                         { 
                             PACUnitCost = t.PACUnitCost,
                             FinPeriodID = t.FinPeriodID,
                             TranCost = kit.TranCost,
                             Qty = kit.Qty,
                             InventoryID = t.InventoryID,
                             ItemClassID = item.ItemClassID,
                             SiteID = kit.SiteID,
                             KitInventoryID = kitItem.KitInventoryID,
                             ProductSiteid = kitItem.SiteID,
                             RefNbr = kitItem.RefNbr
                         };

            // Delete temp table data
            PXDatabase.Delete<LUMPacAssemblyAdjCost>();
            this.ImportPACList.Cache.Clear();

            foreach (var row in result.Distinct().ToList())
            {
                var data = this.ImportPACList.Insert((LUMPacAssemblyAdjCost)this.ImportPACList.Cache.CreateInstance());
                data.FinPeriodID = row.FinPeriodID;
                data.FinPtdCostAssemblyOut = row.TranCost;
                data.FinPtdQtyAssemblyOut = row.Qty;
                data.PACUnitCost = row.PACUnitCost;
                data.InventoryID = row.InventoryID;
                data.ItemClassID = row.ItemClassID;
                data.PACIssueCost = row.PACUnitCost * row.Qty;
                data.Siteid = row.SiteID;
                data.AssemblyAdjAmount = data.FinPtdCostAssemblyOut - data.PACIssueCost;
                data.ProductInventoryID = row.KitInventoryID;
                data.ProductAdjAmount = data.AssemblyAdjAmount * -1;
                data.ProductSiteid = row.ProductSiteid;
                data.KitRefNbr = row.RefNbr;
                data.Selected = true;
            }
            this.Actions.PressSave();
            return adapter.Get();
        }

        public PXAction<PACFilter> generateAdjustment;
        [PXButton]
        [PXUIField(DisplayName = "Generate Assembly Adjustment", Enabled = true, MapEnableRights = PXCacheRights.Select)]
        protected virtual IEnumerable GenerateAdjustment(PXAdapter adapter)
        {
            var graph = PXGraph.CreateInstance<INAdjustmentEntry>();
            try
            {
                var filter = this.Filter.Current;
                var impDatas = this.ImportPACList.Select().RowCast<LUMPacAssemblyAdjCost>().ToList();

                if (string.IsNullOrEmpty(filter.FinPeriod))
                    throw new PXException("Period can not be empty!!");
                if (!filter.ItemClassID.HasValue)
                    throw new PXException("ItemClass can not be empty!!");

                if (!impDatas.Any())
                    throw new PXException("No Data Found!!");

                // Create Adjustment
                decimal sum = 0;

                var doc = graph.adjustment.Insert((INRegister)graph.adjustment.Cache.CreateInstance());
                doc.FinPeriodID = filter.FinPeriod;
                doc.TranDesc = "PAC Assembly Adujstment";

                foreach (var row in impDatas.Where(x => x.Selected ?? true))
                {
                    // AssemblyAdjAmount
                    if (Math.Round((row.AssemblyAdjAmount ?? 0), 0) != 0)
                    {
                        var line = graph.transactions.Insert((INTran)graph.transactions.Cache.CreateInstance());
                        graph.transactions.SetValueExt<INTran.inventoryID>(line, row.InventoryID);
                        graph.transactions.SetValueExt<INTran.siteID>(line, row.Siteid);
                        graph.transactions.SetValueExt<INTran.tranCost>(line, row.AssemblyAdjAmount);
                        graph.transactions.SetValueExt<INTran.reasonCode>(line, "KITASSEMBLY");
                        graph.transactions.SetValueExt<INTran.lotSerialNbr>(line, string.Empty);
                        sum += (row.AssemblyAdjAmount ?? 0);
                    }
                    // ProductAdjAmount
                    if (Math.Round((row.ProductAdjAmount ?? 0), 0) != 0)
                    {
                        var line = graph.transactions.Insert((INTran)graph.transactions.Cache.CreateInstance());
                        graph.transactions.SetValueExt<INTran.inventoryID>(line, row.ProductInventoryID);
                        graph.transactions.SetValueExt<INTran.siteID>(line, row.ProductSiteid);
                        graph.transactions.SetValueExt<INTran.tranCost>(line, row.ProductAdjAmount);
                        graph.transactions.SetValueExt<INTran.reasonCode>(line, "PACADJ");
                        graph.transactions.SetValueExt<INTran.lotSerialNbr>(line, string.Empty);
                        sum += (row.ProductAdjAmount ?? 0);
                    }
                }
                doc.TotalCost = sum;
                graph.Save.Press();
                // Delete temp table data
                PXDatabase.Delete<LUMPacAssemblyAdjCost>();
                this.ImportPACList.Cache.Clear();
            }
            catch (Exception ex)
            {
                throw new PXException(ex.Message);
            }
            throw new PXRedirectRequiredException(graph, "");
        }

    }
}
