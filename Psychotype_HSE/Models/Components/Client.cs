using System;
using System.Collections.Generic;
using System.Linq;
using VkNet.Model;
using VkNet.Enums.Filters;

namespace Psychotype.Models.Components
{
    /// <summary>
    /// Class of abstract client of vk
    /// Its may be user or community
    /// </summary>
    public abstract class Client
    {
        /// <summary>
        /// Link to the clients wall
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// Id of client in Vk base
        /// </summary>
        public ulong VkId { get; set; }

        /// <summary>
        /// This method gets VkId from Link to client
        /// </summary>
        /// <returns> Id of client </returns>
        protected ulong GetIdFromLink()
        {
            var api = Api.Get();
            var user = api.Users.Get(new long[] { (long)VkId }).FirstOrDefault();
            return (ulong)user.Id;
        }

        /// <summary>
        /// This methods gets all posts from clients wall
        /// </summary>
        /// <returns> WallObject instance </returns>
        public abstract WallGetObject GetAllPosts();

        /// <summary>
        /// This method gets all most popular words on wall
        /// Except words in DataBase (like "по", "с" and etc)
        /// </summary>
        /// <returns> List of words </returns>
        public abstract List<string> GetMostPopularWordsOnWall();
    }
}
