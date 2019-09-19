using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class Form
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="يجب إدخال اسم الفورم")]
        [Display(Name = "اسم فورم الوثيقة")]
        [StringLength(maximumLength: 50, MinimumLength = 4, ErrorMessage = "يجب أن يكون طول الاسم أكبر من 3")]

        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "تاريخ الإنشاء")]
        public string CreatedAt { get; set; }

        [Display(Name = " اسم الشخص المنشىء ")]

        public string CreatedById { set; get; }
        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }





        //Update Informations:

        [Display(Name = "تاريخ آخر تعديل ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string UpdatedAt { get; set; }
        [Display(Name = "آخر تعديل  بواسطة ")]

        public string UpdatedById { set; get; }

        [ForeignKey("UpdatedById")]
        public ApplicationUser UpdatedBy { set; get; }


        // Collections
        public ICollection<Field> Fields { set; get; }
        public ICollection<Document> Documents { set; get; }

    }
}