using Logger;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

            he.car = new Car();
            he.car.Window = new CarPart() { Price = 154.2 };
            he.car.Wheel = new CarPart() { Price = 589.2 };

            
            FileLogger.DeleteLogFile();
            FileLogger.Settings.MaxFileSizeMB = 3;
            FileLogger.WriteLine(he);

            Person she = null;
            FileLogger.WriteLine(she);


            FileLogger.WriteLine("Herodot", "Batistuta", "Zeklo", "Kurin");
        }        
    }
}
