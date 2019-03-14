using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using VkNet.Enums.Filters;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace Psychotype_HSE.Models.Components
{
    public class Community : Client
    {
        public Community(string link)
        {
            Link = link;
            VkId = GetIdFromLink();
        }

        public void getSubsForBots(string filePath = null)
        {
            if (filePath == null)
                filePath = @"D:\Documents\LARDocs\HSE\GroupDynamics\DataBases\bots.csv";
            var api = Api.Get();
        
            var users = api.Groups.GetMembers(new GroupsGetMembersParams() { GroupId = VkId.ToString(), Fields = UsersFields.All });
            //List<string> texts = GetTexts(timeFrom, timeTo);
            //Fully overwrites file 

            using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                sw.WriteLine("name;surname;sex;has_photo;fb;inst;twi;sky;friends;followers;groups;pages;subscriptions;photos;user_photo;audios;videos;is_default_link;ownPosts;reposts;views");
                       
                //sw.WriteLine("жжж");
                foreach (var usr in users)
                {
                    writeUser(sw, usr);                                     
                }
                //foreach (var text in texts)
                sw.WriteLine();
            }
            
          
            //using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            //{
            //    foreach (var text in texts)
            //        sw.WriteLine(text.Replace("\n", " "));
            //}
        }

        void writeUser(StreamWriter sw, VkNet.Model.User usr)
        {
            //Api.Get().Users.Get(new List<long> { usr.Id } )[0].can
            var api = Api.Get();
            usr = api.Users.Get(new long[] { usr.Id }, ProfileFields.All)[0];
           // usr = Api.Get().Users.Get(new List<long> { usr.Id })[0];

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
            //catch (InvalidCastException)
            //{
            //    //???
            //    return;
            //}
            //catch (InvalidParameterException)
            //{
            //    //???
            //    return;
            //}


            //Для дебага
            swWrite(sw, usr.FirstName);
            swWrite(sw, usr.LastName);

            //м/ж
            swWrite(sw, (usr.Sex ==  VkNet.Enums.Sex.Female ? 1 : 0).ToString());

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

            //id
            Regex linkMask = new Regex("id_?[0-9]+");
            swWrite(sw, linkMask.IsMatch(usr.Domain) ? 1 : 0);

            var id = usr.Id;
            //WallGetObject wall = Api.Get().Wall.Get(new VkNet.Model.RequestParams.WallGetParams
            //{
            //    OwnerId = id
            //});

            //int friends = Api.Get().Friends.Get(new VkNet.Model.RequestParams.FriendsGetParams
            //{
            //    UserId = VkId
            //}).Count;          
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
            sw.WriteLine();
            //swWrite(sw, usr.Counters.Videos.ToString());
            //swWrite(sw, id.Counters..ToString());
            //api.Groups.Get(new VkNet.Model.RequestParams.GroupsGetParams() { UserId = id.ge }, false).Count
            //sw.Write(id.FriendLists.Count.ToString());
            //sw.Write(";");
            //id.FriendLists.Count;
        }
        void swWrite(StreamWriter sw, object str)
        {
            if(str == null)           
                sw.Write("null");
            else
               sw.Write(str.ToString());
            sw.Write(";");
        }
    }
}