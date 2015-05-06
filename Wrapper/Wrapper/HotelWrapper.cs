using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using NSoup.Common;
using NSoup.Nodes;
using NSoup.Select;

namespace Wrapper
{
    public class HotelWrapper
    {
        private bool isBlocked = false;
        public void Execute()
        {
            int i = 0;
            try
            {
                for (i = 1; i < 1000000; i++)
                {
                    WebClient webClient = new WebClient();
                    WebRequest webRequest = WebRequest.Create(string.Format("http://ihotel.elong.com/{0}/", 304101));
                    Document htmlDoc = NSoup.NSoupClient.Parse(webRequest.GetResponse().GetResponseStream(), "utf-8");

                    string hotelName = GetHotelName(htmlDoc);
                    if (isBlocked)
                    {
                        LogHelper.WriteLog(typeof(HotelWrapper), string.Format("Stops at {0}", i));
                        break;
                    }
                    else
                    {
                        string hotelAdd = GetHotelAddress(htmlDoc);
                        string hotelDescription = GetHotelDescription(htmlDoc);
                        List<string> hotelComments = GetHotelComments(htmlDoc);

                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(HotelWrapper), ex, string.Format("Execute stoped at {0}.", i));
            }

        }

        #region nsoup back up methods
        //public void Test1(string url)
        //{
        //    NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(url);
        //}
        //public void Test3(string url)
        //{
        //    WebRequest webRequest = WebRequest.Create(url);
        //    NSoup.Nodes.Document doc = NSoup.NSoupClient.Parse(webRequest.GetResponse().GetResponseStream(), "utf-8");
        //}
        #endregion

        public string GetHotelName(Document htmlDoc)
        {
            string fullTitle = htmlDoc.Title;

            string hotelName = string.Empty;

            if (fullTitle.IndexOf("【") < 0)
                isBlocked = true;
            else
                hotelName = fullTitle.Replace(fullTitle.Substring(fullTitle.IndexOf("预订】")), "").Replace("【", "");

            return hotelName;
        }

        public string GetHotelAddress(Document htmlDoc)
        {
            string address = string.Empty;
            Elements addElments = htmlDoc.GetElementsByClass("addr");
            if (addElments != null && addElments.Count > 0)
                address = addElments[0].Text();
            return address;
        }

        public string GetHotelDescription(Document htmlDoc)
        {
            StringBuilder sbDescription = new StringBuilder();
            Element descrptionElement = htmlDoc.GetElementById("hotelSummaryMore");
            if (descrptionElement != null)
            {
                string htmlString = descrptionElement.Html();
                NSoup.Nodes.Document desDoc = NSoup.NSoupClient.Parse(htmlString);
                Elements elements = desDoc.GetElementsByTag("p");
                foreach (var e in elements)
                {
                    sbDescription.Append(e.Text());
                }
            }
            else
            {
                isBlocked = true;
            }
            return sbDescription.ToString();
        }

        public List<string> GetHotelComments(Document htmlDoc)
        {



            List<string> commentList = new List<string>();

            Element commentDiv = htmlDoc.GetElementById("divDDReviewList");
            Elements commentLink = commentDiv.GetElementsByTag("p");
            return null;
        }
    }
}
