using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Psychotype_HSE.Models
{
    public class PageDataModel
    {
        public class PopularWordsAttributes
        {
            // префикс для получения ссылок на словарь
            public static string dictLink = "https://wiktionary.org/wiki/";
            // список пар <корень, формы>
            public List<Tuple<string, List<String>>> response = new List<Tuple<string, List<string>>>();

            public List<int> count = new List<int>();

            public PopularWordsAttributes() { }

            public PopularWordsAttributes(Components.User user, DateTime timeFrom,
                                                DateTime timeTo, int numberOfWords)
            {
                List<List<String>> popularWords =
                    user.GetMostPopularWordsOnWall(timeFrom, timeTo, numberOfWords);

                // для всех слов создаем пары <слово, формы>
                for (int i = 0; i < numberOfWords; i++)
                    if (i + 1 <= popularWords.Count)
                    {
                        count.Add(popularWords[i].Count);

                        HashSet<string> set = new HashSet<string>();
                        foreach (string s in popularWords[i])
                        {
                            set.Add(s.ToLower());
                        }
                        List<String> list = set.ToList();
                        String leading = list[0];
                        list.RemoveAt(0);
                        response.Add(new Tuple<string, List<string>>(leading, list));
                    }
            }
        }

        DateTime timeFrom = new DateTime(2006, 1, 1);
        DateTime timeTo = DateTime.Now;
        int numberOfWord = 10;

        private String id = "";
        private double botProbability = 0;
        private double suicideProbability = 0;
        private bool hasWords = false;
        private PopularWordsAttributes popularWords = new PopularWordsAttributes();
        private string fullName = "Имя Фамилия";
        private string photoURL = "https://vk.com/images/camera_200.png?ava=1";
        private List<string> description = new List<string>();
        private string mayersBriggs = "";

        public string MayersBriggs => mayersBriggs;

        public string FullName => fullName;

        public string PhotoURL => photoURL;

        public List<string> Description => description;

        public double BotProbability => botProbability;

        public double SuicideProbability => suicideProbability;

        public bool HasWords => hasWords;

        public PopularWordsAttributes PopularWords => popularWords;

        public bool isLinkValid { set; get; }

        public String Id
        {
            get { return id; }
            set
            {
	            var api = Api.Get();
                VkNet.Model.User vkUser = new VkNet.Model.User();

                isLinkValid = false;

                if (value != null)
                {
	                var splitedId = value.Split('/');
	                id = splitedId[splitedId.Length - 1];
                }
                else id = "";

                // If something wrong inside this try block
                // further conditions won't be satisfied.
                try
                {
                    if (id == "")
                        throw new Exception("Void input");
                    user = new Components.User(id);
                    isLinkValid = true;
                    // Get profile description
                    vkUser =
                     api.Users.Get(new string[] { id }, VkNet.Enums.Filters.ProfileFields.All).First();
                    botProbability = user.IsBot();
                }
                catch (Exception)
                {
                    id = "";
                    isLinkValid = false;
                    botProbability = 0;
                }
                
                // From this poin we can be sure if link is valid.
                if (isLinkValid)
                {
                    string s = "";
                    int i = 0;

                    // Get most frequent words
                    popularWords = new PopularWordsAttributes(user, timeFrom, timeTo, numberOfWord);

                    // Predict suicide probability
                    suicideProbability = user.SuicideProbability(timeFrom, timeTo, id);

                    // Predict Mayers-Briggs test result
                    mayersBriggs = user.GetMyerBriggsType();


                    try
                    {
                        fullName = vkUser.FirstName + " " + vkUser.LastName;
                        if (fullName == "")
                            throw new Exception();
                    }
                    catch (Exception)
                    {
                        fullName = "ФИО недоступно";
                    }

                    photoURL = vkUser.Photo50.AbsoluteUri;

                    s = vkUser.Status;
                    if (s != null & s != "")
                        description.Add("статус: " + s);
                    s = "";

                    s = "пол: ";
                    i = (int)vkUser.Sex;
                    switch (i)
                    {
                        case 0:
                            s += "не указан";
                            break;

                        case 1:
                            s += "женский";
                            break;

                        case 2:
                            s += "мужской";
                            break;
                    }
                    description.Add(s);

                    s = "";

                    if (vkUser.Country != null)
                        description.Add("страна: " + vkUser.Country.Title);
                    s = "";

                    if (vkUser.City != null)
                        description.Add("город: " + vkUser.City.Title);
                    s = "";

                    s = vkUser.MobilePhone;
                    if (s != null & s != "")
                        description.Add("моб. тел.: " + s);
                    s = "";

                    s = vkUser.HomePhone;
                    if (s != null & s != "")
                        description.Add("дом. тел.: " + s);

                    s = "отношения: ";
                    if (vkUser.Relation != null)
                    {
                        i = (int)vkUser.Relation;
                        switch (i)
                        {
                            case (1):
                                description.Add(s + "не женат/ замужем");
                                break;
                            case (2):
                                description.Add(s + "есть друг/ подруга");
                                break;
                            case (3):
                                description.Add(s + "помолвлен(а)");
                                break;
                            case (4):
                                description.Add(s + "женат(а)");
                                break;
                            case (5):
                                description.Add(s + "всё сложно");
                                break;
                            case (6):
                                description.Add(s + "в активном поиске");
                                break;
                            case (7):
                                description.Add(s + "влюблен(а)");
                                break;
                            case (8):
                                description.Add(s + "в гражданском браке");
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    popularWords = new PopularWordsAttributes();
                    suicideProbability = 0;
                }

                hasWords = popularWords.response.Count != 0;
            }
        }

        Components.User user = new Components.User("");
        public PageDataModel() : this("") { }
        public PageDataModel(string rawId)
        {
            this.Id = rawId;
        }
    }
}