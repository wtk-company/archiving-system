using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class FavouriteForms
    {
        [Key]
        public int Id { get; set; }

        
        public string UserId { set; get; }
        [ForeignKey("UserId")]
        public ApplicationUser User { set; get; }



        public int FormId { set; get; }
        [ForeignKey("FormId")]
        public virtual Form Form { set; get; }
    }
}