using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Windows.Forms;

namespace shutdown_timer
{
    public partial class Form1 : Form
    {
        double counter = 0;

        public Form1()
        {
            InitializeComponent();
        }

        public void shutdown()
        {
            /*
             uint32 Win32Shutdown(
             [in] sint32 Flags,
             [in] sint32 Reserved = 
            );

            WMIServiceObject = GetObject(
    "Winmgmts:{impersonationLevel=impersonate,(Debug,Shutdown)}")
    ForEach ComputerObject In WMIServiceObject.InstancesOf("Win32_OperatingSystem") 
    testResult = ComputerObject.Win32Shutdown(2 + 4, 0) 
    'reboot
    'testResult = ComputerObject.Win32Shutdown(0, 0) 'logoff 
    ' testResult = ComputerObject.Win32Shutdown(8 + 4, 0) 'shutdown 

            */

            ManagementClass mClass = new ManagementClass("Win32_OperatingSystem");
            mClass.Get();
            ManagementBaseObject mboClass_shutdown = mClass.GetMethodParameters("Win32Shutdown");
            //label5.Text = mboClass_shutdown.GetType().ToString();
            mClass.Scope.Options.EnablePrivileges = true;
            mboClass_shutdown["Flags"] = 1;
            mboClass_shutdown["Reserved"] = 0;
            foreach (ManagementObject moClass in mClass.GetInstances())
            {
              //moClass.InvokeMethod("Win32Shutdown", mboClass_shutdown, null);
            }
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Shutdown Timer by Bariz";
        }


        public void startButton_Click(object sender, EventArgs e)
        {
            //Console.WriteLine("Start");
            startButton.Text = "Stop";
            timer1.Enabled = true;
            //Console.WriteLine(counter);
            if (counter != 0)
            {
                //Console.WriteLine("Inner");
                counter = 0;
                timer1.Enabled = false;
                startButton.Text = "Start";
                makeReadOnly();
            }
            else
            {
                makeReadOnly();
                getNum();
                //Console.WriteLine("Ends");
            }
        }

        public void timer1_Tick(object sender, EventArgs e)
        {
            displayNums(counter);
//            label6.Text = Convert.ToString(counter);
            //label5.Text = "i:" + i++;
            if(counter == 0)
            {
                timer1.Enabled = false;
                shutdown();
            }
            counter--;
        }


        public void getNum()
        {
            counter = (Convert.ToInt32(numericUpDown1.Value) * 3600) +
                             (Convert.ToInt32(numericUpDown2.Value) * 60) +
                             Convert.ToInt32(numericUpDown3.Value);
           //label6.Text = Convert.ToString(inMillisec);
        }

        public void displayNums(double totalMs)
        {   

            double hour = Math.Floor(totalMs / 3600);

            double mins = Math.Floor((totalMs - (hour * 3600)) / 60);
            
            double seconds = Math.Floor((totalMs - (hour * 3600) - (mins * 60) ));

            numericUpDown1.Value = Convert.ToDecimal(hour);
            numericUpDown2.Value = Convert.ToDecimal(mins);
            numericUpDown3.Value = Convert.ToDecimal(seconds);
        }

        public void makeReadOnly()
        {
            if(timer1.Enabled == true){
                numericUpDown1.ReadOnly = true;
                numericUpDown2.ReadOnly = true;
                numericUpDown3.ReadOnly = true;
            }
            else
            {
                numericUpDown1.ReadOnly = false;
                numericUpDown2.ReadOnly = false;
                numericUpDown3.ReadOnly = false;
            }

        }
    }
}
