using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ArchiveProject2019.Models;
using System.Data.Entity;

namespace ArchiveProject2019.HelperClasses
{

    public class UsersDepartmentAndGroupsForms
    {
        public static List<Form> GetUsersForms(string UsertID)
        {
            List<Form> Forms = new List<Form>();

            ApplicationDbContext _context = new ApplicationDbContext();
            List<int> UserFormsID = new List<int>();
            List<int> AllUsersFormsId = new List<int>();


            if (_context.Users.Find(UsertID).RoleName.Equals("Master"))
            {


                AllUsersFormsId = _context.Forms.Select(a => a.Id).ToList();
            }
            else
            {


                List<int> FM = new List<int>();
                int TypeOfFormsDisplay = _context.Users.Include(a => a.jobTitle).SingleOrDefault(a => a.Id.Equals(UsertID)).jobTitle.TypeOfDisplayForm;
                switch (TypeOfFormsDisplay)
                {
                    case 0:

                        FM = UserDepartmentForms.GetUserDepartmentFormsNormal(UsertID);
                        UserFormsID = _context.Forms.Where(a => FM.Contains(a.Id)).Select(a => a.Id).ToList();

                        break;
                    case 1:

                        FM = UserDepartmentForms.GetUserDepartmentFormsAndFirstChildrenLevel(UsertID);
                        UserFormsID = _context.Forms.Where(a => FM.Contains(a.Id)).Select(a => a.Id).ToList();

                        break;

                    case 2:
                        FM = UserDepartmentForms.GetUserDepartmentFormsAndAllchildren(UsertID);
                        UserFormsID = _context.Forms.Where(a => FM.Contains(a.Id)).Select(a => a.Id).ToList();

                        break;

                    case 3:
                        FM = UserDepartmentForms.GetUserDepartmentAndAllSiblings(UsertID);
                        UserFormsID = _context.Forms.Where(a => FM.Contains(a.Id)).Select(a => a.Id).ToList();

                        break;

                    case 4:
                        FM = UserDepartmentForms.GetUserDepartmentFormsAndParentDepartmentForms(UsertID);
                        UserFormsID = _context.Forms.Where(a => FM.Contains(a.Id)).Select(a => a.Id).ToList();

                        break;

                    case 5:
                        FM = UserDepartmentForms.GetUserDepartmentFormsAndAllParentDepartmentForms(UsertID);

                        UserFormsID = _context.Forms.Where(a => FM.Contains(a.Id)).Select(a => a.Id).ToList();


                        break;

                }
                List<int> UserGroupsId = new List<int>();
                UserGroupsId = _context.UsersGroups.Where(a => a.UserId.Equals(UsertID)).Select(a => a.GroupId).ToList();
                List<int> UserGroupsFormsId = new List<int>();
                if (UserGroupsId.Count() > 0)
                {
                    UserGroupsFormsId = _context.FormGroups.Where(a => UserGroupsId.Contains(a.GroupId)).Select(a => a.FormId).ToList();
                    AllUsersFormsId = UserFormsID.Union(UserGroupsFormsId).Distinct().ToList();
              

                }
                else
                {

                    AllUsersFormsId = UserFormsID.Distinct().ToList();
                }

            }

             Forms = _context.Forms.Where(a => AllUsersFormsId.Contains(a.Id)).ToList();
            return Forms;


        }
    }
}