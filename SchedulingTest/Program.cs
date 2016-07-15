using System;
using System.IO;
using System.Threading.Tasks;
using Hangfire;

namespace SchedulingTest
{
    internal class Program
    {
        protected static object Locker = new object();

        private static void Main(string[] args)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage(
                "Data Source=WUSMG185230-TJ1;Initial Catalog=HangfireTest;User Id=hangfire;Password=Hang_fire;");

            using (var server = new BackgroundJobServer())
            {
                Console.WriteLine("Hangfire Server started. Press any key to exit...");
                Console.WriteLine();
                for (var i = 0; i < 1000; i++)
                {
                    RecurringJob.AddOrUpdate($"test-job-{i}", () => Execute(), Cron.Minutely);
                }

                Console.ReadKey();
            }
        }

        public static void Execute()
        {
            var now = DateTime.Now;
            var machineName = Environment.MachineName;
            lock (Locker)
            {
                File.AppendAllText("C:\\hangfire.txt", $"Yo! {machineName} -- {now}{Environment.NewLine}");
            }
        }
    }
}