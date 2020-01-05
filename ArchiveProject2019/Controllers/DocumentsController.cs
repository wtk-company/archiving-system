using ArchiveProject2019.Models;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArchiveProject2019.ViewModel;
using Microsoft.AspNet.Identity;

using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using ArchiveProject2019.HelperClasses;
using System.Globalization;
using System.Diagnostics;
using ArchiveProject2019.Security;
using System.Runtime.Serialization.Formatters.Binary;

namespace ArchiveProject2019.Controllers
{
    public class DocumentsController : Controller
    {
        

        ApplicationDbContext _context;

        public DocumentsController()
        {
            this._context = new ApplicationDbContext();
        }


        

        [AccessDeniedAuthorizeattribute(ActionName = "DocumentCreate")]

        public ActionResult Form()
        {
            ViewBag.Current = "Document";

            var viewModel = new DocFromsViewModel()
            {
                DocId = -1,
                Froms = UsersDepartmentAndGroupsForms.GetUsersForms(this.User.Identity.GetUserId()),
                IsReplay = false,
            };

            return View(viewModel);
        }



        
        [AccessDeniedAuthorizeattribute(ActionName = "RelateDocumentCreate")]
        public ActionResult RelateDocument(int id)
        {
            ViewBag.Current = "Document";

            var viewModel = new DocFromsViewModel()
            {
                DocId = id,
                Froms = UsersDepartmentAndGroupsForms.GetUsersForms(this.User.Identity.GetUserId()),
                IsReplay = false,
            };

            return View("Form", viewModel);
        }

        
        [AccessDeniedAuthorizeattribute(ActionName = "ReplayDocumentCreate")]
        public ActionResult ReplayDocument(int id)
        {
            ViewBag.Current = "Document";

            var viewModel = new DocFromsViewModel()
            {
                DocId = id,
                Froms = UsersDepartmentAndGroupsForms.GetUsersForms(this.User.Identity.GetUserId()),
                IsReplay = true,
            };

            return View("Form", viewModel);
        }

        // GET: /Documents/Create

        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentCreate")]
        public ActionResult Create(int Id = 0, int docId = -1, bool IsReplay = false, int Standard = -1,bool IsGeneralize=false)
        {
            ViewBag.Current = "Document";

            string CurrentUser = this.User.Identity.GetUserId();
            if (Standard != -1)
            {
                if (Standard == 0)
                {

                    Id = _context.Forms.Where(a => a.Type == 1).FirstOrDefault().Id;
                }
                else
                {
                    Id = Standard;
                }
            }
            var Fields = _context.Fields.Include(c => c.Form).Where(f => f.FormId == Id).ToList();

            List<Value> Values = new List<Value>();

            foreach (var field in Fields)
            {
                var value = new Value
                {
                    FieldId = field.Id,
                };

                Values.Add(value);
            }

            FieldsValuesViewModel viewModel = new FieldsValuesViewModel
            {
                Fields = Fields,
                Values = Values
            };

            var myModel = new DocumentDocIdFieldsValuesViewModel()
            {
                DocId = docId,
                Document = new Models.Document() { FormId = Id },
                FieldsValues = viewModel,
                IsReplay = IsReplay,
            };



            ViewBag.Gereralize = IsGeneralize;

            ViewBag.Parties = new SelectList(_context.Parties.ToList(), "Id", "Name");


            //Asmi :
            ViewBag.TypeMailId = new SelectList(_context.TypeMails.ToList(), "Id", "Name");
            ViewBag.kinds = new SelectList(_context.Kinds.ToList(), "Id", "Name");
            ViewBag.RelatedGroups = new SelectList(_context.Groups.ToList(), "Id", "Name");
            ViewBag.StatusId = new SelectList(_context.DocumentStatuses.ToList(), "Id", "Name");
            ViewBag.RelatedDepartments = new SelectList(DepartmentListDisplay.CreateDepartmentListDisplay(), "Id", "Name");
            ViewBag.RelatedUsers = new SelectList(_context.Users.Where(a => !a.RoleName.Equals("Master")).ToList(), "Id", "FullName");
            ViewBag.ResponsibleUserId = new SelectList(_context.Users.Where(a => !a.RoleName.Equals("Master")).ToList(), "Id", "FullName");
            return View(myModel);
        }

        [NonAction]
        public static byte[] DecodeUrlBase64(string s)
        {
            //s = s.Replace('-', '+').Replace('_', '/').PadRight(4 * ((s.Length + 3) / 4), '=');
            var arrayOfString = s.Split(',');

            return Convert.FromBase64String(arrayOfString[1]);
        }


        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentCreate")]
        public ActionResult Create(DocumentDocIdFieldsValuesViewModel viewModel, IEnumerable<HttpPostedFileBase> UploadFile, IEnumerable<HttpPostedFileBase> FieldFile, IEnumerable<string> PartyIds, IEnumerable<string> RelatedGroups, IEnumerable<string> RelatedDepartments, IEnumerable<string> RelatedUsers)
        {
            ViewBag.Current = "Document";

            string CurrentUser = this.User.Identity.GetUserId();
            int UserDepId = _context.Users.Find(CurrentUser).DepartmentId.Value;
            bool Status = true;



            FieldsValuesViewModel FVVM = new FieldsValuesViewModel();
            FVVM = viewModel.FieldsValues;


            if (FVVM != null)
            {

                for (int i = 0; i < FVVM.Values.Count(); i++)
                {

                    if ((!FVVM.Fields[i].Type.Equals("file")) && FVVM.Fields[i].IsRequired && FVVM.Values[i].FieldValue == null)
                    {
                        ModelState.AddModelError("FieldsValues.Values[" + i + "].Id", "يجب إدخال قيمة الحقل، لا يمكن أن يكون فارغ");
                        Status = false;
                    }

                }



                int k = 0;
                for (int i = 0; i < FVVM.Values.Count(); i++)
                {

                    if (FVVM.Fields[i].Type.Equals("file"))
                    {
                        if (FVVM.Fields[i].IsRequired == true)
                        {
                            if (FieldFile.ElementAt(k) == null)
                            {

                                ModelState.AddModelError("FieldsValues.Values[" + i + "].Id", "يجب إختيار ملف محدد، لا يمكن أن يكون فارغ");
                                Status = false;
                            }
                        }

                        k++;

                    }
                }




                //فقط سوف يتم اختبار الحقل من النمط الرقمي والأيميل ورقم الهاتف 
                for (int i = 0; i < viewModel.FieldsValues.Values.Count(); i++)
                {


                    if ((viewModel.FieldsValues.Fields[i].Type.Equals("float")) && viewModel.FieldsValues.Values[i].FieldValue != null)
                    {

                        if (CheckFileFormatting.IsFloat(viewModel.FieldsValues.Values[i].FieldValue) == false)
                        {
                            ModelState.AddModelError("FieldsValues.Values[" + i + "].Id", "يجب ان يكون قيمةالحقل رقمي، الرجاء إعادة الإدخال");

                            Status = false;
                        }
                    }



                    if ((viewModel.FieldsValues.Fields[i].Type.Equals("email")) && viewModel.FieldsValues.Values[i].FieldValue != null)
                    {

                        if (CheckFileFormatting.IsEmail(viewModel.FieldsValues.Values[i].FieldValue) == false)
                        {
                            ModelState.AddModelError("FieldsValues.Values[" + i + "].Id", "يجب ان يكون قيمةالحقل بريد إلكتروني، الرجاء إعادة الإدخال");

                            Status = false;
                        }
                    }


                    if ((viewModel.FieldsValues.Fields[i].Type.Equals("phone")) && viewModel.FieldsValues.Values[i].FieldValue != null)
                    {

                        if (CheckFileFormatting.IsPhone(viewModel.FieldsValues.Values[i].FieldValue) == false)
                        {
                            ModelState.AddModelError("FieldsValues.Values[" + i + "].Id", "يجب ان يكون قيمةالحقل رقم هاتف، الرجاء إعادة الإدخال");

                            Status = false;
                        }
                    }
                }




            }//End if 


            //Check Mail numbare and mail date:
            TypeMail mail = _context.TypeMails.Find(viewModel.Document.TypeMailId);

            if (mail != null && mail.Type == 1 && viewModel.Document.MailingDate == null)
            {
                ModelState.AddModelError("Document.MailingDate", "ادخل تاريخ ورود البريد");
                Status = false;
            }

            if (mail != null && mail.Type == 1 && viewModel.Document.MailingNumber == null)
            {
                ModelState.AddModelError("Document.MailingNumber", "ادخل رقم ورود البريد ");
                Status = false;
            }

            if (mail != null && mail.Type.Equals(2) && PartyIds == null)
            {
                ModelState.AddModelError("PartyIds", "حدد جهات استلام البريد");
                Status = false;
            }

            var lasFile = UploadFile.Last();
            foreach (HttpPostedFileBase file in UploadFile)
            {
                //Image Extentions:
                bool ImageExtention = CheckFileFormatting.PermissionFile(file);

                if (ImageExtention == false)
                {
                    Status = false;
                    ModelState.AddModelError("Document.FileUrl", "صيغة الملف المحمل غير مدعومة أعد الإدخال مرة أخرى");
                    return View(viewModel);
                }
            }







            //Error:
            ViewBag.TypeMailId = new SelectList(_context.TypeMails.ToList(), "Id", "Name", viewModel.Document.TypeMailId);
            ViewBag.kinds = new SelectList(_context.Kinds.ToList(), "Id", "Name", viewModel.Document.KindId);
            ViewBag.RelatedGroups = new SelectList(_context.Groups.ToList(), "Id", "Name");
            ViewBag.StatusId = new SelectList(_context.DocumentStatuses.ToList(), "Id", "Name", viewModel.Document.StatusId);
            ViewBag.RelatedDepartments = new SelectList(DepartmentListDisplay.CreateDepartmentListDisplay().Where(a=>a.Id!= UserDepId), "Id", "Name");
            ViewBag.RelatedUsers = new SelectList(_context.Users.Where(a => !a.RoleName.Equals("Master")).ToList(), "Id", "FullName");
            ViewBag.ResponsibleUserId = new SelectList(_context.Users.Where(a => !a.RoleName.Equals("Master")).ToList(), "Id", "FullName", viewModel.Document.ResponsibleUserId);



            if (Status == false)
            {
                return View(viewModel);
            }

            if (ModelState.IsValid)
            {

                // Get Current User Id
                var UserId = User.Identity.GetUserId();

                if (!ManagedAes.IsSaveInDb)
                {


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

                            if (ManagedAes.IsCipher)
                            {
                                ManagedAes.EncryptFile(imageBytes, imgPath);
                            }
                            else
                            {
                                System.IO.File.WriteAllBytes(imgPath, imageBytes);
                            }

                            viewModel.Document.Name += imageName + "_##_";
                            viewModel.Document.FileUrl += s1 + "_##_";
                        }
                    }



                    if (UploadFile != null)
                    {


                        foreach (var file in UploadFile)
                        {
                            if (file != null)
                            {
                                //Save File In Uploads
                                string FileName = Path.GetFileName(file.FileName);

                                string s1 = DateTime.Now.ToString("yyyyMMddhhHHmmss") + FileName;
                                string path = Path.Combine(Server.MapPath("~/Uploads"), s1);

                                if (ManagedAes.IsCipher)
                                {
                                    ManagedAes.EncryptFile(file, path);
                                }
                                else
                                {
                                    file.SaveAs(path);
                                }


                                viewModel.Document.Name += FileName + "_##_";
                                viewModel.Document.FileUrl += s1 + "_##_";
                            }
                        }

                        // Cut last 4 split string
                        if (viewModel.Document.Name != null)
                        {
                            viewModel.Document.Name = viewModel.Document.Name.Substring(0, viewModel.Document.Name.Length - 4);
                            viewModel.Document.FileUrl = viewModel.Document.FileUrl.Substring(0, viewModel.Document.FileUrl.Length - 4);
                        }
                    }
                }
                // Document Details:
                viewModel.Document.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                viewModel.Document.CreatedById = UserId;
                viewModel.Document.FormId = viewModel.Document.FormId;
                viewModel.Document.DepartmentId = _context.Users.Find(CurrentUser).DepartmentId.Value;



                if (viewModel.Document.ResponsibleUserId == null)
                {
                    viewModel.Document.NotificationUserId = UserId;
                    viewModel.Document.ResponsibleUserId = UserId;
                } else {
                    viewModel.Document.NotificationUserId = viewModel.Document.ResponsibleUserId;
                }

                // Encrypt Document Attributes.
                if (ManagedAes.IsCipher)
                {
                    viewModel.Document.Address = ManagedAes.EncryptText(viewModel.Document.Address);
                    viewModel.Document.CreatedAt = ManagedAes.EncryptText(viewModel.Document.CreatedAt);
                    viewModel.Document.Description = ManagedAes.EncryptText(viewModel.Document.Description);
                    viewModel.Document.DocumentDate = ManagedAes.EncryptText(viewModel.Document.DocumentDate);
                    viewModel.Document.DocumentNumber = ManagedAes.EncryptText(viewModel.Document.DocumentNumber);
                    viewModel.Document.FileUrl = ManagedAes.EncryptText(viewModel.Document.FileUrl);
                    viewModel.Document.MailingDate = ManagedAes.EncryptText(viewModel.Document.MailingDate);
                    viewModel.Document.MailingNumber = ManagedAes.EncryptText(viewModel.Document.MailingNumber);
                    viewModel.Document.Name = ManagedAes.EncryptText(viewModel.Document.Name);
                    viewModel.Document.Notes = ManagedAes.EncryptText(viewModel.Document.Notes);
                    viewModel.Document.NotificationDate = ManagedAes.EncryptText(viewModel.Document.NotificationDate);
                    viewModel.Document.Subject = ManagedAes.EncryptText(viewModel.Document.Subject);
                    viewModel.Document.UpdatedAt = ManagedAes.EncryptText(viewModel.Document.UpdatedAt);
                }

                _context.Documents.Add(viewModel.Document);
                _context.SaveChanges();

                // Save Multiple Files In Db (begin)
                if (ManagedAes.IsSaveInDb)
                {
                    
                    var scannedImages = Request.Form.GetValues("myfile");
                    if (scannedImages != null)
                    {
                        int i = 0;
                        foreach (var ImgStr in scannedImages)
                        {
                            i++;
                            var fileStoredInDb = new FilesStoredInDb();

                            fileStoredInDb.DocumentId = viewModel.Document.Id;

                            string imageName = "scannedImage" + i + ".jpg";

                            var imgAsByteArray = ManagedAes.DecodeUrlBase64(ImgStr);

                            if (ManagedAes.IsCipher)
                            {
                                // Encrypt File Name.
                                fileStoredInDb.FileName = ManagedAes.EncryptText(imageName);
                                // Encrypt File.
                                var EncryptedImgAsByteArray = ManagedAes.EncryptArrayByte(imgAsByteArray);
                                fileStoredInDb.File = new byte[EncryptedImgAsByteArray.Length];
                                fileStoredInDb.File = EncryptedImgAsByteArray;
                            }
                            else
                            {
                                // File Name.
                                fileStoredInDb.FileName = imageName;
                                // File.
                                fileStoredInDb.File = new byte[imgAsByteArray.Length];
                                fileStoredInDb.File = imgAsByteArray;
                            }

                            _context.FilesStoredInDbs.Add(fileStoredInDb);
                            _context.SaveChanges();
                        }
                    }
             

                    foreach (HttpPostedFileBase file in UploadFile)
                    {
                        if (file != null)
                        {
                            var fileStoredInDb = new FilesStoredInDb();

                            fileStoredInDb.DocumentId = viewModel.Document.Id;

                            string FileName = Path.GetFileName(file.FileName);
                            fileStoredInDb.FileName = FileName;

                            fileStoredInDb.File = new byte[file.ContentLength];
                            file.InputStream.Read(fileStoredInDb.File, 0, file.ContentLength);

                            if (ManagedAes.IsCipher)
                            {
                                // Encrypt File Name.
                                fileStoredInDb.FileName = ManagedAes.EncryptText(FileName);
                                // Encrypt File.
                                fileStoredInDb.File = ManagedAes.EncryptArrayByte(fileStoredInDb.File);
                            }

                            _context.FilesStoredInDbs.Add(fileStoredInDb);
                            _context.SaveChanges();
                        }
                    }
                }
                // ./-- Save Multiple Files In Db (end)


                //==========RelatedDepartments && Related Group======================

                //Notification time:
                string NotificationTime = string.Empty;
                //List of users id:
                List<string> UsersId = new List<string>();



                List<string> RelatedDep = new List<string>();
                if(RelatedDepartments!=null)
                {

                    RelatedDep= RelatedDepartments.ToList();
                }
                RelatedDep.Add(UserDepId.ToString());



                if (RelatedDep != null)
                {

                    foreach (string Department_Id in RelatedDep)
                    {
                        var _DocumentDepartment = new DocumentDepartment()
                        {

                            EnableEdit = true,
                            EnableRelate = true,
                            EnableReplay = true,
                            EnableSeal = true,
                            DocumentId = viewModel.Document.Id,
                            DepartmentId = Convert.ToInt32(Department_Id),
                            CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                            CreatedById = this.User.Identity.GetUserId()
                        };

                        _context.DocumentDepartments.Add(_DocumentDepartment);
                        _context.SaveChanges();

                        //Notifications:
                        NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                        int Dep_Id = Convert.ToInt32(Department_Id);
                        //Users in this Department:
                        UsersId = _context.Users.Where(a => a.DepartmentId == Dep_Id).Select(a => a.Id).ToList();
                        //Notification object:
                        Notification notification = null;


                        //Users in this department:
                        List<ApplicationUser> Users = _context.Users.Where(a => UsersId.Contains(a.Id)).ToList();
                        foreach (ApplicationUser user in Users)
                        {

                            notification = new Notification()
                            {

                                CreatedAt = NotificationTime,
                                Active = false,
                                UserId = user.Id,
                                Message = "تم إضافة وثيقة جديدة للقسم الحالي، رقم الوثيقة :" + viewModel.Document.DocumentNumber + " موضوع الوثيقة :" + viewModel.Document.Subject
                                + " ،عنوان الوثيقة :" + viewModel.Document.Address + "،وصف الوثيقة :" + viewModel.Document.Description
                               ,
                                NotificationOwnerId = UserId
                            };
                            _context.Notifications.Add(notification);
                        }



                    }
                }
                if (RelatedGroups != null)
                {

                    foreach (string Group_Id in RelatedGroups)
                    {
                        var _DocumentGroup = new DocumentGroup()
                        {

                            EnableEdit = true,
                            EnableRelate = true,
                            EnableReplay = true,
                            EnableSeal = true,
                            DocumentId = viewModel.Document.Id,
                            GroupId = Convert.ToInt32(Group_Id),
                            CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                            CreatedById = this.User.Identity.GetUserId()
                        };

                        _context.DocumentGroups.Add(_DocumentGroup);
                        _context.SaveChanges();

                        //Notifications:

                        NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                        int GroupId = Convert.ToInt32(Group_Id);
                        UsersId = _context.UsersGroups.Where(a => a.GroupId == GroupId).Select(a => a.UserId).ToList();

                        string GroupName = _context.Groups.Find(GroupId).Name;
                        Notification notification = null;

                        List<ApplicationUser> Users = _context.Users.Where(a => UsersId.Contains(a.Id)).ToList();
                        foreach (ApplicationUser user in Users)
                        {

                            notification = new Notification()
                            {

                                CreatedAt = NotificationTime,
                                Active = false,
                                UserId = user.Id,
                                Message = "تم إضافة وثيقة جديدة للمجموعة :" + GroupName + "، رقم الوثيقة :" + viewModel.Document.Name + " ، موضوع الوثيقة:" + viewModel.Document.Subject +
                                " ، عنوان الوثيقة:" + viewModel.Document.Address + " ،وصف الوثيقة :" + viewModel.Document.Description
                               ,
                                NotificationOwnerId = UserId
                            };
                            _context.Notifications.Add(notification);
                        }

                    }
                }


                if (RelatedUsers != null)
                {

                    foreach (string User_Id in RelatedUsers)
                    {
                        var _DocumentUser = new DocumentUser()
                        {

                            EnableEdit = true,
                            EnableRelate = true,
                            EnableReplay = true,
                            EnableSeal = true,
                            DocumentId = viewModel.Document.Id,
                            UserId = User_Id,
                            CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                            CreatedById = this.User.Identity.GetUserId()
                        };

                        _context.DocumentUsers.Add(_DocumentUser);
                        _context.SaveChanges();

                        //Notifications:
                        NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");


                        //Notification object:
                        Notification notification = null;




                        notification = new Notification()
                        {

                            CreatedAt = NotificationTime,
                            Active = false,
                            UserId = User_Id,
                            Message = "تم إضافة وثيقة جديدة  ، رقم الوثيقة :" + viewModel.Document.DocumentNumber + " موضوع الوثيقة :" + viewModel.Document.Subject
                            + " ،عنوان الوثيقة :" + viewModel.Document.Address + "،وصف الوثيقة :" + viewModel.Document.Description
                           ,
                            NotificationOwnerId = UserId
                        };
                        _context.Notifications.Add(notification);




                    }
                }
                _context.SaveChanges();

                //==================================== end =========================




                if (FVVM != null)
                {
                    //values details:
                    foreach (var value in viewModel.FieldsValues.Values)
                    {
                        // Craete Date
                        value.Document_id = viewModel.Document.Id;
                        value.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                        value.CreatedById = UserId;

                        _context.Values.Add(value);
                        _context.SaveChanges();
                    }
                }
                // store outgoing parties
                if (PartyIds != null && mail.Type == 2)
                {
                    foreach (string partyId in PartyIds)
                    {
                        var DocParty = new DocumentParty()
                        {
                            DocumentId = viewModel.Document.Id,
                            PartyId = Convert.ToInt32(partyId),
                            CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                            CreatedById = UserId
                        };

                        _context.DocumentParties.Add(DocParty);
                        _context.SaveChanges();
                    }
                }
                // document famely status (begin)
                var parentDocId = viewModel.DocId;

                // Relate Document
                if (parentDocId != -1 && !viewModel.IsReplay)
                {
                    var relateDoc = new RelatedDocument()
                    {
                        RelatedDocId = viewModel.Document.Id,
                        CreatedById = UserId,
                        Document_id = parentDocId,
                    };

                    _context.RelatedDocuments.Add(relateDoc);
                    _context.SaveChanges();

                    return RedirectToAction("GetRelatedDocument", new { Id = parentDocId, msg = "CreateSuccess" });

                }

                // Replay Document
                if (parentDocId != -1 && viewModel.IsReplay)
                {
                    var relateDoc = new ReplayDocument()
                    {
                        ReplayDocId = viewModel.Document.Id,
                        CreatedById = UserId,
                        Document_id = parentDocId,
                    };

                    _context.ReplayDocuments.Add(relateDoc);
                    _context.SaveChanges();

                    return RedirectToAction("GetReplayDocument", new { Id = parentDocId, msg = "CreateSuccess" });

                }
                return RedirectToAction("Index", new { id = viewModel.Document.Id });
            }
            return View(viewModel);
        }



        [HttpGet]


        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentEdit")]
        public ActionResult Edit(int? id)
        {

            string CurrentUser = this.User.Identity.GetUserId();
            ViewBag.Current = "Document";




            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            var Document = _context.Documents.Include(a => a.Department).Include(d => d.FilesStoredInDbs).Include(dk => dk.Kind).Include(p => p.Party).Include(t => t.TypeMail).Include(a => a.Values).Include(a => a.Form).FirstOrDefault(a => a.Id == id);
            if (Document == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            ViewBag.CanEdit = false;

            if (Document.CreatedById.Equals(CurrentUser))
            {
                ViewBag.CanEdit = true;
            }

            if (!string.IsNullOrEmpty(Document.ResponsibleUserId))
            {
                if (Document.ResponsibleUserId.Equals(CurrentUser))
                {
                    ViewBag.CanEdit = true;

                }
            }

            var Fields = _context.Fields.Include(c => c.Form).Where(f => f.FormId == Document.FormId).ToList();
            List<Value> Values = new List<Value>();
            Values = Document.Values.ToList();
            FieldsValuesViewModel viewModel = new FieldsValuesViewModel
            {
                Fields = Fields,
                Values = Values
            };


            if (_context.Users.Find(CurrentUser).DepartmentId.HasValue)
            {
                int Dep_Id = _context.Users.Find(CurrentUser).DepartmentId.Value;
                ViewBag.Departments = new SelectList(_context.Departments.Where(a => a.Id == Dep_Id).ToList(), "Id", "Name");

            }
            else
            {
                ViewBag.Departments = new SelectList(_context.Departments.ToList(), "Id", "Name", Document.DepartmentId);

            }
            ViewBag.Parties = new SelectList(_context.Parties.ToList(), "Id", "Name", Document.PartyId);
            ViewBag.TypeMailId = new SelectList(_context.TypeMails.ToList(), "Id", "Name");
            ViewBag.Groups = new SelectList(_context.Groups.ToList(), "Id", "Name");
            ViewBag.DepartmentList = new SelectList(_context.Departments.ToList(), "Id", "Name");


            //[Asmi new]:
            ViewBag.StatusId = new SelectList(_context.DocumentStatuses.ToList(), "Id", "Name", Document.StatusId);
            ViewBag.kinds = new SelectList(_context.Kinds.ToList(), "Id", "Name", Document.KindId);

            ViewBag.ResponsibleUserId = new SelectList(_context.Users.Where(a => !a.RoleName.Equals("Master")).ToList(), "Id", "FullName", Document.ResponsibleUserId);

            //[Related Departments]:


            //============== Related Users/Departments/Groups=============================
            SelectListItem sl;
            List<SelectListItem> ListSl = new List<SelectListItem>();
            foreach (var G in DepartmentListDisplay.CreateDepartmentListDisplay().Where(a=>a.Id!=Document.DepartmentId).ToList())
            {
                sl = new SelectListItem()
                {

                    Text = G.Name,
                    Value = G.Id.ToString(),
                    Selected = _context.DocumentDepartments.Any(a => a.DocumentId == Document.Id && a.DepartmentId == G.Id) ? true : false
                };

                ListSl.Add(sl);

            }
            ViewBag.RelatedDepartments = ListSl;




            //Related Groups:


            List<SelectListItem> ListS2 = new List<SelectListItem>();
            foreach (var G in _context.Groups.ToList())
            {
                sl = new SelectListItem()
                {

                    Text = G.Name,
                    Value = G.Id.ToString(),
                    Selected = _context.DocumentGroups.Any(a => a.DocumentId == Document.Id && a.GroupId == G.Id) ? true : false
                };

                ListS2.Add(sl);

            }
            ViewBag.RelatedGroups = ListS2;


            //RelatedUsers:
            List<SelectListItem> ListS3 = new List<SelectListItem>();
            foreach (var U in _context.Users.Where(a => !a.RoleName.Equals("Master")).ToList())
            {
                sl = new SelectListItem()
                {

                    Text = U.FullName,
                    Value = U.Id.ToString(),
                    Selected = _context.DocumentUsers.Any(a => a.DocumentId == Document.Id && a.UserId == U.Id) ? true : false
                };

                ListS3.Add(sl);

            }
            ViewBag.RelatedUsers = ListS3;



            //================= End Related Opertaions==================

            //Party ids:
            if(Document.TypeMailId==2)

            {

                List<SelectListItem> ListS4 = new List<SelectListItem>();
                foreach (var G in _context.Parties.ToList())

                {
                    sl = new SelectListItem()
                    {

                        Text = G.Name,
                        Value = G.Id.ToString(),
                        Selected = _context.DocumentParties.Any(a => a.DocumentId == Document.Id && a.PartyId == G.Id) ? true : false
                    };

                    ListS4.Add(sl);

                }
                ViewBag.PartyIds = ListS4;

            }

            //Party ids:
            if (Document.TypeMailId == 2)
            {

                List<SelectListItem> ListS4 = new List<SelectListItem>();
                foreach (var G in _context.Parties.ToList())

                {
                    sl = new SelectListItem()
                    {

                        Text = G.Name,
                        Value = G.Id.ToString(),
                        Selected = _context.DocumentParties.Any(a => a.DocumentId == Document.Id && a.PartyId == G.Id) ? true : false
                    };

                    ListS4.Add(sl);

                }
                ViewBag.PartyIds = ListS4;

            }
            // Decrypt Document Attributes.
            if (ManagedAes.IsCipher)
            {
                Document.Address = ManagedAes.DecryptText(Document.Address);
                Document.CreatedAt = ManagedAes.DecryptText(Document.CreatedAt);
                Document.Description = ManagedAes.DecryptText(Document.Description);
                Document.DocumentDate = ManagedAes.DecryptText(Document.DocumentDate);
                Document.DocumentNumber = ManagedAes.DecryptText(Document.DocumentNumber);
                Document.FileUrl = ManagedAes.DecryptText(Document.FileUrl);
                Document.MailingDate = ManagedAes.DecryptText(Document.MailingDate);
                Document.MailingNumber = ManagedAes.DecryptText(Document.MailingNumber);
                Document.Name = ManagedAes.DecryptText(Document.Name);
                Document.Notes = ManagedAes.DecryptText(Document.Notes);
                Document.NotificationDate = ManagedAes.DecryptText(Document.NotificationDate);
                Document.Subject = ManagedAes.DecryptText(Document.Subject);
                Document.UpdatedAt = ManagedAes.DecryptText(Document.UpdatedAt);


                var files = Document.FilesStoredInDbs.ToList();
                var filesStoredInDbs = new List<FilesStoredInDb>(files.Count);
                for (int i = 0; i < files.Count; i++)
                {
                    filesStoredInDbs.Add(
                        new FilesStoredInDb
                        {
                            FileName = ManagedAes.DecryptText(files[i].FileName),
                            DocumentId = files[i].DocumentId,
                            File = ManagedAes.DecryptArrayByte(files[i].File),
                            Id = files[i].Id
                        }
                        );
                }

                if (ManagedAes.IsSaveInDb)
                {
                    var names = Document.FilesStoredInDbs.Count;
                    var existfiles = Enumerable.Repeat(true, names).ToList();

                    var myModel = new DocumentDocIdFieldsValuesViewModel()
                    {
                        Document = Document,
                        FieldsValues = viewModel,
                        ExistFiles = existfiles,
                        FilesStoredInDbs = filesStoredInDbs,
                        TypeMail = Document.TypeMail.Type,
                        IsSaveInDb = ManagedAes.IsSaveInDb,
                    };

                    return View(myModel);
                }
                else
                {
                    var urls = Document.FileUrl.Split(new string[] { "_##_" }, StringSplitOptions.None);
                    var existfiles = Enumerable.Repeat(true, urls.Length).ToList();

                    var myModel = new DocumentDocIdFieldsValuesViewModel()
                    {
                        Document = Document,
                        FieldsValues = viewModel,
                        ExistFiles = existfiles,
                    };

                    return View(myModel);
                }

            }

            if (ManagedAes.IsSaveInDb)
            {
                var names = Document.FilesStoredInDbs.Count;
                var existfiles = Enumerable.Repeat(true, names).ToList();

                var myModel = new DocumentDocIdFieldsValuesViewModel()
                {
                    Document = Document,
                    FieldsValues = viewModel,
                    ExistFiles = existfiles,
                    FilesStoredInDbs = Document.FilesStoredInDbs.ToList(),
                    TypeMail = Document.TypeMail.Type,
                    IsSaveInDb = ManagedAes.IsSaveInDb,
                };

                return View(myModel);
            }
            else
            {
                var urls = Document.FileUrl.Split(new string[] { "_##_" }, StringSplitOptions.None);
                var existfiles = Enumerable.Repeat(true, urls.Length).ToList();

                var myModel = new DocumentDocIdFieldsValuesViewModel()
                {
                    Document = Document,
                    FieldsValues = viewModel,
                    ExistFiles = existfiles,
                };

                return View(myModel);
            }
        }



        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentEdit")]
        public ActionResult Edit(DocumentDocIdFieldsValuesViewModel viewModel, HttpPostedFileBase[] UploadFile, IEnumerable<HttpPostedFileBase> FieldFile
            , IEnumerable<string> RelatedDepartments, IEnumerable<string> RelatedGroups
            , IEnumerable<string> RelatedUsers,


            IEnumerable<string> PartyIds)
        {

            ViewBag.Current = "Document";
            string CurrentUser = this.User.Identity.GetUserId();
            bool Status = true;




            bool CanEdit = false;



            //ViewBag.CanEdit = false;

            //if (viewModel.Document.CreatedById.Equals(CurrentUser))
            //{
            //    CanEdit = true;
            //    ViewBag.CanEdit = true;
            //}

            //if (!string.IsNullOrEmpty(viewModel.Document.ResponsibleUserId))
            //{
            //    if (viewModel.Document.ResponsibleUserId.Equals(CurrentUser))
            //    {
            //        CanEdit = true;

            //        ViewBag.CanEdit = true;

            //    }
            //}

            if (viewModel.Document.TypeMailId == 2 && PartyIds == null)
            {
                ModelState.AddModelError("PartyIds", "حدد جهات استلام البريد");
                Status = false;
            }




            if (viewModel.Document.TypeMailId == 1 && viewModel.Document.MailingDate == null)
            {
                ModelState.AddModelError("Document.MailingDate", "ادخل تاريخ ورود البريد");
                Status = false;
            }

            if (viewModel.Document.TypeMailId == 1 && viewModel.Document.MailingNumber == null)
            {
                ModelState.AddModelError("Document.MailingNumber", "ادخل رقم ورود البريد ");
                Status = false;
            }

            FieldsValuesViewModel FVVM = new FieldsValuesViewModel();
            FVVM = viewModel.FieldsValues;

            if (FVVM != null)
            {
                //اختبار ضرورة الحقول عدا الحقل من النمط ملف، فيحتاج إلى معاملة خاصة

                for (int i = 0; i < viewModel.FieldsValues.Values.Count(); i++)
                {

                    if ((!viewModel.FieldsValues.Fields[i].Type.Equals("file")) && viewModel.FieldsValues.Fields[i].IsRequired && viewModel.FieldsValues.Values[i].FieldValue == null)
                    {

                        ModelState.AddModelError("FieldsValues.Values[" + i + "].Id", "يجب إدخال قيمة الحقل، لا يمكن أن يكون فارغ");
                        Status = false;
                    }

                }

                //اختبار الحقل من النمط صورة إذا كان ضروري او لا

                //أختبار صيغة كل ملف من الملفات

                int j = 0;

                for (int i = 0; i < FVVM.Values.Count(); i++)
                {
                    if (FVVM.Fields[i].Type.Equals("file"))
                    {
                        if (FieldFile.ElementAt(j) != null)
                        {
                            bool Extention = CheckFileFormatting.PermissionFile(FieldFile.ElementAt(j));

                            if (Extention == false)
                            {
                                Status = false;
                                ModelState.AddModelError("FieldsValues.Values[" + i + "].Id", " صيغةالملف غير مدعومةالرجاء إعادةالإدخال");
                            }
                            else
                            {
                                string FileName = Path.GetFileName(FieldFile.ElementAt(j).FileName);

                                //Save In Server
                                string s1 = DateTime.Now.ToString("yyyyMMddhhHHmmss") + FileName;
                                string path = Path.Combine(Server.MapPath("~/Uploads"), s1);
                                FieldFile.ElementAt(j).SaveAs(path);

                                //Save In Db:
                                viewModel.FieldsValues.Values[i].FieldValue = "~/Uploads/" + s1;
                            }
                            // else
                        }
                        // file Not null

                        j++;
                    }
                    // if {file}

                }

                //فقط سوف يتم اختبار الحقل من النمط الرقمي والأيميل ورقم الهاتف 
                for (int i = 0; i < viewModel.FieldsValues.Values.Count(); i++)
                {
                    if ((viewModel.FieldsValues.Fields[i].Type.Equals("float")) && viewModel.FieldsValues.Values[i].FieldValue != null)
                    {

                        if (CheckFileFormatting.IsFloat(viewModel.FieldsValues.Values[i].FieldValue) == false)
                        {
                            ModelState.AddModelError("FieldsValues.Values[" + i + "].Id", "يجب ان يكون قيمةالحقل رقمي، الرجاء إعادة الإدخال");

                            Status = false;
                        }
                    }

                    if ((viewModel.FieldsValues.Fields[i].Type.Equals("email")) && viewModel.FieldsValues.Values[i].FieldValue != null)
                    {

                        if (CheckFileFormatting.IsEmail(viewModel.FieldsValues.Values[i].FieldValue) == false)
                        {
                            ModelState.AddModelError("FieldsValues.Values[" + i + "].Id", "يجب ان يكون قيمةالحقل بريد إلكتروني، الرجاء إعادة الإدخال");

                            Status = false;
                        }
                    }

                    if ((viewModel.FieldsValues.Fields[i].Type.Equals("phone")) && viewModel.FieldsValues.Values[i].FieldValue != null)
                    {

                        if (CheckFileFormatting.IsPhone(viewModel.FieldsValues.Values[i].FieldValue) == false)
                        {
                            ModelState.AddModelError("FieldsValues.Values[" + i + "].Id", "يجب ان يكون قيمةالحقل رقم هاتف، الرجاء إعادة الإدخال");

                            Status = false;
                        }
                    }

                }
            }

            if (_context.Users.Find(CurrentUser).DepartmentId.HasValue)
            {
                int Dep_Id = _context.Users.Find(CurrentUser).DepartmentId.Value;
                ViewBag.Departments = new SelectList(_context.Departments.Where(a => a.Id == Dep_Id).ToList(), "Id", "Name");

            }
            else
            {
                ViewBag.Departments = new SelectList(_context.Departments.ToList(), "Id", "Name", viewModel.Document.DepartmentId);

            }
            ViewBag.Parties = new SelectList(_context.Parties.ToList(), "Id", "Name", viewModel.Document.PartyId);
            ViewBag.TypeMailId = new SelectList(_context.TypeMails.ToList(), "Id", "Name");
            ViewBag.Groups = new SelectList(_context.Groups.ToList(), "Id", "Name");
            ViewBag.DepartmentList = new SelectList(_context.Departments.ToList(), "Id", "Name");


            //Asmi [Return ]:
            ViewBag.StatusId = new SelectList(_context.DocumentStatuses.ToList(), "Id", "Name", viewModel.Document.StatusId);

            ViewBag.kinds = new SelectList(_context.Kinds.ToList(), "Id", "Name", viewModel.Document.KindId);


            ViewBag.ResponsibleUserId = new SelectList(_context.Users.Where(a => !a.RoleName.Equals("Master")).ToList(), "Id", "FullName", viewModel.Document.ResponsibleUserId);

            //===================== Related Departments/Groups/Users=====================
            //[Related Departments]:
            SelectListItem sl;
            List<SelectListItem> ListSl = new List<SelectListItem>();
            foreach (var G in DepartmentListDisplay.CreateDepartmentListDisplay().ToList())
            {
                sl = new SelectListItem()
                {

                    Text = G.Name,
                    Value = G.Id.ToString(),
                    Selected = _context.DocumentDepartments.Any(a => a.DocumentId == viewModel.Document.Id && a.DepartmentId == G.Id) ? true : false
                };

                ListSl.Add(sl);

            }
            ViewBag.RelatedDepartments = ListSl;





            //Related Groups:


            List<SelectListItem> ListS2 = new List<SelectListItem>();
            foreach (var G in _context.Groups.ToList())
            {
                sl = new SelectListItem()
                {

                    Text = G.Name,
                    Value = G.Id.ToString(),
                    Selected = _context.DocumentGroups.Any(a => a.DocumentId == viewModel.Document.Id && a.GroupId == G.Id) ? true : false
                };

                ListS2.Add(sl);

            }
            ViewBag.RelatedGroups = ListS2;


            //RelatedUsers:
            List<SelectListItem> ListS3 = new List<SelectListItem>();
            foreach (var U in _context.Users.Where(a => !a.RoleName.Equals("Master")).ToList())
            {
                sl = new SelectListItem()
                {

                    Text = U.FullName,
                    Value = U.Id.ToString(),
                    Selected = _context.DocumentUsers.Any(a => a.DocumentId == viewModel.Document.Id && a.UserId == U.Id) ? true : false
                };

                ListS3.Add(sl);

            }
            ViewBag.RelatedUsers = ListS3;

            if (viewModel.Document.TypeMailId == 2)
            {

                List<SelectListItem> ListS4 = new List<SelectListItem>();
                foreach (var G in _context.Parties.ToList())

                {
                    sl = new SelectListItem()
                    {

                        Text = G.Name,
                        Value = G.Id.ToString(),
                        Selected = _context.DocumentParties.Any(a => a.DocumentId == viewModel.Document.Id && a.PartyId == G.Id) ? true : false
                    };

                    ListS4.Add(sl);

                }
                ViewBag.PartyIds = ListS4;

            }

            //Status Model=false{status==false}
            if (Status == false)
            {
                return View(viewModel);
            }

            if (ModelState.IsValid)
            {
                if (viewModel.ExistFiles != null)
                {
                    if (ManagedAes.IsSaveInDb)
                    {
                        /*
                         * start code
                         * get image from scanner,
                         * save it in database
                         * 
                         */
                        var scannedImages = Request.Form.GetValues("myfile");
                        if (scannedImages != null)
                        {
                            int i = 0;
                            foreach (var ImgStr in scannedImages)
                            {
                                i++;
                                var fileStoredInDb = new FilesStoredInDb();

                                fileStoredInDb.DocumentId = viewModel.Document.Id;


                                string imageName = "scannedImage" + i + ".jpg";

                                var imgAsByteArray = ManagedAes.DecodeUrlBase64(ImgStr);

                                if (ManagedAes.IsCipher)
                                {
                                    // Encrypt File Name.
                                    fileStoredInDb.FileName = ManagedAes.EncryptText(imageName);
                                    // Encrypt File.
                                    var EncryptedImgAsByteArray = ManagedAes.EncryptArrayByte(imgAsByteArray);
                                    fileStoredInDb.File = new byte[EncryptedImgAsByteArray.Length];
                                    fileStoredInDb.File = EncryptedImgAsByteArray;
                                }
                                else
                                {
                                    // File Name.
                                    fileStoredInDb.FileName = imageName;
                                    // File.
                                    fileStoredInDb.File = new byte[imgAsByteArray.Length];
                                    fileStoredInDb.File = imgAsByteArray;
                                }

                                _context.FilesStoredInDbs.Add(fileStoredInDb);
                                _context.SaveChanges();
                            }
                        }
                        /*
                         * end code
                         * get image from scanner,
                         * save it in database
                         * 
                         */

                        for (int i = 0; i < viewModel.ExistFiles.Count; i++)
                        {
                            if (viewModel.FilesStoredInDbs != null && i < viewModel.FilesStoredInDbs.Count)
                            {
                                if (viewModel.ExistFiles[i]) // true
                                {
                                    if (UploadFile[i] != null)
                                    {
                                        //Save File In DB

                                        string FileName = Path.GetFileName(UploadFile[i].FileName);
                                        viewModel.FilesStoredInDbs[i].FileName = FileName;

                                        viewModel.FilesStoredInDbs[i].File = new byte[UploadFile[i].ContentLength];
                                        UploadFile[i].InputStream.Read(viewModel.FilesStoredInDbs[i].File, 0, UploadFile[i].ContentLength);

                                        if (ManagedAes.IsCipher)
                                        {
                                            // Encrypt File Name.
                                            viewModel.FilesStoredInDbs[i].FileName = ManagedAes.EncryptText(FileName);
                                            // Encrypt File.
                                            viewModel.FilesStoredInDbs[i].File = ManagedAes.EncryptArrayByte(viewModel.FilesStoredInDbs[i].File);
                                        }

                                        _context.Entry(viewModel.FilesStoredInDbs[i]).State = EntityState.Modified;
                                        _context.SaveChanges();
                                    }
                                }
                                else
                                {
                                    var filesStoredInDb = _context.FilesStoredInDbs.Find(viewModel.FilesStoredInDbs[i].Id);

                                    if (ManagedAes.IsCipher)
                                    {
                                        filesStoredInDb.FileName = ManagedAes.DecryptText(filesStoredInDb.FileName);
                                        filesStoredInDb.File = ManagedAes.DecryptArrayByte(filesStoredInDb.File);
                                    }

                                    // remove old file from DB
                                    _context.FilesStoredInDbs.Remove(filesStoredInDb);
                                    _context.SaveChanges();
                                }
                            }
                            else if (viewModel.ExistFiles[i] && UploadFile[i] != null)
                            {
                                //Save File In DB

                                var FileStoredInDb = new FilesStoredInDb();

                                string FileName = Path.GetFileName(UploadFile[i].FileName);
                                FileStoredInDb.FileName = FileName;

                                FileStoredInDb.File = new byte[UploadFile[i].ContentLength];
                                UploadFile[i].InputStream.Read(FileStoredInDb.File, 0, UploadFile[i].ContentLength);

                                FileStoredInDb.DocumentId = viewModel.Document.Id;

                                if (ManagedAes.IsCipher)
                                {
                                    // Encrypt File Name.
                                    viewModel.FilesStoredInDbs[i].FileName = ManagedAes.EncryptText(FileName);
                                    // Encrypt File.
                                    viewModel.FilesStoredInDbs[i].File = ManagedAes.EncryptArrayByte(viewModel.FilesStoredInDbs[i].File);
                                }

                                _context.FilesStoredInDbs.Add(FileStoredInDb);
                                _context.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        var urls = viewModel.Document.FileUrl.Split(new string[] { "_##_" }, StringSplitOptions.None);
                        var url = "";
                        var fileNames = viewModel.Document.Name.Split(new string[] { "_##_" }, StringSplitOptions.None);
                        var fileName = "";

                        /*
                         * start code
                         * get image from scanner,
                         * save it in server
                         * 
                         */
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
                                string s1 = DateTime.Now.ToString("yyyyMMddhhHHmmss");
                                string imageName = s1 + "scannedImage" + i + ".jpg";

                                //set the image path
                                string imgPath = Path.Combine(path, imageName);

                                byte[] imageBytes = ManagedAes.DecodeUrlBase64(ImgStr);

                                if (ManagedAes.IsCipher)
                                {
                                    ManagedAes.EncryptFile(imageBytes, imgPath);
                                }
                                else
                                {
                                    System.IO.File.WriteAllBytes(imgPath, imageBytes);
                                }

                                viewModel.Document.Name += imageName + "_##_";
                                viewModel.Document.FileUrl += s1 + "_##_";
                            }
                        }
                        /*
                         * end code 
                         * get image from scanner,
                         * save it in server
                         * 
                         */

                        for (int i = 0; i < viewModel.ExistFiles.Count; i++)
                        {

                            if (i < urls.Length)
                            {
                                if (viewModel.ExistFiles[i]) // true
                                {
                                    if (UploadFile[i] != null)
                                    {
                                        // remove old file from server
                                        var oldPath = Request.MapPath("~/Uploads/" + urls[i]);
                                        if (System.IO.File.Exists(oldPath))
                                        {
                                            System.IO.File.Delete(oldPath);
                                        }

                                        //Save File In Uploads
                                        string FileName = Path.GetFileName(UploadFile[i].FileName);
                                        //Save In DB:
                                        viewModel.Document.Name = FileName;

                                        string s1 = DateTime.Now.ToString("yyyyMMddhhHHmmss") + FileName;
                                        string path = Path.Combine(Server.MapPath("~/Uploads/"), s1);
                                        //UploadFile[i].SaveAs(path);
                                        if (ManagedAes.IsCipher)
                                        {
                                            ManagedAes.EncryptFile(UploadFile[i], path);
                                        }
                                        else
                                        {
                                            UploadFile[i].SaveAs(path);
                                        }

                                        url += s1 + "_##_";
                                        fileName += FileName + "_##_";
                                    }
                                    else
                                    {
                                        url += urls[i] + "_##_";
                                        fileName += fileNames[i] + "_##_";

                                    }
                                }
                                else
                                {
                                    // remove old file from server
                                    var oldPath = Request.MapPath("~/Uploads/" + urls[i]);
                                    if (System.IO.File.Exists(oldPath))
                                    {
                                        System.IO.File.Delete(oldPath);
                                    }
                                }
                            }
                            else if (viewModel.ExistFiles[i] && UploadFile[i] != null)
                            {
                                //Save File In Uploads
                                string FileName = Path.GetFileName(UploadFile[i].FileName);
                                //Save In DB:
                                viewModel.Document.Name = FileName;

                                string s1 = DateTime.Now.ToString("yyyyMMddhhHHmmss") + FileName;
                                string path = Path.Combine(Server.MapPath("~/Uploads/"), s1);
                                UploadFile[i].SaveAs(path);

                                url += s1 + "_##_";
                                fileName += FileName + "_##_";
                            }
                        }
                        if (url.Length > 0)
                        {
                            viewModel.Document.FileUrl = url.Substring(0, url.Length - 4);
                            viewModel.Document.Name = fileName.Substring(0, fileName.Length - 4);
                        }
                        else
                        {
                            viewModel.Document.FileUrl = "";
                            viewModel.Document.Name = "";
                        }
                    }
                }

                //Document Update:
                viewModel.Document.UpdatedById = this.User.Identity.GetUserId();

                viewModel.Document.UpdatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");

                if (viewModel.Document.ResponsibleUserId == null)
                {
                    viewModel.Document.NotificationUserId = viewModel.Document.CreatedById;
                    viewModel.Document.ResponsibleUserId = viewModel.Document.CreatedById;
                }
                else
                {
                    viewModel.Document.NotificationUserId = viewModel.Document.ResponsibleUserId;

                }

                // Encrypt Document Attributes.
                if (ManagedAes.IsCipher)
                {
                    viewModel.Document.Address = ManagedAes.EncryptText(viewModel.Document.Address);
                    viewModel.Document.CreatedAt = ManagedAes.EncryptText(viewModel.Document.CreatedAt);
                    viewModel.Document.Description = ManagedAes.EncryptText(viewModel.Document.Description);
                    viewModel.Document.DocumentDate = ManagedAes.EncryptText(viewModel.Document.DocumentDate);
                    viewModel.Document.DocumentNumber = ManagedAes.EncryptText(viewModel.Document.DocumentNumber);
                    viewModel.Document.FileUrl = ManagedAes.EncryptText(viewModel.Document.FileUrl);
                    viewModel.Document.MailingDate = ManagedAes.EncryptText(viewModel.Document.MailingDate);
                    viewModel.Document.MailingNumber = ManagedAes.EncryptText(viewModel.Document.MailingNumber);
                    viewModel.Document.Name = ManagedAes.EncryptText(viewModel.Document.Name);
                    viewModel.Document.Notes = ManagedAes.EncryptText(viewModel.Document.Notes);
                    viewModel.Document.NotificationDate = ManagedAes.EncryptText(viewModel.Document.NotificationDate);
                    viewModel.Document.Subject = ManagedAes.EncryptText(viewModel.Document.Subject);
                    viewModel.Document.UpdatedAt = ManagedAes.EncryptText(viewModel.Document.UpdatedAt);
                }

                _context.Entry(viewModel.Document).State = EntityState.Modified;
                _context.SaveChanges();





                //if (CanEdit == true)
                //{

                string NotificationTime = string.Empty;
                //List of users id:
                List<string> UsersId = new List<string>();


                var UserId = User.Identity.GetUserId();



                List<string> SelectedDocumentDepartments = new List<string>();
                SelectedDocumentDepartments = _context.DocumentDepartments.Where(a => a.DocumentId == viewModel.Document.Id).Select(a => a.DepartmentId.ToString()).ToList();


                List<string> RelatedDep = new List<string>();
                    if(RelatedDepartments!=null)
                {

                    RelatedDep=RelatedDepartments.ToList();
                }
                RelatedDep.Add(viewModel.Document.DepartmentId.ToString());
                if (RelatedDep != null)
                {
                    DocumentDepartment _DocumentDepartment = null;
                    List<string> ExpectDocumentDepartment = new List<string>();
                    ExpectDocumentDepartment = SelectedDocumentDepartments.Except(RelatedDep).ToList();
                    foreach (string _DocumentDepartment_Id in RelatedDep)
                    {


                        if (SelectedDocumentDepartments.Contains(_DocumentDepartment_Id))
                        {

                            continue;
                        }
                        _DocumentDepartment = new DocumentDepartment()
                        {

                            DocumentId = viewModel.Document.Id,
                            EnableEdit = true,
                            EnableRelate = true,
                            EnableReplay = true,
                            EnableSeal = true,
                            DepartmentId = Convert.ToInt32(_DocumentDepartment_Id),
                            CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                            CreatedById = this.User.Identity.GetUserId()
                        };

                        _context.DocumentDepartments.Add(_DocumentDepartment);

                        NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");


                        int Dep_Id = Convert.ToInt32(_DocumentDepartment_Id);
                        //Users in this Department:
                        UsersId = _context.Users.Where(a => a.DepartmentId == Dep_Id).Select(a => a.Id).ToList();
                        //Notification object:
                        Notification notification = null;


                        //Users in this department:
                        List<ApplicationUser> Users = _context.Users.Where(a => UsersId.Contains(a.Id)).ToList();
                        foreach (ApplicationUser user in Users)
                        {

                            notification = new Notification()
                            {

                                CreatedAt = NotificationTime,
                                Active = false,
                                UserId = user.Id,
                                Message = "تم إضافة وثيقة جديدة للقسم الحالي، رقم الوثيقة :" + viewModel.Document.DocumentNumber + " موضوع الوثيقة :" + viewModel.Document.Subject
                                + " ،عنوان الوثيقة :" + viewModel.Document.Address + "،وصف الوثيقة :" + viewModel.Document.Description
                               ,
                                NotificationOwnerId = UserId
                            };
                            _context.Notifications.Add(notification);
                        }

                    }


                    DocumentDepartment deleteDocumentDepartment;
                    foreach (string s in ExpectDocumentDepartment)
                    {
                        deleteDocumentDepartment = _context.DocumentDepartments.Where(a => a.DocumentId == viewModel.Document.Id && a.DepartmentId.ToString().Equals(s)).SingleOrDefault();
                        int Dep_Id = deleteDocumentDepartment.DepartmentId;
                        _context.DocumentDepartments.Remove(deleteDocumentDepartment);




                        NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");



                        //Users in this Department:
                        UsersId = _context.Users.Where(a => a.DepartmentId == Dep_Id).Select(a => a.Id).ToList();
                        //Notification object:
                        Notification notification = null;


                        //Users in this department:
                        List<ApplicationUser> Users = _context.Users.Where(a => UsersId.Contains(a.Id)).ToList();
                        foreach (ApplicationUser user in Users)
                        {

                            notification = new Notification()
                            {

                                CreatedAt = NotificationTime,
                                Active = false,
                                UserId = user.Id,
                                Message = "تم إزالة وثيقة من للقسم الحالي، رقم الوثيقة :" + viewModel.Document.DocumentNumber + " موضوع الوثيقة :" + viewModel.Document.Subject
                                + " ،عنوان الوثيقة :" + viewModel.Document.Address + "،وصف الوثيقة :" + viewModel.Document.Description
                               ,
                                NotificationOwnerId = UserId
                            };
                            _context.Notifications.Add(notification);
                        }
                    }
                    _context.SaveChanges();

                }

                else
                {
                    foreach (DocumentDepartment _DocumentDepartment in _context.DocumentDepartments.Where(a => a.DocumentId == viewModel.Document.Id))
                    {

                        int Dep_Id = _DocumentDepartment.DepartmentId;
                        _context.DocumentDepartments.Remove(_DocumentDepartment);



                        NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");



                        //Users in this Department:
                        UsersId = _context.Users.Where(a => a.DepartmentId == Dep_Id).Select(a => a.Id).ToList();
                        //Notification object:
                        Notification notification = null;


                        //Users in this department:
                        List<ApplicationUser> Users = _context.Users.Where(a => UsersId.Contains(a.Id)).ToList();
                        foreach (ApplicationUser user in Users)
                        {

                            notification = new Notification()
                            {

                                CreatedAt = NotificationTime,
                                Active = false,
                                UserId = user.Id,
                                Message = "تم إزالة وثيقة من للقسم الحالي، رقم الوثيقة :" + viewModel.Document.DocumentNumber + " موضوع الوثيقة :" + viewModel.Document.Subject
                                + " ،عنوان الوثيقة :" + viewModel.Document.Address + "،وصف الوثيقة :" + viewModel.Document.Description
                               ,
                                NotificationOwnerId = UserId
                            };
                            _context.Notifications.Add(notification);
                        }


                    }

                    _context.SaveChanges();

                }



                //Related Groups:
                List<string> SelectedDocumentGroups = new List<string>();
                SelectedDocumentGroups = _context.DocumentGroups.Where(a => a.DocumentId == viewModel.Document.Id).Select(a => a.GroupId.ToString()).ToList();
                if (RelatedGroups != null)
                {
                    DocumentGroup _DocumentGroup = null;
                    List<string> ExpectDocumentGroups = new List<string>();
                    ExpectDocumentGroups = SelectedDocumentGroups.Except(RelatedGroups).ToList();
                    foreach (string _DocumentGroup_Id in RelatedGroups)
                    {


                        if (SelectedDocumentGroups.Contains(_DocumentGroup_Id))
                        {

                            continue;
                        }
                        _DocumentGroup = new DocumentGroup()
                        {

                            DocumentId = viewModel.Document.Id,
                            EnableEdit = true,
                            EnableRelate = true,
                            EnableReplay = true,
                            EnableSeal = true,
                            GroupId = Convert.ToInt32(_DocumentGroup_Id),
                            CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                            CreatedById = this.User.Identity.GetUserId()
                        };

                        _context.DocumentGroups.Add(_DocumentGroup);
                        NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                        int GroupId = Convert.ToInt32(_DocumentGroup_Id);
                        UsersId = _context.UsersGroups.Where(a => a.GroupId == GroupId).Select(a => a.UserId).ToList();

                        string GroupName = _context.Groups.Find(GroupId).Name;
                        Notification notification = null;

                        List<ApplicationUser> Users = _context.Users.Where(a => UsersId.Contains(a.Id)).ToList();
                        foreach (ApplicationUser user in Users)
                        {

                            notification = new Notification()
                            {

                                CreatedAt = NotificationTime,
                                Active = false,
                                UserId = user.Id,
                                Message = "تم إضافة وثيقة جديدة للمجموعة :" + GroupName + "، رقم الوثيقة :" + viewModel.Document.Name + " ، موضوع الوثيقة:" + viewModel.Document.Subject +
                                " ، عنوان الوثيقة:" + viewModel.Document.Address + " ،وصف الوثيقة :" + viewModel.Document.Description
                               ,
                                NotificationOwnerId = UserId
                            };
                            _context.Notifications.Add(notification);
                        }
                    }

                    _context.SaveChanges();
                    DocumentGroup deleteDocumentGroup;
                    foreach (string s in ExpectDocumentGroups)
                    {
                        deleteDocumentGroup = _context.DocumentGroups.Where(a => a.DocumentId == viewModel.Document.Id && a.GroupId.ToString().Equals(s)).SingleOrDefault();
                        int GroupId = Convert.ToInt32(s);

                        _context.DocumentGroups.Remove(deleteDocumentGroup);

                        NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                        UsersId = _context.UsersGroups.Where(a => a.GroupId == GroupId).Select(a => a.UserId).ToList();

                        string GroupName = _context.Groups.Find(GroupId).Name;
                        Notification notification = null;

                        List<ApplicationUser> Users = _context.Users.Where(a => UsersId.Contains(a.Id)).ToList();
                        foreach (ApplicationUser user in Users)
                        {

                            notification = new Notification()
                            {

                                CreatedAt = NotificationTime,
                                Active = false,
                                UserId = user.Id,
                                Message = "تم إزالة وثيقة من المجموعة :" + GroupName + "، رقم الوثيقة :" + viewModel.Document.Name + " ، موضوع الوثيقة:" + viewModel.Document.Subject +
                                " ، عنوان الوثيقة:" + viewModel.Document.Address + " ،وصف الوثيقة :" + viewModel.Document.Description
                               ,
                                NotificationOwnerId = UserId
                            };
                            _context.Notifications.Add(notification);
                        }
                    }
                    _context.SaveChanges();

                }

                else
                {
                    foreach (DocumentGroup _DocumentGroup in _context.DocumentGroups.Where(a => a.DocumentId == viewModel.Document.Id))
                    {
                        int GroupId = _DocumentGroup.GroupId;
                        _context.DocumentGroups.Remove(_DocumentGroup);
                        NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                        UsersId = _context.UsersGroups.Where(a => a.GroupId == GroupId).Select(a => a.UserId).ToList();

                        string GroupName = _context.Groups.Find(GroupId).Name;
                        Notification notification = null;

                        List<ApplicationUser> Users = _context.Users.Where(a => UsersId.Contains(a.Id)).ToList();
                        foreach (ApplicationUser user in Users)
                        {

                            notification = new Notification()
                            {

                                CreatedAt = NotificationTime,
                                Active = false,
                                UserId = user.Id,
                                Message = "تم إزالة وثيقة من المجموعة :" + GroupName + "، رقم الوثيقة :" + viewModel.Document.Name + " ، موضوع الوثيقة:" + viewModel.Document.Subject +
                                " ، عنوان الوثيقة:" + viewModel.Document.Address + " ،وصف الوثيقة :" + viewModel.Document.Description
                               ,
                                NotificationOwnerId = UserId
                            };
                            _context.Notifications.Add(notification);
                        }
                    }

                    _context.SaveChanges();

                }




                //Relaated Users:

                List<string> SelectedDocumentUsers = new List<string>();
                SelectedDocumentUsers = _context.DocumentUsers.Where(a => a.DocumentId == viewModel.Document.Id).Select(a => a.UserId.ToString()).ToList();
                if (RelatedUsers != null)
                {
                    DocumentUser _DocumentUser = null;
                    List<string> ExpectDocumentUsers = new List<string>();
                    ExpectDocumentUsers = SelectedDocumentUsers.Except(RelatedUsers).ToList();
                    foreach (string _DocumentUser_Id in RelatedUsers)
                    {


                        if (SelectedDocumentUsers.Contains(_DocumentUser_Id))
                        {

                            continue;
                        }
                        _DocumentUser = new DocumentUser()
                        {

                            DocumentId = viewModel.Document.Id,
                            EnableEdit = true,
                            EnableRelate = true,
                            EnableReplay = true,
                            EnableSeal = true,
                            UserId = _DocumentUser_Id,
                            CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                            CreatedById = this.User.Identity.GetUserId()
                        };

                        _context.DocumentUsers.Add(_DocumentUser);
                        NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");


                        //Notification object:
                        Notification notification = null;




                        notification = new Notification()
                        {

                            CreatedAt = NotificationTime,
                            Active = false,
                            UserId = _DocumentUser_Id,
                            Message = "تم إضافة وثيقة جديدة  ، رقم الوثيقة :" + viewModel.Document.DocumentNumber + " موضوع الوثيقة :" + viewModel.Document.Subject
                            + " ،عنوان الوثيقة :" + viewModel.Document.Address + "،وصف الوثيقة :" + viewModel.Document.Description
                           ,
                            NotificationOwnerId = UserId
                        };
                        _context.Notifications.Add(notification);


                    }
                    _context.SaveChanges();

                    DocumentUser deleteDocumentUser;
                    foreach (string s in ExpectDocumentUsers)
                    {
                        deleteDocumentUser = _context.DocumentUsers.Where(a => a.DocumentId == viewModel.Document.Id && a.UserId.ToString().Equals(s)).SingleOrDefault();

                        _context.DocumentUsers.Remove(deleteDocumentUser);
                        NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");


                        //Notification object:
                        Notification notification = null;




                        notification = new Notification()
                        {

                            CreatedAt = NotificationTime,
                            Active = false,
                            UserId = s,
                            Message = "تم ازالة وثيقة   ، رقم الوثيقة :" + viewModel.Document.DocumentNumber + " موضوع الوثيقة :" + viewModel.Document.Subject
                            + " ،عنوان الوثيقة :" + viewModel.Document.Address + "،وصف الوثيقة :" + viewModel.Document.Description
                           ,
                            NotificationOwnerId = UserId
                        };
                        _context.Notifications.Add(notification);

                    }
                    _context.SaveChanges();

                }

                else
                {
                    foreach (DocumentUser _DocumentUser in _context.DocumentUsers.Where(a => a.DocumentId == viewModel.Document.Id))
                    {
                        string s = _DocumentUser.UserId;
                        _context.DocumentUsers.Remove(_DocumentUser);
                        NotificationTime = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");


                        //Notification object:
                        Notification notification = null;




                        notification = new Notification()
                        {

                            CreatedAt = NotificationTime,
                            Active = false,
                            UserId = s,
                            Message = "تم ازالة وثيقة   ، رقم الوثيقة :" + viewModel.Document.DocumentNumber + " موضوع الوثيقة :" + viewModel.Document.Subject
                            + " ،عنوان الوثيقة :" + viewModel.Document.Address + "،وصف الوثيقة :" + viewModel.Document.Description
                           ,
                            NotificationOwnerId = UserId
                        };
                        _context.Notifications.Add(notification);

                    }
                }

                _context.SaveChanges();





                //Party Ids:

                if (viewModel.Document.TypeMailId == 2)
                {
                    List<string> SelectedDocumentPartyId = new List<string>();
                    SelectedDocumentPartyId = _context.DocumentParties.Where(a => a.DocumentId == viewModel.Document.Id).Select(a => a.PartyId.ToString()).ToList();
                    if (PartyIds != null)
                    {
                        DocumentParty _Documentparty = null;
                        List<string> ExpectDocumentParties = new List<string>();
                        ExpectDocumentParties = SelectedDocumentPartyId.Except(PartyIds).ToList();
                        foreach (string _DocumentParty_Id in PartyIds)
                        {


                            if (SelectedDocumentPartyId.Contains(_DocumentParty_Id))
                            {

                                continue;
                            }
                            _Documentparty = new DocumentParty()
                            {

                                DocumentId = viewModel.Document.Id,
                                PartyId = Convert.ToInt32(_DocumentParty_Id),

                                CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                                CreatedById = this.User.Identity.GetUserId()
                            };

                            _context.DocumentParties.Add(_Documentparty);


                        }
                        _context.SaveChanges();

                        DocumentParty deleteDocumentParty;
                        foreach (string s in ExpectDocumentParties)
                        {
                            deleteDocumentParty = _context.DocumentParties.Where(a => a.DocumentId == viewModel.Document.Id && a.PartyId.ToString().Equals(s)).SingleOrDefault();

                            _context.DocumentParties.Remove(deleteDocumentParty);




                        }
                        _context.SaveChanges();

                    }



                    _context.SaveChanges();
                }






                //}



                if (FVVM != null)
                {
                    foreach (Value value in viewModel.FieldsValues.Values)
                    {
                        // Craete Date
                        value.Document_id = viewModel.Document.Id;

                        _context.Entry(value).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }
                return RedirectToAction("Index", new { id = "EditSuccess" });
            }
            return View(viewModel);
        }






        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentIndex")]
        public ActionResult Index(string id = "none",string Notf="none")
        {
            ViewBag.Current = "Document";
            ViewBag.Page = 0;



            if (!id.Equals("none"))
            {
                ViewBag.Msg = id;
            }
            else
            {
                ViewBag.Msg = null;
            }

            ViewBag.Kinds = new SelectList(_context.Kinds.ToList(), "Id", "Name");

            ViewBag.MailType = new SelectList(_context.TypeMails.ToList(), "Id", "Name");
            ViewBag.Forms = new SelectList(_context.Forms.ToList(), "Id", "Name");

            string currentUserId = this.User.Identity.GetUserId();

            IEnumerable<Document> documents;
            if (Notf.Equals("Notf"))
            {
                string CurrentUserId = this.User.Identity.GetUserId();
                DateTime TodayDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy/MM/dd").Replace("-", "/"), "yyyy/MM/dd", null);

                documents = _context.Documents.Include(a=>a.TypeMail).Where(a => a.NotificationUserId.Equals(CurrentUserId) && a.NotificationDate != null).ToList();
                documents = documents.Where(a => EqualDate(a.NotificationDate, TodayDate)).OrderByDescending(a=>a.NotificationDate);
            }
            else
            {
             documents = _context.Documents.Where(a => a.CreatedById.Equals(currentUserId)).Include(a => a.TypeMail).OrderByDescending(a => a.CreatedAt).Take(10);

            }

            var Documents = _context.Documents.Where(a => a.CreatedById.Equals(currentUserId)).Include(a => a.TypeMail).OrderByDescending(a => a.CreatedAt).Take(10).ToList();


            // Decrypt Document Attributes.
            if (ManagedAes.IsCipher)
            {
                foreach (var Document in Documents)
                {
                    Document.DocumentNumber = ManagedAes.DecryptText(Document.DocumentNumber);
                    Document.DocumentDate = ManagedAes.DecryptText(Document.DocumentDate);
                    Document.Subject = ManagedAes.DecryptText(Document.Subject);
                    Document.CreatedAt = ManagedAes.DecryptText(Document.CreatedAt);
                    //Document.Address = ManagedAes.DecryptText(Document.Address);
                    //Document.Description = ManagedAes.DecryptText(Document.Description);
                    //Document.FileUrl = ManagedAes.DecryptText(Document.FileUrl);
                    //Document.MailingDate = ManagedAes.DecryptText(Document.MailingDate);
                    //Document.MailingNumber = ManagedAes.DecryptText(Document.MailingNumber);
                    //Document.Name = ManagedAes.DecryptText(Document.Name);
                    //Document.Notes = ManagedAes.DecryptText(Document.Notes);
                    //Document.NotificationDate = ManagedAes.DecryptText(Document.NotificationDate);
                    //Document.UpdatedAt = ManagedAes.DecryptText(Document.UpdatedAt);
                }
            }
            
            return View(Documents);
        }


        
        [HttpPost]
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentIndex")]
        public ActionResult Index(string RetrievalCount, string DocumentSubject, string DocumentModel, string OrderBY, string OrderType, string DocumentNumber, string DocumentForm, string DocumentKind, string DocumentMail, string DocFirstDate, string DocEndDate)
        {

            string currentUserId = this.User.Identity.GetUserId();

            List<Document> documents = null;
            DateTime fdate, ldate;
            if (DocFirstDate == null || DocFirstDate == "")
            {
                fdate = DateTime.MinValue;
            }
            else
            {
                DocFirstDate = DocFirstDate.Replace("-", "/");
                fdate = DateTime.ParseExact(DocFirstDate, "yyyy/MM/dd", null);
            }

            if (DocEndDate == null || DocEndDate == "")
            {
                ldate = DateTime.MaxValue;
            }
            else
            {
                DocEndDate = DocEndDate.Replace("-", "/");
                ldate = DateTime.ParseExact(DocEndDate, "yyyy/MM/dd", null);
            }



            List<int> MyDocId = new List<int>();

            switch (DocumentModel)
            {

                case "1":
                    MyDocId = UserDocumentsID.UserCreatedDocument(currentUserId).ToList();

                    break;

                case "2":
                    MyDocId = UserDocumentsID.UserDeocumentNotification(currentUserId).ToList();

                    break;

                case "3":
                    MyDocId = UserDocumentsID.UserDepartmentDocument(currentUserId).ToList();

                    break;

                case "4":
                    MyDocId = UserDocumentsID.UserDocumentGroups(currentUserId).ToList();

                    break;
                case "5":
                    MyDocId = UserDocumentsID.UserAllDocuments(currentUserId).ToList();

                    break;
                case "6":
                    MyDocId = UserDocumentsID.UserNotificationTodayDocument(currentUserId).ToList();

                    break;


                case "7":
                    MyDocId = UserDocumentsID.UserMyDepartmentDocument(currentUserId).ToList();

                    break;

                case "8":
                    MyDocId = UserDocumentsID.UserDocumentTrend(currentUserId).ToList();

                    break;

            }

            documents = _context.Documents.Where(a => MyDocId.Contains(a.Id)).Include(a => a.TypeMail).ToList();

            // Decrypt Document Attributes.
            if (ManagedAes.IsCipher)
            {
                foreach (var Document in documents)
                {
                    Document.Address = ManagedAes.DecryptText(Document.Address);
                    Document.CreatedAt = ManagedAes.DecryptText(Document.CreatedAt);
                    Document.Description = ManagedAes.DecryptText(Document.Description);
                    Document.DocumentDate = ManagedAes.DecryptText(Document.DocumentDate);
                    Document.DocumentNumber = ManagedAes.DecryptText(Document.DocumentNumber);
                    Document.FileUrl = ManagedAes.DecryptText(Document.FileUrl);
                    Document.MailingDate = ManagedAes.DecryptText(Document.MailingDate);
                    Document.MailingNumber = ManagedAes.DecryptText(Document.MailingNumber);
                    Document.Name = ManagedAes.DecryptText(Document.Name);
                    Document.Notes = ManagedAes.DecryptText(Document.Notes);
                    Document.NotificationDate = ManagedAes.DecryptText(Document.NotificationDate);
                    Document.Subject = ManagedAes.DecryptText(Document.Subject);
                    Document.UpdatedAt = ManagedAes.DecryptText(Document.UpdatedAt);
                }
            }

            documents = (from d in documents
                         where
                         d.DocumentNumber.Contains(DocumentNumber) &&


                                             d.Subject.Contains(DocumentSubject)
                                             &&

                                             d.KindId.ToString().Contains(DocumentKind)
                                          &&

                                             d.FormId.ToString().Contains(DocumentForm)
                                             &&


                                             // Filter by Mail Type
                                             d.TypeMailId.ToString().Contains(DocumentMail)
                         select (d)).ToList();

            documents = documents.Where(a => BiggerThan(a.DocumentDate, fdate) && SmallerThan(a.DocumentDate, ldate)).ToList();

            switch (OrderBY)
            {
                case "1":

                    if (OrderType.Equals("1"))
                    {
                        documents = documents.OrderBy(a => a.DocumentNumber).ToList();
                    }
                    else
                    {
                        documents = documents.OrderByDescending(a => a.DocumentNumber).ToList();

                    }



                    break;


                case "2":

                    if (OrderType.Equals("1"))
                    {
                        documents = documents.OrderBy(a => a.Subject).ToList();
                    }
                    else
                    {
                        documents = documents.OrderByDescending(a => a.Subject).ToList();

                    }



                    break;


                case "3":

                    if (OrderType.Equals("1"))
                    {
                        documents = documents.OrderBy(a => a.CreatedAt).ToList();
                    }
                    else
                    {
                        documents = documents.OrderByDescending(a => a.CreatedAt).ToList();

                    }



                    break;


                case "4":

                    if (OrderType.Equals("1"))
                    {
                        documents = documents.OrderBy(a => a.DocumentDate).ToList();
                    }
                    else
                    {
                        documents = documents.OrderByDescending(a => a.DocumentDate).ToList();

                    }



                    break;
            }
            documents = documents.Take(Convert.ToInt32(RetrievalCount)).ToList();
            
            
            return PartialView("_search", documents);
        }




        
        [AccessDeniedAuthorizeattribute(ActionName = "RelateDocumentCreateList")]
        public ActionResult Relate(int? Id)
        {
            string CurrentUserId = this.User.Identity.GetUserId();

            if (Id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");

            }

            //Document document = _context.Documents.Find(Id);

            //if (ManagedAes.IsCipher)
            //{
            //    document.Address = ManagedAes.DecryptText(document.Address);
            //    document.CreatedAt = ManagedAes.DecryptText(document.CreatedAt);
            //    document.Description = ManagedAes.DecryptText(document.Description);
            //    document.DocumentDate = ManagedAes.DecryptText(document.DocumentDate);
            //    //document.DocumentNumber = ManagedAes.DecryptText(document.DocumentNumber);
            //    //document.FileUrl = ManagedAes.DecryptText(document.FileUrl);
            //    //document.MailingDate = ManagedAes.DecryptText(document.MailingDate);
            //    //document.MailingNumber = ManagedAes.DecryptText(document.MailingNumber);
            //    //document.Name = ManagedAes.DecryptText(document.Name);
            //    //document.Notes = ManagedAes.DecryptText(document.Notes);
            //    //document.NotificationDate = ManagedAes.DecryptText(document.NotificationDate);
            //    //document.Subject = ManagedAes.DecryptText(document.Subject);
            //    //document.UpdatedAt = ManagedAes.DecryptText(document.UpdatedAt);
            //}

            //if (document == null)
            //{
            //    return RedirectToAction("HttpNotFoundError", "ErrorController");
            //}

            List<int> DocumentRelate = UserRelatedDocumentsId.UserAllDocuments(CurrentUserId).ToList();
            DocumentRelate = DocumentRelate.Except(new List<int>() {Id.Value }).ToList();
            List<Document> Documents = _context.Documents.Where(a => DocumentRelate.Contains(a.Id)).ToList();

            ViewBag.DocId = Id.Value;

            if (ManagedAes.IsCipher)
            {
                foreach (var Document in Documents)
                {
                    Document.DocumentNumber = ManagedAes.DecryptText(Document.DocumentNumber);
                    Document.Subject = ManagedAes.DecryptText(Document.Subject);
                    Document.CreatedAt = ManagedAes.DecryptText(Document.CreatedAt);
                    Document.DocumentDate = ManagedAes.DecryptText(Document.DocumentDate);
                    //Document.Address = ManagedAes.DecryptText(Document.Address);
                    //Document.Description = ManagedAes.DecryptText(Document.Description);
                    //Document.FileUrl = ManagedAes.DecryptText(Document.FileUrl);
                    //Document.MailingDate = ManagedAes.DecryptText(Document.MailingDate);
                    //Document.MailingNumbefr = ManagedAes.DecryptText(Document.MailingNumber);
                    //Document.Name = ManagedAes.DecryptText(Document.Name);
                    //Document.Notes = ManagedAes.DecryptText(Document.Notes);
                    //Document.NotificationDate = ManagedAes.DecryptText(Document.NotificationDate);
                    //Document.UpdatedAt = ManagedAes.DecryptText(Document.UpdatedAt);
                }
            }

            return View(Documents);

        }

        [HttpPost]

        
        [AccessDeniedAuthorizeattribute(ActionName = "RelateDocumentCreateList")]
        public ActionResult Relate(List<int> Documents, int DocId)
        {
            string CurrentUserid = this.User.Identity.GetUserId();
            RelatedDocument Relatedocument = null;

            if(Documents!=null)

            {

                foreach (int DId in Documents)
                {

                    Relatedocument = new RelatedDocument()
                    {
                        Document_id=DocId,
                        RelatedDocId=DId,
                        CreatedById=CurrentUserid,
                    };

                    _context.RelatedDocuments.Add(Relatedocument);
                }
                _context.SaveChanges();


                return RedirectToAction("GetRelatedDocument", new { Id = DocId, msg = "CreateSuccess" });
            }

            else
            {
                return RedirectToAction("GetRelatedDocument", new { Id = DocId, msg = "CreateError" });                
            }

        }



        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentDelete")]
        public ActionResult Delete(int? id)
        {

            ViewBag.Current = "Document";

            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }
            var document = _context.Documents.Include(a => a.Department).Include(dk => dk.Kind).Include(p => p.Party).Include(t => t.TypeMail).Include(b => b.CreatedBy).Include(a => a.Form).FirstOrDefault(a => a.Id == id);

            if (ManagedAes.IsCipher)
            {
                document.Address = ManagedAes.DecryptText(document.Address);
                document.CreatedAt = ManagedAes.DecryptText(document.CreatedAt);
                document.Description = ManagedAes.DecryptText(document.Description);
                document.DocumentDate = ManagedAes.DecryptText(document.DocumentDate);
                //document.DocumentNumber = ManagedAes.DecryptText(document.DocumentNumber);
                //document.FileUrl = ManagedAes.DecryptText(document.FileUrl);
                //document.MailingDate = ManagedAes.DecryptText(document.MailingDate);
                //document.MailingNumber = ManagedAes.DecryptText(document.MailingNumber);
                //document.Name = ManagedAes.DecryptText(document.Name);
                //document.Notes = ManagedAes.DecryptText(document.Notes);
                //document.NotificationDate = ManagedAes.DecryptText(document.NotificationDate);
                //document.Subject = ManagedAes.DecryptText(document.Subject);
                //document.UpdatedAt = ManagedAes.DecryptText(document.UpdatedAt);
            }

            if (document == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            string userId = this.User.Identity.GetUserId();
            if (DocumentOperation.DocumentCanDelete(userId, id.Value) == false)
            {
                return RedirectToAction("Index", new { msg = "ErrorOperation" });

            }

            return View(document);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]


        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentDelete")]
        public ActionResult Confirm(int? id)
        {
            ViewBag.Current = "Document";

            var document = _context.Documents.Find(id);

            if (ManagedAes.IsCipher)
            {
                document.Address = ManagedAes.DecryptText(document.Address);
                document.CreatedAt = ManagedAes.DecryptText(document.CreatedAt);
                document.Description = ManagedAes.DecryptText(document.Description);
                document.DocumentDate = ManagedAes.DecryptText(document.DocumentDate);
                document.DocumentNumber = ManagedAes.DecryptText(document.DocumentNumber);
                document.FileUrl = ManagedAes.DecryptText(document.FileUrl);
                document.MailingDate = ManagedAes.DecryptText(document.MailingDate);
                document.MailingNumber = ManagedAes.DecryptText(document.MailingNumber);
                document.Name = ManagedAes.DecryptText(document.Name);
                document.Notes = ManagedAes.DecryptText(document.Notes);
                document.NotificationDate = ManagedAes.DecryptText(document.NotificationDate);
                document.Subject = ManagedAes.DecryptText(document.Subject);
                document.UpdatedAt = ManagedAes.DecryptText(document.UpdatedAt);
            }

            //Beacuse sycle when cascade:
            var values = _context.Values.Where(v => v.Document_id == id).ToList();

            if (values.Any())
            {
                foreach (var value in values)
                {
                    _context.Values.Remove(value);
                    _context.SaveChanges();
                }

            }

            _context.Documents.Remove(document);

          

            List<SealDocument> Seals = _context.SealDocuments.Where(a => a.DocumentId == id).ToList();
            foreach (SealDocument r in Seals)
            {

                _context.SealDocuments.Remove(r);
            }



            List<ReplayDocument> Replays = _context.ReplayDocuments.Where(a => a.ReplayDocId == id).ToList();
            foreach (ReplayDocument r in Replays)
            {
               
                _context.ReplayDocuments.Remove(r);
            }


            List <RelatedDocument> Relates = _context.RelatedDocuments.Where(a => a.RelatedDocId == id).ToList();
            foreach (RelatedDocument r in Relates)
            {
                
                
                _context.RelatedDocuments.Remove(r);
            }




            _context.SaveChanges();
            return RedirectToAction("Index", new { Id = "DeleteSuccess" });

        }




        //Child:

        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentRemoveChildRate")]

        public ActionResult RemoveChildRate(int? id)
        {

            ViewBag.Current = "Document";

            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }
            Document document = _context.Documents.Include(a => a.Department).Include(dk => dk.Kind).Include(p => p.Party).Include(t => t.TypeMail).Include(b => b.CreatedBy).Include(a => a.Form).FirstOrDefault(a => a.Id == id);
            if (document == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            string userId = this.User.Identity.GetUserId();
            if (DocumentOperation.DocumentRemoveChild(userId, id.Value) == false)
            {
                return RedirectToAction("Index", new { msg = "ErrorOperation" });

            }

            return View(document);

        }

        
         [HttpPost, ActionName("RemoveChildRate")]
        [ValidateAntiForgeryToken]

        [AccessDeniedAuthorizeattribute(ActionName = "DocumentRemoveChildRate")]

   //     [AccessDeniedAuthorizeattribute(ActionName = "DocumentDelete")]
        public ActionResult RemoveChildRateConfirm (int? id)
        {
            ViewBag.Current = "Document";

            List<RelatedDocument> Relates = _context.RelatedDocuments.Where(a => a.Document_id == id).ToList();
            foreach(RelatedDocument r in Relates)
            {
                _context.RelatedDocuments.Remove(r);

            }
            _context.SaveChanges();

            return RedirectToAction("Index", new { Id = "RemoveRateSuccess" });

        }


        //***************


        //Parent:


        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentRemoveParentRate")]

        public ActionResult RemoveParentRate(int? id)
        {

            ViewBag.Current = "Document";

            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }
            Document document = _context.Documents.Include(a => a.Department).Include(dk => dk.Kind).Include(p => p.Party).Include(t => t.TypeMail).Include(b => b.CreatedBy).Include(a => a.Form).FirstOrDefault(a => a.Id == id);
            if (document == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            string userId = this.User.Identity.GetUserId();
            if (DocumentOperation.DocumentRemoveParent(userId, id.Value) == false)
            {
                return RedirectToAction("Index", new { msg = "ErrorOperation" });

            }

            return View(document);

        }

        [HttpPost, ActionName("RemoveParentRate")]
        [ValidateAntiForgeryToken]


        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentRemoveParentRate")]

        public ActionResult RemoveParentRateConfirm(int? id)
        {
            ViewBag.Current = "Document";

            List<RelatedDocument> Relates = _context.RelatedDocuments.Where(a => a.RelatedDocId == id).ToList();
            foreach (RelatedDocument r in Relates)
            {
                _context.RelatedDocuments.Remove(r);

            }
            _context.SaveChanges();

            return RedirectToAction("Index", new { Id = "RemoveRateSuccess" });

        }







        [HttpPost]
        public ActionResult Release(int id, int id2)
        {

            ViewBag.Current = "RelatedDocument";

            var relDoc = _context.RelatedDocuments.Find(id);


            _context.RelatedDocuments.Remove(relDoc);
            _context.SaveChanges();

            return RedirectToAction("GetRelatedDocument", new { Id = id2, msg = "DeleteSuccess" });

        }

        public ActionResult ReleaseDocument(int id, int id2)
        {

            ViewBag.Current = "RelatedDocument";

            var relDoc = _context.RelatedDocuments.Find(id);
            
            _context.RelatedDocuments.Remove(relDoc);
            _context.SaveChanges();

            return RedirectToAction("GetRelatedDocument", new { Id = id2, msg = "DeleteSuccess" });

        }






        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentDetails")]
        public ActionResult Details(int? id)
        {
            ViewBag.Current = "Document";

            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            var Document = _context.Documents.Include(d => d.FilesStoredInDbs).Include(a => a.Form).Include(dk => dk.Kind).Include(p => p.Party).Include(t => t.TypeMail)
                .Include(a => a.Department).Include(a => a.CreatedBy)
                .FirstOrDefault(a => a.Id == id);

            var Fields = _context.Fields.Where(f => f.FormId == Document.FormId).ToList();
            var Values = _context.Values.Where(a => a.Document_id == Document.Id).ToList();
            var seals = _context.SealDocuments.Where(s => s.DocumentId == Document.Id).Include(s => s.Document).Include(s => s.CreatedBy).ToList();

            if (Document == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            if (ManagedAes.IsCipher)
            {
                 Document.Address = ManagedAes.DecryptText(Document.Address);
                    Document.CreatedAt = ManagedAes.DecryptText(Document.CreatedAt);
                    Document.Description = ManagedAes.DecryptText(Document.Description);
                    Document.DocumentDate = ManagedAes.DecryptText(Document.DocumentDate);
                    Document.DocumentNumber = ManagedAes.DecryptText(Document.DocumentNumber);
                    Document.FileUrl = ManagedAes.DecryptText(Document.FileUrl);
                    Document.MailingDate = ManagedAes.DecryptText(Document.MailingDate);
                    Document.MailingNumber = ManagedAes.DecryptText(Document.MailingNumber);
                    Document.Name = ManagedAes.DecryptText(Document.Name);
                    Document.Notes = ManagedAes.DecryptText(Document.Notes);
                    Document.NotificationDate = ManagedAes.DecryptText(Document.NotificationDate);
                    Document.Subject = ManagedAes.DecryptText(Document.Subject);
                    Document.UpdatedAt = ManagedAes.DecryptText(Document.UpdatedAt);
            

                var files = Document.FilesStoredInDbs.ToList();
                var filesStoredInDbs = new List<FilesStoredInDb>(files.Count);
                for (int i = 0; i < files.Count; i++)
                {
                    filesStoredInDbs.Add(
                        new FilesStoredInDb
                        {
                            FileName = ManagedAes.DecryptText(files[i].FileName),
                            DocumentId = files[i].DocumentId,
                            File = ManagedAes.DecryptArrayByte(files[i].File),
                            Id = files[i].Id
                        }
                        );
                }
                var viewModel = new DocumentFieldsValuesViewModel
                {
                    Document = Document,
                    Fields = Fields,
                    Values = Values,
                    FilesStoredInDbs = filesStoredInDbs,
                    IsSaveInDb = ManagedAes.IsSaveInDb,
                    Seals = seals,
                };
            
                return View(viewModel);
            }


            var ViewModel = new DocumentFieldsValuesViewModel
            {
                Document = Document,
                Fields = Fields,
                Values = Values,
                FilesStoredInDbs = Document.FilesStoredInDbs.ToList(),
                IsSaveInDb = ManagedAes.IsSaveInDb,
                Seals = seals,
            };

            return View(ViewModel);
        }


        public FileResult DownloadDocument(int? id, string fileName)
        {
            if (id == null)
            {
                string filePath = Server.MapPath("~/Uploads/").Replace(@"\", "/") + fileName;
                byte[] fileBytes = ManagedAes.DecryptFile(filePath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            else
            {
                var FileStoredInDb = _context.FilesStoredInDbs.Find(id);
                var file = FileStoredInDb.File;
                fileName = FileStoredInDb.FileName;

                if (ManagedAes.IsCipher)
                {
                    fileName = ManagedAes.DecryptText(FileStoredInDb.FileName);
                    file = ManagedAes.DecryptArrayByte(FileStoredInDb.File);
                }

                return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
        }
        public FileResult DisplayDocument(int? id, string fileName)
        {
            if (id == null)
            {
                string filePath = Server.MapPath("~/Uploads/").Replace(@"\", "/") + fileName;

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
                var FileStoredInDb = _context.FilesStoredInDbs.Find(id);
                var file = FileStoredInDb.File;
                fileName = FileStoredInDb.FileName;

                if (ManagedAes.IsCipher)
                {
                    fileName = ManagedAes.DecryptText(FileStoredInDb.FileName);
                    file = ManagedAes.DecryptArrayByte(FileStoredInDb.File);
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

            return DownloadDocument(id, fileName);
        }

        



        public bool BiggerThan(string s1, DateTime s2)
        {
            if(string.IsNullOrEmpty(s1))
            {
                return true;
            }
            s1 = s1.Replace("-", "/");
            return DateTime.ParseExact(s1, "yyyy/MM/dd", null) >= s2;

        }


        public bool SmallerThan(string s1, DateTime s2)
        {
                if (string.IsNullOrEmpty(s1))
            {
                return true;
            }
            
            s1 = s1.Replace("-", "/");
            return DateTime.ParseExact(s1, "yyyy/MM/dd", null) <= s2;

        }






        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentIndex")]
        public ActionResult GetRelatedDocument(int id, string msg = "none")
        {


            ViewBag.Current = "Document";
            string CurrentUserId = this.User.Identity.GetUserId();
            if (!msg.Equals("none"))
            {
                ViewBag.Msg = msg;
            }
            else
            {
                ViewBag.Msg = null;
            }

            Document d = _context.Documents.Find(id);
            if(d==null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            List<int> DocIds = new List<int>();
            DocIds = UserDocumentsID.UserRelateDocument(CurrentUserId, id).ToList();
            var documents = _context.Documents.Where(a => DocIds.Contains(a.Id)).OrderByDescending(a=>a.CreatedAt).Include(a => a.TypeMail).ToList();

            // Decrypt Document Attributes.
            if (ManagedAes.IsCipher)
            {
                foreach (var Document in documents)
                {
                    Document.DocumentNumber = ManagedAes.DecryptText(Document.DocumentNumber);
                    Document.Subject = ManagedAes.DecryptText(Document.Subject);
                    Document.CreatedAt = ManagedAes.DecryptText(Document.CreatedAt);
                    Document.DocumentDate = ManagedAes.DecryptText(Document.DocumentDate);
                    //Document.Address = ManagedAes.DecryptText(Document.Address);
                    //Document.Description = ManagedAes.DecryptText(Document.Description);
                    //Document.FileUrl = ManagedAes.DecryptText(Document.FileUrl);
                    //Document.MailingDate = ManagedAes.DecryptText(Document.MailingDate);
                    //Document.MailingNumber = ManagedAes.DecryptText(Document.MailingNumber);
                    //Document.Name = ManagedAes.DecryptText(Document.Name);
                    //Document.Notes = ManagedAes.DecryptText(Document.Notes);
                    //Document.NotificationDate = ManagedAes.DecryptText(Document.NotificationDate);
                    //Document.UpdatedAt = ManagedAes.DecryptText(Document.UpdatedAt);
                }
            }

            return View(documents);
        }



        
        [AccessDeniedAuthorizeattribute(ActionName = "DocumentIndex")]
        public ActionResult GetReplayDocument(int id, string msg = "none")
        {
            string CurrentUserId = this.User.Identity.GetUserId();

            ViewBag.Current = "Document";

            if (!msg.Equals("none"))
            {
                ViewBag.Msg = msg;
            }
            else
            {
                ViewBag.Msg = null;
            }

            Document d = _context.Documents.Find(id);
            if (d == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");

            }
            List<int> DocIds = new List<int>();

            DocIds = UserDocumentsID.UserReplayDocument(CurrentUserId, id).ToList();
            var documents = _context.Documents.Where(a => DocIds.Contains(a.Id)).OrderByDescending(a => a.CreatedAt).Include(a => a.TypeMail).ToList();

            // Decrypt Document Attributes.
            if (ManagedAes.IsCipher)
            {
                foreach (var Document in documents)
                {
                    Document.DocumentNumber = ManagedAes.DecryptText(Document.DocumentNumber);
                    Document.Subject = ManagedAes.DecryptText(Document.Subject);
                    Document.CreatedAt = ManagedAes.DecryptText(Document.CreatedAt);
                    Document.DocumentDate = ManagedAes.DecryptText(Document.DocumentDate);
                    //Document.Address = ManagedAes.DecryptText(Document.Address);
                    //Document.Description = ManagedAes.DecryptText(Document.Description);
                    //Document.FileUrl = ManagedAes.DecryptText(Document.FileUrl);
                    //Document.MailingDate = ManagedAes.DecryptText(Document.MailingDate);
                    //Document.MailingNumber = ManagedAes.DecryptText(Document.MailingNumber);
                    //Document.Name = ManagedAes.DecryptText(Document.Name);
                    //Document.Notes = ManagedAes.DecryptText(Document.Notes);
                    //Document.NotificationDate = ManagedAes.DecryptText(Document.NotificationDate);
                    //Document.UpdatedAt = ManagedAes.DecryptText(Document.UpdatedAt);
                }
            }

            return View(documents);
        }




        public bool EqualDate(string s1, DateTime s2)
        {
            try
            {


                s1 = s1.Replace("-", "/");
                return DateTime.ParseExact(s1, "yyyy/MM/dd", null) == s2;
            }
            catch (Exception e)
            {
                return false;

            }

        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
    }
}