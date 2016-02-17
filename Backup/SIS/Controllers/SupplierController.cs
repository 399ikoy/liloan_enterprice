using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIS.Models;
using PagedList;

namespace SIS.Controllers
{
    public class SupplierController : Controller
    {
        private sis_dbEntities db = new sis_dbEntities();

        //
        // GET: /Supplier/
        public ActionResult Index(string sortOrder = null, string currentFilter = null, string search = null, int? page = 1, string searchBy = null)
        {
            var supplier = from d in db.Supplier_Info
                        select d;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortName = String.IsNullOrEmpty(sortOrder) ? "name" : "";

            var product_info = db.Product_Info.Include(p => p.Category).Include(p => p.Sub_Category).Include(p => p.Supplier_Info);
            ViewBag.count = product_info.Count();
            ViewBag.unit = db.Units.ToList().Count();
            ViewBag.category = db.Categories.ToList().Count();
            ViewBag.supplier = supplier.Count();


            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentFilter;
            }

            if (searchBy == "default")
            {
                supplier = supplier.OrderByDescending(x => x.active_flag);
            }

            //searching of an item
            if (!String.IsNullOrEmpty(search))
            {
                supplier = supplier.Where(x => x.supplier_name.Contains(search) || x.supplier_contact_info.Contains(search) || x.supplier_contact_person.Contains(search) || x.supplier_address.Contains(search) || x.company_name.Contains(search));
            }
            ViewBag.CurrentFilter = search;

            //sorting of the columns
            switch (sortOrder)
            {
                case "name":
                    supplier = supplier.OrderBy(x => x.supplier_name);
                    break;
                default:
                    supplier = supplier.OrderByDescending(x => x.active_flag);
                    break;
            }

            //get only 20 of the list from the database
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var returnMe = supplier.ToPagedList(pageNumber, pageSize);
            return View(returnMe);

            //return View(product_info.ToList());
        }

        //
        // GET: /Supplier/Details/5

        public ActionResult Details(int id = 0)
        {
            Supplier_Info supplier_info = db.Supplier_Info.Find(id);
            if (supplier_info == null)
            {
                return HttpNotFound();
            }
            return PartialView(supplier_info);
        }

        //
        // POST: /Supplier/Create

        [HttpGet]
        public ActionResult Create(Supplier_Info supplier_info)
        {
            if (ModelState.IsValid)
            {
                supplier_info.active_flag = true;
                db.Supplier_Info.Add(supplier_info);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(supplier_info);
        }

        //
        // GET: /Supplier/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Supplier_Info supplier_info = db.Supplier_Info.Find(id);
            if (supplier_info == null)
            {
                return HttpNotFound();
            }
            return PartialView(supplier_info);
        }

        //
        // POST: /Supplier/Edit/5

        [HttpPost]
        public ActionResult Edit(Supplier_Info supplier_info)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplier_info).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supplier_info);
        }

        //
        // GET: /Supplier/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Supplier_Info supplier_info = db.Supplier_Info.Find(id);
            if (supplier_info == null)
            {
                return HttpNotFound();
            }
            return PartialView(supplier_info);
        }

        //
        // POST: /Supplier/Delete/5

        [HttpGet]
        public ActionResult DeleteInfo(int id)
        {
            Supplier_Info supplier_info = db.Supplier_Info.Find(id);
            db.Entry(supplier_info).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}