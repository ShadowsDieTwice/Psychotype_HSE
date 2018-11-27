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
            WallGetObject wall = Api.Get().Wall.Get(new VkNet.Model.RequestParams.WallGetParams { });
            foreach (Post post in wall.WallPosts)
            {
                if (post.Date.Value.Date <= timeTo && post.Date.Value.Date >= timeFrom)
                    curPosts.Add(post);
            }
            if (curPosts.Count != 0)
                return curPosts;
            //а что делать, если постов за это время не было?

            throw new NotImplementedException();
        }

        /// <summary>
        /// This method gets all most popular words on wall
        /// Except words in DataBase (like "по", "с" and etc)
        /// </summary>
        /// <returns> List of words </returns>
        public virtual List<string> GetMostPopularWordsOnWall(DateTime timeFrom, DateTime timeTo)
        {
            //нужно как-то ловить exception, если нет постов
            List<Post> posts = GetAllPosts(timeFrom, timeTo);
            Dictionary<string, int> popularWords = new Dictionary<string, int>();
            char[] separators = { ' ', '\n', '\t', ',', '.', '!', '?' };
            foreach (Post post in posts)
            {
                string[] words = post.Text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                string curWord;
                foreach (string word in words)
                {
                    if (word.Length > 2)
                    {
                        curWord = word.Substring(0, word.Length - 2); //так так, а как быть с окончанием?
                        if (popularWords.ContainsKey(curWord))
                            popularWords[curWord]++;
                        else
                            popularWords.Add(curWord, 0);
                    }
                }
            }
            List<string> Top10Words = new List<string>();
            foreach (var pair in popularWords.OrderBy(pair => pair.Value))
            {
                int i = 0;
                if (i < 10)
                {
                    i++;
                    Top10Words.Add(pair.Key);
                }
                else
                {
                    return Top10Words;
                }
            }
            //хм, что делать, если постов не было?
            throw new NotImplementedException();
        }
    }
}