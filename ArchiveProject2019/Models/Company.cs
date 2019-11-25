using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "اسم الشركة")]
        public string Name { get; set; }

        [Display(Name = "نبذة عن الشركة")]
        public string Description { get; set; }

        [Display(Name = "عنوان الشركة")]

        public string Address { get; set; }


        [Display(Name = "رقم الأرضي الأول")]
        public int PhoneNumber1 { get; set; }

        [Display(Name = "رقم الأرضي الثاني")]
        public int PhoneNumber2 { get; set; }


        [Display(Name = "رقم الموبايل الأول")]
        public int MobileNumber1 { get; set; }

        [Display(Name = "رقم الموبايل الثاني")]

        public int MobileNumber2 { get; set; }


        [Display(Name = "البريد الالكتروني")]
        public string Mail { get; set; }

        [Display(Name = "الشعار")]
        public byte[] Logo { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "تاريخ انشاء الشركة")]
        public string CompanyDate { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "تاريخ تحديث المعلومات")]
        public string CreatedAt { get; set; }


        // Relate with User Table For Create By.
        [Display(Name = " أنشأ بواسطة ")]
        public string CreateById { get; set; }
        [ForeignKey("CreateById")]
        public ApplicationUser User { get; set; }
    }
}