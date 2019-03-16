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
        public User(string link)
        {
            Link = link;
            VkId = GetIdFromLink();
        }

        public double IsBot()
        {
            //Веса          
            double[] weights = new double[] { 0.01287878811031696, -0.17811418652058975, 0.003960086137254121, -0.10330501486495451, -0.05201748044966795, 0.41885981578605175, 0.28384792498370426, -0.202368103165068, -0.025202852937152742, -0.018657750323834515, -0.08613977350139986, -0.1095679709182284, -0.22031956029207778, -0.22961615725364692, -0.026667569317484877, -0.00010232676830898997, -0.28941480382232576 };
            //Свободный член
            double freePol = 0.178485;
            double[] x = new double[weights.Length];
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
                x[12] = ownPostsCount;
                x[13] = repostsCount;

                int sum = 0;
                foreach (var post in posts)
                    sum += post.Views?.Count ?? 0;
                x[14] = sum;

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

            

            //is_female;has_photo;fb;inst;twi;sky;friends;followers;groups;pages;subscriptions;photos;user_photo;audios;videos;is_default_link;ownPosts;reposts;views
            //0:is_female
            //1:has_photo
            //2:friends
            //3:followers
            //4:groups
            //5:pages
            //6:subscriptions
            //7:photos
            //8:user_photo
            //9:audios
            //10:videos
            //11:is_default_link
            //12:ownPosts
            //13:reposts
            //14:views
            //15:social_networks
            //16:own / repo

            //м/ж
            x[0] = usr.Sex == VkNet.Enums.Sex.Female ? 1 : 0;
            x[1] = usr.PhotoId != null ? 1 : 0;
            //есть фб и тд
            x[15] = usr.Connections.FacebookId != null ? 1 : 0 + usr.Connections.Instagram != null ? 1 : 0 + usr.Connections.Twitter != null ? 1 : 0 + usr.Connections.Skype != null ? 1 : 0;
                    
            // var counters = Api.Get().Account.GetCounters(CountersFilter.All);
            x[2] = usr.Counters.Friends ?? 0;
            x[3] = usr.Counters.Followers ?? 0;
            x[4] = usr.Counters.Groups ?? 0;
            x[5] = usr.Counters.Pages ?? 0;
            x[6] = usr.Counters.Subscriptions ?? 0;
            x[7] = usr.Counters.Photos ?? 0;
            x[8] = usr.Counters.UserPhotos ?? 0;
            x[9] = usr.Counters.Audios ?? 0;
            x[10] = usr.Counters.Videos ?? 0;
          
            Regex linkMask = new Regex("id_?[0-9]+");
            x[11] = linkMask.IsMatch(usr.Domain) ? 1 : 0;

           

            double res = 0;
            for (int i = 0; i < x.Length; ++i)
                res += x[i] * weights[i];
            res += freePol;
            return Sigmoid(res);
        }

        double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));            
        }
    }
}