using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Wrapper.Model;
using System.Data.Common;

namespace Wrapper.Dao
{
    public class HotelCommentDao
    {
        DBHelper dbHelper = new DBHelper();

        private const string SaveHotelCommand = @"INSERT INTO global_data.hotelcomment(Hotelid, PageCount, ShortContent, HotelRate, CommentDate, FullContent)VALUES({0},{1},'{2}',{3},'{4}','{5}')";

        public void SaveHotelComment(HotelComment hotelComment)
        {
            //DbCommand command = null;
            //try
            //{
            //    string commandString = string.Format(SaveHotelCommand, hotelComment.HotelID, hotelComment.PageCount,
            //        hotelComment.ShortContent.Replace("'", "\\'"), hotelComment.HotelRate, hotelComment.CommentDate.ToString("yyyy-MM-dd"), hotelComment.FullContent.Replace("'", "\\'"));
            //    command = dbHelper.GetSqlStringCommond(commandString);
            //    dbHelper.ExecuteNonQuery(command);
            //}
            //catch
            //{
            //    command.Connection.Close();
            //    command.Connection.Open();
            //    SaveHotelComment(hotelComment);
            //}
        }
    }
}
