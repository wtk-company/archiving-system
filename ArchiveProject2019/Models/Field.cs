using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ArchiveProject2019.Resources;

namespace ArchiveProject2019.Models
{
    public class Field
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "FieldNameRequired")]
        [Display(Name = "FieldName", ResourceType = typeof(main_lang))]
        public string Name { get; set; }


        [Display(Name = "IsRequired", ResourceType = typeof(main_lang))]
        public bool IsRequired { set; get; }


        [Required(ErrorMessageResourceType = typeof(main_lang), ErrorMessageResourceName = "FieldNameRequired")]
        [Display(Name = "Type", ResourceType = typeof(main_lang))]
        public string Type { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "CreatedAt", ResourceType = typeof(main_lang))]
        public string CreatedAt { get; set; }


        // Relate with User Table For Create By.
        [Display(Name = "CreatedById", ResourceType = typeof(main_lang))]
        public string CreatedById { set; get; }
        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }


        //Update Informations:
        [Display(Name = "UpdatedAt", ResourceType = typeof(main_lang))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string UpdatedAt { get; set; }


        [Display(Name = "UpdatedById", ResourceType = typeof(main_lang))]
        public string UpdatedById { set; get; }
        [ForeignKey("UpdatedById")]
        public ApplicationUser UpdatedBy { set; get; }


        // Relate with Form Table
        [Display(Name = "FormId", ResourceType = typeof(main_lang))]
        public int FormId { get; set; }
        [ForeignKey("FormId")]
        public Form Form { get; set; }


        //Collections:
        public ICollection<Value> Values { set; get; }
    }
}