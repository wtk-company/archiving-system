using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ArchiveProject2019.Models;
using Microsoft.AspNet.Identity;

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

           string CurerentUserId= HttpContext.Current.User.Identity.GetUserId();

         


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

            List<int> Documents = db.Documents.Where(a => a.CreatedById.Equals(id)).Select(a => a.Id).ToList();
            if(Documents.Count()>0)
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


    }
}