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
        public virtual List<Post> GetAllPosts(DateTime timeFrom, DateTime timeTo)
        {
            List<Post> curPosts = new List<Post>();
            WallGetObject wall = Api.Get().Wall.Get(new VkNet.Model.RequestParams.WallGetParams
            {
                OwnerId = VkId
            });

            foreach (Post post in wall.WallPosts)
            {
                if (post.Date.Value.Date <= timeTo && post.Date.Value.Date >= timeFrom)
                    curPosts.Add(post);
            }

            return curPosts;
        }

        /// <summary>
        /// This method gets all most popular words on wall
        /// Except words in DataBase (like "по", "с" and etc)
        /// </summary>
        /// <returns> List of words </returns>
        public virtual List<string> GetMostPopularWordsOnWall(DateTime timeFrom, DateTime timeTo, int numberOfWords = 10)
        {
            List<Post> posts = GetAllPosts(timeFrom, timeTo);
            Dictionary<string, int> popularWords = new Dictionary<string, int>();
            char[] separators = { ' ', '\n', '\t', ',', '.', '!', '?' };
            foreach (Post post in posts)
            {
                string[] words = post.Text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                // TODO: Develop a method how to count similar words as one
                // TODO: Remove useless words from our set
                foreach (string word in words)
                {
                    if (word.Length > 2)
                    {
                        if (popularWords.ContainsKey(word))
                            popularWords[word]++;
                        else
                            popularWords.Add(word, 0);
                    }
                }
            }

            var popularKeys = popularWords.OrderByDescending(pair => pair.Value).Select(pair => pair.Key);
            return popularKeys.Take(numberOfWords).ToList();
        }
    }
}