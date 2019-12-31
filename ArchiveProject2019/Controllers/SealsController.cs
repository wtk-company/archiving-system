using ArchiveProject2019.HelperClasses;

using ArchiveProject2019.Models;
using ArchiveProject2019.Security;
using ArchiveProject2019.ViewModel;

using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Mvc;

namespace ArchiveProject2019.Controllers
{
    public class SealsController : Controller
    {
        ApplicationDbContext _context;

        public SealsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Seal


        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentSealsIndex")]
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            var seals = _context.SealDocuments.Where(s => s.DocumentId == id).Include(s => s.Files).Include(s => s.Document).Include(s => s.CreatedBy).ToList();

            if (seals == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            if (ManagedAes.IsCipher && seals.Count != 0)
            {
                seals[0].Document.Subject = ManagedAes.DecryptText(seals[0].Document.Subject);
            }

            if (ManagedAes.IsCipher)
            {
                //seals[0].Document.Subject = ManagedAes.DecryptText(seals[0].Document.Subject);
                foreach (var seal in seals)
                {
                    //seal.Document.Subject = ManagedAes.DecryptText(seal.Document.Subject);
                    seal.CreatedAt = ManagedAes.DecryptText(seal.CreatedAt);
                    seal.FileName = ManagedAes.DecryptText(seal.FileName);
                    seal.Message = ManagedAes.DecryptText(seal.Message);
                    foreach (var file in seal.Files)
                    {
                        file.FileName = ManagedAes.DecryptText(file.FileName);
                    }
                }
            }

            return View(seals);
        }


        //
        //[AccessDeniedAuthorizeattribute(ActionName = "DocumentSealsDetails")]




        //// GET: Seal/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return RedirectToAction("BadRequestError", "ErrorController");
        //    }

        //    var seal = _context.SealDocuments.Find(id);

        //    if (seal == null)
        //    {
        //        return RedirectToAction("HttpNotFoundError", "ErrorController");
        //    }

        //    return View(seal);
        //}

        // GET: Seal/Create

        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentSealsCreate")]
        public ActionResult Create(int id)
        {
            var seal = new SealDocument()
            {
                DocumentId = id,
            };

            return View(seal);
        }

        //private static byte[] ManagedAes.DecodeUrlBase64(string s)
        //{
        //    //s = s.Replace('-', '+').Replace('_', '/').PadRight(4 * ((s.Length + 3) / 4), '=');
        //    var arrayOfString = s.Split(',');

        //    return Convert.FromBase64String(arrayOfString[1]);
        //}

        // POST: Seal/Create
        [HttpPost]
        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentSealsCreate")]
        public ActionResult Create(SealDocument Seal, IEnumerable<HttpPostedFileBase> SealFiles)
        {
            if (SealFiles != null)
            {
                foreach (var file in SealFiles)
                {
                    if (!CheckFileFormatting.PermissionFile(file))
                    {
                        ModelState.AddModelError("File", "صيغة الملف غير مدعومة!");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                Seal.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                Seal.CreatedById = User.Identity.GetUserId();

                // Encrypt Document Attributes.
                if (ManagedAes.IsCipher)
                {
                    Seal.CreatedAt = ManagedAes.EncryptText(Seal.CreatedAt);
                    Seal.Message = ManagedAes.EncryptText(Seal.Message);
                }

                _context.SealDocuments.Add(Seal);
                _context.SaveChanges();

                if (ManagedAes.IsSaveInDb)
                {
                    // start code get image from scanner, save it in database.
                    var scannedImages = Request.Form.GetValues("myfile");
                    if (scannedImages != null)
                    {
                        int i = 0;
                        foreach (var ImgStr in scannedImages)
                        {
                            i++;
                            var sealFiles = new SealFiles();

                            sealFiles.SealId = Seal.Id;

                            string imageName = "scannedImage" + i + ".jpg";

                            var imgAsByteArray = ManagedAes.DecodeUrlBase64(ImgStr);

                            if (ManagedAes.IsCipher)
                            {
                                // Encrypt File Name.
                                sealFiles.FileName = ManagedAes.EncryptText(imageName);
                                // Encrypt File.
                                var EncryptedImgAsByteArray = ManagedAes.EncryptArrayByte(imgAsByteArray);
                                sealFiles.File = new byte[EncryptedImgAsByteArray.Length];
                                sealFiles.File = EncryptedImgAsByteArray;
                            }
                            else
                            {
                                // File Name.
                                sealFiles.FileName = imageName;
                                // File.
                                sealFiles.File = new byte[imgAsByteArray.Length];
                                sealFiles.File = imgAsByteArray;
                            }

                            _context.SealFiles.Add(sealFiles);
                            _context.SaveChanges();
                        }
                    }
                    // .// end code // get image from scanner, save it in server.


                    foreach (HttpPostedFileBase file in SealFiles)
                    {
                        if (file != null)
                        {
                            var sealFiles = new SealFiles();

                            sealFiles.SealId = Seal.Id;

                            string FileName = Path.GetFileName(file.FileName);
                            sealFiles.FileName = FileName;

                            sealFiles.File = new byte[file.ContentLength];
                            file.InputStream.Read(sealFiles.File, 0, file.ContentLength);

                            if (ManagedAes.IsCipher)
                            {
                                // Encrypt File Name.
                                sealFiles.FileName = ManagedAes.EncryptText(FileName);
                                // Encrypt File.
                                sealFiles.File = ManagedAes.EncryptArrayByte(sealFiles.File);
                            }

                            _context.SealFiles.Add(sealFiles);
                            _context.SaveChanges();
                        }
                    }
                }
                else
                {
                    // start code get image from scanner, save it in server
                    var scannedImages = Request.Form.GetValues("myfile");
                    if (scannedImages != null)
                    {
                        int i = 0;
                        foreach (var ImgStr in scannedImages)
                        {
                            i++;
                            String path = Server.MapPath("~/Uploads"); //Path

                            //Check if directory exist
                            if (!System.IO.Directory.Exists(path))
                            {
                                System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                            }
                            string imageName = "scannedImage" + i + ".jpg";
                            string s1 = DateTime.Now.ToString("yyyyMMddhhHHmmss") + imageName;

                            //set the image path
                            string imgPath = Path.Combine(path, imageName);
                            byte[] imageBytes = ManagedAes.DecodeUrlBase64(ImgStr);

                            var sealFiles = new SealFiles();
                            sealFiles.SealId = Seal.Id;

                            if (ManagedAes.IsCipher)
                            {
                                ManagedAes.EncryptFile(imageBytes, imgPath);
                                imageName = ManagedAes.EncryptText(imageName);
                                s1 = ManagedAes.EncryptText(s1);
                            }
                            else
                            {
                                System.IO.File.WriteAllBytes(imgPath, imageBytes);
                            }

                            sealFiles.FileName = imageName;

                            sealFiles.FileUrl = s1;

                            _context.SealFiles.Add(sealFiles);
                            _context.SaveChanges();
                        }
                    }
                    // .// end code // get image from scanner, save it in server.

                    foreach (var file in SealFiles)
                    {
                        if (file != null)
                        {
                            var sealFiles = new SealFiles();
                            sealFiles.SealId = Seal.Id;

                            //Save File In Uploads
                            string FileName = Path.GetFileName(file.FileName);

                            string s1 = DateTime.Now.ToString("yyyyMMddhhHHmmss") + FileName;
                            string path = Path.Combine(Server.MapPath("~/Uploads"), s1);

                            if (ManagedAes.IsCipher)
                            {
                                ManagedAes.EncryptFile(file, path);
                                FileName = ManagedAes.EncryptText(FileName);
                                s1 = ManagedAes.EncryptText(s1);
                            }
                            else
                            {
                                file.SaveAs(path);
                            }

                            sealFiles.FileName = FileName;

                            sealFiles.FileUrl = s1;

                            _context.SealFiles.Add(sealFiles);
                            _context.SaveChanges();
                        }
                    }
                }

                return RedirectToAction("Index", new { id = Seal.DocumentId });
            }
            return View(Seal);
        }
    

        //// POST: Seal/Edit/5
        //[HttpPost]
        //
        //[AccessDeniedAuthorizeattribute(ActionName = "DocumentSealsEdit")]
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return RedirectToAction("BadRequestError", "ErrorController");
        //    }

        //    var seal = _context.SealDocuments.Find(id);

        //    if (seal == null)
        //    {
        //        return RedirectToAction("HttpNotFoundError", "ErrorController");
        //    }

        //    return View(seal);
        //}
        //// POST: Seal/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}



        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentSealsDelete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            var seal = _context.SealDocuments.Find(id);

            if (seal == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            if (ManagedAes.IsCipher)
            {
                seal.CreatedAt = ManagedAes.DecryptText(seal.CreatedAt);
                seal.FileName = ManagedAes.DecryptText(seal.FileName);
                seal.Message = ManagedAes.DecryptText(seal.Message);
            }

            return View(seal);
        }

        // POST: Seal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentSealsDelete")]
        public ActionResult Confirm(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            var seal = _context.SealDocuments.Find(id);

            if (seal == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            if (ManagedAes.IsCipher)
            {
                seal.CreatedAt = ManagedAes.DecryptText(seal.CreatedAt);
                seal.FileName = ManagedAes.DecryptText(seal.FileName);
                seal.Message = ManagedAes.DecryptText(seal.Message);
            }

            _context.SealDocuments.Remove(seal);
            _context.SaveChanges();

            return RedirectToAction("Index", new { id = seal.DocumentId });
        }

        
        public FileResult DownloadDocument(int? id)
        {
            var SealFiles = _context.SealFiles.Find(id);
            var fileName = SealFiles.FileName;

            if (!ManagedAes.IsSaveInDb)
            {
                if (ManagedAes.IsCipher)
                {
                    SealFiles.FileUrl = ManagedAes.DecryptText(SealFiles.FileUrl);
                    fileName = ManagedAes.DecryptText(fileName);
                }

                string filePath = Server.MapPath("~/Uploads/").Replace(@"\", "/") + SealFiles.FileUrl;
                byte[] fileBytes = ManagedAes.DecryptFile(filePath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            else
            {
                if (ManagedAes.IsCipher)
                {
                    fileName = ManagedAes.DecryptText(SealFiles.FileName);
                    SealFiles.File = ManagedAes.DecryptArrayByte(SealFiles.File);
                }

                return File(SealFiles.File, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
        }

        
        public FileResult DisplayDocument(int? id)
        {
            var SealFiles = _context.SealFiles.Find(id);
            var fileName = SealFiles.FileName;

            if (!ManagedAes.IsSaveInDb)
            {
                if (ManagedAes.IsCipher)
                {
                    SealFiles.FileUrl = ManagedAes.DecryptText(SealFiles.FileUrl);
                    fileName = ManagedAes.DecryptText(fileName);
                }

                string filePath = Server.MapPath("~/Uploads/").Replace(@"\", "/") + SealFiles.FileUrl;

                if (ManagedAes.IsCipher)
                {
                    var file = ManagedAes.DecryptFile(filePath);
                    MemoryStream memStream = new MemoryStream();
                    BinaryFormatter binForm = new BinaryFormatter();
                    memStream.Write(file, 0, file.Length);
                    memStream.Seek(0, SeekOrigin.Begin);

                    // Images
                    if (filePath.EndsWith("jpeg") || filePath.EndsWith("JPEG"))
                        return new FileStreamResult(memStream, "image/jpeg");

                    if (filePath.EndsWith("jpg") || filePath.EndsWith("JPG"))
                        return new FileStreamResult(memStream, "image/jpg");

                    if (filePath.EndsWith("png") || filePath.EndsWith("PNG"))
                        return new FileStreamResult(memStream, "image/png");

                    if (filePath.EndsWith("gif") || filePath.EndsWith("GIF"))
                        return new FileStreamResult(memStream, "image/gif");

                    // Files
                    if (filePath.EndsWith("pdf") || filePath.EndsWith("PDF"))
                        return new FileStreamResult(memStream, "application/pdf");

                    if (filePath.EndsWith("doc") || filePath.EndsWith("DOC") ||
                        filePath.EndsWith("docx") || filePath.EndsWith("DOCX") ||
                        filePath.EndsWith("xlsx") || filePath.EndsWith("XLSX") ||
                        filePath.EndsWith("pptx") || filePath.EndsWith("PPTX"))
                        return new FileStreamResult(memStream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                }

                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                // Images
                if (filePath.EndsWith("jpeg") || filePath.EndsWith("JPEG"))
                    return new FileStreamResult(fileStream, "image/jpeg");

                if (filePath.EndsWith("jpg") || filePath.EndsWith("JPG"))
                    return new FileStreamResult(fileStream, "image/jpg");

                if (filePath.EndsWith("png") || filePath.EndsWith("PNG"))
                    return new FileStreamResult(fileStream, "image/png");

                if (filePath.EndsWith("gif") || filePath.EndsWith("GIF"))
                    return new FileStreamResult(fileStream, "image/gif");

                // Files
                if (filePath.EndsWith("pdf") || filePath.EndsWith("PDF"))
                    return new FileStreamResult(fileStream, "application/pdf");

                if (filePath.EndsWith("doc") || filePath.EndsWith("DOC") ||
                    filePath.EndsWith("docx") || filePath.EndsWith("DOCX") ||
                    filePath.EndsWith("xlsx") || filePath.EndsWith("XLSX") ||
                    filePath.EndsWith("pptx") || filePath.EndsWith("PPTX"))
                    return new FileStreamResult(fileStream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            }
            else
            {
                var file = SealFiles.File;

                if (ManagedAes.IsCipher)
                {
                    fileName = ManagedAes.DecryptText(SealFiles.FileName);
                    file = ManagedAes.DecryptArrayByte(file);
                }

                // Images
                if (fileName.EndsWith("jpeg") || fileName.EndsWith("JPEG"))
                    return File(file, "image/jpeg");

                if (fileName.EndsWith("jpg") || fileName.EndsWith("JPG"))
                    return File(file, "image/jpg");

                if (fileName.EndsWith("png") || fileName.EndsWith("PNG"))
                    return File(file, "image/png");

                if (fileName.EndsWith("gif") || fileName.EndsWith("GIF"))
                    return File(file, "image/gif");

                // Pdf
                if (fileName.EndsWith("pdf") || fileName.EndsWith("PDF"))
                    return File(file, "application/pdf");

            }

            return DownloadDocument(id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
