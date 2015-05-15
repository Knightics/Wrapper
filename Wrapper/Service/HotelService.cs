using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wrapper.Model;
using NSoup.Nodes;
using NSoup.Select;
using Wrapper.Dao;

namespace Wrapper.Service
{
    public class HotelService
    {

        HotelDao hotelDao = new HotelDao();
        HotelCommentService commentService = new HotelCommentService();
        public int SaveHotel(Hotel hotel)
        {
            return hotelDao.SaveHotel(hotel);
        }

        public Dictionary<int, List<string>> GetPageHotelList(List<string> hotelPageList)
        {
            Dictionary<int, List<string>> pagedHotelCollection = new Dictionary<int, List<string>>();
            for (int i = 0; i < hotelPageList.Count; i++)
            {
                Document doc = WrapperUtility.GetContent(hotelPageList[i]);
                Elements elements = doc.GetElementsByClass("property_title");
                List<string> pagedHotelList = new List<string>();
                foreach (var e in elements)
                {
                    pagedHotelList.Add(string.Format("http://cn.tripadvisor.com{0}", e.Attr("href")));
                }

                pagedHotelCollection.Add(i, pagedHotelList);
            }
            return pagedHotelCollection;
        }

        public Hotel GenerateHotel(string hotelUrl, Document doc, int cityID)
        {
            Hotel hotel = null;

            if (doc != null && doc.Title.IndexOf("-") > 0)
            {
                hotel = new Hotel();
                hotel.CityID = cityID;
                hotel.HotelName = doc.Title.Substring(0, doc.Title.LastIndexOf("-"));

                Elements rateElements = doc.GetElementsByClass("sprite-rating_rr_fill");
                if (rateElements != null && rateElements.Count > 0)
                    hotel.Rate = double.Parse(rateElements[0].Attr("content"));

                Elements addressElements = doc.GetElementsByClass("format_address");
                if (addressElements != null && addressElements.Count > 0)
                    hotel.Address = addressElements[0].Text();
            }
            return hotel;
        }
    }
}
