using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.HelperClasses
{
    public static class StringDateHelper
    {

        public static DateTime ChangeFormat(this string s)
        {
            return DateTime.ParseExact(s, "yyyy/MM/dd", null);
        }

    }
}