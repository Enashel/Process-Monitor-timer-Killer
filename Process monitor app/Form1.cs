using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
namespace Process_monitoring
{

    public partial class monitoring : Form
    {
        TimeSpan rtime;
        Process[] processes;
        Process[] pbyname;
        string prbyname;
        public monitoring()
        {
            InitializeComponent();
        }

        public void submit_Click(object sender, EventArgs e)
        {
            textboxcheck();
            progrm();
        }
        public void worktime_TextChanged(object sender, EventArgs e)
        {
            float fwValue;
            if (!float.TryParse(worktime.Text, out fwValue))
            {
                worktime.Text = "";
            }
        }

        public void mofreq_TextChanged(object sender, EventArgs e)
        {
            float ffValue;
            if (!float.TryParse(mofreq.Text, out ffValue))
            {
                mofreq.Text = "";
            }
        }

        public void monitoring_Load(object sender, EventArgs e)
        {
            ListProcesses();
            timer2.Stop();
            timer1.Stop();
            
        }
        private void ListProcesses()
        {
            processes = Process.GetProcesses();
            foreach (Process p in processes)
            {
                listBox1.Items.Add(p.ProcessName);
            }

        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            ListProcesses();
        }
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            pid.Text = listBox1.SelectedItem.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MessageBox.Show("Program is on for: " + rtime.TotalMinutes.ToString("F") + " minutes");
            timer2.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {if(timer1.Enabled)
            {
                timer2.Start();
                timer2.Interval = 1000;
                progrm();
            }

        }
        public void textboxcheck()
        {
            switch (String.IsNullOrEmpty(pid.Text))
            {
                case true:
                    MessageBox.Show("Please insert a process name");

                    break;
                case false:

                    break;
            }

            switch (String.IsNullOrEmpty(worktime.Text))
            {
                case true:
                    MessageBox.Show("Please insert process run time");

                    break;
                case false:

                    break;
            }
            switch (String.IsNullOrEmpty(mofreq.Text))
            {
                case true:
                    MessageBox.Show("Please insert app monitor frequency time");

                    break;
                case false:

                    break;
            }

        }
        public void progrm()
        {
            prbyname = pid.Text;
            pbyname = Process.GetProcessesByName(prbyname);
            if ((String.IsNullOrEmpty(pid.Text) || String.IsNullOrEmpty(worktime.Text) || String.IsNullOrEmpty(mofreq.Text)) != true)
            {
                if (pbyname.Length > 0)
                {
                    foreach (Process p in pbyname)
                    {

                        try
                        {
                            rtime = DateTime.Now - p.StartTime;
                            textBox1.Text = rtime.TotalSeconds.ToString("F");
                            float mworktime = (float.Parse(worktime.Text) * 1000) * 60;
                            if (mworktime < (float)rtime.TotalMilliseconds)

                            {
                                p.Kill();
                                timer2.Stop();
                                ListProcesses();
                            }
                           
                            timer1.Interval= (int.Parse(mofreq.Text) * 1000) * 60;
                            timer1.Enabled = true;
                            timer1.Start();
                               
                        }
                        catch (Win32Exception except)
                        {
                            if (except.NativeErrorCode == 5)
                                continue;
                            throw;
                        }

                    }
                }
                else
                {
                    textBox1.Text = "Process is not started";
                }
            }


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

