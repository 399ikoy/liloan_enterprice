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
    public class MasterListController : Controller
    {
        private sis_dbEntities db = new sis_dbEntities();

        //
        // GET: /MasterList/

        public ActionResult Index(string sortOrder = null, string currentFilter = null, string search = null, int? page = 1, string searchBy = null)
        {
            var product_info = from d in db.Product_Info
                               select d;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortItem = String.IsNullOrEmpty(sortOrder) ? "item" : "";

            ViewBag.count = product_info.Count();
            ViewBag.unit = db.Units.ToList().Count();
            ViewBag.category = db.Categories.ToList().Count();
            ViewBag.supplier = db.Supplier_Info.ToList().Count();

            ViewBag.category_id = new SelectList(db.Categories.Where(x => x.active_flag == true), "category_id", "category_name");
            ViewBag.sub_category_id = new SelectList(db.Sub_Category.Where(x => x.active_flag == true), "sub_category_id", "sub_category_name");
            ViewBag.supplier_info_id = new SelectList(db.Supplier_Info.Where(x => x.active_flag == true), "supplier_info_id", "supplier_name");
            ViewBag.unit_id = new SelectList(db.Units.Where(x => x.active_flag == true), "unit_id", "unit_name");

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
                product_info = product_info.OrderByDescending(x => x.active_flag);
            }

            //searching of an item
            if (!String.IsNullOrEmpty(search))
            {
                double num;
                if (double.TryParse(search, out num))
                {
                    long qty = Convert.ToInt64(search);
                    product_info = product_info.Where(x => x.product_qty == qty || x.product_orig_price==qty || x.product_sold_price==qty);
                }
                else
                {
                    product_info = product_info.Where(x => x.product_desc.Contains(search) || x.product_remarks.Contains(search));
                }
            }
            ViewBag.CurrentFilter = search;

            //sorting of the columns
            switch (sortOrder)
            {
                case "item":
                    product_info = product_info.OrderBy(x => x.product_desc);
                    break;
                default:
                    product_info = product_info.OrderByDescending(x => x.active_flag);
                    break;
            }

            //get only 20 of the list from the database
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var returnMe = product_info.ToPagedList(pageNumber, pageSize);
            return View(returnMe);

            //return View(product_info.ToList());
        }

        //Limited Notification
        public ActionResult Limited_Notify() 
        {
            return PartialView(db.Product_Info.Where(x=>x.product_qty <= x.product_cut_off).ToList());
        }
        //
        // GET: /MasterList/Details/5

        public ActionResult Details(int id = 0)
        {
            Product_Info product_info = db.Product_Info.Find(id);
            if (product_info == null)
            {
                return HttpNotFound();
            }
            return PartialView(product_info);
        }

        //
        // POST: /MasterList/Create

        [HttpGet]
        public ActionResult Create(Product_Info product_info)
        {
            product_info.created_by = 1;
            product_info.created_date = System.DateTime.Now;
            product_info.active_flag = true;

            db.Product_Info.Add(product_info);
            db.SaveChanges();
            
           
            ViewBag.category_id = new SelectList(db.Categories.Where(x=>x.active_flag==true), "category_id", "category_name", product_info.category_id);
            ViewBag.sub_category_id = new SelectList(db.Sub_Category.Where(x=>x.active_flag==true), "sub_category_id", "sub_category_name", product_info.sub_category_id);
            ViewBag.supplier_info_id = new SelectList(db.Supplier_Info.Where(x=>x.active_flag==true), "supplier_info_id", "supplier_name", product_info.supplier_info_id);
            ViewBag.unit_id = new SelectList(db.Units.Where(x=>x.active_flag==true), "unit_id", "unit_name");
            return RedirectToAction("Index");
        }

        //
        // GET: /MasterList/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Product_Info product_info = db.Product_Info.Find(id);
            if (product_info == null)
            {
                return HttpNotFound();
            }
            ViewBag.category_id = new SelectList(db.Categories.Where(x => x.active_flag == true), "category_id", "category_name", product_info.category_id);
            ViewBag.sub_category_id = new SelectList(db.Sub_Category.Where(x => x.active_flag == true), "sub_category_id", "sub_category_name", product_info.sub_category_id);
            ViewBag.supplier_info_id = new SelectList(db.Supplier_Info.Where(x => x.active_flag == true), "supplier_info_id", "supplier_name", product_info.supplier_info_id);
            ViewBag.unit_id = new SelectList(db.Units.Where(x => x.active_flag == true), "unit_id", "unit_name", product_info.unit_id);
            return PartialView(product_info);
        }

        //
        // POST: /MasterList/Edit/5

        [HttpPost]
        public ActionResult Edit(Product_Info product_info)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product_info).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.category_id = new SelectList(db.Categories.Where(x => x.active_flag == true), "category_id", "category_name", product_info.category_id);
            ViewBag.sub_category_id = new SelectList(db.Sub_Category.Where(x => x.active_flag == true), "sub_category_id", "sub_category_name", product_info.sub_category_id);
            ViewBag.supplier_info_id = new SelectList(db.Supplier_Info.Where(x => x.active_flag == true), "supplier_info_id", "supplier_name", product_info.supplier_info_id);
            ViewBag.unit_id = new SelectList(db.Units.Where(x => x.active_flag == true), "unit_id", "unit_name");
            return View(product_info);
        }

        //
        // GET: /MasterList/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Product_Info product_info = db.Product_Info.Find(id);
            if (product_info == null)
            {
                return HttpNotFound();
            }
            return PartialView(product_info);
        }

        //
        // POST: /MasterList/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product_Info product_info = db.Product_Info.Find(id);
            db.Product_Info.Remove(product_info);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //view category of every product
        public ActionResult viewCategory(string sortOrder = null, string currentFilter = null, string search = null, int? page = 1, string searchBy = null)
        {
            var category = from d in db.Categories
                        select d;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortName = String.IsNullOrEmpty(sortOrder) ? "name" : "";

            var product_info = db.Product_Info.Include(p => p.Category).Include(p => p.Sub_Category).Include(p => p.Supplier_Info);
            ViewBag.count = product_info.Count();
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
                category = category.OrderByDescending(x => x.active_flag);
            }

            //searching of an item
            if (!String.IsNullOrEmpty(search))
            {
                category = category.Where(x => x.category_name.Contains(search));
            }
            ViewBag.CurrentFilter = search;

            //sorting of the columns
            switch (sortOrder)
            {
                case "name":
                    category = category.OrderBy(x => x.category_name);
                    break;
                default:
                    category = category.OrderByDescending(x => x.active_flag);
                    break;
            }

            //get only 20 of the list from the database
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var returnMe = category.ToPagedList(pageNumber, pageSize);
            return View(returnMe);

            //return View(product_info.ToList());
        }

        //
        public ActionResult addCategory() 
        {
            return PartialView();
        }
        //add category
        [HttpGet]
        public ActionResult CategoryAdd(Category category) 
        {
            category.created_by = "jparagados";
            category.created_date = System.DateTime.Now;
            category.active_flag = true;
            db.Categories.Add(category);
            db.SaveChanges();

            return RedirectToAction("viewCategory");
        }

        //add sub category
        public ActionResult addSubCategory(int id) 
        {
            ViewBag.cat_id = id;
            return PartialView();
        
        }

        [HttpGet]
        public ActionResult SubCategoryAdd(int category_id, Sub_Category sub_cat) 
        {
            sub_cat.category_id = category_id;
            sub_cat.created_by = "Jparagados";
            sub_cat.created_date = System.DateTime.Now;
            sub_cat.active_flag = true;
            db.Sub_Category.Add(sub_cat);
            db.SaveChanges();

            return RedirectToAction("viewCategory");
        }

        public ActionResult viewSubCategory(int id)
        {
            var data = db.Sub_Category.Where(x => x.category_id == id).ToList();

            return PartialView(data);
        }

        public ActionResult DeleteCategory(int id) 
        {
            var data = db.Categories.Where(x => x.category_id == id).FirstOrDefault();

            return PartialView(data);
        }

        [HttpGet]
        public ActionResult DeleteCategoryFinal(Category categories, int id) 
        {
            Category category = db.Categories.Find(id);

            category.active_flag = false;
            db.Entry(category).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("viewCategory");
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}