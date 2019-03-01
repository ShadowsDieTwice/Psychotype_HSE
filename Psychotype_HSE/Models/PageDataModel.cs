using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

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

            public PopularWordsAtributes(Components.User user, DateTime timeFrom,
                                                DateTime timeTo, int numberOfWords)
            {
                List<List<String>> popularWords = 
                    user.GetMostPopularWordsOnWall(timeFrom, timeTo, numberOfWords);

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
                        response.Add(new Tuple<string, List<string>>(leading,list));
                    }
            }
        }

        static DateTime timeFrom = new DateTime(2006, 1, 1);
        static DateTime timeTo = DateTime.Now;
        static int numberOfWord = 10;


        private static String id = "";
        private static double botProbability = 0;
        private static double suicideProbability = 0;
        private static bool hasWords = false;
        private static PopularWordsAtributes popularWords = new PopularWordsAtributes();
        private static string fullName = "Имя Фамилия";
        private static string photoURL = "https://vk.com/images/camera_200.png?ava=1";
        private static List<string> description = new List<string>();

        static public string FullName
        {
            get { return fullName;  }
        }

        static public string PhotoURL
        {
            get { return photoURL; }
        }

        static public List<string> Description
        {
            get { return description; }
        }

        static public double BotProbability
        {
            get { return botProbability; }
        }

        static public double SuicideProbability
        {
            get { return suicideProbability; }
        }

        static public bool HasWords
        {
            get { return hasWords; }
        }

        static public PopularWordsAtributes PopularWords
        {
            get { return popularWords; }
        }

        static public bool isLinkValid { set; get; }

        public String Id
        {
            get { return id; }
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

                // If something wrong inside this try block
                // further conditions won't be satisfied.
                try
                {
                    if (id == "")
                        throw new Exception("Void input");
                    user = new Components.User(id);
                    isLinkValid = true;
                }
                catch (Exception)
                {
                    id = "";
                    isLinkValid = false;
                }

                if (isLinkValid)
                {
                    // Link might be valid, meanwhile profile is private.
                    // We'll treat those as invalid links.
                    try
                    {
                        if (isLinkValid)
                            botProbability = user.IsBot();
                        else botProbability = 0;
                    }
                    catch (Exception)
                    {
                        isLinkValid = false;
                        id = "";
                        botProbability = 0;
                    }
                }

                // From this poin we can be sure if link is valid.
                if (isLinkValid)
                {
                    //string curDir = "C:\\Users\\1\\Source\\Repos\\myrachins\\Psychotype_HSE_v2\\Psychotype_HSE\\Files\\";// Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "/Files/"; //.GetCurrentDirectory();
                    string fileData = AppSettings.WorkingDir + id + ".csv";
                    string fileRes = AppSettings.WorkingDir + id + ".txt";
                    
                    //"C:\Users\1\Source\Repos\myrachins\Psychotype_HSE_v2\Psychotype_HSE\Files\"

                    // вызывается отдельно
                    //Util.PythonRunner.RunScript("C:\\Users\\1\\Source\\Repos\\myrachins\\Psychotype_HSE_v2\\Psychotype_HSE\\Util\\Scripts\\suicideScript.py",
                    //    AppSettings.PythonPath, curDir);//, fileData);

                    popularWords = new PopularWordsAtributes(user, timeFrom, timeTo, numberOfWord);

                    suicideProbability = user.SuicideProbability(timeFrom, timeTo, AppSettings.WorkingDir, id );

                    VkNet.Model.User vkUser = 
                     Api.Get().Users.Get(new string[] { id }).First();

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

                    try
                    {
                        photoURL = vkUser.PhotoMax.AbsoluteUri;//Photo200.AbsoluteUri;
                        if (photoURL == "")
                            throw new Exception();
                    }
                    catch (Exception)
                    {
                        photoURL = "https://vk.com/images/camera_200.png?ava=1";
                    }

                    string s = "";
                    int i = 0;

                    try
                    {
                        s = vkUser.Status;
                        if (s != null & s != "")
                            description.Add("статус: " + s);
                    }
                    catch (Exception) { }

                    try
                    {
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
                    }
                    catch (Exception) { }

                    try
                    {
                        s = vkUser.Country.Title;
                        if (s != null & s != "")
                            description.Add("страна: " + s);
                    }
                    catch (Exception) { }

                    try
                    {
                        s = vkUser.City.Title;
                        if (s != null & s != "")
                            description.Add("город: " + s);
                    }
                    catch (Exception) { }

                    try
                    {
                        s = vkUser.MobilePhone;
                        if (s != null & s != "")
                            description.Add("моб. тел.: " + s);
                    }
                    catch (Exception) { }


                }
                else
                {
                    popularWords = new PopularWordsAtributes();
                    suicideProbability = 0;
                }

                hasWords = !(popularWords.response.Count == 0);
            }
        }

        static Components.User user = new Components.User("");

        //static public Components.User User { get { return user; }  }
        
    }
}