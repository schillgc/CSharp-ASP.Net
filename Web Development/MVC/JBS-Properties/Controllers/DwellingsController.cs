using John_Baxter_Schilling.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace John_Baxter_Schilling.Controllers
{
    public class DwellingsController : Controller
    {
        private Property db = new Property();

        // GET: Dwellings
        public ActionResult Index()
        {
            return View(db.Dwellings.ToList());
        }

        // GET: Dwellings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dwelling dwelling = db.Dwellings.Find(id);
            if (dwelling == null)
            {
                return HttpNotFound();
            }
            return View(dwelling);
        }

        // GET: Dwellings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dwellings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PropertyAddressNumber,Availability,CurrentlyRented,PropertyStreetName,PropertyCity,PropertyPostalCode,PropertyPurchaseAmount,PurchaseDate,NamesOnLease,PhoneNumberForTennet,PrimaryTennetEmailAddress,LeaseRenewalDate")] Dwelling dwelling)
        {
            if (ModelState.IsValid)
            {
                db.Dwellings.Add(dwelling);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dwelling);
        }

        // GET: Dwellings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dwelling dwelling = db.Dwellings.Find(id);
            if (dwelling == null)
            {
                return HttpNotFound();
            }
            return View(dwelling);
        }

        // POST: Dwellings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PropertyAddressNumber,Availability,CurrentlyRented,PropertyStreetName,PropertyCity,PropertyPostalCode,PropertyPurchaseAmount,PurchaseDate,NamesOnLease,PhoneNumberForTennet,PrimaryTennetEmailAddress,LeaseRenewalDate")] Dwelling dwelling)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dwelling).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dwelling);
        }

        // GET: Dwellings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dwelling dwelling = db.Dwellings.Find(id);
            if (dwelling == null)
            {
                return HttpNotFound();
            }
            return View(dwelling);
        }

        // POST: Dwellings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dwelling dwelling = db.Dwellings.Find(id);
            db.Dwellings.Remove(dwelling);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}