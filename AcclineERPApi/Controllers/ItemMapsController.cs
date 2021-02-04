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
    public class ItemMapsController : ApiController
    {
        private ASPLEntities db = new ASPLEntities();

        // GET: api/ItemMaps
        public IQueryable<ItemMap> GetItemMaps()
        {
            return db.ItemMaps;
        }

        // GET: api/ItemMaps/5
        [ResponseType(typeof(ItemMap))]
        public ItemMap GetItemMap(string ItemCode)
        {
            ItemMap itemMap = db.ItemMaps.Where(s => s.ItemCode == ItemCode).FirstOrDefault();
            if (itemMap == null)
            {
                return null;
            }
            return itemMap;
        }

        // PUT: api/ItemMaps/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutItemMap(int Id, ItemMap itemMap)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            if (Id != itemMap.Id)
            {
                return BadRequest();
            }           

            try
            {
                ItemMap im = db.ItemMaps.FirstOrDefault(s => s.Id == Id);
                db.ItemMaps.Remove(im);
                await db.SaveChangesAsync();

                //db.Entry(itemMap).State = EntityState.Modified;

                db.ItemMaps.Add(itemMap);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemMapExists(Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.OK);
        }

        // POST: api/ItemMaps
        [ResponseType(typeof(ItemMap))]
        public async Task<IHttpActionResult> PostItemMap(ItemMap itemMap)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            db.ItemMaps.Add(itemMap);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ItemMapExists(itemMap.ItemTypeCode))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return Ok();
            //return CreatedAtRoute("DefaultApi", new { id = itemMap.ItemTypeCode }, itemMap);
        }

        // DELETE: api/ItemMaps/5
        [ResponseType(typeof(ItemMap))]
        public async Task<IHttpActionResult> DeleteItemMap(int id)
        {
            ItemMap itemMap = await db.ItemMaps.FindAsync(id);
            if (itemMap == null)
            {
                return NotFound();
            }

            db.ItemMaps.Remove(itemMap);
            await db.SaveChangesAsync();

            return Ok(itemMap);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ItemMapExists(int id)
        {
            return db.ItemMaps.Count(e => e.ItemTypeCode == id) > 0;
        }
    }
}