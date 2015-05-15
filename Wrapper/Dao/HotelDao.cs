using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wrapper.Model;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace Wrapper.Dao
{
    public class HotelDao
    {
        DBHelper dbHelper = new DBHelper();

        private const string SaveHotelCommand = @"INSERT INTO global_data.hotelinfo(HotelName, HotelCityID, PageCount, HotelDescription)VALUES('{0}',{1},{2},'{3}')";
        private const string SelectMaxID = @"SELECT MAX(idHotelInfo) FROM global_data.hotelinfo";

        public int SaveHotel(Hotel hotel)
        {
            //DbCommand command = null;
            //try
            //{
            //    string commandString = string.Format(SaveHotelCommand, hotel.HotelName.Replace("'", "\\'"), hotel.CityID, hotel.PageCount, hotel.HotelDescription.Replace("'", "\\'"));
            //    command = dbHelper.GetSqlStringCommond(commandString);
            //    dbHelper.ExecuteNonQuery(command);

            //    command = dbHelper.GetSqlStringCommond(SelectMaxID);
            //    object result = dbHelper.ExecuteScalar(command);
            //    return (int)result;
            //}
            //catch
            //{
            //    command.Connection.Close();
            //    command.Connection.Open();
            //    return SaveHotel(hotel);
            //}
            return 10000;
        }
    }
}
