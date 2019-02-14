using System;

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


        static AppSettings()
        { 
            ApplicationId = 6752080;
            AccessToken = "INPUT_YOUR_TOKEN";
            PythonPath = "INPUT_YOUR_PATH";
            PythonScriptPath = "INPUT_YOUR_PATH";
        }
    }
}