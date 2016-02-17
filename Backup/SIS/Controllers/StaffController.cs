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
    public class StaffController : Controller
    {
        private sis_dbEntities db = new sis_dbEntities();

        //
        // GET: /Staff/
        public ActionResult Index(string sortOrder = null, string currentFilter = null, string search = null, int? page = 1, string searchBy = null)
        {
            var staff = from d in db.Staff_Info
                               select d;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortName = String.IsNullOrEmpty(sortOrder) ? "name" : "";

            ViewBag.staff = staff.Count();
            ViewBag.account = db.Account_Info.ToList().Count();
            ViewBag.menu = db.System_Menus.ToList().Count();

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
                staff = staff.OrderByDescending(x => x.active_flag);
            }

            //searching of an item
            if (!String.IsNullOrEmpty(search))
            {
                double num;
                if (double.TryParse(search, out num))
                {
                    long qty = Convert.ToInt64(search);
                    staff = staff.Where(x => x.staff_monthly_paid == qty);
                }
                else
                {
                    staff = staff.Where(x => x.staff_lname.Contains(search) || x.staff_fname.Contains(search));
                }
            }
            ViewBag.CurrentFilter = search;

            //sorting of the columns
            switch (sortOrder)
            {
                case "name":
                    staff = staff.OrderBy(x => x.staff_fname);
                    break;
                default:
                    staff = staff.OrderByDescending(x => x.active_flag);
                    break;
            }

            //get only 20 of the list from the database
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var returnMe = staff.ToPagedList(pageNumber, pageSize);
            return View(returnMe);
        }

        [HttpGet]
        public ActionResult AddAccount(int id = 0) 
        {
            List<SelectListItem> action = new List<SelectListItem>();
            action.Add(new SelectListItem { Text = "Admin", Value = "Admin" });
            action.Add(new SelectListItem { Text = "Staff", Value = "Staff" });
            ViewBag.user_type = action;
            return PartialView(db.Staff_Info.Where(x => x.staff_id == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult AddAccount(Account_Info account, int staff_id, string username, string password, string user_type) 
        {
            account.username = username;
            account.user_type = user_type;
            account.password = password;
            account.active_flag = true;
            db.Account_Info.Add(account);
            db.SaveChanges();

            //updating the Staff_Info
            Staff_Info staff = db.Staff_Info.Find(staff_id);
            staff.account_info_id = account.account_info_id;
            db.Entry(staff).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index","Account");
        }

        //
        // POST: /Staff/Create

        [HttpGet]
        public ActionResult Create(Staff_Info staff_info)
        {
            if (ModelState.IsValid)
            {
                staff_info.account_info_id = null;
                staff_info.active_flag = true;
                db.Staff_Info.Add(staff_info);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(staff_info);
        }

        //
        // GET: /Staff/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Staff_Info staff_info = db.Staff_Info.Find(id);
            if (staff_info == null)
            {
                return HttpNotFound();
            }
            return PartialView(staff_info);
        }

        //
        // POST: /Staff/Edit/5

        [HttpPost]
        public ActionResult Edit(Staff_Info staff_info)
        {
            if (ModelState.IsValid)
            {
                db.Entry(staff_info).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(staff_info);
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}