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
            AccessToken = "ae2f7da77aa4522f30a938410fafccac56861ec7cd8affaa7d613554015a7d8bf40b1065566c48cd63d50";
            // Here we need to initialize all properties with fake account information
        }

    }
}