using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hallo.Controllers {
    public class UserController : Controller {

        private string FilePath {
            get {
                return Server.MapPath("~/App_Data/PmoList.xlsx");
            }
        }

        [HttpGet]
        public ActionResult PmoUpload() {
            FileInfo info = new FileInfo(FilePath);
            return View(info);
        }

        [HttpPost]
        public ActionResult PmoUpload(HttpPostedFileBase file) {

            if (file.ContentLength > 0) {
                file.SaveAs(FilePath);
            }

            FileInfo info = new FileInfo(FilePath);
            return View(info);
        }

        public OleDbConnection GetOledbConnection() {
            return new OleDbConnection(
                @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + FilePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1;ReadOnly=Yes;\""
            );
        }

        public ActionResult PmoInfo() {
            OleDbConnection connection = GetOledbConnection();
            OleDbCommand command = new OleDbCommand("select * from [PMO$]", connection);
            connection.Open();
            OleDbDataReader reader = command.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(reader);

            reader.Close();
            connection.Close();

            return View(dt);
        }
    }
}
