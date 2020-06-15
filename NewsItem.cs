using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RssFeedService
{
    [Serializable]
    public class NewsItem
    {
        public string Title;
        public string Description;
        public string PublishedDate;
        public string NewsChannel;
    }
}
