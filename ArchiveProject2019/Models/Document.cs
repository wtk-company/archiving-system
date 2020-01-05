using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ArchiveProject2019.Resources;

namespace ArchiveProject2019.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "DocName", ResourceType = typeof(main_lang))]
        public string Name { get; set; }


        [Display(Name = "FileUrl", ResourceType = typeof(main_lang))]
        public string FileUrl { get; set; }


        [Display(Name = "Subject", ResourceType = typeof(main_lang))]
        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "SubjectRequired")]
        public string Subject { get; set; }



        [Display(Name = "KindId", ResourceType = typeof(main_lang))]
        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "KindRequired")]
        public int KindId { set; get; }
        [ForeignKey("KindId")]
        public Kind Kind { set; get; }


        [Display(Name = "StatusId", ResourceType = typeof(main_lang))]
        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "StatusRequired")]
        public int StatusId { set; get; }
        [ForeignKey("StatusId")]
        public DocumentStatus DocStatus { set; get; }


        [Display(Name = "MailingNumber", ResourceType = typeof(main_lang))]
        public string MailingNumber { get; set; }


        [Display(Name = "FamelyState", ResourceType = typeof(main_lang))]
        public int? FamelyState { get; set; }


        [Display(Name = "MailingDate", ResourceType = typeof(main_lang))]
        public string MailingDate { get; set; }


        [Display(Name = "Description", ResourceType = typeof(main_lang))]
        public string Description { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "DocumentDate", ResourceType = typeof(main_lang))]
        public string DocumentDate { set; get; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "CreatedAt", ResourceType = typeof(main_lang))]
        public string  CreatedAt { get; set; }




        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "UpdatedAt", ResourceType = typeof(main_lang))]
        public string UpdatedAt { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "NotificationDate", ResourceType = typeof(main_lang))]
        public string NotificationDate { get; set; }



        [Display(Name = "DocumentNumber", ResourceType = typeof(main_lang))]
        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "DocumentNumberRequired")]
        public string DocumentNumber { set; get; }



        [Display(Name = "Address", ResourceType = typeof(main_lang))]
        public string Address { get; set; }


        [Display(Name = "Address", ResourceType = typeof(main_lang))]
        public string Notes { set; get; }


        // Relate with User Table For Create By.
        [Display(Name = "CreatedById", ResourceType = typeof(main_lang))]
        public string CreatedById { set; get; }
        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }


        [Display(Name = "UpdatedById", ResourceType = typeof(main_lang))]
        public string UpdatedById { set; get; }
        [ForeignKey("UpdatedById")]
        public ApplicationUser UpdatedBy { set; get; }



        [Display(Name = "NotificationUserId", ResourceType = typeof(main_lang))]
        public string NotificationUserId { set; get; }


        [Display(Name = "ResponsibleUserId", ResourceType = typeof(main_lang))]
        public string ResponsibleUserId { set; get; }
        [ForeignKey("ResponsibleUserId")]
        public ApplicationUser ResponsibleUser { set; get; }




        // Relate with Department Table
        [Display(Name = "DepartmentId", ResourceType = typeof(main_lang))]
        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "DepartmentRequired")]
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }


        // Relate with Form Table
        [Display(Name = "FormId", ResourceType = typeof(main_lang))]
        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "FormRequired")]
        public int FormId { get; set; }
        [ForeignKey("FormId")]
        public Form Form { get; set; }


        [Display(Name = "TypeMailId", ResourceType = typeof(main_lang))]
        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "TypeMailRequired")]
        public int TypeMailId { set; get; }
        [ForeignKey("TypeMailId")]
        public TypeMail TypeMail { set; get; }


        // Relate with Party Table For Create By.
        [Display(Name = "PartyId", ResourceType = typeof(main_lang))]
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
        public ICollection<SealDocument> SealDocuments { set; get; }
    }
}