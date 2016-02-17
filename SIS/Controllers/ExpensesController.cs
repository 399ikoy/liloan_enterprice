using System;
using System.Globalization;
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
    public class ExpensesController : Controller
    {
        private sis_dbEntities db = new sis_dbEntities();

        //
        // GET: /Expenses/

        public ActionResult Index(string sortOrder = null, string currentFilter = null, string search = null, int? page = 1, string searchBy = null)
        {
            var expenses = from d in db.Expenses
                               select d;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortType = String.IsNullOrEmpty(sortOrder) ? "type" : "";

            ViewBag.expenses_type = new SelectList(db.Expenses_Type_vw, "expenses_type", "expenses_type");
            var user = db.Staff_Info
                       .ToList()
                       .Where(s => s.active_flag == true)
                       .OrderBy(s => s.staff_lname)
                       .Select(s => new
                       {
                           staff_id = s.staff_id,
                           Description = string.Format("{0}, {1}", s.staff_lname, s.staff_fname)
                       })
                       .Distinct();
            ViewBag.staff_id = new SelectList(user, "staff_id", "Description");

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
                expenses = expenses.OrderByDescending(x => x.created_date);
            }

            //searching of an item
            if (!String.IsNullOrEmpty(search))
            {
                double num;
                if (double.TryParse(search, out num))
                {
                    long qty = Convert.ToInt64(search);
                    expenses = expenses.Where(x => x.expenses_amount == qty);
                }
                else
                {
                    expenses = expenses.Where(x => x.expenses_type.Contains(search) || x.remarks.Contains(search));
                }
            }
            ViewBag.CurrentFilter = search;

            //sorting of the columns
            switch (sortOrder)
            {
                case "type":
                    expenses = expenses.OrderByDescending(x => x.expenses_type);
                    break;
                default:
                    expenses = expenses.OrderByDescending(x => x.expenses_id);
                    break;
            }

            //get only 20 of the list from the database
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var returnMe = expenses.ToPagedList(pageNumber, pageSize);
            return View(returnMe);
        }

        //
        // POST: /Expenses/Create

        [HttpPost]
        public ActionResult Create(Expens expens, string expenses_type, string transaction_date, double expenses_amount, string remarks, int staff_id)
        {
            DateTime txtmyDate = DateTime.ParseExact(transaction_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            expens.expenses_type = expenses_type;
            expens.transaction_date = txtmyDate;
            expens.expenses_amount = expenses_amount;
            expens.remarks = remarks;
            expens.staff_id = staff_id;
            expens.created_by = 1;
            expens.created_date = System.DateTime.Now;
            expens.active_flag = true;
            db.Expenses.Add(expens);
            db.SaveChanges();
            

            ViewBag.staff_id = new SelectList(db.Staff_Info, "staff_id", "staff_fname", expens.staff_id);
            ViewBag.created_by = new SelectList(db.Staff_Info, "staff_id", "staff_fname", expens.created_by);
            return RedirectToAction("Index");
        }

        //
        // GET: /Expenses/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Expens expens = db.Expenses.Find(id);
            if (expens == null)
            {
                return HttpNotFound();
            }
            ViewBag.staff_id = new SelectList(db.Staff_Info, "staff_id", "staff_fname", expens.staff_id);
            ViewBag.created_by = new SelectList(db.Staff_Info, "staff_id", "staff_fname", expens.created_by);
            return PartialView(expens);
        }

        //
        // POST: /Expenses/Edit/5

        [HttpPost]
        public ActionResult Edit(Expens expens)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expens).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.staff_id = new SelectList(db.Staff_Info, "staff_id", "staff_fname", expens.staff_id);
            ViewBag.created_by = new SelectList(db.Staff_Info, "staff_id", "staff_fname", expens.created_by);
            return View(expens);
        }

        //
        // GET: /Expenses/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Expens expens = db.Expenses.Find(id);
            if (expens == null)
            {
                return HttpNotFound();
            }
            return PartialView(expens);
        }

        //
        // POST: /Expenses/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Expens expens = db.Expenses.Find(id);
            db.Expenses.Remove(expens);
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