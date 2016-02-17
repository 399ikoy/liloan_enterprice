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
    public class SystemMenuController : Controller
    {
        private sis_dbEntities db = new sis_dbEntities();

        //
        // GET: /SystemMenu/

        public ActionResult Index(string sortOrder = null, string currentFilter = null, string search = null, int? page = 1, string searchBy = null)
        {
            var menu = from d in db.System_Menus
                        select d;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortName = String.IsNullOrEmpty(sortOrder) ? "name" : "";

            ViewBag.staff = db.Staff_Info.ToList().Count();
            ViewBag.account = db.Account_Info.ToList().Count();
            ViewBag.menu = menu.Count();

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
                menu = menu.OrderByDescending(x => x.active_flag);
            }

            //searching of an item
            if (!String.IsNullOrEmpty(search))
            {
                menu = menu.Where(x => x.sm_description.Contains(search) || x.sm_action_name.Contains(search) || x.sm_controller.Contains(search));
            }
            ViewBag.CurrentFilter = search;

            //sorting of the columns
            switch (sortOrder)
            {
                case "name":
                    menu = menu.OrderBy(x => x.sm_description);
                    break;
                default:
                    menu = menu.OrderByDescending(x => x.active_flag);
                    break;
            }

            //get only 20 of the list from the database
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var returnMe = menu.ToPagedList(pageNumber, pageSize);
            return View(returnMe);
        }

        //
        // POST: /SystemMenu/Create

        [HttpPost]
        public ActionResult Create(System_Menus system_menus)
        {
            if (ModelState.IsValid)
            {
                system_menus.active_flag = true;
                db.System_Menus.Add(system_menus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(system_menus);
        }

        //
        // GET: /SystemMenu/Edit/5

        public ActionResult Edit(int id = 0)
        {
            System_Menus system_menus = db.System_Menus.Find(id);
            if (system_menus == null)
            {
                return HttpNotFound();
            }
            return PartialView(system_menus);
        }

        //
        // POST: /SystemMenu/Edit/5

        [HttpPost]
        public ActionResult Edit(System_Menus system_menus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(system_menus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(system_menus);
        }

        //adding the user per menu
        [HttpGet]
        public ActionResult AddUserMenu(int id=0) 
        {
            ViewBag.account_info_id = new SelectList(db.Account_Info.Where(x => x.active_flag == true), "account_info_id", "username");
            return PartialView(db.System_Menus.Where(x=>x.system_menus_id==id).FirstOrDefault());
        }

        //
        public ActionResult AddUserMenu(int system_menus_id, int account_info_id) 
        {
            System_Menu_Roles smr = new System_Menu_Roles();
            smr.account_info_id = account_info_id;
            smr.system_menus_id = system_menus_id;
            smr.active_flag = true;
            db.System_Menu_Roles.Add(smr);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //viewing all users having this menu
        [HttpGet]
        public ActionResult ViewUserMenus(int id = 0) 
        {
            return PartialView(db.System_Menu_Roles.Where(x=>x.system_menus_id==id).ToList());
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}