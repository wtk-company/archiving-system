using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class PermissionsControls
    {
        public string Name { set; get; }
        public string ActionName { set; get; }
        public static List<PermissionsControls> PermissionsStartUp() {

            //List Of Permission
            return new List<PermissionsControls>() {

                new PermissionsControls() {Name="اضافة قسم",ActionName="AddDepartment" },
                new PermissionsControls() {Name="حذف قسم",ActionName="DeleteDepartment" },
                new PermissionsControls() {Name="تعديل قسم",ActionName="EditDepartment" },
                new PermissionsControls() {Name="اضافة نموذج",ActionName="AddForm" },
                new PermissionsControls() {Name="حذف نموذج",ActionName="DeleteForm" },
                new PermissionsControls() {Name="تعديل نموذج",ActionName="EditForm" }
                //Others Permission:


            };
        }
    }
}