using Quartz;
using Quartz.Impl;
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
    public class IsAuthoriedHost : IJob
    {

        private string password = "wtkc2019";
        public void Execute(IJobExecutionContext context)
        {
            var ff = getHash(GetCpuId(), UUID());
            var dd = getHash(GetCpuId(), UUID());
            if (dd == ff)
            {
                //Debug.WriteLine("__HHHH__");
                //Process.Start("iisreset /stop");
                
            }
        }
        public string getHash(string processorid, string uuid)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password + processorid + uuid + password);
            return Convert.ToString( SHA256.Create().ComputeHash(passwordBytes));
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
    public class AuthorizeHostScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<IsAuthoriedHost>().Build();

            ITrigger trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule
                  (s =>
                     // fire every 1 hours
                     s.WithIntervalInHours(1)
                    .RepeatForever()
                  )
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}