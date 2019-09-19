using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ArchiveProject2019.Models;
namespace ArchiveProject2019.HelperClasses
{
    public class CheckDelete
    {
        private static ApplicationDbContext db = new ApplicationDbContext();


        public static bool CheckDepertmentDelete(int id) {

            //Documents:
            //IEnumerable<Document> Documents = db.Documents.Where(a => a.DepartmentId == id);
            //if (Documents.Count() > 0) {

            //    return false;
            //}


         
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



            //check fORMS:
            if(db.FormDepartments.Where(a=>a.DepartmentId==id).Count()>0)
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

            if(db.FormGroups.Where(a=>a.GroupId==id).Count()>0)
            {
                return false;
            }




            return true;
        }


        //public static bool CheckDocumentKindsDelete(int id)
        //{

        //    IEnumerable<int> Documents = db.Documents.Where(a => a.);
        //    if (UsersGroup.Count() > 0)
        //    {
        //        return false;

        //    }
        //    return true;
        //}


        public static bool CheckUserDelete(int id)
        {

            //check documents:



            return true;



        }


    }
}