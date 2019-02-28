using System;
using System.Collections.Generic;
using System.Linq;
using VkNet.Model;
using VkNet.Enums.Filters;
using System.Text.RegularExpressions;
using Psychotype_HSE.Models.Components;
using Psychotype_HSE.Util;
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
        private string link;
        /// <summary>
        /// Link to the client's wall. Format: id123456, public123456, short_name etc.
        /// </summary>
        public string Link
        {
            get { return link; }
            set { link = LinkToShortName(value); }
        }

        /// <summary>
        /// Id of client in Vk base
        /// </summary>
        public long VkId { get; set; }

        /// <summary>
        /// Takes the last part of URL i.e short name of the page
        /// </summary>
        /// <param name="link"> URL of the page </param>
        /// <returns> Client's short name </returns>
        public static string LinkToShortName(string link)
        {           
            return new Regex(@"[^/]+(?=/$|$)").Match(link).Value;
        }

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
            if (filePath == null)
                filePath = AppSettings.UserPosts;

            List<string> texts = GetTexts(timeFrom, timeTo);
            //Fully overwrites file 
            using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
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
        /// Сalculates resulting suicide probability 
        /// </summary>
        /// <param name="timeFrom"></param>
        /// <param name="timeTo"></param>
        /// <param name="writeFile"> Path to SuicidePredictCSV </param>
        /// <param name="readFile"> Path to SuicideResult </param>
        /// <returns></returns>
        public virtual double SuicideProbability(DateTime timeFrom, DateTime timeTo, string writeFile, string readFile)
        {
            //if (readFile == null)
            //    readFile = AppSettings.SuicideResult;
            //if (writeFile == null)
            //    writeFile = AppSettings.SuicidePredictCSV;
        
            List<Post> posts = GetAllPosts(timeFrom, timeTo);
            SaveTextsToCSV(timeFrom, timeTo, writeFile + ".temp");
            File.Delete(writeFile);
            File.Move(writeFile + ".temp", writeFile);

            List<double> probs = new List<double>();
            while (!File.Exists(readFile + ".temp")) {}

            File.Move(readFile + ".temp", readFile);
            using (StreamReader sr = new StreamReader(readFile, System.Text.Encoding.UTF8))
            {
                string str;
                while ((str = sr.ReadLine()) != null)
                {
                    try
                    {
                        probs.Add(double.Parse(str));
                    }
                    catch (FormatException)
                    {
                        //Because of commas in russian doubles
                        str = str.Replace(".", ",");
                        probs.Add(double.Parse(str));
                    }

                }
            }
            File.Delete(readFile);
            if (probs.Count != posts.Count)
                throw new FormatException($"Posts count isn't equal to probabilities count: {posts.Count} != {probs.Count}");

            //Finding weighted arithmetic mean
            double result = 0,  
                normalizer = 0, //Sum of weights
                w;
            for(int i = 0; i < probs.Count; i++)
            {
                w = PostWeight(posts[i]);
                result += probs[i] * w;
                normalizer += w;
            }

            return result / normalizer;
        }

        /// <summary>
        /// Weight of the post that depends on its date
        /// </summary>
        public virtual double PostWeight(Post post)
        {
            //difference in days
            TimeSpan span = DateTime.Now - post.Date.Value;
            return 1 / Math.Log(span.TotalDays);
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
                    //New vvariable so we can change (remove all non-latters) it
                    string tempWord = word; 
                    char[] arr = tempWord.ToCharArray();                  
                    //Leaves only letters in the word
                    tempWord = new string(Array.FindAll<char>(arr, (c => char.IsLetter(c))));                    

                    if (tempWord.Length > 2) //чтобы убрать всякие предлоги и тд
                    {
                        string key = RussianStemmer.GetTheBase(tempWord);
	                    if (!popularWords.ContainsKey(key))
							popularWords.Add(key, new List<string>());

	                    popularWords[key].Add(tempWord);
					}
                }
            }

            var popularKeys = popularWords.OrderByDescending(pair => pair.Value.Count).Select(pair => pair.Value);
            return popularKeys.Take(numberOfWords).ToList();
        }
    }
}