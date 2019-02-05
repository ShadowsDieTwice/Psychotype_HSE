using System;
using System.Collections.Generic;
using VkNet.Model;

namespace Psychotype_HSE.Models.Components
{
    public class Community : Client
    {
        public Community(string link)
        {
            Link = link;
            VkId = GetIdFromLink();
        }
    }
}