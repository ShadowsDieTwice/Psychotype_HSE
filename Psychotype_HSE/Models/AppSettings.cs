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
        /// Login of user application owner 
        /// </summary>
        public static string Login { get; set; }
        /// <summary>
        /// Password of user application owner
        /// </summary>
        public static string Password { get; set; }

        static AppSettings()
        {
            // Here we need to initialize all properties with fake account information
        }

    }
}