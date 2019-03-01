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
        /// <summary>
        /// Path to suicide_predict.csv (data for suicideScript.py)
        /// </summary>
        public static string UserPosts { get; set; }
        /// <summary>
        /// Result of suicide prediction by python script
        /// </summary>
        public static string SuicideResult { get; set; }
        /// <summary>
        /// Directory where python script searches for text input (id.csv)
        /// and leaves probobilities (id.txt).
        /// </summary>
        public static string WorkingDir { get; set; }

        static AppSettings()
        { 
            ApplicationId = 6752080;
            AccessToken = "Access_Token";          
            PythonPath = @"Python_Path";
            PythonScriptPath = @"../../Util/Scripts/suicideScript.py";
            //UserPosts = @"../../Files/userPosts.csv";
            //SuicideResult = @"../../Files/suicide_result.txt";
            WorkingDir = @"../../Files/";
        }
    }
}