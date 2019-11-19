using ArchiveProject2019.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.HelperClasses
{
    public class DocumentStatusStartUp
    {

        public static List<DocumentStatus> DocumentStatusList()
        {


            return new List<DocumentStatus>() {


                new DocumentStatus(){ Type=1,Name="معطلة",CreatedAt=DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss")},
                new DocumentStatus(){ Type=1,Name="قيد المعالجة",CreatedAt=DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss")},
                new DocumentStatus(){ Type=1,Name="تمت المعالجة",CreatedAt=DateTime.Now.ToString("dd/MM/yyyy-HH:mm:ss")}

            };
        }
    }
}