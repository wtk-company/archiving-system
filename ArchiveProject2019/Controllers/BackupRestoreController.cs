
using ArchiveProject2019.HelperClasses;
using ArchiveProject2019.Models;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Configuration;
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
      
        [AccessDeniedAuthorizeattribute(ActionName = "BackupRestoreIndex")]

        public ViewResult Index(string Id="none")
        {
            ViewBag.Current = "Backup";

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

        [Authorize]

        [AccessDeniedAuthorizeattribute(ActionName = "BackupRestoreDownloadDB")]

        public ActionResult DownloadDbBackUp()

        {
            try
            {



                string backlocation = Server.MapPath("~/Uploads/");
                string downloadfilename = "ArchiveDB_" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + ".Bak";
                string backupfile = backlocation + "sample.BAK" + "'";
                String query = "backup database " + "archive_db" + " to disk='" + backupfile;
                //     String mycon = "Data Source=DESKTOP-B3DK4HG; Initial Catalog=" + "archive_db" + "; Integrated Security=true";
                String mycon = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

                SqlConnection con = new SqlConnection(mycon);
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                con.Close();



                string filePath = Path.Combine("~/Uploads/", "sample.BAK");


                return File(filePath, System.Net.Mime.MediaTypeNames.Application.Octet, downloadfilename);
            }
            catch
            {
              return  RedirectToAction("Index", new { Id = "BackupDbError" });
            }
          


        }


        [HttpPost]

        [AccessDeniedAuthorizeattribute(ActionName = "BackupRestoreRestoreDB")]

        public ActionResult UploadBackupDb(HttpPostedFileBase BackupFile)
        {

         

            if(BackupFile==null)
            {
                return RedirectToAction("Index", new { Id = "RestoreDbNullError" });


                
            }

            if(!BackupFile.FileName.EndsWith("BAK",StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index", new { Id = "RestoreDbErrorType" });

            }
            string FileName = Path.GetFileName(BackupFile.FileName);
            //Save In Server


            string path = Path.Combine(Server.MapPath("~/Uploads/"), FileName);


            BackupFile.SaveAs(path);
            try
            {
                if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Uploads/"), "sample.ZIP")))
                {

                    System.IO.File.Delete(Path.Combine(Server.MapPath("~/Uploads/"), "sample.ZIP"));

                }

                if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Uploads/"), "sample.BAK")))
                {

                    System.IO.File.Delete(Path.Combine(Server.MapPath("~/Uploads/"), "sample.BAK"));

                }

                string backup = Server.MapPath("~/Uploads/") + BackupFile.FileName;
                string Q1 = "ALTER DATABASE archive_db SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                String query = "restore database " + "archive_db" + "  from disk='" + backup + "' WITH REPLACE";
                //String mycon = "Data Source=DESKTOP-B3DK4HG; Initial Catalog=master; Integrated Security=true";
                String mycon = ConfigurationManager.ConnectionStrings["BackupConnection"].ConnectionString;

            //    SqlConnection con = new SqlConnection(mycon);
                SqlConnection con = new SqlConnection(mycon);
               if(con.State==System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand();


                cmd.CommandText = Q1;

                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                if (con.State == System.Data.ConnectionState.Open)
                {
                  
                con.Close();
                }


                //
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = query;

                cmd2.Connection = con;
                cmd2.ExecuteNonQuery();
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Close();
                }

                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
               

                SqlCommand cmd3 = new SqlCommand();

                string Q3= "ALTER DATABASE archive_db SET Multi_User";
              

                cmd3.CommandText = Q3;

                cmd3.Connection = con;
                cmd3.ExecuteNonQuery();
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Close();
                }

                System.IO.File.Delete(path);

            }
            catch {


                String mycon = ConfigurationManager.ConnectionStrings["BackupConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(mycon);
           
                SqlCommand cmd = new SqlCommand();

                string Q = "ALTER DATABASE archive_db SET Multi_User";
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }

                cmd.CommandText = Q;

                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Close();
                }

               
                if(System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                return RedirectToAction("Index",new { Id= "RestoreDbError" });



            }


            return RedirectToAction("Index",new { Id= "RestoreDbSuccess" });
        }


     


        [AccessDeniedAuthorizeattribute(ActionName = "BackupRestoreDownloadFiles")]


        public ActionResult BackUpFiles()
        {


            try
            {



                ZipFile BackupFilesAsZip = new ZipFile();

                IEnumerable<string> files = Directory.EnumerateFiles(Server.MapPath("~/Uploads"));
                foreach (string f in files)
                {
                    if (CheckFileFormatting.PermissionFile(f))
                    {

                        BackupFilesAsZip.AddFile(f, "");
                    }

                }




                BackupFilesAsZip.Save(Server.MapPath("~/Uploads/sample.zip"));




                //For Save:
                string downloadfilename = "ArchiveFiles" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + ".zip";
                string filePath = Path.Combine("~/Uploads/", "sample.zip");

                return File(filePath, System.Net.Mime.MediaTypeNames.Application.Octet, downloadfilename);
            }
            catch
            {
                return RedirectToAction("Index", new { Id = "BackupFilesError" });

            }
        }



       


        [HttpPost]
        [AccessDeniedAuthorizeattribute(ActionName = "BackupRestoreRestoreFiles")]

        public ActionResult UploadArchiveFiles(HttpPostedFileBase BackupFile)
        {



            if (BackupFile == null)
            {
                return RedirectToAction("Index", new { Id = "RestoreFilesNullError" });



            }

            if (!BackupFile.FileName.EndsWith("ZIP", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index", new { Id = "RestoreFilesErrorType" });

            }

            string FileName = Path.GetFileName(BackupFile.FileName);

            string path = Path.Combine(Server.MapPath("~/Uploads/"), FileName);

            ZipFile extra = new ZipFile();
            BackupFile.SaveAs(path);
            try
            {

                if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Uploads/"), "sample.ZIP")))
                {

                    System.IO.File.Delete(Path.Combine(Server.MapPath("~/Uploads/"), "sample.ZIP"));

                }

                if (System.IO.File.Exists(Path.Combine(Server.MapPath("~/Uploads/"), "sample.BAK")))
                {

                    System.IO.File.Delete(Path.Combine(Server.MapPath("~/Uploads/"), "sample.BAK"));

                }
               
            extra = ZipFile.Read(path);
         
           string Location= Server.MapPath("~/Uploads/");
           // extra.ExtractAll(Location);
         
            foreach (ZipEntry f in extra)
            {

                f.Extract(Location, ExtractExistingFileAction.OverwriteSilently);
                   
             }




                extra.Dispose();
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                return RedirectToAction("Index", new { Id = "RestoreFilesSuccess" });

            }
            catch 
            {
                extra.Dispose();
               
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                return RedirectToAction("Index", new { Id = "RestoreFilesError" });


            }


        }



    }
}