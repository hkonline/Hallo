using Excel;
using HalloDal.Models;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Hallo.Users;
using System.Threading;

namespace Hallo.Controllers {
    public class UserController : HalloController {

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

                DataTable dt = ReadPmoInfo();
                UserService service = new UserService(
                    Context, int.Parse(ConfigurationManager.AppSettings["ChurchId"]),
                    ConfigurationManager.AppSettings["ChurchName"], dt
                );
                Thread worker = new Thread(service.SyncUserDatabaseWithPmo);
                worker.Start();
                
                //TODO: Lad worker processen logge evt. fejl.
                ViewBag.Message = "Opdatering af brugerdatabase startet.  Det kan tage nogle minutter før alle ændringer er læst ind.";
            }

            FileInfo info = new FileInfo(FilePath);
            return View(info);
        }

        private DataTable ReadPmoInfo() {
            FileStream stream = System.IO.File.Open(FilePath, FileMode.Open, FileAccess.Read);
            var excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            DataSet result = excelReader.AsDataSet();
            excelReader.Close();
            DataTable dt = result.Tables[0];
            foreach (DataColumn col in dt.Columns) {
                col.ColumnName = dt.Rows[0][col.Ordinal].ToString();
            }
            dt.Rows[0].Delete();
            dt.AcceptChanges();
            return dt;
        }

        public ActionResult PmoInfo() {
            DataTable dt = ReadPmoInfo();
            UserService service = new UserService(
                Context, int.Parse(ConfigurationManager.AppSettings["ChurchId"]),
                ConfigurationManager.AppSettings["ChurchName"], dt
            );
            Thread worker = new Thread(service.SyncUserDatabaseWithPmo);
            worker.Start();
            return View();
        }
    }
}
