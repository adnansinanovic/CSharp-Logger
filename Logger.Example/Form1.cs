using Logger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
            he.Age = 5;

            he.Car = new Car();
            he.Car.Window = new CarPart() { Price = 154.2 };
            he.Car.Wheel = new CarPart() { Price = 589.2 };

            
            FileLogger.DeleteLogFile();
            FileLogger.Settings.MaxFileSizeMB = 3;
            //FileLogger.WriteLine(he);

            Person she = null;
            // FileLogger.WriteLine(she);


            FileLogger.WriteLine("Who is it");
            FileLogger.WriteLine("What do you want");
            FileLogger.WriteLine("Below is integer example: ");
            FileLogger.WriteLine(78910);
            FileLogger.WriteLine("Below is double example: ");
            FileLogger.WriteLine(351.453);
            FileLogger.WriteLine((double)(10/3m));

            FileLogger.WriteLine("Below is simple string list: ");
            FileLogger.WriteLine(new List<string>() { "jen", "dva", "tri" });

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
            FileLogger.WriteLine("babo ruco:", he);

            FileLogger.WriteLine("Below are parameters: ");
            FileLogger.Write(1234, he, nullValue, "Kurin", new List<string>() { "jen", "dva", "tri"});

            FileLogger.Write("Example: ", 458);


            string urlBuilder = @"http://www.mapquestapi.com/directions/v2/route?key=Gmjtd%7Cluu22h62nq%2Cra%3Do5-lzand&ambiguities=ignore&outFormat=xml&avoidTimedConditions=false&doReverseGeocode=false&timeType=0&narrativeType=none&enhancedNarrative=false&shapeFormat=raw&generalize=0&generalize=0&locale=en_US&unit=m&routeType=shortest&from=20415+Anza+Ave+Apt+1,Torrrance,CA+90503&to=20+Anza+Ave+Apt+1,Torrrance,CA+90503";


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlBuilder.ToString());
            request.Referer = "drek";
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
            request.Method = "GET";
            request.UseDefaultCredentials = true;
            request.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;

          //  FileLogger.Write(request);
        }        
    }
}

