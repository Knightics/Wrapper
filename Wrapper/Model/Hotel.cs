
using System.Collections.Generic;
using NSoup.Nodes;
using NSoup.Select;

namespace Wrapper.Model
{
    public class Hotel
    {
        public Hotel()
        {
            HotelDescription = "";
        }

        public int HotelID { get; set; }

        public int PageCount { get; set; }

        public string HotelName { get; set; }

        public string Address { get; set; }

        public double Rate { get; set; }

        public int CityID { get; set; }

        public string HotelDescription { get; set; }

        public Dictionary<int, List<HotelComment>> Comments { get; set; }

        
    }
}
