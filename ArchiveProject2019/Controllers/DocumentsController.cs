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

namespace ArchiveProject2019.Controllers
{
    public class DocumentsController : Controller
    {
        private bool IsSaveInDb = false;

        ApplicationDbContext _context;

        public DocumentsController()
        {
            this._context = new ApplicationDbContext();
            this.IsSaveInDb = true;
        }
        public ActionResult Form()
        {
            ViewBag.Current = "Document";

            DocFromsViewModel viewModel = new DocFromsViewModel()
            {
                DocId = -1,
                Froms = UsersDepartmentAndGroupsForms.GetUsersForms(this.User.Identity.GetUserId())
            };

            return View(viewModel);
        }

        public ActionResult RelateDocument(int id)
        {
            ViewBag.Current = "Document";

            var viewModel = new DocFromsViewModel()
            {
                DocId = id,
                Froms = _context.Forms.ToList()
            };

            return View("Form", viewModel);
        }
        [HttpGet]
        // Edit Document
        public ActionResult Edit(int? id)
        {
            ViewBag.Current = "Document";

            if (id == null)
            {
                return RedirectToAction("BadRequestError", "ErrorController");
            }

            var document = _context.Documents.Include(a => a.Department).Include(d => d.FilesStoredInDbs).Include(dk => dk.Kind).Include(p => p.Party).Include(t => t.TypeMail).Include(a => a.Values).Include(a => a.Form).FirstOrDefault(a => a.Id == id);
            if (document == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            var Fields = _context.Fields.Include(c => c.Form).Where(f => f.FormId == document.FormId).ToList();
            List<Value> Values = new List<Value>();
            Values = document.Values.ToList();
            FieldsValuesViewModel viewModel = new FieldsValuesViewModel
            {
                Fields = Fields,
                Values = Values
            };

            ViewBag.Departments = new SelectList(_context.Departments.ToList(), "Id", "Name", document.DepartmentId);
            ViewBag.kinds = new SelectList(_context.Kinds.ToList(), "Id", "Name");
            ViewBag.Parties = new SelectList(_context.Parties.ToList(), "Id", "Name", document.PartyId);
            ViewBag.TypeMailId = new SelectList(_context.TypeMails.ToList(), "Id", "Name");
            ViewBag.Groups = new SelectList(_context.Groups.ToList(), "Id", "Name");
            ViewBag.DepartmentList = new SelectList(_context.Departments.ToList(), "Id", "Name");
            
            if (IsSaveInDb)
            {
                var names = document.FilesStoredInDbs.Count;
                var existfiles = Enumerable.Repeat(true, names).ToList();

                var myModel = new DocumentDocIdFieldsValuesViewModel()
                {
                    Document = document,
                    FieldsValues = viewModel,
                    ExistFiles = existfiles,
                    FilesStoredInDbs = document.FilesStoredInDbs.ToList(),
                    TypeMail = document.TypeMail.Type,
                    IsSaveInDb = IsSaveInDb,
                };

                return View(myModel);
            }
            else
            {
                var urls = document.FileUrl.Split(new string[] { "_##_" }, StringSplitOptions.None);
                var existfiles = Enumerable.Repeat(true, urls.Length).ToList();

                var myModel = new DocumentDocIdFieldsValuesViewModel()
                {
                    Document = document,
                    FieldsValues = viewModel,
                    ExistFiles = existfiles,
                };

                return View(myModel);
            }
        }


        // Edit Conform
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(DocumentDocIdFieldsValuesViewModel viewModel, HttpPostedFileBase[] UploadFile, IEnumerable<HttpPostedFileBase> FieldFile)
        {

            ViewBag.Current = "Document";

            bool Status = true;

            // Check your mail type selection
            //if (viewModel.Document.TypeOfMail == null)
            // {
            //     ModelState.AddModelError("Document.TypeOfMail", "اختر نوع بريد الورود");
            //     Status = false;
            // }

            // Check that the mail number is entered
            //if (viewModel.Document.MailingNumber == null && viewModel.Document.TypeOfMail == "وارد")
            //{
            //    ModelState.AddModelError("Document.MailingNumber", "ادخل رقم ورود البريد");
            //    Status = false;
            //}

            // Check that the mail date is entered
            //if (viewModel.Document.MailingDate == null && viewModel.Document.TypeOfMail == "وارد")
            //{
            //    ModelState.AddModelError("Document.MailingDate", "ادخل تاريخ ورود البريد");
            //    Status = false;
            //}

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

            ViewBag.Departments = new SelectList(_context.Departments.ToList(), "Id", "Name", viewModel.Document.DepartmentId);
            ViewBag.kinds = new SelectList(_context.Kinds.ToList(), "Id", "Name");
            ViewBag.Parties = new SelectList(_context.Parties.ToList(), "Id", "Name", viewModel.Document.PartyId);
            ViewBag.TypeMailId = new SelectList(_context.TypeMails.ToList(), "Id", "Name");
            ViewBag.Groups = new SelectList(_context.Groups.ToList(), "Id", "Name");
            ViewBag.DepartmentList = new SelectList(_context.Departments.ToList(), "Id", "Name");

            //ViewBag.TypeMail = new List<SelectListItem>()
            //{
            //    new SelectListItem { Text="اختر نوع البريد", Value = null},
            //    new SelectListItem { Text="وارد", Value="وارد"},
            //    new SelectListItem { Text="صادر", Value="صادر" },
            //    new SelectListItem { Text="داخلي", Value="داخلي" },
            //    new SelectListItem { Text="ارشيف", Value="ارشيف" },
            //};

            if (UploadFile.ElementAt(0) == null && !viewModel.ExistFiles.Contains(true))
            {
                Status = false;
                ModelState.AddModelError("Document.FileUrl", "يجب إدخال ملفات ");
            }
            else
            {
                var IsEmpty = true;
                for (int i = 0; i < UploadFile.Length; i++)
                {
                    if (UploadFile[i] != null && viewModel.ExistFiles[i] == true)
                    {
                        IsEmpty = false;
                    }
                }
                if (IsEmpty)
                {
                    Status = false;
                    ModelState.AddModelError("Document.FileUrl", "يجب إدخال ملفات ");
                }
            }


            //Status Model=false{status==false}
            if (Status == false)
            {
                return View(viewModel);
            }

            if (ModelState.IsValid)
            {
                if (IsSaveInDb)
                {
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

                                    _context.Entry(viewModel.FilesStoredInDbs[i]).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }
                            }
                            else
                            {
                                var filesStoredInDb = _context.FilesStoredInDbs.Find(viewModel.FilesStoredInDbs[i].Id);
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
                                    UploadFile[i].SaveAs(path);

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
                
                //Document Update:
                _context.Entry(viewModel.Document).State = EntityState.Modified;
                _context.SaveChanges();

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

        // GET: Documents.
        public ActionResult Index(string id = "none")
        {
            ViewBag.Current = "Document";

            if (!id.Equals("none"))
            {
                ViewBag.Msg = id;
            }
            else
            {
                ViewBag.Msg = null;
            }

            // Get all Documents.

            string currentUserId = this.User.Identity.GetUserId();
            List<int> DocRelate = _context.RelatedDocuments.Where(a => a.CreatedById.Equals(currentUserId)).Select(a => a.RelatedDocId).ToList();

            var documents = (this.IsSaveInDb)
                ? _context.Documents.Where(a => a.CreatedById.Equals(currentUserId) && !DocRelate.Contains(a.Id) && a.FileUrl == null).Include(c => c.Form).Include(dk => dk.Kind).Include(p => p.Party).Include(t => t.TypeMail).Include(d => d.Department).Include(a => a.CreatedBy).ToList()
                : _context.Documents.Where(a => a.CreatedById.Equals(currentUserId) && !DocRelate.Contains(a.Id) && a.FileUrl != null).Include(c => c.Form).Include(dk => dk.Kind).Include(p => p.Party).Include(t => t.TypeMail).Include(d => d.Department).Include(a => a.CreatedBy).ToList();
         
            documents = documents.OrderByDescending(a => a.CreatedAt).ToList();

            // Pass To View
            return View(documents);
        }

        public ActionResult Relate(int id)
        {
            var documents = _context.Documents.Where(d => d.Id != id)
                                              .Include(d => d.RelatedDocuments)
                                              .Include(c => c.Form)
                                              .Include(d => d.Department)
                                              .Include(a => a.CreatedBy)
                                              .ToList();

            var relDocs = _context.RelatedDocuments.Where(rd => rd.Document_id == id).ToList();

            var docs = new List<RelatedDocumentViewModel>();

            for (int i = 0; i < documents.Count; i++)
            {
                for (int j = 0; j < relDocs.Count; j++)
                {
                    if (relDocs[j].RelatedDocId == documents[i].Id)
                    {
                        documents.Remove(documents[i]);
                    }
                }
            }
            for (int i = 0; i < documents.Count; i++)
            {
                var DocModel = new RelatedDocumentViewModel()
                {
                    Id = documents[i].Id,
                    Department = documents[i].Department.Name,
                    Form = documents[i].Form.Name,
                    Number = documents[i].DocumentNumber,
                    Subject = documents[i].Subject,
                };

                docs.Add(DocModel);
            }

            var viewModel = new RelateDocumentsViewModel()
            {
                DocId = id,
                RelatedDocuments = docs,
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Relate(RelateDocumentsViewModel viewModel)
        {
            foreach (var item in viewModel.RelatedDocuments)
            {
                if (item.IsRelated == true)
                {
                    var relDoc = new RelatedDocument()
                    {
                        Document_id = viewModel.DocId,
                        RelatedDocId = item.Id
                    };

                    _context.RelatedDocuments.Add(relDoc);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }


        // GET: /Documents/Create
        public ActionResult Create(int Id, int docId)
        {
            ViewBag.Current = "Document";

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

            DocumentDocIdFieldsValuesViewModel myModel = new DocumentDocIdFieldsValuesViewModel()
            {
                DocId = docId,
                Document = new Models.Document() { FormId = Id },
                FieldsValues = viewModel
            };

            ViewBag.Departments = new SelectList(_context.Departments.ToList(), "Id", "Name");
            ViewBag.kinds = new SelectList(_context.Kinds.ToList(), "Id", "Name");
            ViewBag.Parties = new SelectList(_context.Parties.ToList(), "Id", "Name");
            ViewBag.TypeMailId = new SelectList(_context.TypeMails.ToList(), "Id", "Name");
            ViewBag.Groups = new SelectList(_context.Groups.ToList(), "Id", "Name");
            ViewBag.DepartmentList = new SelectList(_context.Departments.ToList(), "Id", "Name");

            return View(myModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(DocumentDocIdFieldsValuesViewModel viewModel, IEnumerable<HttpPostedFileBase> UploadFile, IEnumerable<HttpPostedFileBase> FieldFile, IEnumerable<string> PartyIds, IEnumerable<string> Departments, IEnumerable<string> Groups)
        {
            ViewBag.Current = "Document";

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



                //اختبار الحقل من النمط صورة إذا كان ضروري او لا



                //K :counter for files
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


                //أختبار صيغة كل ملف من الملفات


                int j = 0;


                for (int i = 0; i < FVVM.Values.Count(); i++)
                {



                    if (FVVM.Fields[i].Type.Equals("file"))
                    {

                        if (FieldFile.ElementAt(j) != null)
                        {
                            bool ImageExtention = CheckFileFormatting.PermissionFile(FieldFile.ElementAt(j));

                            if (ImageExtention == false)
                            {
                                Status = false;
                                ModelState.AddModelError("FieldsValues.Values[" + i + "].Id", " صيغةالملف غير مدعومةالرجاء إعادةالإدخال");
                            }
                            else
                            {
                                string FileName = Path.GetFileName(FieldFile.ElementAt(j).FileName);
                                //Save In Server

                                string s1 = DateTime.Now.ToString("yyyyMMddhhHHmmss") + FileName;
                                string path = Path.Combine(Server.MapPath("~/Uploads/"), s1);
                                FieldFile.ElementAt(j).SaveAs(path);

                                //Save In Db:
                                viewModel.FieldsValues.Values[i].FieldValue = "~/Uploads/" + s1;

                            }//else
                        }//file Not null
                        j++;
                    }//If {file}
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


            //Check Files Upload Exist:
            if (UploadFile.ElementAt(0) == null)
            {
                Status = false;
                ModelState.AddModelError("Document.FileUrl", "يجب إدخال ملفات ");
            }

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

            // validate Departments multi select list
            if (mail != null && mail.Type.Equals(3) && Departments == null)
            {
                ModelState.AddModelError("Departments", "حدد الاقسام المستهدفة");
                Status = false;
            }
            // validate Groups multi select list
            if (mail != null && mail.Type.Equals(3) && Groups == null)
            {
                ModelState.AddModelError("Groups", "حدد المجموعات المستهدفة");
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

            ViewBag.Departments = new SelectList(_context.Departments.ToList(), "Id", "Name", viewModel.Document.DepartmentId);
            ViewBag.kinds = new SelectList(_context.Kinds.ToList(), "Id", "Name");
            ViewBag.Parties = new SelectList(_context.Parties.ToList(), "Id", "Name");
            ViewBag.TypeMailId = new SelectList(_context.TypeMails.ToList(), "Id", "Name");
            ViewBag.Groups = new SelectList(_context.Groups.ToList(), "Id", "Name");
            ViewBag.DepartmentList = new SelectList(_context.Departments.ToList(), "Id", "Name");
            //Status Model = false{status==false}
            if (Status == false)
            {
                return View(viewModel);
            }

            if (ModelState.IsValid)
            {

                // Get Current User Id
                var UserId = User.Identity.GetUserId();

                if (!IsSaveInDb)
                {
                    foreach (var file in UploadFile)
                    {
                        if(file != null)
                        {
                            //Save File In Uploads
                            string FileName = Path.GetFileName(file.FileName);

                            string s1 = DateTime.Now.ToString("yyyyMMddhhHHmmss") + FileName;
                            string path = Path.Combine(Server.MapPath("~/Uploads"), s1);
                            file.SaveAs(path);

                            viewModel.Document.Name += FileName + "_##_";
                            viewModel.Document.FileUrl += s1 + "_##_";
                        
                        }
                    }

                    // Cut last 4 split string
                    viewModel.Document.Name = viewModel.Document.Name.Substring(0, viewModel.Document.Name.Length - 4);
                    viewModel.Document.FileUrl = viewModel.Document.FileUrl.Substring(0, viewModel.Document.FileUrl.Length - 4);
                }
                
                // Document Details:
                viewModel.Document.CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss");
                viewModel.Document.CreatedById = UserId;
                viewModel.Document.FormId = viewModel.Document.FormId;

                _context.Documents.Add(viewModel.Document);
                _context.SaveChanges();


                if (IsSaveInDb)
                {
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

                            _context.FilesStoredInDbs.Add(fileStoredInDb);
                            _context.SaveChanges();
                        }
                    }
                }
                // Save Multiple Files In Db

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
                // store Department and Groups for internal Mail
                if (Departments != null && Groups != null && mail.Type == 3)
                {
                    // Store Departments
                    foreach (string DeptId in Departments)
                    {
                        var DocDept = new DocumentTargetDepartment()
                        {
                            DocumentId = viewModel.Document.Id,
                            DepartmentId = Convert.ToInt32(DeptId),
                            CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                            CreatedById = UserId
                        };

                        _context.DocumentTargetDepartments.Add(DocDept);
                        _context.SaveChanges();
                    }
                    // Store Groups
                    foreach (string GroupId in Groups)
                    {
                        var DocGroup = new DocumentTargetGroup()
                        {
                            DocumentId = viewModel.Document.Id,
                            GroupId = Convert.ToInt32(GroupId),
                            CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                            CreatedById = UserId
                        };

                        _context.DocumentTargetGroups.Add(DocGroup);
                        _context.SaveChanges();
                    }
                }
                // Relate Document
                var docId = viewModel.DocId;
                if (docId != -1)
                {
                    var relateDoc = new RelatedDocument()
                    {
                        RelatedDocId = viewModel.Document.Id,
                        CreatedById = UserId,
                        Document_id = docId,
                    };

                    _context.RelatedDocuments.Add(relateDoc);
                    _context.SaveChanges();
                }
                return RedirectToAction("Index", new { id = viewModel.Document.Id });
            }
            return View(viewModel);
        }

        public ActionResult Delete(int? id)
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

            return View(document);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(int? id)
        {
            ViewBag.Current = "Document";

            var document = _context.Documents.Find(id);

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
            _context.SaveChanges();

            return RedirectToAction("Index", new { Id = "DeleteSuccess" });

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

            if (Document == null)
            {
                return RedirectToAction("HttpNotFoundError", "ErrorController");
            }

            var viewModel = new DocumentFieldsValuesViewModel
            {
                Document = Document,
                Fields = Fields,
                Values = Values,
                FilesStoredInDbs = Document.FilesStoredInDbs.ToList(),
                IsSaveInDb = this.IsSaveInDb,
            };

            return View(viewModel);
        }


        public FileResult DownloadDocument(int? id, string fileName)
        {
            if (id == null)
            {
                string filePath = Server.MapPath("~/Uploads/").Replace(@"\", "/") + fileName;
                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            else
            {
                var FileStoredInDb = _context.FilesStoredInDbs.Find(id);
                return File(FileStoredInDb.File, System.Net.Mime.MediaTypeNames.Application.Octet, FileStoredInDb.FileName);
            }
        }
        public FileResult DisplayDocument(int? id, string fileName)
        {
            if (id == null)
            {
                string filePath = Server.MapPath("~/Uploads/").Replace(@"\", "/") + fileName;

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

                // Images
                if (FileStoredInDb.FileName.EndsWith("jpeg") || FileStoredInDb.FileName.EndsWith("JPEG"))
                    return File(FileStoredInDb.File, "image/jpeg");

                if (FileStoredInDb.FileName.EndsWith("jpg") || FileStoredInDb.FileName.EndsWith("JPG"))
                    return File(FileStoredInDb.File, "image/jpg");

                if (FileStoredInDb.FileName.EndsWith("png") || FileStoredInDb.FileName.EndsWith("PNG"))
                    return File(FileStoredInDb.File, "image/png");

                if (FileStoredInDb.FileName.EndsWith("gif") || FileStoredInDb.FileName.EndsWith("GIF"))
                    return File(FileStoredInDb.File, "image/gif");

                // Pdf
                if (FileStoredInDb.FileName.EndsWith("pdf") || FileStoredInDb.FileName.EndsWith("PDF"))
                    return File(FileStoredInDb.File, "application/pdf");

            }

            return DownloadDocument(id, fileName);
        }

        public ActionResult AdvancedDocumentSearch()
        {
            ViewBag.kinds = new SelectList(_context.Kinds.ToList(), "Id", "Name");
            ViewBag.parties = new SelectList(_context.Parties.ToList(), "Id", "Name");
            ViewBag.MailType = new SelectList(_context.TypeMails.ToList(),"Id","Name");

            return View();
        }
        public DateTime stringToDate(string s)
        {
            return DateTime.ParseExact(s, "yyyy/MM/dd", null);
        }





        public bool BiggerThan(string s1, DateTime s2)
        {
            s1 = s1.Replace("-", "/");
            return DateTime.ParseExact(s1, "yyyy/MM/dd", null) >= s2;

        }


        public bool SmallerThan(string s1, DateTime s2)
        {
            s1 = s1.Replace("-", "/");
            return DateTime.ParseExact(s1, "yyyy/MM/dd", null) <= s2;

        }




        [HttpPost]
        public ActionResult AdvancedDocumentSearch(int? RecordCount, int RetrievalCount, string DocNum,
            string DocSubject, string DocKind, string DocParty, string MailType, string DocDescription, string DocFirstDate, string DocEndDate)
        {
            List<Document> documents = null;
            DateTime fdate, ldate;
            if (DocFirstDate == "")
            {
                fdate = DateTime.MinValue;
            }
            else
            {
                DocFirstDate = DocFirstDate.Replace("-", "/");
                fdate = DateTime.ParseExact(DocFirstDate, "yyyy/MM/dd", null);
            }

            if (DocEndDate == "")
            {
                ldate = DateTime.MaxValue;
            }
            else
            {
                DocFirstDate = DocFirstDate.Replace("-", "/");
                ldate = DateTime.ParseExact(DocFirstDate, "yyyy/MM/dd", null);
            }

            if (RecordCount == null)
            {
                documents = _context.Documents.OrderByDescending(d => d.DocumentDate)
                    .Where(
                            // Filter by Document Name
                            d => d.DocumentNumber.Contains(DocNum) &&

                            // Filter by Document Address
                            d.Address.Contains(DocSubject) &&

                            // Filter by Document Description
                            d.Address.Contains(DocDescription) ||

                            // Filter by Document Kind
                            d.KindId.ToString().Equals(DocKind) ||

                            // Filter by Document Party
                            d.PartyId.ToString().Equals(DocParty) ||


                            // Filter by Mail Type
                            d.TypeMailId.ToString().Equals(MailType)

                          //   && BiggerThan(d.DocumentDate, fdate)                            // Filter by Document First Date
                          //    DateTime.ParseExact(d.DocumentDate, "yyyy/MM/dd", null) >=fdate&&

                          // Filter by Document End Data
                          // DateTime.ParseExact(d.DocumentDate, "yyyy/MM/dd", null) <= ldate
                          )
                          .Include(d => d.Department).Include(d => d.Form)
                          .ToList();



                //documents = (from d in _context.Documents
                //            where
                //            d.DocumentNumber.Contains(DocNum) &&
                //               d.Address.Contains(DocSubject) &&

                //              // Filter by Document Description
                //              d.Address.Contains(DocDescription) ||

                //              // Filter by Document Kind
                //              d.KindId.ToString().Equals(DocKind) ||

                //              // Filter by Document Party
                //              d.PartyId.ToString().Equals(DocParty) ||


                //              // Filter by Mail Type
                //              d.TypeMailId.ToString().Equals(MailType) 
                //            select (d)).ToList();


               documents = documents.Where(a => BiggerThan(a.DocumentDate, fdate) &&SmallerThan(a.DocumentDate,ldate)).Take(RetrievalCount).ToList();

            }
            else
            {
                // incerment counter
                ++RecordCount;

                documents = _context.Documents.OrderByDescending(d => d.DocumentDate)
                    .Where(
                            // Filter by Document Name
                            d => d.DocumentNumber.Contains(DocNum) &&

                            // Filter by Document Address
                            d.Address.Contains(DocSubject) &&

                            // Filter by Document Description
                            d.Address.Contains(DocDescription) 

                            //// Filter by Document Kind
                            //d.KindId.Equals(Int32.Parse(DocKind)) &&

                            //// Filter by Document Party
                            //d.PartyId.Equals(Int32.Parse(DocParty)) &&

                            //// Filter by Mail Type
                            //d.TypeMailId.Equals(Int32.Parse(MailType)) &&

                            //// Filter by Document First Date
                            //CheckDate.StringToDate(d.DocumentDate) >= CheckDate.StringToDate(DocFirstDate) &&
                            //// Filter by Document End Data
                            //CheckDate.StringToDate(d.DocumentDate) <= CheckDate.StringToDate(DocEndDate)
                          )
                          .Include(d => d.Department).Include(d => d.Form).Skip(RetrievalCount * RecordCount.Value)
                          .Take(RetrievalCount).ToList();
            }

            ViewBag.kinds = new SelectList(_context.Kinds.ToList(), "Id", "Name");
            ViewBag.Parties = new SelectList(_context.Parties.ToList(), "Id", "Name");

            return PartialView("_search", documents);
        }

        public ActionResult GetRelatedDocument(int id, string msg = "none")
        {
            ViewBag.Current = "Document";

            if (!msg.Equals("none"))
            {
                ViewBag.Msg = msg;
            }
            else
            {
                ViewBag.Msg = null;
            }

            var relDocs = _context.RelatedDocuments.Include(rd => rd.Document).Include(rd => rd.Document.Form).Include(rd => rd.Document.Department).Where(d => d.Document_id == id).ToList();

            return View(relDocs);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
    }
}