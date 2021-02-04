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

namespace AcclineERPApi.Controllers
{
    public class tmpAddedItemsController : ApiController
    {
        private ASPLEntities db = new ASPLEntities();

        // GET: api/tmpAddedItems
        public IQueryable<tmpAddedItem> GettmpAddedItems()
        {
            return db.tmpAddedItems;
        }

        // GET: api/tmpAddedItems/5
        [ResponseType(typeof(tmpAddedItem))]
        [Authorize]
        public async Task<IHttpActionResult> GettmpAddedItem(long id)
        {
            tmpAddedItem tmpAddedItem = await db.tmpAddedItems.FindAsync(id);
            if (tmpAddedItem == null)
            {
                return NotFound();
            }

            return Ok(tmpAddedItem);
        }



        // GET: api/tmpAddedItems/5
        [ResponseType(typeof(tmpAddedItem))]
        [Authorize]
        public List<tmpAddedItem> GettmpAddedItem(string SaleNo)
        {
            List<tmpAddedItem> itemList = new List<tmpAddedItem>();
            var tmpAddedItem = db.tmpAddedItems.Where(x => x.SaleNo == SaleNo).ToList();
            foreach (var item in tmpAddedItem)
            {
                tmpAddedItem tai = new tmpAddedItem();
                tai.ID = item.ID;
                tai.SaleNo = item.SaleNo;
                tai.ItemCode = item.ItemCode;
                tai.LotNo = item.LotNo;
                tai.ItemName = item.ItemName;
                tai.Description = item.Description;
                tai.Qty = item.Qty;
                tai.ExtQty = item.ExtQty;
                tai.ExtUPrice = item.ExtUPrice;
                tai.UnitPrice = item.UnitPrice;
                tai.Vat = item.Vat;
                tai.Discount = item.Discount;
                tai.Amount = item.Amount;
                tai.DiscPerc = item.DiscPerc;
                tai.VatPerc = item.VatPerc;
                itemList.Add(tai);                
            }
            return itemList;
        }

        // PUT: api/tmpAddedItems/5
        [ResponseType(typeof(void))]
        //[Authorize]
        public async Task<IHttpActionResult> PuttmpAddedItem(long id, tmpAddedItem tmpAddedItem)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if (id != tmpAddedItem.ID)
            {
                return BadRequest();
            }

            db.Entry(tmpAddedItem).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tmpAddedItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/tmpAddedItems
        [Authorize]
        [ResponseType(typeof(tmpAddedItem))]
        public async Task<IHttpActionResult> PosttmpAddedItem(tmpAddedItem tmpAddedItem)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            db.tmpAddedItems.Add(tmpAddedItem);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tmpAddedItem.ID }, tmpAddedItem);
        }


        // DELETE: api/tmpAddedItems/5
        [ResponseType(typeof(tmpAddedItem))]
        [Authorize]
        public async Task<IHttpActionResult> DeletetmpAddedItem(long id)
        {
            tmpAddedItem tmpAddedItem = await db.tmpAddedItems.FindAsync(id);
            if (tmpAddedItem == null)
            {
                return NotFound();
            }

            db.tmpAddedItems.Remove(tmpAddedItem);
            await db.SaveChangesAsync();

            return Ok(tmpAddedItem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tmpAddedItemExists(long id)
        {
            return db.tmpAddedItems.Count(e => e.ID == id) > 0;
        }
    }
}