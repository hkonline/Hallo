using HalloDal.Models.AC;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Hallo.Controllers {
    public class AcDateController : HalloController {

        public ActionResult Index() {
            Authorize("AcAdmin");
            ViewBag.Headline = "Administration af AK-datoer";
            ViewBag.ShowLeft = true;
            return View();
        }

        public JsonResult DeleteDate(int id = 0) {
            AcDate date = db.AcDates.Find(id);
            if (date != null) {
                db.AcDates.Remove(date);
                db.SaveChanges();
            }
            return Json(new { success = true });
        }

        public JsonResult CreateDate(DateTime date) {
            db.AcDates.Add(new AcDate { Date = new DateTime(date.Year, date.Month, date.Day) });
            db.SaveChanges();
            return Json(new { success = true });
        }

        public JsonResult UpdateDate(int id, DateTime date) {
            AcDate dbDate = db.AcDates.Find(id);
            dbDate.Date = new DateTime(date.Year, date.Month, date.Day);
            db.SaveChanges();
            return Json(new { success = true });
        }

        public JsonResult GetDates([DataSourceRequest] DataSourceRequest request) {
            List<AcDate> list = db.AcDates.Where(x => x.Date >= DateTime.Today).OrderBy(x=>x.Date).ToList();
            return Json(list.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }
    }
}
