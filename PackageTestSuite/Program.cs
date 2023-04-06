using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Xml.Schema;
using WebUntis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;
using System.Reflection.Metadata;

namespace PackageTestSuite
{
    class Program
    {
        static void Main(string[] args)
        {
            string username = "";
            string password = "";
            string school = "";
            string server = "";
            string clientIdent = "";
            string stringPupilId = "";
            int pupilId = 0;

            try
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().Providers.First();
                if (!config.TryGet("Username", out username)) throw new ArgumentNullException("can't live without username");
                if (!config.TryGet("Password", out password)) throw new ArgumentNullException("can't live without password");
                if (!config.TryGet("School", out school)) throw new ArgumentNullException("can't live without school");
                if (!config.TryGet("ServerIdent", out server)) throw new ArgumentNullException("can't live without server");
                if (!config.TryGet("ClientIdentifier", out clientIdent)) throw new ArgumentNullException("can't live without clientIdent");
                if (!config.TryGet("PupilId", out stringPupilId)) throw new ArgumentNullException("can't live without pupilId");
                if (!int.TryParse(stringPupilId, out pupilId)) throw new InvalidDataException("could not parse int out of stirngPupilId");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            var untis = new WebUntis.WebUntis()
            {
                Username = username,
                Password = password,
                School = school,
                ServerIdent = server,
                ClientIdentifier = clientIdent
            };


            var version = untis.GetVersion();
            Console.WriteLine("Package-Version: " + string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision));

            Console.WriteLine("Login");
            var res = untis.Login().Result;
            if (!res)
            {
                Console.WriteLine("##### no good");
                Environment.Exit(0);
            }
            Console.WriteLine("Session: " + untis.Session.SessionToken);

            Console.WriteLine("Enabling auto-refresh: "+untis.EnableKeepSessionAlive().Result);

            var startTime = DateTime.Now;

            while (true)
            {
                Console.WriteLine("Executing at " + DateTime.Now.ToLongTimeString());
                Console.WriteLine($"Getting timetable for Pupil {pupilId} at school {school}");

                // try one of the nice timetable functions
                // var timetable = untis.GetTimeTable(WebUntisItemType.Student, pupilId, DateTime.Now.AddDays(3), DateTime.Now.AddDays(8)).Result;
                var timetable = untis.GetTimeTableToday(WebUntisItemType.Student, pupilId).Result;


                DateTime curDate = default;
                if (timetable == null)
                {
                    Console.WriteLine("Detected null value, that's not good!");
                    Console.WriteLine("Sleeping 10s...");
                    Thread.Sleep(10_000);
                }
                foreach (var elem in timetable)
                {
                    if (curDate != elem.EntryDate)
                    {
                        Console.WriteLine(elem.EntryDate);
                        curDate = elem.EntryDate;
                    }
                    Console.WriteLine($"{elem.ClassType}: {elem.EntryStartTime}-{elem.EntryEndTime}: {elem.Subject}" + (elem.IsCancelled ? " cancelled" : ""));
                }

                Console.WriteLine("------------------ End of timetable");
                Console.WriteLine("Sleeping 10s...");
                Thread.Sleep(10_000);
                Console.Clear();
            }


            Console.WriteLine("Logging out");
            res = untis.Logout().Result;
            
            if (!res)
            {
                Console.WriteLine("##### no good");
                Environment.Exit(0);
            }
            Console.WriteLine("success");

            Console.WriteLine("<END>");
            Console.ReadLine();
        }
    }
}
