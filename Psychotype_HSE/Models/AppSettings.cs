using System;

namespace Psychotype.Models
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


        static AppSettings()
        { 
            ApplicationId = 6752080;
            AccessToken = "331381e0a41bfa1b04ab54ef7a9927e2efeda6b46aca936edd372feee7ab200b9629c62a165924b11074f";
            // Here we need to initialize all properties with fake account information
        }

    }
}