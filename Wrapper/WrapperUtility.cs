using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSoup.Nodes;
using System.Net;

namespace Wrapper
{
    public static class WrapperUtility
    {
        public static Document GetContent(string url)
        {
            WebClient webClient = new WebClient();
            WebRequest webRequest = WebRequest.Create(url);
            return NSoup.NSoupClient.Parse(webRequest.GetResponse().GetResponseStream(), "utf-8");
        }

        public static List<string> PreparePageList(string formatString, int maxCount, int incremental, string firstPageUrl)
        {
            List<string> urlList = new List<string>();
            string url = string.Empty;

            if (firstPageUrl != null && !string.IsNullOrEmpty(firstPageUrl))
                urlList.Add(firstPageUrl);

            for (int i = incremental; i < maxCount * incremental; i += incremental)
            {
                url = string.Format(formatString, i);
                urlList.Add(url);
            }

            return urlList;
        }

        public static List<string> PrepareCommentPageList(string formatString, int maxCount, int incremental, string hotelUrl)
        {
            List<string> urlList = new List<string>();
            string url = string.Empty;

            string hotelIDString = GenerateHotelIDString(hotelUrl);

            if (hotelUrl != null && !string.IsNullOrEmpty(hotelUrl))
                urlList.Add(hotelUrl);

            for (int i = incremental; i < maxCount * incremental; i += incremental)
            {
                url = string.Format(formatString, hotelIDString, i);
                urlList.Add(url);
            }

            return urlList;
        }

        private static string GenerateHotelIDString(string hotelUrl)
        {
            int end = hotelUrl.LastIndexOf("-Reviews-");
            int start = hotelUrl.LastIndexOf("_Review-");
            return hotelUrl.Substring(start, end - start).Replace("_Review-", "");
        }
    }
}
