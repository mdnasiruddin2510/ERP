using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AcclineERPApi.Models;

namespace WebApi.Controllers
{
    public class SaleViewModelsController : ApiController
    {
        private ASPLEntities db = new ASPLEntities();

        // GET: api/SaleViewModels/5
        [ResponseType(typeof(void))]
        [Authorize]
        public async Task<IHttpActionResult> GettmpAddedItem(string SaleNo)
        {
            var SaleDetail = (from sm in db.SalesMains
                              join sd in db.SalesDetails on sm.SalesMainID equals sd.SalesMainID
                              where sm.SaleNo == SaleNo
                              select new
                              {
                                  SaleNo = sm.SaleNo,
                                  ItemCode = sd.ItemCode,
                                  LotNo = sd.LotNo,
                                  ItemName = (db.ItemInfoes.Where(s => s.ItemCode == sd.ItemCode)
                                                   .Select(c => c.ItemName)).FirstOrDefault(),
                                  Qty = sd.SaleQty,
                                  Description = sd.Description,
                                  UnitPrice = sd.UnitPrice,
                                  Vat = sd.VATAmt,
                                  Discount = sd.DiscAmt,
                                  Amount = sd.SaleQty * sd.UnitPrice,
                                  LocCode = sm.LocCode
                              }).ToList();

            SalesMain SalesMain = db.SalesMains.Where(x => x.SaleNo == SaleNo).FirstOrDefault();
            foreach (var item in SaleDetail)
            {
                if (!SaleDetailExists(item.ItemCode, item.SaleNo))
                {
                    tmpAddedItem tai = new tmpAddedItem();
                    tai.SaleNo = item.SaleNo;
                    tai.ItemCode = item.ItemCode;
                    tai.LotNo = item.LotNo;
                    tai.ItemName = item.ItemName;
                    tai.Description = item.Description;
                    tai.Qty = Convert.ToDecimal(item.Qty);
                    tai.ExtQty = Convert.ToDecimal(db.CurrentStocks.Where(s => s.ItemCode == item.ItemCode && s.LocCode == item.LocCode).Select(s => s.CurrQty).FirstOrDefault());
                    tai.ExtUPrice = Convert.ToDecimal(db.CurrentStocks.Where(s => s.ItemCode == item.ItemCode && s.LocCode == item.LocCode).Select(s => s.UnitPrice).FirstOrDefault());
                    tai.UnitPrice = Convert.ToDecimal(item.UnitPrice);
                    tai.Vat = item.Vat;
                    tai.Discount = item.Discount;
                    tai.Amount = Convert.ToDecimal(item.Amount);
                    db.tmpAddedItems.Add(tai);
                }
                else
                {
                    tmpAddedItem tmpAddedItem = db.tmpAddedItems.Where(s => s.ItemCode == item.ItemCode && s.SaleNo == item.SaleNo).FirstOrDefault();
                    db.tmpAddedItems.Remove(tmpAddedItem);
                    db.SaveChanges();

                    tmpAddedItem tai = new tmpAddedItem();
                    tai.SaleNo = item.SaleNo;
                    tai.ItemCode = item.ItemCode;
                    tai.LotNo = item.LotNo;
                    tai.ItemName = item.ItemName;
                    tai.Description = item.Description;
                    tai.Qty = Convert.ToDecimal(item.Qty);
                    tai.ExtQty = Convert.ToDecimal(db.CurrentStocks.Where(s => s.ItemCode == item.ItemCode && s.LocCode == item.LocCode).Select(s => s.CurrQty).FirstOrDefault());
                    tai.ExtUPrice = Convert.ToDecimal(db.CurrentStocks.Where(s => s.ItemCode == item.ItemCode && s.LocCode == item.LocCode).Select(s => s.UnitPrice).FirstOrDefault());
                    tai.UnitPrice = Convert.ToDecimal(item.UnitPrice);
                    tai.Vat = item.Vat;
                    tai.Discount = item.Discount;
                    tai.Amount = Convert.ToDecimal(item.Amount);
                    db.tmpAddedItems.Add(tai);
                }

            }
            try
            {
                await db.SaveChangesAsync();
                return Ok(SalesMain);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }
        }



        //PUT: api/SaleViewModels/5
        [ResponseType(typeof(void))]
        //[Authorize]
        public async Task<IHttpActionResult> PutSaleViewModel(SaleViewModel saleViewModel)
        {
            bool MaintLot = db.SysSets.Select(s => s.MaintLot).FirstOrDefault();
            db.Entry(saleViewModel.SalesMain).State = EntityState.Modified;
            if (saleViewModel.IssueMain != null)
            {
                db.Entry(saleViewModel.IssueMain).State = EntityState.Modified;
            }

            if (MaintLot)
            {
                #region For sales details
                var SalesDetailIDs = db.SalesDetails.Where(s => s.SalesMainID == saleViewModel.SalesMain.SalesMainID).Select(s => s.SalesDetailID);
                foreach (var SalesDetailID in SalesDetailIDs)
                {
                    SalesDetail SalesDetaildata = await db.SalesDetails.FindAsync(SalesDetailID);
                    db.SalesDetails.Remove(SalesDetaildata);
                }
                foreach (SalesDetail SalesDetail in saleViewModel.SalesDetail)
                {
                    db.SalesDetails.Add(SalesDetail);
                }
                #endregion sales

                #region For Issue details
                if (saleViewModel.IssueDetail != null)
                {
                    foreach (var item in saleViewModel.IssueDetail)
                    {
                        var IssuedQty = db.IssueDetails.Where(s => s.IssueNo == item.IssueNo && s.ItemCode == item.ItemCode).Select(s => s.Qty).FirstOrDefault();

                        var CurrentStocks = db.CurrentStocks.Where(p => p.ItemCode == item.ItemCode && p.LocCode == saleViewModel.IssueMain.FromLocCode).FirstOrDefault();
                        if (IssuedQty != 0 && CurrentStocks != null)
                        {
                            CurrentStocks.CurrQty = ((IssuedQty + CurrentStocks.CurrQty) - item.Qty);
                            db.Entry(CurrentStocks).State = EntityState.Modified;
                        }
                        else
                        {
                            CurrentStocks.CurrQty = (CurrentStocks.CurrQty - item.Qty);
                            db.Entry(CurrentStocks).State = EntityState.Modified;
                        }

                    }

                    var IssueDetails = db.IssueDetails.Where(s => s.IssueNo == saleViewModel.IssueMain.IssueNo).Select(s => s.Id);
                    foreach (var Id in IssueDetails)
                    {
                        IssueDetail IssueDetaildata = await db.IssueDetails.FindAsync(Id);
                        db.IssueDetails.Remove(IssueDetaildata);
                    }

                    foreach (IssueDetail IssueDetail in saleViewModel.IssueDetail)
                    {
                        db.IssueDetails.Add(IssueDetail);
                    }
                }
                #endregion
            }
            var taiIDs = db.tmpAddedItems.Where(x => x.SaleNo == saleViewModel.SalesMain.SaleNo).Select(x => x.ID).ToList();
            foreach (var rId in taiIDs)
            {
                db.tmpAddedItems.Remove(await db.tmpAddedItems.FindAsync(rId));
            }

            try
            {
                await db.SaveChangesAsync();
                return StatusCode(HttpStatusCode.OK);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw ex;
            }

        }





        // POST: api/SaleViewModels
        [ResponseType(typeof(SaleViewModel))]
        [Authorize]
        public async Task<IHttpActionResult> PostSaleViewModel(SaleViewModel saleViewModel)
        {
            using (var transaction = db.Database.BeginTransaction())
            {

                db.SalesMains.Add(saleViewModel.SalesMain);
                db.SaveChanges();
                foreach (SalesDetail item in saleViewModel.SalesDetail)
                {
                    var sMainId = db.SalesMains.Select(t => t.SalesMainID).DefaultIfEmpty().Max();
                    item.SalesMainID = sMainId;
                    db.SalesDetails.Add(item);
                }
                #region For Stock and Cost ledger
                if (saleViewModel.CurrentStock.Count() != 0)
                {
                    foreach (var item in saleViewModel.CurrentStock)
                    {
                        var CurrentStocks = db.CurrentStocks.Where(p => p.ItemCode == item.ItemCode && p.LocCode == item.LocCode).FirstOrDefault();
                        if (CurrentStocks != null)
                        {
                            CurrentStocks.CurrQty = item.CurrQty;
                            db.Entry(CurrentStocks).State = EntityState.Modified;
                        }
                        else
                        {
                            db.CurrentStocks.Add(item);
                        }
                    }
                }
                else if (saleViewModel.CostLedger.Count() != 0)
                {
                    foreach (var item in saleViewModel.CostLedger)
                    {
                        db.CostLedgers.Add(item);
                    }
                }
                #endregion

                #region For Issue
                if (saleViewModel.IssueMain != null)
                {
                    if (saleViewModel.IssueMain.RefDate == DateTime.MinValue)
                    {
                        saleViewModel.IssueMain.RefDate = null;
                    }
                    db.IssueMains.Add(saleViewModel.IssueMain);
                }
                if (saleViewModel.IssueDetail != null)
                {
                    foreach (IssueDetail item in saleViewModel.IssueDetail)
                    {
                        db.IssueDetails.Add(item);
                    }
                }
                #endregion

                var taiIDs = db.tmpAddedItems.Where(x => x.SaleNo == saleViewModel.SalesMain.tSaleNo).Select(x => x.ID).ToList();
                foreach (var rId in taiIDs)
                {
                    db.tmpAddedItems.Remove(await db.tmpAddedItems.FindAsync(rId));
                }
                try
                {
                    await db.SaveChangesAsync();
                    transaction.Commit();
                    return Ok();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    string s = ex.ToString();
                    throw new Exception(s);
                }
            }
        }

        // DELETE: api/SaleViewModels/5
        //[ResponseType(typeof(SaleViewModel))]
        //public async Task<IHttpActionResult> DeleteSaleViewModel(int id)
        //{
        //    SaleViewModel saleViewModel = await db.SaleViewModels.FindAsync(id);
        //    if (saleViewModel == null)
        //    {
        //        return NotFound();
        //    }

        //    db.SaleViewModels.Remove(saleViewModel);
        //    await db.SaveChangesAsync();

        //    return Ok(saleViewModel);
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SaleDetailExists(string itemCode, string SaleNo)
        {
            return db.tmpAddedItems.Count(e => e.ItemCode == itemCode && e.SaleNo == SaleNo) > 0;
        }


    }
}