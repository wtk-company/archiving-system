using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ArchiveProject2019.Controllers
{
    public class BackupRestoreController : Controller
    {
        // GET: BackupRestore
        public ViewResult Index(string Id="none")
        {
            ViewBag.Current = "Roles";

            if (!Id.Equals("none"))
            {
                ViewBag.Msg = Id;

            }
            else
            {
                ViewBag.Msg = null;
            }

            // return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, downloadfilename);
            //return "OK";
            return View();
        }

        public FileResult DownloadDbBackUp()
        {

            string backlocation = Server.MapPath("~/Uploads/");
            string downloadfilename = "ArchiveDB_" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + ".Bak";
            string backupfile = backlocation + downloadfilename + "'";
            String query = "backup database " + "archive_db" + " to disk='" + backupfile;
            String mycon = "Data Source=DESKTOP-B3DK4HG; Initial Catalog=" + "archive_db" + "; Integrated Security=true";
            SqlConnection con = new SqlConnection(mycon);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = query;
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            con.Close();



            string filePath = Path.Combine("~/Uploads/", downloadfilename);


            return File(filePath, System.Net.Mime.MediaTypeNames.Application.Octet, downloadfilename);
        }


        [HttpPost]
        public ActionResult UploadBackupDb(HttpPostedFileBase BackupFile)
        {

            if(BackupFile==null)
            {
                return RedirectToAction("Index", new { Id = "BackupError" });


                
            }

            if(!BackupFile.FileName.EndsWith("BAK",StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index", new { Id = "BackupErrorType" });

            }

            string FileName = Path.GetFileName(BackupFile.FileName);
            //Save In Server

       
            string path = Path.Combine(Server.MapPath("~/Uploads/"), FileName);
         

            BackupFile.SaveAs(path);
            string backup = Server.MapPath("~/Uploads/") + BackupFile.FileName;
            string Q1 = "ALTER DATABASE archive_db SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
            String query = "restore database "+ "archive_db" +"  from disk='" + backup + "' WITH REPLACE";
            String mycon = "Data Source=DESKTOP-B3DK4HG; Initial Catalog=master; Integrated Security=true";
            SqlConnection con = new SqlConnection(mycon);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = Q1;

            cmd.Connection = con;
            cmd.ExecuteNonQuery();
            con.Close();


            //
            con.Open();
            SqlCommand cmd2 = new SqlCommand();
            cmd2.CommandText = query;

            cmd2.Connection = con;
            cmd2.ExecuteNonQuery();
            con.Close();


            //3
            string Q3 = "ALTER DATABASE archive_db SET Multi_User";
            con.Open();
            SqlCommand cmd3 = new SqlCommand();
            cmd3.CommandText = Q3;

            cmd3.Connection = con;
            cmd3.ExecuteNonQuery();
            con.Close();


            return RedirectToAction("Index",new { Id= "BackupSuccess" });
        }

    }
}