using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ArchiveProject2019.Models;
namespace ArchiveProject2019.HelperClasses
{
    public class DepartmentsID
    {

        public static List<int> GetDepartmentIdAndParentID(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Department UserDepartment = db.Departments.Find(id);
            List<int> DepsId = new List<int>();
            DepsId.Add(id);

            if (UserDepartment.ParentId == null)
            {
                return DepsId;
            }

            //First Parent:
            Department d1 = db.Departments.Find(UserDepartment.ParentId);
           
            DepsId.Add(d1.Id);
            
                return DepsId;


           
         


        }
        public static List<int> GetDepartmentIdAndAllParentsID(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();

            Department UserDepartment = db.Departments.Find(id);
            List<int> DepsId = new List<int>();
            DepsId.Add(id);

            if(UserDepartment.ParentId==null)
            {
                return DepsId;
            }
            //First Parent:
            Department d1 = db.Departments.Find(UserDepartment.ParentId);
            DepsId.Add(d1.Id);


            if (d1.ParentId==null)
            {
                return DepsId;


            }
            //Second Parent:

            Department d2 = db.Departments.Find(d1.ParentId);
            DepsId.Add(d2.Id);

            if(d2.ParentId==null)
            {
                return DepsId;
            }

            //third Parent:

            Department d3 = db.Departments.Find(d2.ParentId);
            DepsId.Add(d3.Id);

            if (d3.ParentId == null)
            {
                return DepsId;
            }



            //Fourth Parent:

            Department d4 = db.Departments.Find(d3.ParentId);
            DepsId.Add(d4.Id);

            if (d4.ParentId == null)
            {
                return DepsId;
            }

            //fifth Parent:

            Department d5 = db.Departments.Find(d4.ParentId);
            DepsId.Add(d5.Id);

            return DepsId;


        }

        public static List<int> GetDepartmentIdAndSlibingsID(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<int> DepsId = new List<int>();
            DepsId.Add(id);

            Department UserDepartment = db.Departments.Find(id);
            if(UserDepartment.ParentId==null)
            {
                return DepsId;
            }
            //First Parent:
            Department parent = db.Departments.Find(UserDepartment.ParentId);

           


            List<Department> Departments = db.Departments.Where(a => a.ParentId == parent.Id).ToList();
            foreach(Department slib in Departments)
            {
                DepsId.Add(slib.Id);

            }

            return DepsId;






        }

        public static List<int> GetDepartmentIdAndFirstLevelChild(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            Department UserDepartment = db.Departments.Find(id);
            List<int> DepsId = new List<int>();
            DepsId.Add(id);

           
            List<Department> Departments = db.Departments.Where(a=>a.ParentId==id).ToList();

            foreach(Department child in Departments)
            {
                DepsId.Add(child.Id);
            }

            return DepsId;






        }


        public static List<int> GetDepartmentIdAndAllLevelChild(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<int> DepsId = new List<int>();
            DepsId.Add(id);

            List<Department> Departments1 = db.Departments.Where(a => a.ParentId == id).ToList();
            List<Department> departments2 = new List<Department>();

            if(Departments1.Count()<=0)
            {
                return DepsId;
            }
            //First Level:
            foreach (Department child in Departments1)
            {
                DepsId.Add(child.Id);


                List<Department> childDepartments = db.Departments.Where(a => a.ParentId == child.Id).ToList();
                if(childDepartments.Count()>0)
                {
                departments2.AddRange(child.ChildDepartment);

                }
            }

            //Second Level:
            if(departments2.Count()<=0)
            {
                return DepsId;
            }

            List<Department> departments3= new List<Department>();

            foreach (Department child in departments2)
            {
                DepsId.Add(child.Id);
                List<Department> childDepartments = db.Departments.Where(a => a.ParentId == child.Id).ToList();
                if (childDepartments.Count() > 0)
                {
                    departments3.AddRange(child.ChildDepartment);
                }


            }



            //Thirs Level:
            if (departments3.Count() <= 0)
            {
                return DepsId;
            }

            List<Department> departments4 = new List<Department>();

            foreach (Department child in departments3)
            {
                DepsId.Add(child.Id);
                List<Department> childDepartments = db.Departments.Where(a => a.ParentId == child.Id).ToList();
                if (childDepartments.Count() > 0)
                {
                    departments4.AddRange(child.ChildDepartment);
                }


            }



            //Fourth Level:
            if (departments4.Count() <= 0)
            {
                return DepsId;
            }

            List<Department> departments5 = new List<Department>();

            foreach (Department child in departments4)
            {
                DepsId.Add(child.Id);
                List<Department> childDepartments = db.Departments.Where(a => a.ParentId == child.Id).ToList();
                if (childDepartments.Count() > 0)
                {
                    departments5.AddRange(child.ChildDepartment);
                }


            }



            //Fifth Level:
            if (departments5.Count() <= 0)
            {
                return DepsId;
            }

            List<Department> departments6 = new List<Department>();

            foreach (Department child in departments5)
            {
                DepsId.Add(child.Id);
                List<Department> childDepartments = db.Departments.Where(a => a.ParentId == child.Id).ToList();
                if (childDepartments.Count() > 0)
                {
                    departments6.AddRange(child.ChildDepartment);
                }


            }




            if (departments6.Count() <= 0)
            {
                return DepsId;
            }

         

            foreach (Department child in departments6)
            {
                DepsId.Add(child.Id);
               
            }


            return DepsId.Distinct().ToList();






        }






    }
}