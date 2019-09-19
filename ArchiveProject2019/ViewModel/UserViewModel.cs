using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.ViewModel
{
    public class UserViewModel
    {

        [Display(Name = "الاسم الثلاثي")]


        public string FullName { set; get; }
        [Display(Name = " الجنس")]

        public string Gender { set; get; }



        [Display(Name = "القسم")]
        public string DepartmentName { set; get; }




        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "تاريخ الإنشاء")]
        public string CreatedAt { get; set; }

        //Users Control:
        [Display(Name = " اسم الشخص المنشىء ")]
        public string CreatedBy { set; get; }



  
        [Display(Name = " اسم الدور ")]

        public string RoleName { set; get; }





        [Display(Name = "الدور الوظيفي")]
        public string JobTitle{ set; get; }





        [Display(Name = "تاريخ آخر تعديل ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string UpdatedAt { get; set; }


        [Display(Name = "آخر تعديل  بواسطة ")]
        
        public string UpdatedBy { set; get; }



        [Display(Name = "اسم المستخدم")]

        public string UserName { set; get; }


        [Display(Name = " البريد الإلكتروني")]

        public string Email { set; get; }


    }
}