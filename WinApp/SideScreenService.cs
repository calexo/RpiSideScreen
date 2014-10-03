using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using System.Threading;


namespace SideScreen
{
    class SideScreenService : ServiceBase
    {
        private System.Diagnostics.EventLog eventLog1;
    
        static void Main()
        {
            System.ServiceProcess.ServiceBase[] ServicesToRun;
            ServicesToRun =
              new System.ServiceProcess.ServiceBase[] { new SideScreenService() };
            System.ServiceProcess.ServiceBase.Run(ServicesToRun);
        }

        public SideScreenService()
        {
            InitializeComponent();
            if (!System.Diagnostics.EventLog.SourceExists("SideScreen"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "SideScreen", "SideScreen Log");
            }
            eventLog1.Source = "SideScreen";
            eventLog1.Log = "Application";
        }


        private void InitializeComponent()
        {
            this.eventLog1 = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).BeginInit();
            // 
            // SideScreenService
            // 
            this.ServiceName = "SideScreen";
            ((System.ComponentModel.ISupportInitialize)(this.eventLog1)).EndInit();

        }

        public Thread StartTheThread(String ip)
        {
            var t = new Thread(() => SideScreen.Run(ip));
            t.Start();
            return t;
        }

        public void Log(String msg)
        {
            eventLog1.WriteEntry(msg);
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("In OnStart");
            //Thread t = new Thread(new ThreadStart(SideScreen.Run("192.168.2.183")));
            //SideScreen.Run("192.168.2.183");
            //Thread t = new Thread(new ParameterizedThreadStart(myParamObject));
            Thread t = StartTheThread("192.168.2.103");
            //t.Start();

            eventLog1.WriteEntry("End OnStart");
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("In OnStop");
            SideScreen.shouldRun = false;
        }

    }
}
