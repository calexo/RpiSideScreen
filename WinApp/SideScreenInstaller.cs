using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace SideScreen
{
    [System.ComponentModel.RunInstaller(true)]
    public class SideScreenInstaller : Installer
    {
        private ServiceProcessInstaller processInstaller;
        private ServiceInstaller serviceInstaller;

        public SideScreenInstaller()
        {
            // This call is required by the Designer.
            InitializeComponent();
        }


        private void InitializeComponent()
        {
            this.processInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // processInstaller
            // 
            this.processInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.processInstaller.Password = null;
            this.processInstaller.Username = null;
            // 
            // serviceInstaller
            // 
            this.serviceInstaller.DisplayName = "SideScreen WinService";
            this.serviceInstaller.ServiceName = "SideScreen";
            this.serviceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.serviceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceInstaller_AfterInstall);
            // 
            // SideScreenInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceInstaller,
            this.processInstaller});

        }

        private void serviceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {

        }

    }

}