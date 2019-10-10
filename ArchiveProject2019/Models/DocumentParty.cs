using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class DocumentParty
    {
        [Key]
        public int Id { get; set; }


        // Relate with Document Table
        [Display(Name = "اسم الوثيقة")]
        [Required(ErrorMessage = "يجب إختيار الوثيقة")]
        public int DocumentId { get; set; }
        [ForeignKey("DocumentId")]
        public Document Document { get; set; }


        // Relate with Party Table
        [Required(ErrorMessage = "يجب إختيار الجهة")]
        [Display(Name = "اسم الجهة")]
        public int PartyId { get; set; }
        [ForeignKey("PartyId")]
        public Party Party { get; set; }


        [Display(Name = "تاريخ الإنشاء")]
        public string CreatedAt { get; set; }


        [Display(Name = " أنشأ بواسطة ")]
        public string CreatedById { set; get; }
        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }
    }
}