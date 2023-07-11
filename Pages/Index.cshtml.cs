using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace task_3.Pages
{
    public class IndexModel : PageModel
    {
        public List<RssItem> RssItems { get; set; }

        public async Task OnGetAsync()
        {
            try
            {
                string rssUrl = "http://scripting.com/rss.xml";

                HttpClient httpClient = new HttpClient();
                string xmlContent = await httpClient.GetStringAsync(rssUrl);

                RssItems = ParseRssItems(xmlContent);
            }
            catch (Exception ex)
            {
                // Handle exception or logging
                RssItems = null;
            }
        }

        private List<RssItem> ParseRssItems(string xmlContent)
        {
            List<RssItem> items = new List<RssItem>();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlContent);

            XmlNodeList itemNodes = xmlDoc.SelectNodes("//item");

            foreach (XmlNode itemNode in itemNodes)
            {
                RssItem rssItem = new RssItem();
                rssItem.Title = itemNode.SelectSingleNode("title")?.InnerText;
                rssItem.Description = itemNode.SelectSingleNode("description")?.InnerText;
                rssItem.PubDate = DateTime.Parse(itemNode.SelectSingleNode("pubDate")?.InnerText);
                rssItem.Link = itemNode.SelectSingleNode("link")?.InnerText;
                rssItem.Guid = itemNode.SelectSingleNode("guid")?.InnerText;

                items.Add(rssItem);
            }

            return items;
        }
    }

    public class RssItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PubDate { get; set; }
        public string Link { get; set; }
        public string Guid { get; set; }
    }
}
