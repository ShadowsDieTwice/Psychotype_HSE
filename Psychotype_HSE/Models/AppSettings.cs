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
            AccessToken = "2a18554c466cb7c9a500d148c77ff3ac864d07dcb05f08167990fddbe502bd458443d19e01a2edaa769a0";
            PythonPath = @"C:\ProgramData\Anaconda3\python.exe";
            PythonScriptPath = @"C:\Users\1\Source\Repos\myrachins\Psychotype_HSE_v2\Psychotype_HSE\Util\Scripts\suicideScript.py";
            //PythonPsychotypeScriptPath = @"M:/Grudina/HSE psychotype2/Psychotype_HSE/Util/Scripts/psychotypeScript.py";
            //UserPosts = @"M:/Grudina/HSE psychotype2/Psychotype_HSE/Files/userPosts.csv";
            //SuicideResult = @"M:/Grudina/HSE psychotype2/Psychotype_HSE/Files/suicide_result.txt";
            WorkingDir = @"C:\Users\1\Source\Repos\myrachins\Psychotype_HSE_v2\Psychotype_HSE\Files\";
        }
    }
}