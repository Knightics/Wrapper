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
    public class HotelCommentService
    {
        HotelCommentDao hotelCommentDao = new HotelCommentDao();
        public void SaveHotelComment(HotelComment hotelComment)
        {
            hotelCommentDao.SaveHotelComment(hotelComment);
        }

        public int GetCommentMaxPageCount(Document doc)
        {
            string pages = doc.GetElementsByClass("pageNumbers").Text;
            int lastSpace = pages.LastIndexOf(" ");
            int maxPageStart = pages.LastIndexOf(" ") + 1;
            int length = pages.Length - maxPageStart;
            int result = 0;
            int.TryParse(pages.Substring(maxPageStart, length), out result);
            return result;
        }

        public void GenerateSpecifiedAndSaveHotelComments(int hotelID, Document doc, string commentFirstPageUrl)
        {
            int commentMaxPage = GetCommentMaxPageCount(doc);

            List<string> commentPageList = WrapperUtility.PrepareCommentPageList(HotelComment.FormatString, commentMaxPage, HotelComment.Incremental, commentFirstPageUrl);

            GetAndSavePageHotelCommentList(hotelID, commentPageList);

        }

        private void GetAndSavePageHotelCommentList(int hotelID, List<string> hotelCommentPageList)
        {
            for (int i = 0; i < hotelCommentPageList.Count; i++)
            {
                Document doc = WrapperUtility.GetContent(hotelCommentPageList[i]);

                Elements allComments = doc.GetElementsByClass("reviewSelector");
                List<int> commentIDList = new List<int>();

                string[] splitedUrl = hotelCommentPageList[i].Split('-');
                string hotelIdentity = string.Empty;
                if (splitedUrl.Length > 3)
                {
                    hotelIdentity = splitedUrl[1] + "-" + splitedUrl[2];
                }

                StringBuilder extendReviewRequest = new StringBuilder("http://cn.tripadvisor.com/ExpandedUserReviews-");
                extendReviewRequest.Append(hotelIdentity);
                extendReviewRequest.Append("?target=");
                extendReviewRequest.Append(allComments[0].Id.Replace("review_", ""));
                extendReviewRequest.Append("&context=0&reviews=");
                for (int j = 0; j < allComments.Count; j++)
                {
                    string commentAppendString = allComments[j].Id.Replace("review_", "");
                    extendReviewRequest.Append(commentAppendString);
                    if (j != allComments.Count - 1)
                        extendReviewRequest.Append(",");
                }
                extendReviewRequest.Append("&servlet=Hotel_Review&expand=1");




















                StringBuilder commentsRequestUrlPrefix = new StringBuilder("http://cn.tripadvisor.com/UserReviewController?a=rblock&r=");

                for (int j = 0; j < allComments.Count; j++)
                {
                    string commentAppendString = allComments[j].Id.Replace("review_", "");
                    commentsRequestUrlPrefix.Append(commentAppendString);
                    if (j != allComments.Count - 1)
                        commentsRequestUrlPrefix.Append(":");
                }

                commentsRequestUrlPrefix.Append("&type=0&tr=false&n=16");

                Document commentDoc = WrapperUtility.GetContent(commentsRequestUrlPrefix.ToString());

                Elements elements = commentDoc.GetElementsByClass("innerBubble");

                HotelComment comment = new HotelComment();
                foreach (var e in elements)
                {
                    string shortContent = e.GetElementsByClass("noQuotes").Text.Replace("。", "");
                    if (!string.IsNullOrEmpty(shortContent))
                    {
                        comment.ShortContent = shortContent;

                        DateTime date = DateTime.Now;

                        string contentDateString = e.GetElementsByClass("ratingDate").Text.Replace("年", "-").Replace("月", "-").Replace("日的点评", "");

                        string attrDateString = e.GetElementsByClass("ratingDate").Attr("title").Replace("年", "-").Replace("月", "-").Replace("日", "");

                        if (DateTime.TryParse(contentDateString, out date))
                            comment.CommentDate = date;
                        else
                            comment.CommentDate = DateTime.Parse(attrDateString);

                        comment.HotelRate = float.Parse(e.GetElementsByClass("sprite-rating_s_fill").Attr("alt").Replace("分", ""));

                        comment.FullContent = e.GetElementsByClass("partial_entry").Text;

                        comment.PageCount = i;

                        comment.HotelID = hotelID;

                        SaveHotelComment(comment);
                    }
                    else
                        break;
                }
            }
        }
    }
}
