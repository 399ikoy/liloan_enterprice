using PagedList;
using SIS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SIS.Controllers
{
    public class AccountsController : Controller
    {
        private sis_dbEntities db = new sis_dbEntities();

        //
        // GET: /Accounts/
        //angular delete info
        [HttpGet]
        public ActionResult delete(int id = 0) 
        {
            Account_Info acc = db.Account_Info.Find(id);
            acc.active_flag = false;
            db.Entry(acc).State = EntityState.Modified;
            //db.Account_Info.Remove(acc);
            db.SaveChanges();

            return null;
        }
        public ActionResult Index(string sortOrder = null, string currentFilter = null, string search = null, int? page = 1, string searchBy = null)
        {
            var account = from d in db.Account_Info
                          select d;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortItem = String.IsNullOrEmpty(sortOrder) ? "username" : "";

            ViewBag.users = account.Count();
            ViewBag.staffs = db.Staff_Info.ToList().Count();
            ViewBag.menus = db.System_Menus.ToList().Count();

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
                account = account.OrderByDescending(x => x.active_flag);
            }

            //searching of an item
            if (!String.IsNullOrEmpty(search))
            {
                account = account.Where(x => x.username.Contains(search) || x.user_type.Contains(search));
            }
            ViewBag.CurrentFilter = search;

            //sorting of the columns
            switch (sortOrder)
            {
                case "username":
                    account = account.OrderBy(x => x.username);
                    break;
                default:
                    account = account.OrderByDescending(x => x.active_flag);
                    break;
            }

            //get only 10 of the list from the database
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var returnMe = account.ToPagedList(pageNumber, pageSize);
            return View(returnMe);
        }

        public ActionResult sample() 
        {
            return View();
        }

        [HttpGet]
        public ActionResult sample1() 
        {
            db.Configuration.ProxyCreationEnabled = false;

            var bookList = db.Account_Info.Where(x=>x.active_flag==true).ToList();
            return Json(bookList, JsonRequestBehavior.AllowGet);
        }


        //changing of password
        public ActionResult ChangePassword(int id=0) 
        {
            return PartialView(db.Account_Info.Where(x=>x.account_info_id==id).FirstOrDefault());
        }

        //send the newly password set
        public ActionResult PasswordChange(Account_Info acc) 
        {
            db.Entry(acc).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //login
        public ActionResult login() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginUser(string username, string password) 
        {
            var data = db.Account_Info.Where(x => x.username == username && x.password == password).FirstOrDefault();
            //when user exists
            if (data != null) 
            {
                var inactive = db.Account_Info.Where(x => x.login == true).ToList();
                //set to inactive all open login

                if (inactive != null)
                {
                    foreach (var item in inactive)
                    {
                        Account_Info acc = db.Account_Info.Find(item.account_info_id);
                        acc.login = false;
                        db.Entry(acc).State = EntityState.Modified;
                    }
                    db.SaveChanges();

                    //
                    Account_Info acc1 = db.Account_Info.Find(data.account_info_id);
                    acc1.login = true;
                    db.Entry(acc1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else 
                {
                    Account_Info acc1 = db.Account_Info.Find(data.account_info_id);
                    acc1.login = true;
                    db.Entry(acc1).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("login","Accounts");

        }

        public ActionResult getMenus() 
        {
            var acc = db.Account_Info.Where(x=>x.login==true).FirstOrDefault();
            var data = db.System_Menu_Roles.Where(x => x.account_info_id == acc.account_info_id).ToList();

            return PartialView(data);
        }

        //adding the menu per user
        [HttpGet]
        public ActionResult AddMenuUser(int id = 0)
        {
            ViewBag.system_menus_id = new SelectList(db.System_Menus.Where(x => x.active_flag == true), "system_menus_id", "sm_description");
            return PartialView(db.Account_Info.Where(x => x.account_info_id == id).FirstOrDefault());
        }

        //
        public ActionResult AddMenuUser(int system_menus_id, int account_info_id)
        {
            var check = db.System_Menu_Roles.Where(x => x.system_menus_id == system_menus_id && x.account_info_id == account_info_id).FirstOrDefault();
            if (check == null)
            {
                System_Menu_Roles smr = new System_Menu_Roles();
                smr.account_info_id = account_info_id;
                smr.system_menus_id = system_menus_id;
                smr.active_flag = true;
                db.System_Menu_Roles.Add(smr);
                db.SaveChanges();

                TempData["MessageAlert"] = "Successfully added!";
                return RedirectToAction("Index");
            }
            else 
            {
                TempData["MessageAlert1"] = "Menu for this user already exist!";
                return RedirectToAction("Index");
            }
        }

        //viewing all users having this menu
        [HttpGet]
        public ActionResult ViewMenusUser(int id = 0)
        {
            return PartialView(db.System_Menu_Roles.Where(x => x.account_info_id == id).ToList());
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}