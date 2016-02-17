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
    public class UnitController : Controller
    {
        private sis_dbEntities db = new sis_dbEntities();

        public ActionResult Index(string sortOrder = null, string currentFilter = null, string search = null, int? page = 1, string searchBy = null)
        {
            var units = from d in db.Units
                               select d;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortName = String.IsNullOrEmpty(sortOrder) ? "name" : "";

            ViewBag.count = db.Product_Info.Count();
            ViewBag.unit = db.Units.ToList().Count();
            ViewBag.category = db.Categories.ToList().Count();
            ViewBag.supplier = db.Supplier_Info.ToList().Count();

            
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
                units = units.OrderByDescending(x => x.active_flag);
            }

            //searching of an item
            if (!String.IsNullOrEmpty(search))
            {
                units = units.Where(x => x.unit_code.Contains(search) || x.unit_name.Contains(search));
            }
            ViewBag.CurrentFilter = search;

            //sorting of the columns
            switch (sortOrder)
            {
                case "name":
                    units = units.OrderBy(x => x.unit_name);
                    break;
                default:
                    units = units.OrderByDescending(x => x.active_flag);
                    break;
            }

            //get only 20 of the list from the database
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var returnMe = units.ToPagedList(pageNumber, pageSize);
            return View(returnMe);

            //return View(product_info.ToList());
        }

        //
        // POST: /Unit/Create

        [HttpGet]
        public ActionResult Create(Unit unit)
        {
            unit.created_by = "Jparagados";
            unit.created_date = System.DateTime.Now;
            unit.active_flag = true;
            db.Units.Add(unit);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //
        // GET: /Unit/Edit/5
        [HttpGet]
        public ActionResult Edit(int id = 0)
        {
            Unit unit = db.Units.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return PartialView(unit);
        }

        //
        // POST: /Unit/Edit/5

        [HttpGet]
        public ActionResult EditData(Unit unit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unit).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(unit);
        }

        //
        // GET: /Unit/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Unit unit = db.Units.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return PartialView(unit);
        }

        //
        // POST: /Unit/Delete/5

        [HttpGet]
        public ActionResult DeleteUnit(int id)
        {
            Unit unit = db.Units.Find(id);
            unit.active_flag = false;
            db.Entry(unit).State = EntityState.Modified;
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