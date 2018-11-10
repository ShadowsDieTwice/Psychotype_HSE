using System;
using System.Collections.Generic;
using VkNet.Model;

namespace Psychotype.Models.Components
{
    public class User : Client
    {
        public User(string link)
        {
            Link = link;
            VkId = GetIdFromLink();
        }

        public override WallGetObject GetAllPosts()
        {
            // TODO: Implement method
            throw new NotImplementedException();
        }

        public override List<string> GetMostPopularWordsOnWall()
        {
            // TODO: Implement method
            throw new NotImplementedException();
        }
    }
}