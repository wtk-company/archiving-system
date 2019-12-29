using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class JobTitle
    {

        [Key]
        public int Id { get; set; }

        [Display(Name = "الاسم الوظيفي ")]
        [Required(ErrorMessage = "يجب إدخال اسم القسم")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "يجب أن يكون طول الاسم أكبر من 2")]
        public string Name { get; set; }



        [Display(Name = "الرمز الوظيفي  ")]
      
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "يجب أن يكون طول الاسم أكبر  من 2")]
        public string Symbol { get; set; }

        [Display(Name = "العدد الأعظمي للأعضاء")]

        [Required(ErrorMessage ="يجب إدخال العدد الأعظمي")]
        [Range(1,100,ErrorMessage ="يجب أن يكون العدد منطقي")]
        public int MaximumMember { set; get; }


        [Display(Name="نمط إظهار النماذج")]
        [Required(ErrorMessage ="يجب إختيار نمط إظهار النماذج")]
        public int TypeOfDisplayForm { set; get; }








        [Display(Name = "نمط إظهار الوثائق")]
        [Required(ErrorMessage = "يجب إختيار نمط إظهار الوثائق")]
        public int TypeOfDisplayDocument{ set; get; }





        [Display(Name = "تاريخ الإنشاء")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string CreatedAt { get; set; }
        [Display(Name = "تم الإنشاء بواسطة ")]
        public string CreatedById { set; get; }

        [ForeignKey("CreatedById")]
        public ApplicationUser CreatedBy { set; get; }






        //Update Informations:

        [Display(Name = "تاريخ آخر تعديل ")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy-HH:mm:ss}")]
        public string UpdatedAt { get; set; }



        [Display(Name = "آخر تعديل  بواسطة")]
        public string UpdatedById { set; get; }

        [ForeignKey("UpdatedById")]
        public ApplicationUser UpdatedBy { set; get; }
        public  ICollection<ApplicationUser> Users{ set; get; }





    }
}