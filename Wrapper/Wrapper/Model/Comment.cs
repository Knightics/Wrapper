using System;

namespace Wrapper.Model
{
    public class Comment
    {
        public int CommentID { get; set; }

        public int HotelID { get; set; }

        public string ShortContent { get; set; }

        public float HotelRate { get; set; }

        public DateTime CommentDate { get; set; }

        public string FullContent { get; set; }
    }
}
