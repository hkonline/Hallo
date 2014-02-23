using Hallo.ViewModels;
using HalloDal.Models.Content;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hallo.Controllers {

    public class FileController : HalloController {

        // Liste med alle filer
        public ActionResult List() {
            ViewBag.Headline = "Filer";
            return View();
        }

        // Ajax: Returnerer alle filer
        public JsonResult GetFiles([DataSourceRequest] DataSourceRequest request) {
            List<FileViewModel> list = new List<FileViewModel>();
            foreach (HalloFile file in db.Files) list.Add(new FileViewModel(file));
            return Json(list.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        // Ajax: Slet en fil
        public JsonResult DeleteFile(int id) {
            HalloFile file = db.Files.Find(id);
            if (file != null) {
                System.IO.File.Delete(GetFilePath(file));
                db.Files.Remove(file);
                db.SaveChanges();
            }
            return Json(new { success = true });
        }

        // Tilføj en ny fil
        public ActionResult AddFile() {
            HalloFile newFile = new HalloFile();
            db.Files.Add(newFile);
            db.SaveChanges();
            return RedirectToAction("Edit/" + newFile.Id);
        }

        // Ret en fil
        public ActionResult Edit(int id, String description = null) {
            ViewBag.Headline = "Upload / Ret fil";
            
            HalloFile file = db.Files.Find(id);

            if (description != null) {
                file.Description = description;
                db.SaveChanges();
            }

            return View(file);
        }

        // Ajax: Gem fil
        [HttpPost]
        public ActionResult SaveFile(int id, HttpPostedFileBase file) {
            HalloFile dbFile = db.Files.Find(id);

            if (file != null) {
                dbFile.Extension = file.FileName.Substring(file.FileName.LastIndexOf('.') + 1);
                String filename = GetFilePath(dbFile);
                file.SaveAs(filename);
                db.SaveChanges();
            }

            return RedirectToAction("Edit/" + id);
        }

        private string GetFilePath(HalloFile dbFile) {
            return Server.MapPath("~/") +
                ConfigurationManager.AppSettings["FileDirectoryUrl"].Substring(1) +
                "/File" + dbFile.Id + "." + dbFile.Extension;
        }
    }
}