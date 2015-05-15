using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSoup.Nodes;
using NSoup.Select;
using NSoup;
using Wrapper.Model;
using Wrapper.Service;

namespace Wrapper
{
    public class TripAdvisorWrapper
    {
        HotelService hotelService = new HotelService();

        HotelCommentService commentService = new HotelCommentService();

        public void Execute()
        {
            //Tokyo
            List<string> tokyoHotelPageList = WrapperUtility.PreparePageList(TAConstants.TokyoHotelFormatString, TAConstants.TokyoHotelPageMaxCount, TAConstants.Incremental, TAConstants.TokyoHotelFirstPageUrl);

            HandSpecifiedDestinationHotels(tokyoHotelPageList, 1);

            //Seoul
            List<string> seoulHotelPageList = WrapperUtility.PreparePageList(TAConstants.SeoulHotelFormatString, TAConstants.SeoulHotelPageMaxCount, TAConstants.Incremental, TAConstants.SeoulHotelFirstPageUrl);

            HandSpecifiedDestinationHotels(seoulHotelPageList, 2);

            //Chiang Mai
            List<string> chiangMaiHotelPageList = WrapperUtility.PreparePageList(TAConstants.SeoulHotelFormatString, TAConstants.SeoulHotelPageMaxCount, TAConstants.Incremental, TAConstants.SeoulHotelFirstPageUrl);

            HandSpecifiedDestinationHotels(chiangMaiHotelPageList, 3);
        }

        private void HandSpecifiedDestinationHotels(List<string> HotelPageList, int cityID)
        {
            Dictionary<int, List<string>> pagedHotelCollection = hotelService.GetPageHotelList(HotelPageList);
            GenerateAndSaveHotels(pagedHotelCollection, cityID);
        }

        private void GenerateAndSaveHotels(Dictionary<int, List<string>> pagedHotelCollection, int cityID)
        
        {
            if (pagedHotelCollection != null && pagedHotelCollection.Count > 0)
            {
                for (int i = 0; i < pagedHotelCollection.Count; i++)
                {
                    List<string> hotelList = pagedHotelCollection[i];
                    foreach (var hotelUrl in hotelList)
                    {
                        Document doc = WrapperUtility.GetContent(hotelUrl);
                        Hotel hotel = hotelService.GenerateHotel(hotelUrl, doc, cityID);
                        hotel.PageCount = i;
                        int hotelID = hotelService.SaveHotel(hotel);
                        commentService.GenerateSpecifiedAndSaveHotelComments(hotelID, doc, hotelUrl);
                    }
                }
            }
        }
    }
}

