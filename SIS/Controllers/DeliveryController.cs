using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using SIS.Models;

namespace SIS.Controllers
{
    public class DeliveryController : Controller
    {
        private sis_dbEntities db = new sis_dbEntities();

        public ActionResult Index()
        {
            var delivery = db.Deliveries.Include(p => p.Receipt).Include(p => p.Staff_Info);
            return View(delivery);
        }

        public ActionResult AllDelivery()
        {
            var all_delivery = db.Deliveries.Include(p => p.Receipt).Include(p => p.Staff_Info);
            return View(all_delivery);
        }
    }
}
