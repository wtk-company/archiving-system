using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.HelperClasses
{
    public class CheckDate
    {
        // Convert string date dd-mm-yyyy To date yyyy/mm/dd
        public static DateTime StringToDate(string StringDate)
        {
            StringDate = StringDate.Replace("-", "/");
            DateTime date = DateTime.ParseExact(StringDate, "yyyy/MM/dd", null);
            return date;
        }
    }
}