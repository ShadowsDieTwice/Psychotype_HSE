using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Psychotype_HSE.Models;
using Psychotype_HSE.Util;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Psychotype_HSE
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            PythonRunner.RunScript(AppSettings.PythonScriptPath, AppSettings.PythonPath);
            //,AppSettings.WorkingDir);//, AppSettings.UserPosts);
            //Database.SetInitializer(new PsyhotypeDbInitializer());
        }

        void Application_End(object sender, EventArgs e)
        {
            // подключение
            IPEndPoint ipe = new IPEndPoint(AppSettings.LocalIP, AppSettings.ClientPort);
            Socket socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipe);
            // передаем в данные
            Byte[] bytesSent = Encoding.UTF32.GetBytes("~exit~::");
            socket.Send(bytesSent, bytesSent.Length, 0);
        }
    }
}
