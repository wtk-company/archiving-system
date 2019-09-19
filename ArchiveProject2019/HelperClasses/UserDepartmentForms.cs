using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ArchiveProject2019.Models;
namespace ArchiveProject2019.HelperClasses
{
    public class UserDepartmentForms
    {
        public static List<int> GetUserDepartmentFormsNormal(string UserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<int> FormsId = new List<int>();
            int DepartmentId = db.Users.Find(UserId).DepartmentId.Value;

            FormsId = db.FormDepartments.Where(a => a.DepartmentId == DepartmentId && a.Is_Active == true)
                .Select(a=>a.FormId).ToList();

            return FormsId;
        }


        public static List<int> GetUserDepartmentFormsAndParentDepartmentForms(string UserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<int> FormsId = new List<int>();
            //Department Of user:
            int DepartmentId = db.Users.Find(UserId).DepartmentId.Value;

            //Department and parent
            List<int> Departments = DepartmentsID.GetDepartmentIdAndParentID(DepartmentId);

            FormsId = db.FormDepartments.Where(a => Departments.Contains(a.DepartmentId) && a.Is_Active == true)
                .Select(a => a.FormId).Distinct().ToList();

            return FormsId;
        }

        public static List<int> GetUserDepartmentFormsAndAllParentDepartmentForms(string UserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<int> FormsId = new List<int>();
            //Department Of user:
            int DepartmentId = db.Users.Find(UserId).DepartmentId.Value;

            //Department and parent
            List<int> Departments = DepartmentsID.GetDepartmentIdAndAllParentsID(DepartmentId);

            FormsId = db.FormDepartments.Where(a => Departments.Contains(a.DepartmentId) && a.Is_Active == true)
                .Select(a => a.FormId).Distinct().ToList();

            return FormsId;
        }


        public static List<int> GetUserDepartmentFormsAndFirstChildrenLevel(string UserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<int> FormsId = new List<int>();
            //Department Of user:
            int DepartmentId = db.Users.Find(UserId).DepartmentId.Value;

            //Department and parent
            List<int> Departments = DepartmentsID.GetDepartmentIdAndFirstLevelChild(DepartmentId);

            FormsId = db.FormDepartments.Where(a => Departments.Contains(a.DepartmentId) && a.Is_Active == true)
                .Select(a => a.FormId).Distinct().ToList();

            return FormsId;
        }


        public static List<int> GetUserDepartmentFormsAndAllchildren(string UserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<int> FormsId = new List<int>();
            //Department Of user:
            int DepartmentId = db.Users.Find(UserId).DepartmentId.Value;

            //Department and parent
            List<int> Departments = DepartmentsID.GetDepartmentIdAndAllLevelChild(DepartmentId);

            FormsId = db.FormDepartments.Where(a => Departments.Contains(a.DepartmentId) && a.Is_Active == true)
                .Select(a => a.FormId).Distinct().ToList();

            return FormsId;
        }


        public static List<int> GetUserDepartmentAndAllSiblings(string UserId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<int> FormsId = new List<int>();
            //Department Of user:
            int DepartmentId = db.Users.Find(UserId).DepartmentId.Value;

            //Department and parent
            List<int> Departments = DepartmentsID.GetDepartmentIdAndSlibingsID(DepartmentId);

            FormsId = db.FormDepartments.Where(a => Departments.Contains(a.DepartmentId) && a.Is_Active == true)
                .Select(a => a.FormId).Distinct().ToList();




            return FormsId;
        }





    }
}