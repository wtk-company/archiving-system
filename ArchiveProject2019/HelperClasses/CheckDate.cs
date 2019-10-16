using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.HelperClasses
{
    public class CheckDate
    {
        // Convert string date dd-mm-yyyy To date yyyy/mm/dd
        public static DateTime StringToDateMin(string StringDate)
        {
            if (StringDate != "")
            {
                string s = StringDate.Replace("-", "/");
                return DateTime.ParseExact(s, "yyyy/MM/dd", null);
            }
            else
            {
                string s = DateTime.MinValue.ToString("yyyy/MM/dd");
                return DateTime.ParseExact(s, "yyyy/MM/dd", null);
            }
        }

        public static DateTime StringToDateMax(string StringDate)
        {
            if (StringDate != "")
            {
                string s = StringDate.Replace("-", "/");
                return DateTime.ParseExact(s, "yyyy/MM/dd", null);
            }
            else
            {
                string s = DateTime.MaxValue.ToString("yyyy/MM/dd");
                return DateTime.ParseExact(s, "yyyy/MM/dd", null);
            }
        }

    }
}