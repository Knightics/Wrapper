using System;
using NSoup.Nodes;
using System.Collections.Generic;
using NSoup.Select;
using System.Text;

namespace Wrapper.Model
{
    public class HotelComment
    {
        public int CommentID { get; set; }

        public int HotelID { get; set; }

        public int PageCount { get; set; }

        public string ShortContent { get; set; }

        public float HotelRate { get; set; }

        public DateTime CommentDate { get; set; }

        public string FullContent { get; set; }

        #region Comments Constants

        public const string FormatString = "http://cn.tripadvisor.com/Hotel_Review-{0}-Reviews-or{1}.html";

        public const int Incremental = 10;


        #endregion
    }
}
