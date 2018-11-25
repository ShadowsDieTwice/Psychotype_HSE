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
            AccessToken = "a978bcd5c251c73bbca667124829b6ffb6553058b5145943ed725df37ab7ea3066f90f3ca38a13350e338";
            // Here we need to initialize all properties with fake account information
        }

    }
}