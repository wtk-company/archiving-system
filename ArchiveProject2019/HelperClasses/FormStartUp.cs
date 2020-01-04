using ArchiveProject2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.HelperClasses
{
    public class FormStartUp
    {

        public static Form  CreateFormStartUp()
        {

            return new Form() {

                Name="النموذج التقليدي",
                CreatedAt = DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss"),
                Type=1


            };
        }
    }
}