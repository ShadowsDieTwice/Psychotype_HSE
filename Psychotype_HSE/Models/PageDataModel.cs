using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Psychotype_HSE.Models
{
    public class PageDataModel
    {
        public class PopularWordsAtributes
        {
            // префикс для получения ссылок на словарь
            public static string dictLink = "https://ru.wiktionary.org/wiki/";
            // список пар <корень, формы>
            public List<Tuple<string, List<String>>> response = new List<Tuple<string, List<string>>>();

            public PopularWordsAtributes() { }

            public PopularWordsAtributes(Components.User user,
                DateTime timeFrom, DateTime timeTo, int numberOfWords)
            {
                List<List<String>> popularWords = user.GetMostPopularWordsOnWall(
                       timeFrom, timeTo, numberOfWords
                    );

                // для всех слов создаем пары <корень, формы>
                /*
                for (int i = 0; i < numberOfWords; i++)
                    if (i + 1 <= popularWords.Count)
                        response.Add(new Tuple<string, List<string>>(
                            Components.RussianStemmer.GetTheBase(popularWords[i][0]),
                            popularWords[i]));
                */
                for (int i = 0; i < numberOfWords; i++)
                    if (i + 1 <= popularWords.Count)
                    {
                        HashSet<string> set = new HashSet<string>();
                        foreach (string s in popularWords[i])
                        {
                            set.Add(s);
                        }
                        List<String> list = set.ToList();
                        String leading = list[0];
                        list.RemoveAt(0);
                        response.Add(new Tuple<string, List<string>>(
                            leading,
                            list));
                    }
            }
        }

        public double BotProbability
        {
            get
            {
                try
                {
                    if (isLinkValid)
                        return this.User.IsBot();
                }
                catch (Exception)
                {
                    isLinkValid = false;
                    Id = "";
                }
                
                return 0;
            }
        }
        DateTime timeFrom = new DateTime(2006, 1, 1);
        DateTime timeTo = DateTime.Now;
        int numberOfWord = 10;

        /// <summary>
        /// Must be calles after PopularWords, or an exception will be thrown! 
        /// </summary>
        public bool hasWords { set; get; }

        public PopularWordsAtributes PopularWords
        {
            get
            {
                PopularWordsAtributes popularWords;
                if (isLinkValid)
                    popularWords = new PopularWordsAtributes(User, timeFrom, timeTo, numberOfWord);
                else
                    popularWords =  new PopularWordsAtributes();
                
                hasWords = !(popularWords.response.Count == 0);

                return popularWords;
            }
        }

        public String Id { get; set; }
        public Components.User User
        {
            get
            {
                return new Components.User(Id);
            }
        }

        public PageDataModel() : this("") { }

        public bool isLinkValid { set; get; }

        public PageDataModel(String init_id) //: this(new Components.User(id))
        {
            string[] splitedId;
            string id;

            isLinkValid = false;

            if (init_id != null)
            {
                splitedId = init_id.Split('/');
                id = splitedId[splitedId.Length - 1];
            }
            else id = "";
            
            try
            {
                if (id == "")
                    throw new Exception("Void input");
                var test = (new Psychotype_HSE.Models.Components.User(id));
                //model = new PageDataModel(id);
                Id = id;
                isLinkValid = true;
            }
            catch (Exception)
            {
                Id = "";
            }
            
        }

        public PageDataModel(Components.User user)
        {
            //User = user;
            //Id = user.VkId.ToString();
            //PopularWords = new PopularWordsAtributes(user, timeFrom, timeTo, numberOfWord);
        }
    }
}