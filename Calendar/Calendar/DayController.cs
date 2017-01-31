using System;

namespace Calendar
{
    class DayController
    {
        DateTime today;
        string TodayStr;
        // 今日の情報
        private int todayYear;
        private int todayMonth;
        private int todayDay;

        public DayController()
        {
            today = new DateTime();
            today = DateTime.Today;
            TodayStr = today.ToString();
            todayYear = int.Parse(TodayStr.Substring(0, 4));
            todayMonth = int.Parse(TodayStr.Substring(5, 2));
            todayDay = int.Parse(TodayStr.Substring(8, 2));
        }

        public void GetTodayProperty(ref int Year, ref int Month, ref int Day)
        {
            Year = todayYear;
            Month = todayMonth;
            Day = todayDay;
        }

        // 今日の年月日を取得
        public void GetDateProperty(ref int Year, ref int Month, ref int Day, DateTime date)
        {
            string str = date.ToString();
            Year = int.Parse(str.Substring(0, 4));
            Month = int.Parse(str.Substring(5, 2));
            Day = int.Parse(str.Substring(8, 2));
        }

        // 曜日を取得する
        public int GetWeekNum(int Year, int Month, int Day)
        {
            DateTime MonthStart = new DateTime(Year, Month, Day, 0, 0, 0);
            //int weekNum = (int)MonthStart.DayOfWeek;
            //if (--weekNum < 0) { weekNum = 6; }
            return (int)MonthStart.DayOfWeek;
        }

        // 与えた月の日にちの数を返す
        public int GetDayNum(int Year, int Month)
        {
            return DateTime.DaysInMonth(Year, Month);
        }

        // 与えた日が今日であるか返す
        public Boolean IfToday(int Year, int Month, int Day)
        {
            if (Year == todayYear && Month == todayMonth && Day == todayDay)
            {
                return true;
            }
            else { return false; }
        }
    }
}
