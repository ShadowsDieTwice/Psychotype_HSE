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
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using VkNet.Model.RequestParams;
using VkNet.Model.Attachments;

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
        /// This methods gets all posts from clients wall
        /// </summary>
        /// <returns> WallObject instance </returns>
        public virtual List<Post> GetAllPosts(DateTime timeFrom, DateTime timeTo)
        {
            List<Post> curPosts = new List<Post>();
            Thread.Sleep(300);
            WallGetParams wallParams = new WallGetParams();
            wallParams.OwnerId = VkId;
            try
            {
                WallGetObject wall = Api.Get().Wall.Get(wallParams, true);
                foreach (Post post in wall.WallPosts)
                {
                    var innerPosts = post.CopyHistory;
                    foreach (var innerPost in innerPosts)
                    {
                        if (innerPost.Date.Value.Date <= timeTo && post.Date.Value.Date >= timeFrom)
                            curPosts.Add(innerPost);
                    }
                    if (post.Date.Value.Date <= timeTo && post.Date.Value.Date >= timeFrom)
                        curPosts.Add(post);
                }
            }
            catch
            {
                // bug in library caused by generic casting
                // https://github.com/vknet/vk/pull/744
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
        public virtual double SuicideProbability(DateTime timeFrom, DateTime timeTo, string id)
        {
            List<Post> posts = GetAllPosts(timeFrom, timeTo);

            // подготавливаем сообщение
            List<string> texts = GetTexts(timeFrom, timeTo);
            String request = ""; 
            foreach (var text in texts)
                request += text.Replace("\n", " ") + "\n";
            
            // подключение
            IPEndPoint ipe = new IPEndPoint(AppSettings.LocalIP, AppSettings.ClientPort);
            Socket socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ipe);

            if (request.Length > 0)
            {
                // передаем в данные
                request = request.Remove(request.Length - 1);
                request = $" {request.Length + 1}::" + request;
                Byte[] bytesSent = Encoding.UTF32.GetBytes(request);
                Byte[] bytesReceived = new Byte[256];
                string responce = "";
                socket.Send(bytesSent, bytesSent.Length, 0);

                // получаем результат
                int bytes = socket.Receive(bytesReceived, bytesReceived.Length, 0);
                responce = Encoding.UTF8.GetString(bytesReceived, 0, bytes);
                List<double> probs = new List<double>();

                foreach (string prob in responce.Split('\n'))
                {
                    try
                    {
                        probs.Add(double.Parse(prob));
                    }
                    catch (FormatException)
                    {
                        //Because of commas in russian doubles
                        String str = prob.Replace(".", ",");
                        if (str != "") probs.Add(double.Parse(str));
                        //else probs.Add(0);
                    }
                }

                double result = 0,
                    normalizer = 0, //Sum of weights
                    w;
                for (int i = 0; i < probs.Count; i++)
                {
                    w = PostWeight(posts[i]);
                    result += probs[i] * w;
                    normalizer += w;
                }
                if (normalizer == 0)
                    return 0;

                return result / normalizer;
            }
            return 0;
        }

        /// <summary>
        /// Weight of the post that depends on its date
        /// </summary>
        public virtual double PostWeight(Post post)
        {
            //difference in days
            if (post.Text == "")
                return 0;
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
    }
}