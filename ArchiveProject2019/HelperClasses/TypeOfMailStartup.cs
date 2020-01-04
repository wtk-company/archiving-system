using ArchiveProject2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.HelperClasses
{
    public class TypeOfMailStartup
    {
        public static List<TypeMail> GetTypes()
        {

            return new List<TypeMail>() {

                new TypeMail(){Name="وارد",Type=1,CreatedAt=DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss")},
                new TypeMail(){Name="صادر",Type=2,CreatedAt=DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss")},
                new TypeMail(){Name="داخلي",Type=3,CreatedAt=DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss")},
                new TypeMail(){Name="أرشيف",Type=4,CreatedAt=DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss")},


            };
            
            
           

        }
    }
}