using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

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
        /// Path to python interpreter (in a way...)
        /// </summary>
        public static string PythonPath { get; set; }
        /// <summary>
        /// Directory where python script searches for text input (id.csv)
        /// and leaves probabilities (id.txt).
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

            PythonPath = @"YOUR_PYTHON_PATH";
            WorkingDir = @"DIRECTORY_TO_EXECUTABLE_PROJECT";
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