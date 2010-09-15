using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using BidsForKids.Data.Models;

namespace BidsForKids.Controllers
{
    [HandleError]
    public class AdminController : Controller
    {
        public ActionResult Menu()
        {
            ViewData["Message"] = "Please choose an admin item from the menu below";
            return View();
        }

        public ActionResult BackupDatabase()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult StartDatabaseBackup()
        {
            try
            {
                var dc = new ProcurementDataClassesDataContext();


                ContentResult result = new ContentResult();

                string backupLocation = ConfigurationManager.AppSettings["SQLBackupLocation"];

                if (string.IsNullOrEmpty(backupLocation) == true)
                {
                    throw new ApplicationException("SQLBackupLocation is not set in web.config");
                }

                dc.BackupDatabase(backupLocation);

                result.Content = "Database has been backed up.";

                return result;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                ContentResult errResult = new ContentResult();
                errResult.Content = "Error backing up database: " + ex.Message;
                return errResult;
            }
        }
    }
}
