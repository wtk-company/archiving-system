using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.ViewModel
{
    public class GroupUserViewModel
    {
        public string UserId { get; set; }

        [Display(Name = "اسم المستخدم")]
        public string UserName { get; set; }

        [Display(Name = "الاسم الثلاثي")]
        public string FullName { get; set; }
        public int GroupId { get; set; }
      [Display(Name ="عضو في المجموعة")]

        public bool IsGroupMember { get; set; }
    }
}