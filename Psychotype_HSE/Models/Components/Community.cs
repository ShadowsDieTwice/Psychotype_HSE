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

        /// <summary>     
        /// Writes a sample of subscribers for using it in model for bots
        /// </summary>
        /// <param name="filePath">Path to .csv file</param>
        public void GetSubsForBots(string filePath = null)
        {
            if (filePath == null)
                filePath = @"D:\Documents\LARDocs\HSE\GroupDynamics\DataBases\bots.csv";
            var api = Api.Get();

            var users = api.Groups.GetMembers(new GroupsGetMembersParams() { GroupId = VkId.ToString(), Fields = UsersFields.All });
            //List<string> texts = GetTexts(timeFrom, timeTo);
            //Fully overwrites file 

            using (StreamWriter sw = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
            {
                sw.WriteLine("name;surname;is_female;has_photo;fb;inst;twi;sky;friends;followers;groups;pages;subscriptions;photos;user_photo;audios;videos;is_default_link;ownPosts;reposts;views;");

                //sw.WriteLine("жжж");
                foreach (var usr in users)
                {
                    WriteUser(sw, usr);
                }
                //foreach (var text in texts)
                sw.WriteLine();
            }
        }

    }
}