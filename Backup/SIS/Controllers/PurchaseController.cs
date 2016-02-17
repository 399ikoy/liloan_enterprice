using System;
using System.Globalization;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SIS.Models;

namespace SIS.Controllers
{
    public class PurchaseController : Controller
    {
        private sis_dbEntities db = new sis_dbEntities();

        //
        // GET: /Purchase/

        public ActionResult Index()
        {
            var product_purchase = db.Product_Purchase.Include(p => p.Product_Info);
            return View(product_purchase.ToList());
        }

        //getting the out of stock items
        public ActionResult out_of_stocks() 
        {
            return View(db.Out_Of_Stock_Transactions_vw.ToList());
        }
        //adding the orders
        [HttpGet]
        public ActionResult addOrders(List<Out_Of_Stock_Transactions_vw> list = null) 
        {
            foreach (var i in list)
            {
                int val = 1;
                bool y = Convert.ToBoolean(val);
                if (i.active_flag == y)
                {
                    var data1 = db.Product_Purchase.Where(x=>x.product_info_id==i.product_info_id && x.pt_id==null).FirstOrDefault();
                    if (data1 != null)
                    {
                        Product_Purchase pp1 = db.Product_Purchase.Find(data1.product_purchase_id);
                        pp1.purchase_qty = i.product_qty;
                        db.Entry(pp1).State = EntityState.Modified;
                    }
                    else 
                    {
                        Product_Purchase pp = new Product_Purchase();
                        pp.product_info_id = i.product_info_id;
                        pp.purchase_qty = i.product_qty;
                        pp.purchase_new_price = i.product_orig_price;
                        pp.active_flag = true;
                        pp.pt_id = null;
                        db.Product_Purchase.Add(pp);
                    }
                }
            }
            db.SaveChanges();

            ////getting data from Product_Purchase table
            //var data = db.Current_Order_Items.Where(x => x.active_flag==true).ToList();
            //return View("Current_Order_Items",data);

            return RedirectToAction("Current_Order_Items");
        }

        //Current Order Items
        public ActionResult Current_Order_Items() 
        {
            //dropdown for supplier
            ViewBag.supplier_id = new SelectList(db.Supplier_Info.Where(x => x.active_flag == true), "supplier_info_id", "supplier_name");
            
            return View(db.Current_Order_Items.Where(x => x.active_flag == true).ToList());
        }
        //auto-complete implementation - Item_Added
        [HttpGet]
        public JsonResult Item_Added(string term) 
        {
            var result = (from r in db.Product_Info_vw
                          where r.product_code.ToLower().Contains(term.ToLower()) || r.product_desc.ToLower().Contains(term.ToLower()) || r.sub_category_name.ToLower().Contains(term.ToLower()) 
                          || r.category_name.ToLower().Contains(term.ToLower())
                          select new
                          {
                              r.product_info_id,
                              r.complete_info
                          }).Distinct().GroupBy(r => r.product_info_id).Select(r => r.FirstOrDefault());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //additional item to order
        [HttpGet]
        public ActionResult AddNewItem(int product_info_id = 0, int purchase_qty = 0) 
        {
            var data1 = db.Product_Purchase.Where(x => x.product_info_id == product_info_id && x.pt_id == null).FirstOrDefault();
            if (data1 != null)
            {
                Product_Purchase pp1 = db.Product_Purchase.Find(data1.product_purchase_id);
                pp1.purchase_qty = purchase_qty;
                db.Entry(pp1).State = EntityState.Modified;
            }
            else
            {
                //getting the original price first from Product_Info table
                Product_Info pi = db.Product_Info.Find(product_info_id);
                Product_Purchase pp = new Product_Purchase();
                pp.product_info_id = product_info_id;
                pp.purchase_qty = purchase_qty;
                pp.purchase_new_price = pi.product_orig_price;
                pp.active_flag = true;
                pp.pt_id = null;
                db.Product_Purchase.Add(pp);
            }

            db.SaveChanges();

            return RedirectToAction("Current_Order_Items");
        }

        //modify the item being ordered

        [HttpGet]
        public ActionResult Edit_Purchase_item(int id = 0) 
        {
            return PartialView(db.Product_Purchase.Where(x => x.product_purchase_id == id).FirstOrDefault());
        }
        public ActionResult Modify_Purchase_Item(Product_Purchase pp) 
        {
            db.Entry(pp).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Current_Order_Items");
        }
        //getting the data to be deleted purchase
        [HttpGet]
        public ActionResult Delete_Purchase_Item(int id = 0) 
        {
            return PartialView(db.Product_Purchase.Where(x => x.product_purchase_id == id).FirstOrDefault());
        }
        //deleting the purchase items

        [HttpGet]
        public ActionResult Remove_Purchase_Item(int id=0)
        {
            Product_Purchase product_purchase = db.Product_Purchase.Find(id);
            db.Product_Purchase.Remove(product_purchase);
            db.SaveChanges();

            return RedirectToAction("Current_Order_Items");
        }

        //adding purchase transactions
        [HttpGet]
        public ActionResult Purchase_Transactions(string purchase_name = null, string expected_date= null, int supplier_id = 0) 
        {
            DateTime txtmyDate = DateTime.ParseExact(expected_date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //adding the data into Purchase_Transactions table
            Purchase_Transactions pt = new Purchase_Transactions();
            pt.purchase_name = purchase_name;
            pt.purchase_date = System.DateTime.Now;
            pt.supplier_id = supplier_id;
            pt.expected_date = txtmyDate;
            pt.status = "Ordered";
            pt.order_by = 1;
            db.Purchase_Transactions.Add(pt);
            db.SaveChanges();

            var data = db.Product_Purchase.Where(x => x.pt_id == null && x.active_flag == true).ToList();
            foreach (var item in data) 
            {
                Product_Purchase pt1 = db.Product_Purchase.Find(item.product_purchase_id);
                pt1.pt_id = pt.pt_id;
                db.Entry(pt1).State = EntityState.Modified;

            }
            db.SaveChanges();

            return RedirectToAction("Ordered_lists");
        }

        //getting all the Purchase Items with transaction
        public ActionResult Ordered_Lists() 
        {
            return View(db.PT_OrderedList_vw.ToList());
        }

        //viewing all purchase item
        public ActionResult Ordered_Lists_Items(int id=0) 
        {
            var data = db.PT_OrderedList_vw.Where(x => x.pt_id == id).FirstOrDefault();
            ViewBag.purchase_date = data.purchase_date;
            ViewBag.purchase_name = data.purchase_name;
            ViewBag.order_by = data.staff_fname + ' ' + data.staff_lname;
            ViewBag.total = data.totalPT;
            ViewBag.status = data.status;
            return View(db.Purchase_Transactions_vw.Where(x=>x.pt_id==id).ToList());
        }

        //Delivery of Purchase Item
        [HttpPost]
        public ActionResult Delivery(List<Purchase_Transactions_vw> list = null, int pt_id=0)
        {
            Purchase_Transactions pt = db.Purchase_Transactions.Find(pt_id);
            pt.status = "Delivered";
            db.Entry(pt).State = EntityState.Modified;
            foreach (var i in list)
            {
                int val = 1;
                bool y = Convert.ToBoolean(val);
                //once checked - update the data
                Product_Purchase pp1 = db.Product_Purchase.Find(i.product_purchase_id);
                pp1.product_info_id = i.product_info_id;
                pp1.purchase_qty = i.purchase_qty;
                pp1.purchase_new_price = i.purchase_new_price;
                pp1.pt_id = i.pt_id;
                if (i.active_flag == y)
                {
                    //updating the master list for the added stocks
                    Product_Info pi = db.Product_Info.Find(i.product_info_id);
                    pi.product_orig_price = i.purchase_new_price;
                    pi.product_qty = pi.product_qty + i.purchase_qty;
                    pi.updated_by = 1;
                    pi.updated_date = System.DateTime.Now;
                    db.Entry(pi).State = EntityState.Modified;
                    //same status for delivered items
                    pp1.active_flag = i.active_flag;
                }
                else 
                {
                    //setting the active flag to false once item is cancelled
                    pp1.active_flag = false;
                }
                db.Entry(pp1).State = EntityState.Modified;
                

            }
            db.SaveChanges();

            return RedirectToAction("Ordered_Lists");
        }

        //Instant delivery
        public ActionResult AllDelivery(int id = 0) 
        {
            //changing the status of the Purchase_Transactions
            Purchase_Transactions pt = db.Purchase_Transactions.Find(id);
            pt.status = "Delivered";
            db.Entry(pt).State = EntityState.Modified;
            
            //getting all the Purchase Items for this transactions
            var data = db.Product_Purchase.Where(x => x.pt_id == id&&x.active_flag==true).ToList();
            foreach (var i in data) 
            {
                Product_Info pi = db.Product_Info.Find(i.product_info_id);
                pi.product_orig_price = i.purchase_new_price;
                pi.product_qty = pi.product_qty + i.purchase_qty;
                pi.updated_by = 1;
                pi.updated_date = System.DateTime.Now;
                db.Entry(pi).State = EntityState.Modified;
            }
            db.SaveChanges();

            return RedirectToAction("Ordered_Lists");
        }

        //Cancelling the delivery
        public ActionResult CancelledDelivery(int id = 0) 
        {
            //changing the status of the Purchase_Transactions
            Purchase_Transactions pt = db.Purchase_Transactions.Find(id);
            pt.status = "Cancelled";
            db.Entry(pt).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Ordered_Lists");
        }
        //Reports
        public ActionResult Reports() 
        {
            return View();
        }

        //getting the data for total purchase
        public JsonResult Query_PTPurchase() 
        {
            var data_monthly = db.Database.SqlQuery<string>("SELECT * FROM dbo.PT_Monthly").ToList();
           
            return Json(data_monthly, JsonRequestBehavior.AllowGet);
            //return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Get(int year)
        {
            var alldata = (from item in db.PT_OrderedList_vw
                           where item.expected_date.Value.Year == year
                           group item by item.expected_date.Value.Month into grp
                           select new
                           {
                               totalPT = grp.Sum(x => x.totalPT.Value),
                               Month = (double)grp.Max(x => x.expected_date.Value.Month)
                           }).ToList();
            var final_data = new[]
                {  
                    new { label="Total Purchase", data = alldata.Select(x=>new double[]{ x.Month, x.totalPT })}
            
                };

             return Json(final_data, JsonRequestBehavior.AllowGet);
        }

        //Purchase Notification
        public ActionResult Purchase_Notify() 
        {
            return PartialView(db.PT_OrderedList_vw.Where(x => x.status == "Ordered").ToList());
        }
        //
        // GET: /Purchase/Details/5
        public ActionResult Details(int id = 0)
        {
            Product_Purchase product_purchase = db.Product_Purchase.Find(id);
            if (product_purchase == null)
            {
                return HttpNotFound();
            }
            return View(product_purchase);
        }

        //
        // GET: /Purchase/Create

        public ActionResult Create()
        {
            ViewBag.product_info_id = new SelectList(db.Product_Info, "product_info_id", "product_desc");
            return View();
        }

        //
        // POST: /Purchase/Create

        [HttpPost]
        public ActionResult Create(Product_Purchase product_purchase)
        {
            if (ModelState.IsValid)
            {
                db.Product_Purchase.Add(product_purchase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.product_info_id = new SelectList(db.Product_Info, "product_info_id", "product_desc", product_purchase.product_info_id);
            return View(product_purchase);
        }

        //
        // GET: /Purchase/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Product_Purchase product_purchase = db.Product_Purchase.Find(id);
            if (product_purchase == null)
            {
                return HttpNotFound();
            }
            ViewBag.product_info_id = new SelectList(db.Product_Info, "product_info_id", "product_desc", product_purchase.product_info_id);
            return View(product_purchase);
        }

        //
        // POST: /Purchase/Edit/5

        [HttpPost]
        public ActionResult Edit(Product_Purchase product_purchase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product_purchase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.product_info_id = new SelectList(db.Product_Info, "product_info_id", "product_desc", product_purchase.product_info_id);
            return View(product_purchase);
        }

        //
        // GET: /Purchase/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Product_Purchase product_purchase = db.Product_Purchase.Find(id);
            if (product_purchase == null)
            {
                return HttpNotFound();
            }
            return View(product_purchase);
        }

        //
        // POST: /Purchase/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product_Purchase product_purchase = db.Product_Purchase.Find(id);
            db.Product_Purchase.Remove(product_purchase);
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