using ArchiveProject2019.Models;
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


        public bool IsMasterRole { set; get; }
        [Display(Name = "عدد   وثائقي ")]

        public int MyTotalDocument { set; get; }

        [Display(Name = "عدد مجموعاتي ")]

        public int MyGroupsCount { set; get; }

        [Display(Name = "آخر تاريخ إضافة وثيقة")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public string LastMyDocumentCreate { set; get; }

        [Display(Name = " عدد الأقسام الكلية")]
   
        public int DepartmentsCount { set; get; }




        [Display(Name = "عدد  أنواع الوثائق ")]

        public int TotalDocumentKindCount { set; get; }
        [Display(Name = "آخر تاريخ إضافة ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string LastDocumentKindCreate { set; get; }

        [Display(Name = "آخر تاريخ تحديث ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string LastDocumentKindUpdate { set; get; }

        [Display(Name = "أكثر نوع مستخدم")]

        public string HightDocumentKindUsing { set; get; }










        [Display(Name = "عدد الأقسام الرئيسية")]
        public int MainDepartmentCount { set; get; }
        [Display(Name = "عدد الأقسام الكلية")]
        public int AllDepartmentsCount { set; get; }
        [Display(Name = "آخر تاريخ إضافة ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string LastDateDepartmentCreate { set; get; }


        [Display(Name = "آخر تاريخ تحديث ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string LastDateDepartmentUpdate { set; get; }



        [Display(Name = "عدد  المجموعات الكلية")]
  
        public int AllGroupsCount { set; get; }
        [Display(Name = "آخر تاريخ إضافة ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string LastGroupCreate { set; get; }



        [Display(Name = "آخر تاريخ تحديث ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string LastGroupUpdate { set; get; }
        [Display(Name = "عدد   المستخدمين في المجموعات")]

        public int TotalUserInGroup { set; get; }

        [Display(Name = "أكثر مجموعة للمستخدمين ")]

        public string HightGroupUsers { set; get; }






        [Display(Name = "عدد  المستخدمين الكلي ")]

        public int TotalUserCount { set; get; }
        [Display(Name = "آخر تاريخ إضافة ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string LastUserCreate { set; get; }

        [Display(Name = "آخر تاريخ تحديث ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string LastUserUpdate { set; get; }





        [Display(Name = "عدد  النماذج الكلية ")]

        public int TotalFormsCount { set; get; }
        [Display(Name = "آخر تاريخ إضافة ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string LastFormCreate { set; get; }

        [Display(Name = "آخر تاريخ تحديث ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string LastFormUpdate { set; get; }

        [Display(Name = "أكثر نموذج مستخدم")]
       
        public string HightFormUsing { set; get; }





        [Display(Name = "عدد  الوثائق الكلية ")]

        public int TotalDocumentCount { set; get; }
        [Display(Name = "آخر تاريخ إضافة ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string LastDocumentCreate { set; get; }



        [Display(Name = "عدد   أنواع البريد ")]

        public int TotalMailsCount { set; get; }
        [Display(Name = "آخر تاريخ إضافة ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string LastMailsCreate { set; get; }


        [Display(Name = "آخر تاريخ تحديث ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string LastMailsUpdate { set; get; }

        [Display(Name = "أكثر نوع بريد مستخدم")]

        public string HightMailUsing { set; get; }



        //Personal Informations:
        [Display(Name = "الإسم الثلاثي")]
        public string FullName { set; get; }

        [Display(Name = "اسم المستخدم")]
        public string UserName { set; get; }

        [Display(Name = "البريد الإلكتروني")]
        public string Email { set; get; }


        [Display(Name = "الجنس")]
        public string Gender { set; get; }

        [Display(Name = "آخر تاريخ تحديث ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]

        public string UserUpdate { set; get; }


        [Display(Name = "الدور")]
        public string RoleName { set; get; }


        [Display(Name = "تاريخ الإنشاء")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]

        public string UserCreateAt { set; get; }


        [Display(Name = " القسم الخاص")]
        public string DepartmentName { set; get; }

        [Display(Name = "المسمى الوظيفي")]
        public string JobTitle { set; get; }





        //Groups:
         public List<GroupsUserInformation> UserGroups { set; get; }
        public Company Company { set; get; }

        //Permissions:

        public List<Form> FavoriteForm { set; get; }












    }
}