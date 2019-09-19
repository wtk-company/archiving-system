using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class Field
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage ="يجب إدخال اسم الحقل")]
        [Display(Name = "اسم الحقل")]
        public string Name { get; set; }


        [Display(Name = "ضروري؟ ")]
        public bool IsRequired { set; get; }


        [Required(ErrorMessage = "يجب إدخال انمط")]
        [Display(Name = "النمط")]
        public string Type { get; set; }


        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        [Display(Name = "تاريخ الإنشاء")]
        public string CreatedAt { get; set; }


        // Relate with User Table For Create By.
        [Display(Name = " أنشأ بواسطة ")]
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


        // Relate with Form Table
        [Display(Name = " النموذج")]
        public int FormId { get; set; }
        [ForeignKey("FormId")]
        public Form Form { get; set; }


        //Collections:
        public ICollection<Value> Values { set; get; }
    }
}