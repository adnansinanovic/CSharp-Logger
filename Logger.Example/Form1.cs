using Logger.Fomatters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Windows.Forms;

namespace Logger.Example
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            Person he = new Person();
            he.Name = "Henrik";
            he.LastName = "Larsson";
            he.Age = 6;

            he.Car = new Car();
            he.Car.Window = new CarPart() { Price = 154.2 };
            he.Car.Wheel = new CarPart() { Price = 589.2 };

            FileLogger.SetDumpMethod(ObjectDumpMethod.Traverse);
            FileLogger.Settings.FilePath = @"C:\Folder\LogFile.log";
            FileLogger.DeleteLogFile();

            FileLogger.DumperSettings.AddFormatter(new DateTimeFormatter("yyyy-MM-dd HH:mm:ss.fffff"));
            FileLogger.DumperSettings.WriteElementType = false;
            FileLogger.DumperSettings.MaxDepth = 6;
            FileLogger.Settings.MaxFileSizeMB = 3;

            FileLogger.WriteLine("Below is simple string list: ");
            FileLogger.WriteLine(new List<string>() { "jen", "dva", "tri" });

            FileLogger.WriteLine("Who is it");
            FileLogger.WriteLine("What do you want");
            FileLogger.WriteLine("Below is integer example: ");
            FileLogger.WriteLine(78910);
            FileLogger.WriteLine("Below is double example: ");
            FileLogger.WriteLine(351.453);
            FileLogger.WriteLine((double)(10 / 3m));

            FileLogger.WriteLine("Below is list of lists of string: ");
            FileLogger.WriteLine(new List<List<string>>()
            {
                new List<string> { "L00", "L01", "L02"},
                new List<string> { "L10", "L11", "L12", "L13"},
                null,
            });

            FileLogger.WriteLine("Below is null: ");
            object nullValue = null;
            FileLogger.WriteLine(nullValue);

            FileLogger.WriteLine("Below is object: ");
            FileLogger.WriteLine("Row first:", he, "Last row");

            FileLogger.WriteLine("Below are parameters: ");
            FileLogger.Write(1234, he, nullValue, "Kurin", new List<string>() { "jen", "dva", "tri" });

            FileLogger.Write("Example: ", 458);

            FileLogger.Write(new int[4]);
            FileLogger.Write(DateTime.Now);


            Planet planet = new Planet();
            planet.Mass = 50;
            planet.SetName = "Green Planet";
            planet.Population = 54789;
            planet.Radius = 35;
            planet.Sattelites = 2;

            FileLogger.WriteLine($"Planet without formatter", planet);
            FileLogger.DumperSettings.AddFormatter(new PlanetFormatter());
            FileLogger.WriteLine($"Planet with ormatter", planet);


            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://www.google.com");
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
            httpWebRequest.Method = "GET";

            FileLogger.WriteLine(httpWebRequest);

            try
            {
                try
                {
                    try
                    {
                        throw new Exception("Hi. I am exception.");

                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Hi. I'm exception 2.", ex);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Hi. I'm exception 3.", ex);
                }

            }
            catch (Exception e)
            {
                FileLogger.WriteLine(e, "Exception TITLE");
            }

            FileLogger.WriteLine(new P1());
        }
    }
}

