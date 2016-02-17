using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIS.Models;

namespace SIS.Controllers
{
    public class SalesController : Controller
    {
        private sis_dbEntities db = new sis_dbEntities();

        //
        // GET: /Sales/

        public ActionResult Index()
        {
            List<SelectListItem> payment = new List<SelectListItem>();
            payment.Add(new SelectListItem { Text = "Cash", Value = "Cash" });
            payment.Add(new SelectListItem { Text = "Credit", Value = "Credit" });
            ViewBag.paymentType = payment;

            db.Configuration.ProxyCreationEnabled = false;
            ViewBag.prod = db.Product_Info.ToList();
            var product_sales = db.Product_Sales.Include(p => p.Product_Info).Include(p => p.Receipt);
            return View(product_sales);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Receipt resibo) {
            if (ModelState.IsValid)
            {
                db.Receipts.Add(resibo);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Sales/Details/5

        public ActionResult Details(int id = 0)
        {
            Product_Sales product_sales = db.Product_Sales.Find(id);
            if (product_sales == null)
            {
                return HttpNotFound();
            }
            return View(product_sales);
        }

        //
        // GET: /Sales/Create

        public ActionResult Create()
        {
            ViewBag.product_info_id = new SelectList(db.Product_Info, "product_info_id", "product_desc");
            ViewBag.receipt_id = new SelectList(db.Receipts, "receipt_id", "customer_name");
            return View();
        }

        //
        // POST: /Sales/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product_Sales product_sales)
        {
            if (ModelState.IsValid)
            {
                db.Product_Sales.Add(product_sales);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.product_info_id = new SelectList(db.Product_Info, "product_info_id", "product_desc", product_sales.product_info_id);
            ViewBag.receipt_id = new SelectList(db.Receipts, "receipt_id", "customer_name", product_sales.receipt_id);
            return View(product_sales);
        }

        //
        // GET: /Sales/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Product_Sales product_sales = db.Product_Sales.Find(id);
            if (product_sales == null)
            {
                return HttpNotFound();
            }
            ViewBag.product_info_id = new SelectList(db.Product_Info, "product_info_id", "product_desc", product_sales.product_info_id);
            ViewBag.receipt_id = new SelectList(db.Receipts, "receipt_id", "customer_name", product_sales.receipt_id);
            return View(product_sales);
        }

        //
        // POST: /Sales/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product_Sales product_sales)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product_sales).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.product_info_id = new SelectList(db.Product_Info, "product_info_id", "product_desc", product_sales.product_info_id);
            ViewBag.receipt_id = new SelectList(db.Receipts, "receipt_id", "customer_name", product_sales.receipt_id);
            return View(product_sales);
        }

        //
        // GET: /Sales/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Product_Sales product_sales = db.Product_Sales.Find(id);
            if (product_sales == null)
            {
                return HttpNotFound();
            }
            return View(product_sales);
        }

        //
        // POST: /Sales/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product_Sales product_sales = db.Product_Sales.Find(id);
            db.Product_Sales.Remove(product_sales);
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