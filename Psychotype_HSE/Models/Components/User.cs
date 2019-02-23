using System;
using System.Collections.Generic;
using VkNet.Model;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Psychotype_HSE.Models.Components
{
    public class User : Client
    {
        public User(string link)
        {
            Link = link;
            VkId = GetIdFromLink();
        }

        //public bool IsBot { get; set; }

        public double IsBot()
        {
            int counter = 0;
            var api = Api.Get();

            //check if the user changed his id-link
            Regex linkMask = new Regex("id_?[0-9]+");
            if (linkMask.IsMatch(Link))
                counter += 10;
            else
                counter -= 15;


            //check how many group are followed by the user
            if (api.Groups.Get(new VkNet.Model.RequestParams.GroupsGetParams() { UserId = VkId }, false).Count > 250)
                counter += 5;
            if (api.Groups.Get(new VkNet.Model.RequestParams.GroupsGetParams() { UserId = VkId }, false).Count > 500)
                counter += 10;
            if (api.Groups.Get(new VkNet.Model.RequestParams.GroupsGetParams() { UserId = VkId }, false).Count > 1000)
                counter += 10;

            //check the popularity of user's post 
            WallGetObject wall = Api.Get().Wall.Get(new VkNet.Model.RequestParams.WallGetParams
            {
                OwnerId = VkId
            });
            var posts = wall.WallPosts;
            int friends = api.Friends.Get(new VkNet.Model.RequestParams.FriendsGetParams
            {
                UserId = VkId
            }, false).Count;
            int sum = 0;
            foreach (var post in posts)
                sum += post.Views?.Count ?? 0;
            if (sum / posts.Count < friends / 4)
                counter += 15;
            if (sum / posts.Count < friends / 6)
                counter += 30;



            //check if the user has a few friends (fake page or bot)
            if (api.Friends.Get(new VkNet.Model.RequestParams.FriendsGetParams() { UserId = VkId }, false).Count < 15)
                counter += 50;

            double val = Math.Max(counter / (0.6 * 110), 0);
            return Math.Min(1, val);
        }

    }
}