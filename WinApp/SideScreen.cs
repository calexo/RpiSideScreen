using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using CoreAudioApi;
using System.Management;

namespace SideScreen
{
    class SideScreen
    {
        public static MMDeviceEnumerator devEnum = new MMDeviceEnumerator();
        public static MMDevice device =
          devEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);

        private static String ip = "192.168.2.183";

        public static bool shouldRun = true;

        private static System.Diagnostics.EventLog eventLog1;

        private static void Send(String type, String value)
        {
            //eventLog1.WriteEntry("Sending " + type + " : " + value + " to " + SideScreen.ip);
            System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();

            //clientSocket.Connect("127.0.0.1", 5024);
            clientSocket.Connect(SideScreen.ip, 5024);
            NetworkStream serverStream = clientSocket.GetStream();


            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(type + " " + value);
            serverStream.Write(outStream, 0, outStream.Length);
            //serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
            //returndata = System.Text.Encoding.ASCII.GetString(inStream);


            serverStream.Flush();
        }

        public static int getSoundVolume()
        {

            //bool mute = false;
            float masterVolume = 0;

            //for (int i = 0; i < device.AudioSessionManager.Sessions.Count; i++)
            //{
            //    AudioSessionControl session = device.AudioSessionManager.Sessions[i];
            //    if (session.State == AudioSessionState.AudioSessionStateActive)
            //    {
            //        Console.WriteLine("Session :{0}", i);
            //        Console.WriteLine("DisplayName: {0}", session.DisplayName);
            //        Console.WriteLine("State: {0}", session.State);
            //        Console.WriteLine("IconPath: {0}", session.IconPath);
            //        Console.WriteLine("SessionIdentifier: {0}", session.SessionIdentifier);
            //        Console.WriteLine("SessionInstanceIdentifier: {0}", session.SessionInstanceIdentifier);
            //        Console.WriteLine("ProcessID: {0}", session.ProcessID);
            //        Console.WriteLine("IsSystemIsSystemSoundsSession: {0}", session.IsSystemIsSystemSoundsSession);
            //        //Process p = Process.GetProcessById((int)session.ProcessID);
            //        //Console.WriteLine("ProcessName: {0}", p.ProcessName);
            //        //Console.WriteLine("MainWindowTitle: {0}", p.MainWindowTitle);
            //        AudioMeterInformation mi = session.AudioMeterInformation;
            //        SimpleAudioVolume vol = session.SimpleAudioVolume;
            //        //Console.WriteLine("---[Hotkeys]---");
            //        //Console.WriteLine("M  Toggle Mute");
            //        //Console.WriteLine(",  Lower volume");
            //        //Console.WriteLine(",  Raise volume");
            //        //Console.WriteLine("Q  Quit");
            //        //int start = Console.CursorTop;
            //        //while (true)
            //        //{
            //        //    //Draw a VU meter
            //        //    int len = (int)(mi.MasterPeakValue * 79);
            //        //    Console.SetCursorPosition(0, start);
            //        //    for (int j = 0; j < len; j++)
            //        //        Console.Write("*");
            //        //    for (int j = 0; j < 79 - len; j++)
            //        //        Console.Write(" ");
            //        //    Console.SetCursorPosition(0, start + 1);

            //            mute=vol.Mute;
            //            masterVolume= vol.MasterVolume;
            //            Console.WriteLine("Mute   : {0}    ", vol.Mute);
            //            Console.WriteLine("Master : {0:0.00}    ", vol.MasterVolume * 100);
            //            //if (Console.KeyAvailable)
            //            //{
            //            //    ConsoleKeyInfo key = Console.ReadKey();
            //            //    switch (key.Key)
            //            //    {
            //            //        case ConsoleKey.M:
            //            //            vol.Mute = !vol.Mute;
            //            //            break;
            //            //        case ConsoleKey.Q:
            //            //            return;
            //            //        case ConsoleKey.OemComma:
            //            //            float curvol = vol.MasterVolume - 0.1f;
            //            //            if (curvol < 0) curvol = 0;
            //            //            vol.MasterVolume = curvol;
            //            //            break;
            //            //        case ConsoleKey.OemPeriod:
            //            //            float curvold = vol.MasterVolume + 0.1f;
            //            //            if (curvold > 1) curvold = 1;
            //            //            vol.MasterVolume = curvold;
            //            //            break;
            //            //    }

            //            //}
            //        //}
            //    }
            //}

            masterVolume =
                device.AudioEndpointVolume.MasterVolumeLevelScalar * 100;

            //return Int16.Parse(masterVolume*100);
            Console.WriteLine("Volume : {0}", (int)masterVolume);
            return (int)masterVolume;




        }
        public static void Run(String ip)
        {
            Double freeMem = 0;
            Double totalMem = 0;
            Double[] Proc;
            Double Procs = 0;

            //if (args.Length == 1)
            //{
            //    //System.Console.WriteLine("Please enter a numeric argument.");
            //    System.Console.WriteLine("Using IP for SideScreen : " + args[0]);
            //    SideScreen.ip = args[0];
            //}
            SideScreen.ip = ip;

            eventLog1 = new System.Diagnostics.EventLog();
            //((System.ComponentModel.ISupportInitialize)(eventLog1)).BeginInit();
            //((System.ComponentModel.ISupportInitialize)(this.eventLog1)).EndInit();
            eventLog1.Source = "SideScreen";
            eventLog1.Log = "Application";

            eventLog1.WriteEntry("Go Run");


            while (shouldRun)
            {
                //eventLog1.WriteEntry("Proc collecting...");
                Proc = new Double[255];
                try
                {
                    ManagementObjectSearcher searcher =
                        new ManagementObjectSearcher("root\\CIMV2",
                        "SELECT * FROM Win32_OperatingSystem");

                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        freeMem = System.Convert.ToDouble(queryObj["FreePhysicalMemory"]) / 1024;
                        totalMem = System.Convert.ToDouble(queryObj["TotalVisibleMemorySize"]) / 1024;


                        //Console.WriteLine("-----------------------------------");
                        //Console.WriteLine("Win32_OperatingSystem instance");
                        //Console.WriteLine("-----------------------------------");
                        Console.WriteLine("FreePhysicalMemory: {0}", freeMem);
                        //Console.WriteLine("FreeVirtualMemory: {0}", queryObj["FreeVirtualMemory"]);
                        //Console.WriteLine("TotalVirtualMemorySize: {0}", queryObj["TotalVirtualMemorySize"]);
                        Console.WriteLine("TotalVisibleMemorySize: {0}", totalMem);
                        Console.WriteLine("Free Mem : {0} %", (freeMem / totalMem) * 100);



                    }
                }
                catch (ManagementException e)
                {
                    Console.WriteLine("An error occurred while querying for WMI data: " + e.Message);
                }

                try
                {
                    ManagementObjectSearcher searcher2 =
                        new ManagementObjectSearcher("root\\CIMV2",
                        "SELECT * FROM Win32_PerfFormattedData_Counters_ProcessorInformation");
                    //int procI=0;
                    int procNumber = 0;
                    int threadNumber = 0;
                    foreach (ManagementObject queryObj in searcher2.Get())
                    {
                        if (!queryObj["Name"].ToString().Contains("Total"))
                        {
                            //Console.WriteLine("-----------------------------------");
                            //Console.WriteLine("Win32_PerfFormattedData_Counters_ProcessorInformation instance");
                            //Console.WriteLine("-----------------------------------");
                            Console.WriteLine("Name: {0}", queryObj["Name"]);
                            //Console.WriteLine("PercentofMaximumFrequency: {0}", queryObj["PercentofMaximumFrequency"]);

                            procNumber = Int16.Parse(queryObj["Name"].ToString().Split(',').GetValue(0).ToString());
                            threadNumber = Int16.Parse(queryObj["Name"].ToString().Split(',').GetValue(1).ToString());
                            Console.WriteLine("Thread: {0} = {1}-{2}", queryObj["Name"], procNumber, threadNumber);
                            Proc[threadNumber + procNumber * 4] = Double.Parse(queryObj["PercentProcessorTime"].ToString());
                            Console.WriteLine("PercentProcessorTime: {0}", queryObj["PercentProcessorTime"]);
                        }
                        else if (queryObj["Name"].ToString().Equals("_Total"))
                        {
                            Console.WriteLine("Name: {0}", queryObj["Name"]);
                            Procs = Double.Parse(queryObj["PercentProcessorTime"].ToString());
                        }
                    }
                }
                catch (ManagementException e)
                {
                    Console.WriteLine("An error occurred while querying for WMI data: " + e.Message);
                }

                //eventLog1.WriteEntry("Sending to " + ip);
                try
                {

                    Send("TotalMem", totalMem.ToString());
                    Send("FreeMem", freeMem.ToString());
                    Send("Proc1", Proc[0].ToString());
                    Send("Proc2", Proc[1].ToString());
                    Send("Proc3", Proc[2].ToString());
                    Send("Proc4", Proc[3].ToString());
                    //Send("Proc5", Proc[4].ToString());
                    //Send("Proc6", Proc[5].ToString());
                    //Send("Proc7", Proc[6].ToString());
                    //Send("Proc8", Proc[7].ToString());
                    Send("Procs", Procs.ToString());
                    Send("Vol", getSoundVolume().ToString());
                }
                catch (SocketException e)
                {
                    Console.WriteLine("RpiScreen not available, trying again in 10 sec : " + e.Message);
                    //eventLog1.WriteEntry("RpiScreen not available, trying again in 10 sec : " + e.Message);
                    System.Threading.Thread.Sleep(10000);
                }
                System.Threading.Thread.Sleep(1000);

                
            }

        }
    }
}
