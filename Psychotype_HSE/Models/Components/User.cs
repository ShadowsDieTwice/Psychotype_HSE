using System;
using System.Collections.Generic;
using VkNet.Model;
using System.Text.RegularExpressions;
using System.Diagnostics;
using VkNet.Model.RequestParams;
using System.IO;
using VkNet.Enums.Filters;
using VkNet.Exception;

namespace Psychotype_HSE.Models.Components
{
    public class User : Client
    {
        public double[] UserNumericParams { get; set; }
        public User(string link)
        {
            Link = link;
            VkId = GetIdFromLink();
        }

        public double IsBot()
        {
            //Веса  - взяты из линейной модели. Последний элемент - свободный член.
            double[] weights = new double[] { 0.01287878811031696, -0.17811418652058975, 0.003960086137254121, -0.10330501486495451, -0.05201748044966795, 0.41885981578605175, 0.28384792498370426, -0.202368103165068, -0.025202852937152742, -0.018657750323834515, -0.08613977350139986, -0.1095679709182284, -0.22031956029207778, -0.22961615725364692, -0.026667569317484877, -0.00010232676830898997, -0.28941480382232576, 0.178485};    
            return LogRegression(weights);              
        }

        double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));            
        }

        /// <summary>
        /// Returns result of logistic regression prediction for concrete weights
        /// </summary>
        /// <param name="weights">Weights that are taken from models. Last element is free term.</param>
        /// <returns></returns>
        public double LogRegression(double[] weights, bool update = false)
        {
            if (UserNumericParams == null || update)
                       FillUserParams();
            double[] x = UserNumericParams;
            double res = 0;
            for (int i = 0; i < x.Length; ++i)
                res += x[i] * weights[i];
            //Добавление свободного члена
            res += weights[weights.Length - 1];
            return Sigmoid(res);
        }

        public double[] FillUserParams()
        {
            double[] x = new double[17];
            var api = Api.Get();
            VkNet.Model.User usr = api.Users.Get(new long[] { VkId }, ProfileFields.All)[0];
            WallGetObject wall;
            //Предпологаю, что проверка на закрытость страницы уже пройдена
            try
            {
                wall = api.Wall.Get(new VkNet.Model.RequestParams.WallGetParams
                {
                    OwnerId = usr.Id
                });

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
                //12:ownPosts
                x[12] = ownPostsCount;

                //13:reposts
                x[13] = repostsCount;

                int sum = 0;
                foreach (var post in posts)
                    sum += post.Views?.Count ?? 0;
                //14:views
                x[14] = sum;

                //16:own / repo
                //19 использовалось как макс. значение при обучении модели
                x[16] = repostsCount != 0 ? ownPostsCount / repostsCount : 19;
            }
            catch (Exception)
            {
                x[12] = 0;
                x[13] = 0;
                x[14] = 0;
                x[16] = 19;
            }
            //0:is_female
            x[0] = usr.Sex == VkNet.Enums.Sex.Female ? 1 : 0;
            //1:has_photo
            x[1] = usr.PhotoId != null ? 1 : 0;
            //15:social_networks
            //есть ли фб и тд
            x[15] = (usr.Connections.FacebookId != null) |
                     (usr.Connections.Instagram != null) |
                       (usr.Connections.Twitter != null) |
                       (usr.Connections.Skype != null) ? 1 : 0;
            //2:friends
            x[2] = usr.Counters.Friends ?? 0;
            //3:followers
            x[3] = usr.Counters.Followers ?? 0;
            //4:groups
            x[4] = usr.Counters.Groups ?? 0;
            //5:pages
            x[5] = usr.Counters.Pages ?? 0;
            //6:subscriptions
            x[6] = usr.Counters.Subscriptions ?? 0;
            //7:photos
            x[7] = usr.Counters.Photos ?? 0;
            //8:user_photo
            x[8] = usr.Counters.UserPhotos ?? 0;
            //9:audios
            x[9] = usr.Counters.Audios ?? 0;
            //10:videos
            x[10] = usr.Counters.Videos ?? 0;

            Regex linkMask = new Regex("id_?[0-9]+");
            //11:is_default_link
            x[11] = linkMask.IsMatch(usr.Domain) ? 1 : 0;

            UserNumericParams = x;
            return x;
        }

        public string GetMyerBriggsType()
        {
            return new string(new char[] { MB_IE(), MB_SN(), MB_TF(), MB_JP()});
        }

        public char MB_IE()
        {
            double[] weights = new double[] { -0.20760827, -0.07135797, -0.21874391, -0.40388973, -0.16355935,
       -0.27051003, -0.01950142, -0.18646525,  0.13481103,  0.11483201,
        0.1511013 ,  0.15015035,  0.03001608, -0.02808363,  0.04263623,
        0.06657751, -0.01784196, 0.10314735 };
            return LogRegression(weights) > 0.5 ? 'I' : 'E';
        }
        public char MB_SN()
        {
            //Веса  - взяты из линейной модели
            double[] weights = new double[] {-0.02468234,  0.15876401,  0.00579   ,  0.1070706 , -0.10083021,
        0.11801783, -0.01302666, -0.1998583 , -0.15007201,  0.15581001,
        0.04291784,  0.36379137, -0.12846177,  0.12524391, -0.01329884,
       -0.17497432, -0.15059215 };
            return LogRegression(weights) > 0.5 ? 'N' : 'S';
        }
        public char MB_TF()
        {
            //Веса  - взяты из линейной модели. Последний элемент - свободный член.
            double[] weights = new double[] { -0.12601739,  0.05884192, -0.19327059, -0.06606932, -0.17279054,
       -0.20514629, -0.09192736,  0.1508643 , -0.0260291 , -0.01591977,
       -0.09140993,  0.25117588, -0.27571903,  0.18540367,  0.05127317,
       -0.19280001, -0.04398638, 0.09505607 };
            return LogRegression(weights) > 0.5 ? 'T' : 'F';
        }
        public char MB_JP()
        {
            //Веса  - взяты из линейной модели. Последний элемент - свободный член
            // double[] weights = new double[] {  };
            double[] weights = new double[] { 0.17529813,  0.37443151,  0.16527531, -0.38677119,  0.12411282, -0.12357333, -0.16544805,  0.121947  ,  0.02655503,  0.17872141, 0.18261538,  0.13771823,  0.28387095,  0.0497508 , -0.07209034,  -0.0931934 , -0.04085194, 0.00224221 };
            return LogRegression(weights) > 0.5 ? 'J' : 'P';
        }

    }
}