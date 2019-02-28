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

                // для всех слов создаем пары <слово, формы>
                for (int i = 0; i < numberOfWords; i++)
                    if (i + 1 <= popularWords.Count)
                    {
                        HashSet<string> set = new HashSet<string>();
                        foreach (string s in popularWords[i])
                        {
                            set.Add(s.ToLower());
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

        static DateTime timeFrom = new DateTime(2006, 1, 1);
        static DateTime timeTo = DateTime.Now;
        static int numberOfWord = 10;


        private static String id = "";
        private static double botProbability = 0;
        private static bool hasWords = false;
        private static PopularWordsAtributes popularWords = new PopularWordsAtributes();

        static public double BotProbability
        {
            get
            {
                return botProbability;
            }
        }

        static public bool HasWords
        {
            get { return hasWords; }
        }

        static public PopularWordsAtributes PopularWords
        {
            get
            {
                return popularWords;
            }
        }

        public String Id
        {
            get
            {
                return id;
            }
            set
            {
                string[] splitedId;

                isLinkValid = false;

                if (value != null)
                {
                    splitedId = value.Split('/');
                    id = splitedId[splitedId.Length - 1];
                }
                else id = "";

                try
                {
                    if (id == "")
                        throw new Exception("Void input");
                    var test = (new Psychotype_HSE.Models.Components.User(id));
                    isLinkValid = true;
                }
                catch (Exception)
                {
                    id = "";
                }

                if (isLinkValid)
                {
                    user = new Components.User(Id);
                }

                if (isLinkValid)
                {
                    try
                    {
                        if (isLinkValid)
                            botProbability = User.IsBot();
                        else botProbability = 0;
                    }
                    catch (Exception)
                    {
                        isLinkValid = false;
                        id = "";
                        botProbability = 0;
                    }
                }

                if (isLinkValid)
                {
                    if (isLinkValid)
                        popularWords = new PopularWordsAtributes(User, timeFrom, timeTo, numberOfWord);
                    else
                        popularWords = new PopularWordsAtributes();

                    hasWords = !(popularWords.response.Count == 0);
                }
            }
        }

        static Components.User user = new Components.User("");

        static public Components.User User
        {
            get
            {
                return user;
            }
        }


        static public bool isLinkValid { set; get; }
    }
}