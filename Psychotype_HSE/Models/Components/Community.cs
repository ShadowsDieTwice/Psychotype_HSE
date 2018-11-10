using System;
using System.Collections.Generic;
using VkNet.Model;

namespace Psychotype.Models.Components
{
    public class Community : Client
    {
        public Community(string link)
        {
            Link = link;
            VkId = GetIdFromLink();
        }

        public override WallGetObject GetAllPosts()
        {
            // TODO: Implement method

            // Дело сделано со смыслом 
            throw new NotImplementedException();
        }

        public override List<string> GetMostPopularWordsOnWall()
        {
            // TODO: Implement method
            throw new NotImplementedException();
        }
    }
}