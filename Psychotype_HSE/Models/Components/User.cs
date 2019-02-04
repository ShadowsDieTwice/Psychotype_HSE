using System;
using System.Collections.Generic;
using VkNet.Model;
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
    }
}