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

     





        public static string CreateDepartmentDisplay(int id)
        {
            string DepartmentName = "";

            List<string> DepartmentsName = new List<string>();
            ApplicationDbContext db = new ApplicationDbContext();
          
            IEnumerable<Department> departments = db.Departments.ToList();
            Department dep = db.Departments.Find(id);
            
                if (dep.ParentId == null)
                {

                return dep.Name;
                }

                else
                {

                    Department DepartmentFirst = new Department();
                    Department DepartmentScond = new Department();

                    Department DepartmentThird = new Department();

                    Department DepartmentFourth = new Department();




                  
                    //First Parent:
                    DepartmentFirst = db.Departments.Find(dep.ParentId.Value);
                    DepartmentsName.Add(DepartmentFirst.Name);

              
                //Second Parent:
                if (DepartmentFirst.ParentId != null)
                    {

                        DepartmentScond = db.Departments.Find(DepartmentFirst.ParentId.Value);
                          DepartmentsName.Add(DepartmentScond.Name);


                }


                //Third Parent:
                if (DepartmentScond.ParentId != null)
                    {

                        DepartmentThird = db.Departments.Find(DepartmentScond.ParentId.Value);
                          DepartmentsName.Add(DepartmentThird.Name);



                }


                //Fourth Parent
                if (DepartmentThird.ParentId != null)
                    {

                        DepartmentFourth = db.Departments.Find(DepartmentThird.ParentId.Value);
                    DepartmentsName.Add(DepartmentFourth.Name);



                }

               

                 
                }

                DepartmentsName.Reverse();

                foreach (string s in DepartmentsName)
            {
                DepartmentName = DepartmentName  + s+ " >> ";
            }

            DepartmentName = DepartmentName + dep.Name;



            return DepartmentName;

    }






        public static List<DepartmentListDisplay> CreateDepartmentListDisplay()
        {


            List<string> DepartmentsName;
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
                    DepartmentsName=new List<string>();
                    Department DepartmentFirst = new Department();
                    Department DepartmentScond = new Department();

                    Department DepartmentThird = new Department();

                    Department DepartmentFourth = new Department();




                    string DepartmentName = "";
                    //First Parent:
                    DepartmentFirst = db.Departments.Find(dep.ParentId.Value);
                    DepartmentsName.Add(DepartmentFirst.Name);
                   // DepartmentNam = DepartmentFirst.Name;
                    //Second Parent:
                    if (DepartmentFirst.ParentId != null)
                    {

                        DepartmentScond = db.Departments.Find(DepartmentFirst.ParentId.Value);
                        DepartmentsName.Add(DepartmentScond.Name);



                    }


                    //Third Parent:
                    if (DepartmentScond.ParentId != null)
                    {

                        DepartmentThird = db.Departments.Find(DepartmentScond.ParentId.Value);
                        DepartmentsName.Add(DepartmentThird.Name);



                    }


                    //Fourth Parent
                    if (DepartmentThird.ParentId != null)
                    {

                        DepartmentFourth = db.Departments.Find(DepartmentThird.ParentId.Value);
                        DepartmentsName.Add(DepartmentFourth.Name);



                    }

                    DepartmentsName.Reverse();

                    foreach (string s in DepartmentsName)
                    {
                        DepartmentName = DepartmentName + s + " >> ";
                    }

                    DepartmentName = DepartmentName + dep.Name;

                    DepartmentCheckBox = new DepartmentListDisplay
                    {
                        Id = dep.Id,
                        Name = DepartmentName

                    };
                }
           

                DepartmentCheckBoxList.Add(DepartmentCheckBox);

            }

            return DepartmentCheckBoxList.OrderBy(a => a.Name).ToList();

        }

    }
}