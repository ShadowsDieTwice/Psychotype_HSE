using System;
using System.Collections.Generic;
using System.Linq;
using VkNet.Model;
using VkNet.Enums.Filters;
using System.Text.RegularExpressions;
using Psychotype_HSE.Models.Components;
using System.Diagnostics;
using System.IO;

namespace Psychotype_HSE.Models.Components
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
            try
            {
                var obj = api.Utils.ResolveScreenName(Link);
                if (obj == null)
                    throw new NullReferenceException("There is no page with link: " + Link);
                return (long)obj.Id;
            }
            catch (ArgumentNullException)
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets all texts from client's posts
        /// </summary>
        /// <returns> List of texts </returns>
        public virtual List<string> GetTexts(DateTime timeFrom, DateTime timeTo)
        {
            List<Post> posts = GetAllPosts(timeFrom, timeTo);
            List<string> texts = new List<string>();    
            foreach (Post post in posts)
            {
                texts.Add(post.Text);
            }
            return texts;
        }

        /// <summary>
        /// Writes all texts to csv file with header "text"
        /// </summary>     
        public virtual void SaveTextsToCSV(DateTime timeFrom, DateTime timeTo, string filePath = null) //AppSettings.SuicidePredictCSV)
        {
            //TODO: add filePath default value to AppSetting properly?
            if (filePath == null)
                filePath = AppSettings.SuicidePredictCSV;

            List<string> texts = GetTexts(timeFrom, timeTo);
            string firstLine = "text";
            //Fully overwrites file 
            using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                sw.WriteLine(firstLine);
                foreach (var text in texts)
                    sw.WriteLine(text.Replace("\n"," "));
            }
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
        /// This method gets all most popular words (in groups) on wall
        /// Each group is like (cat, cats, catty)
        /// Except words in DataBase (like "по", "с" and etc)
        /// Works only with Russian words
        /// </summary>
        /// <returns> List of words </returns>
        public virtual List<List<string>> GetMostPopularWordsOnWall(DateTime timeFrom, DateTime timeTo, int numberOfWords = 10)
        {
            List<Post> posts = GetAllPosts(timeFrom, timeTo);
            Dictionary<string, List<string>> popularWords = new Dictionary<string, List<string>>();
            char[] separators = { ' ', '\n', '\t', ',', '.', '!', '?' };
            foreach (Post post in posts)
            {
                string[] words = post.Text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    if (word.Length > 2) //чтобы убрать всякие предлоги и тд
                    {
                        string key = RussianStemmer.GetTheBase(word);
	                    if (!popularWords.ContainsKey(key))
							popularWords.Add(key, new List<string>());

	                    popularWords[key].Add(word);
					}
                }
            }

            var popularKeys = popularWords.OrderByDescending(pair => pair.Value.Count).Select(pair => pair.Value);
            return popularKeys.Take(numberOfWords).ToList();
        }
    }
}