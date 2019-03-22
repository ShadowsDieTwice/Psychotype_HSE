using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace Psychotype_HSE.Models
{
    /// <summary>
    /// All app settings which is required 
    /// </summary>
    public static class AppSettings
    {
        /// <summary>
        /// Id of registered VK application (VkApi works only beyond such applications)
        /// </summary>
        public static ulong ApplicationId { get; set; }
        /// <summary>
        /// Access token for VK API
        /// </summary>
        public static string AccessToken { get; set; }
        /// <summary>
        /// Path to python model script 
        /// </summary>
        public static string PythonScriptPath { get; set; }
		/// <summary>
		/// Full path to 'python.exe'
		/// Example: 'C:\Users\User\AppData\Local\Programs\Python\Python36\python.exe'
		/// </summary>
		public static string PythonPath { get; set; }
		/// <summary>
		/// Full path to '~/Util/Scripts' directory
		/// Example: 'C:\Psychotype_HSE\Psychotype_HSE\Util\Scripts\'
		/// </summary>
		public static string WorkingDir { get; set; }
        /// <summary>
        /// Port on localhost, that make predictions about suicide.
        /// </summary>
        public static int ClientPort { get; set; }
        /// <summary>
        /// IPv4 address of current machine
        /// </summary>
        public static IPAddress LocalIP { get; set; }
        /// <summary>
        /// VK login
        /// </summary>
        public static string Login { set; get; }
        /// <summary>
        /// VK password
        /// </summary>
        public static string Password { set; get; }

        static AppSettings()
        {
            ApplicationId = 6752080;
            Login = "+79636540385";
            Password = "16032019GD";
            PythonScriptPath = @"suicideScript.py";

            PythonPath = @"PYTHON_PATH";
            WorkingDir = @"PATH_TO_~/Util/Scripts/";
			ClientPort = 1111;

            // receive local ip address
            foreach (var ip in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    LocalIP = ip;
                    break;
                }
            }
        }
    }
}