using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ArchiveProject2019.Models;
namespace ArchiveProject2019.HelperClasses
{
    public class CheckJobTitleDepartment
    {
        public static bool CheckJobTitleDepartmentCreateUser(int DepId,int JobId)
        {
         ApplicationDbContext db = new ApplicationDbContext();

            IEnumerable<ApplicationUser> Users = db.Users.Where(a => a.DepartmentId == DepId && a.JobTitleId == JobId);
            JobTitle jobTitle = db.JobTitles.Find(JobId);
               
                if(Users.Count()==jobTitle.MaximumMember)   {
                return false;

                 }

            return true;

        }



        public static bool CheckJobTitleDepartmentCreateUser(int DepId, int JobId,string Uid)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            IEnumerable<ApplicationUser> Users = db.Users.Where(a => a.DepartmentId == DepId && a.JobTitleId == JobId&& !a.Id.Equals(Uid,StringComparison.OrdinalIgnoreCase));
            JobTitle jobTitle = db.JobTitles.Find(JobId);

            if (Users.Count() == jobTitle.MaximumMember)
            {
                return false;

            }

            return true;

        }

    }
}