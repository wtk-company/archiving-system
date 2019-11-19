﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "اسم الملف")]

        public string Name { get; set; }
        

        [Display(Name = " الملف")]
        public string FileUrl { get; set; }


        [Display(Name = "الموضوع")]
        [Required(ErrorMessage = "يجب إختيار الموضوع")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "طول الموضوع يجب أن يكون بين 5 و 50 محرف")]
        public string Subject { get; set; }


        [Display(Name = "النوع")]
        [Required(ErrorMessage ="يجب إختيار النوع")]
        public int KindId { set; get; }
        [ForeignKey("KindId")]
        public Kind Kind { set; get; }


        [Display(Name = "حالة الوثيقة")]
        [Required(ErrorMessage = "يجب إختيار الحالة")]
        public int StatusId { set; get; }
        [ForeignKey("StatusId")]
        public DocumentStatus Status { set; get; }


        [Display(Name = "رقم البريد")]
        public string MailingNumber { get; set; }

        [Display(Name = "تاريخ البريد")]
        public string MailingDate { get; set; }


        [Display(Name = "الوصف")]
        [StringLength(1000, MinimumLength = 2, ErrorMessage = "طول الموضوع يجب أن يكون أكبر من 2")]
        public string Description { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "تاريخ إصدار الوثيقه")]
        public string DocumentDate { set; get; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "تاريخ الإنشاء")]
        public string  CreatedAt { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "تاريخ التنبيه")]
        public string NotificationDate { get; set; }



        [Display(Name = "رقم الوثيقة")]
        public string DocumentNumber { set; get; }


        [Display(Name = "العنوان")]
        public string Address { get; set; }


        [Display(Name="ملاحظات")]
        public string Notes { set; get; }

        // Relate with User Table For Create By.
        [Display(Name = " أنشأ بواسطة ")]
        public string CreatedById { set; get; }
        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }
        

        // Relate with Department Table
        [Display(Name ="القسم")]
        [Required(ErrorMessage ="يجب إختيار القسم")]
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }


        // Relate with Form Table
        [Display(Name = "اسم النموذج")]
        [Required(ErrorMessage ="يجب إختيار النموذج")]
        public int FormId { get; set; }
        [ForeignKey("FormId")]
        public Form Form { get; set; }

        [Display(Name = "نوع البريد")]
        [Required(ErrorMessage = "يجب إختيار نوع البريد")]
        public int TypeMailId { set; get; }
        [ForeignKey("TypeMailId")]
        public TypeMail TypeMail { set; get; }


        // Relate with Party Table For Create By.
       
        [Display(Name = "الجهة")]
        public int? PartyId { set; get; }
        [ForeignKey("PartyId")]
        public Party Party { set; get; }


        //Collections:
        public ICollection<DocumentDepartment> DocumentDepartments { set; get; }
        public ICollection<DocumentGroup> DocumentGroups { set; get; }
        public ICollection<DocumentUser> DocumentUsers { set; get; }



        public ICollection<Value> Values { set; get; }
        public ICollection<RelatedDocument> RelatedDocuments { set; get; }
        public ICollection<DocumentParty> DocumentParties { set; get; }
        public ICollection<FilesStoredInDb> FilesStoredInDbs { set; get; }
        public ICollection<ReplayDocument> ReplayDocuments { set; get; }
    }
}