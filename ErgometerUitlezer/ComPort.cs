﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErgometerUitlezer
{
    class ComPort
    {
        private SerialPort comPort;

        public ComPort()
        {
            comPort = null;
        }

        public Boolean Connect(string port)
        {
            comPort = new SerialPort();
            comPort.PortName = port;
            comPort.DataBits = 8;
            comPort.Parity = Parity.None;
            comPort.StopBits = StopBits.One;
            comPort.BaudRate = 9600;

            comPort.Open();

            return comPort.IsOpen;
        }

        public Boolean Disconnect()
        {
            comPort.Close();
            return !comPort.IsOpen;
        }

        public Boolean IsOpen()
        {
            if (comPort != null)
                return comPort.IsOpen;
            else
                return false;
        }

        public void Write(string input)
        {
            if(IsOpen())
            {
                comPort.WriteLine(input);
            }
        }

        public string Read()
        {
            if (IsOpen())
            {
                return comPort.ReadLine();
            }

            return "";
        }
    }
}
