using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ArchiveProject2019.Models;
using System.ComponentModel.DataAnnotations;

namespace ArchiveProject2019.HelperClasses
{
    public class DepartmentListDisplay
    {

        public int Id { set; get; }
        [Display(Name ="اسم القسم")]
        public string Name { set; get; }

     


        public static List<DepartmentListDisplay> CreateDepartmentListDisplay()
        {

            ApplicationDbContext db = new ApplicationDbContext();
            List<DepartmentListDisplay> DepartmentCheckBoxList = new List<DepartmentListDisplay>();
            IEnumerable<Department> departments = db.Departments.ToList();

            DepartmentListDisplay DepartmentCheckBox;
            foreach (Department dep in departments)
            {
                if (dep.ParentId == null)
                {

                    DepartmentCheckBox = new DepartmentListDisplay
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
                        DepartmentNam = DepartmentNam + " >>" + DepartmentScond.Name;


                    }


                    //Third Parent:
                    if (DepartmentScond.ParentId != null)
                    {

                        DepartmentThird = db.Departments.Find(DepartmentScond.ParentId.Value);
                        DepartmentNam = DepartmentNam + " >> " + DepartmentThird.Name;


                    }


                    //Fourth Parent
                    if (DepartmentThird.ParentId != null)
                    {

                        DepartmentFourth = db.Departments.Find(DepartmentThird.ParentId.Value);
                        DepartmentNam = DepartmentNam + " >> " + DepartmentFourth.Name;


                    }

                    DepartmentNam = DepartmentNam + " >> " + dep.Name;

                    DepartmentCheckBox = new DepartmentListDisplay
                    {
                        Id = dep.Id,
                        Name = DepartmentNam

                    };
                }


                DepartmentCheckBoxList.Add(DepartmentCheckBox);

            }

            return DepartmentCheckBoxList.OrderBy(a=>a.Name).ToList();

        }






    }
}