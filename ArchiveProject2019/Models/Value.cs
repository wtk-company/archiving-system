using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ArchiveProject2019.Resources;

namespace ArchiveProject2019.Models
{
    public class Value
    {
        [Key]
        public int Id { get; set; }


        [Display(Name = "FieldValue", ResourceType = typeof(main_lang))]
        public string FieldValue { get; set; }


        [Display(Name = "CreatedAt", ResourceType = typeof(main_lang))]
        public string CreatedAt { get; set; }


        [Display(Name = "CreatedById", ResourceType = typeof(main_lang))]
        public string CreatedById { set; get; }
        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }

        
        // relate with Field Table
        [Display(Name = "FieldId", ResourceType = typeof(main_lang))]
        public int FieldId { get; set; }
        public Field Field { get; set; }


        // Relate with Document Table
        [Display(Name = "Document_id", ResourceType = typeof(main_lang))]
        public int Document_id { get; set; }
        [ForeignKey("Document_id")]
        public Document Docuemnt { get; set; }
    }
}