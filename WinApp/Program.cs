using System;
using System.Collections.Generic;
using System.Text;




namespace SideScreen
{
    class Program
    {

        public static void Main(string[] args)
        {
            System.ServiceProcess.ServiceBase.Run(new SideScreenService());
        }
    }
}
