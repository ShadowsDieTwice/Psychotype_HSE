using System;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace Psychotype.Models
{
    /// <summary>
    /// Wrapper class for VkNet.VkApi class for single ton pattern
    /// </summary>
    public static class Api
    {
        /// <summary>
        /// Api of VK
        /// </summary>
        private static VkApi api;

        /// <summary>
        /// This method implements single ton pattern for VkNet.VkApi instance
        /// </summary>
        /// <returns> One existing VkNet.VkApi instance </returns>
        public static VkApi Get()
        {
            if (api != null)
                return api;

            api = new VkApi();
            api.Authorize(new ApiAuthParams
            {
                ApplicationId = AppSettings.ApplicationId,
                AccessToken = AppSettings.AccessToken,
                Settings = Settings.All
            });

            return api;
        }
    }
}