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



            //All Forms for Master
            if (_context.Users.Find(UsertID).RoleName.Equals("Master"))
            {


                AllUsersFormsId = _context.Forms.Select(a => a.Id).ToList();
            }
            else
            {
                int DepUserId = _context.Users.Find(UsertID).DepartmentId.Value;
                List<int> FM = _context.FormDepartments.Where(a => a.DepartmentId == DepUserId&& a.Is_Active==true).Select(a => a.FormId).ToList();
                  UserFormsID = _context.Forms.Where(a => FM.Contains(a.Id)).Select(a => a.Id).ToList();

                      
                List<int> UserGroupsId = new List<int>();
                UserGroupsId = _context.UsersGroups.Where(a => a.UserId.Equals(UsertID)).Select(a => a.GroupId).ToList();
                List<int> UserGroupsFormsId = new List<int>();
                if (UserGroupsId.Count() > 0)
                {
                    UserGroupsFormsId = _context.FormGroups.Where(a => UserGroupsId.Contains(a.GroupId)&&a.Is_Active==true).Select(a => a.FormId).ToList();
                    AllUsersFormsId = UserFormsID.Union(UserGroupsFormsId).Distinct().ToList();
              

                }
                else
                {

                    AllUsersFormsId = UserFormsID.Distinct().ToList();
                }

            }

             Forms = _context.Forms.Where(a => AllUsersFormsId.Contains(a.Id)||a.Type==1).ToList();
            return Forms;


        }
    }
}