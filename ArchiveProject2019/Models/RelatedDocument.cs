using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class RelatedDocument
    {
        [Key]
        public int Id { get; set; }

        public int RelatedDocId { get; set; }

     

        [Display(Name = " أنشأ بواسطة ")]
        public string CreatedById { set; get; }
        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }

        // Relate with Document table
        public int Document_id { get; set; }
        [ForeignKey("Document_id")]
        public Document Document { get; set; }
    }
}