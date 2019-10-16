using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.ViewModel
{
    public class GroupsUserInformation
    {

        [Display(Name="اسم المجموعة")]
        public string Name { set; get; }
        [Display(Name = "عدد الأعضاء")]

        public int UsersCount { set; get; }

       
    }
}