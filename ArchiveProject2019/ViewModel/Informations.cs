using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.ViewModel
{
    public class Informations
    {
        [Display(Name = "عدد الأقسام الرئيسية")]
        public int MainDepartmentCount { set; get; }
        [Display(Name = "عدد الأقسام الكلية")]
        public int AllDepartmentsCount { set; get; }
        [Display(Name = "آخر تاريخ إضافة ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string LastDateDepartmentCreate { set; get; }
    }
}