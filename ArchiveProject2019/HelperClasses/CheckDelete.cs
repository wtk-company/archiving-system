using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ArchiveProject2019.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ArchiveProject2019.HelperClasses
{
    public class CheckDelete
    {
        private static ApplicationDbContext db = new ApplicationDbContext();


        public static bool CheckDepertmentDelete(int id) {

           

         
            IEnumerable<ApplicationUser> users = db.Users.Where(a => a.DepartmentId == id);

            if (users.Count() > 0)
            {
                return false;


            }

            //Check Child:
            if (db.Departments.Where(a => a.ParentId == id).Count() > 0)
            {
                return false;
            }



            if(db.DocumentDepartments.Any(a=>a.DepartmentId==id))
            {
                return false;
            }


            //Documents:
            if(db.Documents.Any(a=>a.DepartmentId==id))
            {
                return false;
            }
           

            //Users:

            if(db.Users.Any(a=>a.DepartmentId==id))
            {
                return false;
            }
           
            return true;

        }

        public static bool CheckRoleDelete(string id)
        {
            string RoleName = db.Roles.Find(id).Name;
            IEnumerable<ApplicationUser> users = db.Users.Where(a => a.RoleName.Equals(RoleName));
            if (users.Count() > 0)
            {
                return false;

            }

       

         


            return true;
        }

        public static bool CheckJobTitleDelete(int id)
        {
            IEnumerable<ApplicationUser> users = db.Users.Where(a => a.JobTitleId == id);
            if (users.Count() > 0)
            {
                return false;

            }
            return true;
        }



        public static bool CheckGroupDelete (int id)
        {

            IEnumerable<UserGroup> UsersGroup = db.UsersGroups.Where(a => a.GroupId == id);
            if (UsersGroup.Count() > 0)
            {
                return false;

            }

           //Document

            if(db.DocumentGroups.Where(a=>a.GroupId==id).Count()>0)
            {
                return false;
            }


            return true;
        }



        public static bool CheckDocumentKindsDelete(int id)

        {

            IEnumerable<int> Documents = db.Documents.Where(a => a.KindId == id).Select(a=>a.Id);
            if (Documents.Count() > 0)
            {
                return false;

            }
            return true;
        }


      

        public static bool checkFormDelete(int id)
        {
            Form f = db.Forms.Find(id);
            //Documents:

            if(f.Type==1)
            {
                return false;
            }

            List<Document> docs = db.Documents.Where(a => a.FormId == id).ToList();
            if(docs.Count()>0)
            {
                return false;

            }
            return true;

        }

        public static bool checkFieldsDelete(int id)
        {
            List<Value> values = db.Values.Where(a => a.FieldId == id).ToList();

            if(values.Count()>0)
            {
                return false;
            }

            return true;

        }


        public static bool CheckTypeMaildelete(int id)
        {

            IEnumerable<Document> documents = db.Documents.Where(a => a.TypeMailId == id);
            if (documents.Count() > 0)
            {
                return false;

            }

            TypeMail mail = db.TypeMails.Find(id);
            if(mail.Type!=0)
            {
                return false;
            }




            return true;
        }



        public static bool CheckPartydelete(int id)
        {

            IEnumerable<Document> documents = db.Documents.Where(a => a.PartyId == id);
            if (documents.Count() > 0)
            {
                return false;

            }

       




            return true;
        }

        public static bool CheckDocumentStatusDelete(int id)
        {

            IEnumerable<Document> documents = db.Documents.Where(a => a.StatusId == id);
            if (documents.Count() > 0)
            {
                return false;

            }

            DocumentStatus mail = db.DocumentStatuses.Find(id);
            if (mail.Type == 1)
            {
                return false;
            }




            return true;
        }


        public static bool CheckTypeMailEdit(int id)
        {

           

            TypeMail mail = db.TypeMails.Find(id);
            if (mail.Type != 0)
            {
                return false;
            }




            return true;
        }


        public static bool CheckDocumentStatusEdit(int id)
        {



            DocumentStatus mail = db.DocumentStatuses.Find(id);
            if (mail.Type == 1)
            {
                return false;
            }




            return true;
        }

        public static bool CheckUserDelete(string id)
        {

            ApplicationUser user = db.Users.Find(id);
            if(user.RoleName.Equals("Master") && user.IsDefaultMaster==true)
            {
                return false;

            }

            

            //created by|| Updated by:
           if(db.Users.Any(a=>a.CreatedById.Equals(id)||a.UpdatedByID.Equals(id)))
            {
                return false;
            }
            if (db.Company.Any(a => a.CreateById.Equals(id)))
            {
                return false;
            }

            if (db.Departments.Any(a => a.CreatedById.Equals(id) || a.UpdatedById.Equals(id)))
            {
                return false;
            }

            if (db.Documents.Any(a => a.CreatedById.Equals(id) || a.UpdatedById.Equals(id)))
            {
                return false;
            }

            if (db.Documents.Any(a => a.CreatedById.Equals(id) || a.UpdatedById.Equals(id)))
            {
                return false;
            }
            if (db.DocumentStatuses.Any(a => a.CreatedById.Equals(id) || a.UpdatedById.Equals(id)))
            {
                return false;
            }

            if (db.DocumentStatuses.Any(a => a.CreatedById.Equals(id) || a.UpdatedById.Equals(id)))
            {
                return false;
            }
            if (db.Fields.Any(a => a.CreatedById.Equals(id) || a.UpdatedById.Equals(id)))
            {
                return false;
            }

            if (db.FormDepartments.Any(a => a.CreatedById.Equals(id) || a.UpdatedById.Equals(id)))
            {
                return false;
            }

            if (db.FormGroups.Any(a => a.CreatedById.Equals(id) || a.UpdatedById.Equals(id)))
            {
                return false;
            }

            if (db.Forms.Any(a => a.CreatedById.Equals(id) || a.UpdatedById.Equals(id)))
            {
                return false;
            }
            if (db.Groups.Any(a => a.CreatedById.Equals(id) || a.UpdatedById.Equals(id)))
            {
                return false;
            }

            if (db.JobTitles.Any(a => a.CreatedById.Equals(id) || a.UpdatedById.Equals(id)))
            {
                return false;
            }

            if (db.Kinds.Any(a => a.CreatedById.Equals(id) || a.UpdatedById.Equals(id)))
            {
                return false;
            }

            if (db.Parties.Any(a => a.CreatedById.Equals(id) || a.UpdatedById.Equals(id)))
            {
                return false;
            }

            if (db.TypeMails.Any(a => a.CreatedById.Equals(id) || a.UpdatedById.Equals(id)))
            {
                return false;
            }

            if (db.SealDocuments.Any(a => a.CreatedById.Equals(id) ))
            {
                return false;
            }

            if (db.UsersGroups.Any(a => a.CreatedById.Equals(id) || a.UpdatedById.Equals(id)))
            {
                return false;
            }

            if (db.Users.Any(a => a.CreatedById.Equals(id) || a.UpdatedByID.Equals(id)))
            {
                return false;
            }

            RoleManager<ApplicationRoles> manger= new RoleManager<ApplicationRoles>(new RoleStore<ApplicationRoles>(db));

            if(manger.Roles.Any(a=>a.CreatedById.Equals(id)||a.UpdatedById.Equals(id)))
            {
                return false;
            }
            return true;



        }


        public static bool CheckUserLockOut(string id)
        {

            ApplicationUser user = db.Users.Find(id);
            if (user.RoleName.Equals("Master") && user.IsDefaultMaster == true)
            {
                return false;

            }
            return true;



        }


        public static bool CheckUserEdit(string id)
        {

            ApplicationUser user = db.Users.Find(id);
            if (user.RoleName.Equals("Master") && user.IsDefaultMaster == true)
            {
                return false;

            }
            return true;



        }

        public static bool CheckDocumentDepartmentDelete(int docId,int DepId)
        {

            int DocumentdepId = db.Documents.Find(DepId).DepartmentId;
            if(DocumentdepId==DepId)
            {
                return true;
            }

            return false;



        }

    }
}