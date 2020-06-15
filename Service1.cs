using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.ServiceModel.Syndication;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Xml;
using System.Timers;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace RssFeedService
{
    public partial class Service1 : ServiceBase
    {
        public Timer delay = new Timer();
        public Service1()
        {
            InitializeComponent();
            delay = new Timer(300000) { AutoReset = true};
            delay.Elapsed += ReadRss;
        }

        protected override void OnStart(string[] args)
        {
            delay.Start();
        }

        protected override void OnStop()
        {
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        

        protected void ReadRss(object sender, ElapsedEventArgs e)
        {
            Rss20FeedFormatter rssFormatter;
            XmlSerializer writer = new XmlSerializer(typeof(List<NewsItem>));
            //bool Firstrun = true;

            using (var xmlReader = XmlReader.Create
               ("https://www.thenews.com.pk/rss/1/1"))
            {
                rssFormatter = new Rss20FeedFormatter();
                rssFormatter.ReadFrom(xmlReader);

            }
            List<NewsItem> news = new List<NewsItem>();
            foreach (var syndicationItem in rssFormatter.Feed.Items)
            {
                NewsItem newsItem = new NewsItem();
                newsItem.Title =  syndicationItem.Title.Text;
                newsItem.PublishedDate =    syndicationItem.PublishDate.ToString();

                //avoid tags
                string description = syndicationItem.Summary.Text;
                description = Regex.Replace(description, @"<.+?>", "");
                newsItem.Description = WebUtility.HtmlDecode(description);

                newsItem.NewsChannel = rssFormatter.Feed.Title.Text;
                news.Add(newsItem);
            }

            FileStream file = new FileStream(@"C:\Users\Naqib\Documents\Visual Studio 2015\Thenews.xml", FileMode.Create, FileAccess.Write);
            writer.Serialize(file, news);
            file.Close();
        }

    }
}
