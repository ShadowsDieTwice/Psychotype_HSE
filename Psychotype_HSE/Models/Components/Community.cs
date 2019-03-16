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
    }
}