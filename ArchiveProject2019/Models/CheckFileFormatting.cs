using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArchiveProject2019.Models
{
    public class CheckFileFormatting
    {
        //All File Format:
        private static string [] FileFormat= { "jpg","jpeg","png","pdf","gif","doc","docx","xlsx","txt", "pptx" };

        //Image format:
        private static string[] ImageFormat = { "jpeg","jpg","png","gif"};
        //M.S.word
        private static string[] WordFormat = { "doc","docx"};
        //M.S.Excel
        private static string[] ExcelFormat = { "xlsx"};
        //M.S.PowerPoint 
        private static string[] PowerpointFormat = {"pptx" };
        //text format
        private static string[]TextFormat = { "txt" };
        //Pdf Format
        private static string[] PDFFormat = { "pdf" };






        //For All File:
        public static bool PermissionFile(HttpPostedFileBase FB)
        {

            bool Ok = false;
            if (FB != null)
            {
                foreach(string ends in FileFormat)
                {
                    if(FB.FileName.EndsWith(ends,StringComparison.OrdinalIgnoreCase))
                    {
                        Ok = true;
                    }

                }
            }
            else
            {
                Ok = true;
            }
            return Ok;

        }


        public static bool PermissionFile(string FB)
        {

            bool Ok = false;
            if (FB != null)
            {
                foreach (string ends in FileFormat)
                {
                    if (FB.EndsWith(ends, StringComparison.OrdinalIgnoreCase))
                    {
                        Ok = true;
                    }
                }
            }
            else
            {
                Ok = true;
            }

            return Ok;

        }

        //Check Image File:{file,string}
        public static bool IsImage(HttpPostedFileBase FB)
        {
            bool Ok = false;
            if (FB != null)
            {
                foreach(string s in ImageFormat)
                {
                    if(FB.FileName.EndsWith(s,StringComparison.OrdinalIgnoreCase))
                    {
                        Ok = true;
                        break;
                    }
                }
            }
            else
            {
                Ok = true;
            }
            return Ok;

        }

        public static bool IsImage(string FB)
        {
            bool Ok = false;
            if (FB != null)
            {
                foreach (string s in ImageFormat)
                {
                    if (FB.EndsWith(s, StringComparison.OrdinalIgnoreCase))
                    {
                        Ok = true;
                        break;
                    }
                }
            }
            else
            {
                Ok = true;
            }

            return Ok;

        }



        //Check Word File{file,string}
        public static bool IsWord(HttpPostedFileBase FB)
        {
            bool Ok = false;
            if (FB != null)
            {
                foreach (string s in WordFormat)
                {
                    if (FB.FileName.EndsWith(s, StringComparison.OrdinalIgnoreCase))
                    {
                        Ok = true;
                        break;
                    }
                }
            }
            else
            {
                Ok = true;
            }

            return Ok;

        }

        public static bool IsWord(string FB)
        {
            bool Ok = false;

            if (FB != null)
            {
                foreach (string s in WordFormat)
                {
                    if (FB.EndsWith(s, StringComparison.OrdinalIgnoreCase))
                    {
                        Ok = true;
                        break;
                    }
                }
            }
            else
            {
                Ok = true;
            }
            return Ok;
        }


        //Check Pdf File{file,string}
        public static bool IsPDF(HttpPostedFileBase FB)
        {
            bool Ok = false;

            if (FB != null)
            {
                foreach (string s in PDFFormat)
                {
                    if (FB.FileName.EndsWith(s, StringComparison.OrdinalIgnoreCase))
                    {
                        Ok = true;
                        break;
                    }
                }
            }
            else
            {
                Ok = true;
            }
            return Ok;
        }

        public static bool IsPDF(string FB)
        {
            bool Ok = false;

            if (FB != null)
            {
                foreach (string s in PDFFormat)
                {
                    if (FB.EndsWith(s, StringComparison.OrdinalIgnoreCase))
                    {
                        Ok = true;
                        break;
                    }
                }
            }
            else
            {
                Ok = true;
            }
            return Ok;

        }


        //Check Text File{file,string}
        public static bool IsText(HttpPostedFileBase FB)
        {
            bool Ok = false;

            if (FB != null)
            {
                foreach (string s in TextFormat)
                {
                    if (FB.FileName.EndsWith(s, StringComparison.OrdinalIgnoreCase))
                    {
                        Ok = true;
                        break;
                    }
                }
            }
            else
            {
                Ok = true;
            }
            return Ok;

        }

        public static bool IsText(string FB)
        {
            bool Ok = false;

            if (FB != null)
            {
                foreach (string s in TextFormat)
                {
                    if (FB.EndsWith(s, StringComparison.OrdinalIgnoreCase))
                    {
                        Ok = true;
                        break;
                    }
                }
            }
            else
            {
                Ok = true;
            }
            return Ok;

        }

        //Check Excel File{file,string}
        public static bool IsExcel(HttpPostedFileBase FB)
        {
            bool Ok = false;

            if (FB != null)
            {
                foreach (string s in ExcelFormat)
                {
                    if (FB.FileName.EndsWith(s, StringComparison.OrdinalIgnoreCase))
                    {
                        Ok = true;
                        break;
                    }
                }
            }
            else
            {
                Ok = true;
            }
            return Ok;

        }

        public static bool IsExcel(string FB)
        {
            bool Ok = false;

            if (FB != null)
            {
                foreach (string s in ExcelFormat)
                {
                    if (FB.EndsWith(s, StringComparison.OrdinalIgnoreCase))
                    {
                        Ok = true;
                        break;
                    }
                }
            }
            else
            {
                Ok = true;
            }
            return Ok;

        }



        //Check P.Point File{file,string}
        public static bool IsPowerpoint(HttpPostedFileBase FB)
        {
            bool Ok = false;

            if (FB != null)
            {
                foreach (string s in ExcelFormat)
                {
                    if (FB.FileName.EndsWith(s, StringComparison.OrdinalIgnoreCase))
                    {
                        Ok = true;
                        break;
                    }
                }
            }
            else
            {
                Ok = true;
            }
            return Ok;

        }

        public static bool IsPowerpoint(string FB)
        {
            bool Ok = false;

            if (FB != null)
            {
                foreach (string s in ExcelFormat)
                {
                    if (FB.EndsWith(s, StringComparison.OrdinalIgnoreCase))
                    {
                        Ok = true;
                        break;
                    }
                }
            }
            else
            {
                Ok = true;
            }
            return Ok;

        }


        public static  bool IsEmail(string Em)
        {
            try
            {
                var add = new System.Net.Mail.MailAddress(Em);
                return add.Address == Em;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsPhone(string x)
        {
            return System.Text.RegularExpressions.Regex.Match(x, @"^([0][9][1-9][0-9]{7})$").Success;
           
        }

        public static bool IsFloat(string Fl)
        {
            float x;
            try
            {
                if(float.TryParse(Fl,out x))
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }

        }
    }
}