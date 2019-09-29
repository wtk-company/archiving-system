using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.HelperClasses
{
    public class TypeOfMailStartup
    {
        public static List<string> GetTypes()
        {

            return new List<string>() {

                "وارد","صادر","أرشيف","داخلي"
            };
            
            
            
           

        }
    }
}