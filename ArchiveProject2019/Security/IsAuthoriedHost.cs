using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Management;
using System.IO;
using System.Collections;

namespace ArchiveProject2019.Security
{
    public class IsAuthoriedHost
    {

        private static string password = "wtkc2019";
        private static string processorId = "wtkc2019";
        private static string uuid = "wtkc2019";

        public static bool checkAuthorize()
        {
            //string AuthoridHost = getHash(processorId, uuid);

            var AuthoridHost = getHash(GetCpuId(), UUID());
            var CurrentHost = getHash(GetCpuId(), UUID());

            if (AuthoridHost.Equals(CurrentHost))
            {
                return true;
            }

            return false;
        }

        public static string getHash(string processorid, string uuid)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password + processorid + uuid + password);
            return System.Text.Encoding.Default.GetString( SHA256.Create().ComputeHash(passwordBytes));
        }

        // get processor id
        private static string GetCpuId()
        {
            var mbs = new ManagementObjectSearcher("select ProcessorId from Win32_processor");
            var mbsList = mbs.Get();
            var cpuId = "";
            foreach (var mo in mbsList)
            {
                cpuId = mo["ProcessorId"].ToString();
                break;
            }
            return cpuId;
        }
        // get mother board identifier (Univerally Unique Identifier)
        private static string UUID()
        {
            string uuid = string.Empty;

            ManagementClass mc = new ManagementClass("Win32_ComputerSystemProduct");
            ManagementObjectCollection moc = mc.GetInstances();

            foreach (ManagementObject mo in moc)
            {
                uuid = mo.Properties["UUID"].Value.ToString();
                break;
            }

            return uuid;
        }
    }
    
}