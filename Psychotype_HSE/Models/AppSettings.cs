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
            AccessToken = "468b8f1f3717146699c57a42f29f766223d3a7b7d96911aefc07c2ffab480cbb20992aff8e4537dce5429";
        }
    }
}