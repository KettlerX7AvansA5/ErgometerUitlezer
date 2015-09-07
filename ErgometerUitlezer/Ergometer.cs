﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErgometerUitlezer
{
    public partial class Ergometer : Form
    {
        private ComPort comPort;

        public Ergometer()
        {
            InitializeComponent();
            comPort = new ComPort();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if(!comPort.IsOpen())
            {
                if (comPort.Connect(ComPortBox.Text))
                {
                    connectButton.Text = "Disconnect";
                    statusButton.Enabled = true;
                    resetButton.Enabled = true;
                    timerStatus.Enabled = true;
                    inputTextBox.Enabled = true;
                    timeButton.Enabled = true;
                    distanceButton.Enabled = true;
                    powerButton.Enabled = true;
                    energyButton.Enabled = true;
                    ComPortBox.Enabled = false;

                    comPort.Write("RS");
                    comPort.Read();
                    Thread.Sleep(200);
                    comPort.Write("CM");
                    comPort.Read();
                    Thread.Sleep(200);

                    comPort.Write("ST");
                    string response = comPort.Read();
                    Console.WriteLine(response);

                    Meting test = FormatHelper.Status(response);
                    string test2 = test.ToString();
                    richTextBox1.Text = test2;
                }
            }
            else
            {
                if(comPort.Disconnect())
                {
                    connectButton.Text = "Connect";
                    timerStatus.Checked = false;
                    statusButton.Enabled = false;
                    resetButton.Enabled = false;
                    timerStatus.Enabled = false;
                    inputTextBox.Enabled = false;
                    timeButton.Enabled = false;
                    distanceButton.Enabled = false;
                    powerButton.Enabled = false;
                    energyButton.Enabled = false;
                    ComPortBox.Enabled = true;
                    richTextBox1.Text = "";
                    
                }
            }
            
           
        }

        private void statusButton_Click(object sender, EventArgs e)
        {
            comPort.Write("ST");
            string response = comPort.Read();
            Console.WriteLine(response);
            richTextBox1.Text = FormatHelper.Status(response).ToString();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            comPort.Write("RS");
            comPort.Read();
            Thread.Sleep(200);
            comPort.Write("CM");
            comPort.Read();
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            if(timerStatus.Checked)
            {
                comPort.Write("ST");
                string response = comPort.Read();
                Console.WriteLine(response);
                richTextBox1.Text = FormatHelper.Status(response).ToString();
            }
        }

        private void timeButton_Click(object sender, EventArgs e)
        {
            comPort.Write("RS");
            Thread.Sleep(200);
            string[] temp = inputTextBox.Text.Split(':');
            int minutes = int.Parse(temp[0]);
            int seconds = int.Parse(temp[1]);
            comPort.Write("PT " + minutes.ToString() + seconds.ToString());
            inputTextBox.Text = "";
            comPort.Read();
        }

        private void distanceButton_Click(object sender, EventArgs e)
        {
            comPort.Write("RS");
            Thread.Sleep(200);
            int temp = int.Parse(inputTextBox.Text);
            comPort.Write("PD " + temp.ToString());
            inputTextBox.Text = "";
            comPort.Read();
        }

        private void powerButton_Click(object sender, EventArgs e)
        {
            int temp = int.Parse(inputTextBox.Text);
            comPort.Write("PW " + temp.ToString());
            inputTextBox.Text = "";
            comPort.Read();
        }

        private void energyButton_Click(object sender, EventArgs e)
        {
            comPort.Write("RS");
            Thread.Sleep(200);
            int temp = int.Parse(inputTextBox.Text);
            comPort.Write("PE " + temp.ToString());
            inputTextBox.Text = "";
            comPort.Read();
        }

        private void timerStatus_CheckedChanged(object sender, EventArgs e)
        {
            if (timerStatus.Checked)
            {
                statusButton.Enabled = false;
                updateTimer.Start();
            }
            else
            {
                statusButton.Enabled = true;
                updateTimer.Stop();
            }
        }
    }
}
