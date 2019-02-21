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

            public PopularWordsAtributes() {}

            public PopularWordsAtributes(Components.User user, 
                DateTime timeFrom, DateTime timeTo, int numberOfWords)
            {
                List<List<String>> popularWords = user.GetMostPopularWordsOnWall(
                       timeFrom, timeTo, numberOfWords
                    );

                // для всех слов создаем пары <корень, формы>
                for (int i = 0; i < numberOfWords; i++)
                    if (i + 1 <= popularWords.Count)
                        response.Add(new Tuple<string, List<string>>(
                            Components.RussianStemmer.GetTheBase(popularWords[i][0]),
                            popularWords[i]));
            }
        }

        DateTime timeFrom = new DateTime(2006, 1, 1);
        DateTime timeTo = DateTime.Now;
        int numberOfWord = 10;

        public PopularWordsAtributes PopularWords
        {
            get
            {
                return new PopularWordsAtributes(User, timeFrom, timeTo, numberOfWord);
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

        public PageDataModel() : this("") {}

        public PageDataModel(String id) //: this(new Components.User(id))
        { Id = id; }

        public PageDataModel(Components.User user)
        {
            //User = user;
            //Id = user.VkId.ToString();
            //PopularWords = new PopularWordsAtributes(user, timeFrom, timeTo, numberOfWord);
        }
    }
}