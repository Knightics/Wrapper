using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wrapper
{
    public static class TAConstants
    {
        #region Common

        public const int Incremental = 30;
        
        #endregion


        #region Tokyo

        public  const string TokyoHotelFirstPageUrl = "http://cn.tripadvisor.com/Hotels-g298184-Tokyo_Tokyo_Prefecture_Kanto-Hotels.html";

        public const string TokyoHotelFormatString = "http://cn.tripadvisor.com/Hotels-g298184-oa{0}-Tokyo_Tokyo_Prefecture_Kanto-Hotels.html#ACCOM_OVERVIEW";

        public const int TokyoHotelPageMaxCount = 25;

        #endregion

        #region Seoul

        public const string SeoulHotelFirstPageUrl = "http://www.tripadvisor.cn/Hotels-g294197-Seoul-Hotels.html#ACCOM_OVERVIEW";

        public const string SeoulHotelFormatString = "http://www.tripadvisor.cn/Hotels-g294197-oa{0}-Seoul-Hotels.html#ACCOM_OVERVIEW";

        public const int SeoulHotelPageMaxCount = 14;

        #endregion

        #region Chiang Mai

        public const string ChiangMaiHotelFirstPageUrl = "http://www.tripadvisor.cn/Hotels-g293917-Chiang_Mai-Hotels.html";

        public const string ChiangMaiHotelFormatString = "http://www.tripadvisor.cn/Hotels-g293917-oa{0}-Chiang_Mai-Hotels.html#ACCOM_OVERVIEW";

        public const int ChiangMaiHotelPageMaxCount = 12;

        #endregion

    }
}
