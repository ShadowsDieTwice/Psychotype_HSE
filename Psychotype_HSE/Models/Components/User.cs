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

        
    }
}