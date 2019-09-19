using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class DepertmentsListCheckbox
    {

        
        public int Id{set;get ;}
        public string Name { set; get; }

        public bool Selected { set; get; }


        public static List<DepertmentsListCheckbox>CreateDepartmentCheckBoxList()
        {

            ApplicationDbContext db = new ApplicationDbContext();
            List<DepertmentsListCheckbox> DepartmentCheckBoxList = new List<DepertmentsListCheckbox>();
            IEnumerable<Department> departments = db.Departments.ToList();

            DepertmentsListCheckbox DepartmentCheckBox;
            foreach (Department dep in departments)
            {
                if (dep.ParentId == null)
                {

                    DepartmentCheckBox = new DepertmentsListCheckbox
                    {
                        Id = dep.Id,
                        Name = dep.Name

                    };
                }

                else
                {

                    Department DepartmentFirst = new Department();
                    Department DepartmentScond = new Department();

                    Department DepartmentThird = new Department();

                    Department DepartmentFourth = new Department();




                    string DepartmentNam = "";
                    //First Parent:
                    DepartmentFirst = db.Departments.Find(dep.ParentId.Value);
                    DepartmentNam = DepartmentFirst.Name;
                    //Second Parent:
                    if (DepartmentFirst.ParentId != null)
                    {

                        DepartmentScond = db.Departments.Find(DepartmentFirst.ParentId.Value);
                        DepartmentNam = DepartmentNam + "/" + DepartmentScond.Name;


                    }


                    //Third Parent:
                    if (DepartmentScond.ParentId != null)
                    {

                        DepartmentThird = db.Departments.Find(DepartmentScond.ParentId.Value);
                        DepartmentNam = DepartmentNam + "/" + DepartmentThird.Name;


                    }


                    //Fourth Parent
                    if (DepartmentThird.ParentId != null)
                    {

                        DepartmentFourth = db.Departments.Find(DepartmentThird.ParentId.Value);
                        DepartmentNam = DepartmentNam + "/" + DepartmentFourth.Name;


                    }

                    DepartmentNam = DepartmentNam + "/" + dep.Name;

                    DepartmentCheckBox = new DepertmentsListCheckbox
                    {
                        Id = dep.Id,
                        Name = DepartmentNam

                    };
                }


                DepartmentCheckBoxList.Add(DepartmentCheckBox);

            }

            return DepartmentCheckBoxList;

        }










        //public static List<DepertmentsListCheckbox> CreateDepartmentCheckBoxList(string UserId)
        //{

         
        //    ApplicationDbContext db = new ApplicationDbContext();
        //    IEnumerable<int> DepartmenUser = db.UserDepartments.Where(a => a.UserId.Equals(UserId)).Select(a=>a.DepartmentId);
        // List <DepertmentsListCheckbox> DepartmentCheckBoxList = new List<DepertmentsListCheckbox>();
        //    IEnumerable<Department> departments = db.Departments.ToList();

        //    DepertmentsListCheckbox DepartmentCheckBox;
        //    foreach (Department dep in departments)
        //    {
        //        if (dep.ParentId == null)
        //        {


        //            DepartmentCheckBox = new DepertmentsListCheckbox
        //            {
        //                Id = dep.Id,
        //                Name = dep.Name,
        //                Selected = DepartmenUser.Any(a => a == dep.Id) ? true : false
                        

        //            };
        //        }

        //        else
        //        {

        //            Department DepartmentFirst = new Department();
        //            Department DepartmentScond = new Department();

        //            Department DepartmentThird = new Department();

        //            Department DepartmentFourth = new Department();




        //            string DepartmentNam = "";
        //            //First Parent:
        //            DepartmentFirst = db.Departments.Find(dep.ParentId.Value);
        //            DepartmentNam = DepartmentFirst.Name;
        //            //Second Parent:
        //            if (DepartmentFirst.ParentId != null)
        //            {

        //                DepartmentScond = db.Departments.Find(DepartmentFirst.ParentId.Value);
        //                DepartmentNam = DepartmentNam + "/" + DepartmentScond.Name;


        //            }


        //            //Third Parent:
        //            if (DepartmentScond.ParentId != null)
        //            {

        //                DepartmentThird = db.Departments.Find(DepartmentScond.ParentId.Value);
        //                DepartmentNam = DepartmentNam + "/" + DepartmentThird.Name;


        //            }


        //            //Fourth Parent
        //            if (DepartmentThird.ParentId != null)
        //            {

        //                DepartmentFourth = db.Departments.Find(DepartmentThird.ParentId.Value);
        //                DepartmentNam = DepartmentNam + "/" + DepartmentFourth.Name;


        //            }

        //            DepartmentNam = DepartmentNam + "/" + dep.Name;

        //            DepartmentCheckBox = new DepertmentsListCheckbox
        //            {
        //                Id = dep.Id,
        //                Name = DepartmentNam,
        //                Selected = DepartmenUser.Any(a => a == dep.Id) ? true : false


        //            };
        //        }


        //        DepartmentCheckBoxList.Add(DepartmentCheckBox);

        //    }

        //    return DepartmentCheckBoxList;

        //}
    }
}