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
        /// Link to the client's wall. Format: id123456, public123456, short_name etc.
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// Id of client in Vk base
        /// </summary>
        public long VkId { get; set; }

        /// <summary>
        /// This method gets VkId from Link to client
        /// </summary>
        /// <returns> Id of client </returns>
        protected long GetIdFromLink()
        {
            var api = Api.Get();
            //Возвращает по короткому имени объект(может быть пользователь, группа или приложение)
            var obj = api.Utils.ResolveScreenName(Link);
            if (obj == null)
                throw new NullReferenceException("There is no page with link: " + Link);
            return (long)obj.Id;
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
