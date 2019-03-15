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
using System.Threading;
using VkNet.Model.Attachments;
using VkNet.Exception;

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
            Thread.Sleep(300);
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
        public virtual double SuicideProbability(DateTime timeFrom, DateTime timeTo, string dir, string id)
            // string writeFile, string readFile)
        {
            //if (readFile == null)
            //    readFile = AppSettings.SuicideResult;
            //if (writeFile == null)
            //    writeFile = AppSettings.SuicidePredictCSV;
            string writeFile = dir + "\\" + id + ".csv";
            string readFile = dir + "\\" + id + ".txt";

            List<Post> posts = GetAllPosts(timeFrom, timeTo);
            if (!File.Exists(writeFile))
            {
                SaveTextsToCSV(timeFrom, timeTo, writeFile);// + ".temp");
            }
            //File.Delete(writeFile);
            //File.Move(writeFile + ".temp", writeFile);

            List<double> probs = new List<double>();
            
            // все виснет
            while (!File.Exists(readFile))
            {
                Thread.Sleep(100);
            }

            //File.Move(readFile + ".temp", readFile);

            // extra precautions for reading from file in use
            while (true)
            {
                try
                {
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
                        break;
                    }
                }
                catch (IOException)
                {
                    Thread.Sleep(100);
                }
            }



            try
            {
                File.Delete(readFile);
            }
            catch
            {

            }
            //if (probs.Count != posts.Count)
            //    throw new FormatException($"Posts count isn't equal to probabilities count: {posts.Count} != {probs.Count}");

            //Finding weighted arithmetic mean
            double result = 0,  
                normalizer = 0, //Sum of weights
                w;
            for(int i = 0; i < posts.Count; i++)
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

        
        /// <summary>
        /// Записывает данные о юзере в строку в .csv
        /// </summary>
        /// <param name="sw"></param>
        /// <param name="usr"></param>
        protected void WriteUser(StreamWriter sw, VkNet.Model.User usr)
        {        
            var api = Api.Get();
            //TODO - тут в конце System.FormatException
            usr = api.Users.Get(new long[] { usr.Id }, ProfileFields.All)[0];

            //Проверка - закрыта ли страница
            WallGetObject wall;
            try
            {

                wall = api.Wall.Get(new VkNet.Model.RequestParams.WallGetParams
                {
                    OwnerId = usr.Id
                });
            }
            catch (UserDeletedOrBannedException)
            {
                //если профиль приватный
                return;
            }
            catch (CannotBlacklistYourselfException)
            {
                //если профиль приватный
                return;
            }
            catch (InvalidCastException)
            {
                //???
                return;
            }
            catch (InvalidParameterException)
            {
                //???
                return;
            }


            //Для дебага
            swWrite(sw, usr.FirstName);
            swWrite(sw, usr.LastName);

            //м/ж
            swWrite(sw, (usr.Sex == VkNet.Enums.Sex.Female ? 1 : 0).ToString());

            //есть ава 
            //swWrite(sw, usr.HasPhoto);
            swWrite(sw, usr.PhotoId != null ? 1 : 0);
            //есть фб и тд
            swWrite(sw, (usr.Connections.FacebookId != null ? 1 : 0).ToString());
            swWrite(sw, (usr.Connections.Instagram != null ? 1 : 0).ToString());
            swWrite(sw, (usr.Connections.Twitter != null ? 1 : 0).ToString());
            swWrite(sw, (usr.Connections.Skype != null ? 1 : 0).ToString());

            // var counters = Api.Get().Account.GetCounters(CountersFilter.All);
            swWrite(sw, usr.Counters.Friends);
            swWrite(sw, usr.Counters.Followers);
            swWrite(sw, usr.Counters.Groups);
            swWrite(sw, usr.Counters.Pages);

            swWrite(sw, usr.Counters.Subscriptions);
            swWrite(sw, usr.Counters.Photos);
            swWrite(sw, usr.Counters.UserPhotos?.ToString() ?? null);
            swWrite(sw, usr.Counters.Audios?.ToString() ?? null);
            swWrite(sw, usr.Counters.Videos?.ToString() ?? null);
            //swWrite(sw, id.Counters.Albums.ToString());
            Regex linkMask = new Regex("id_?[0-9]+");
            swWrite(sw, linkMask.IsMatch(usr.Domain) ? 1 : 0);

            var id = usr.Id;              
            var posts = wall.WallPosts;
            int ownPostsCount = 0;
            int repostsCount = 0;
            foreach (var post in posts)
            {
                if (post.CopyHistory.Count > 0)
                    repostsCount++;
                else
                    ownPostsCount++;
            }
            swWrite(sw, ownPostsCount);
            swWrite(sw, repostsCount);

            int sum = 0;
            foreach (var post in posts)
                sum += post.Views?.Count ?? 0;
            swWrite(sw, sum.ToString());
        
        }
        void swWrite(StreamWriter sw, object str)
        {
            if (str == null)
                sw.Write("null");
            else
                sw.Write(str.ToString());
            sw.Write(";");
        }
    }
}
