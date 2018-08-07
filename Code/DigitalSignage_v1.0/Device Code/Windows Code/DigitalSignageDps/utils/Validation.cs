using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalSignageDps
{
    public class Validation
    {
        private static Dictionary<DayOfWeek, string> dayDictionary;
        static Validation()
        {
            dayDictionary = new Dictionary< DayOfWeek,string>();
            dayDictionary.Add(DayOfWeek.Monday ,"mon");
            dayDictionary.Add(DayOfWeek.Tuesday,"tue");
            dayDictionary.Add( DayOfWeek.Wednesday,"wed");
            dayDictionary.Add( DayOfWeek.Thursday, "thu");
            dayDictionary.Add( DayOfWeek.Friday, "fri");
            dayDictionary.Add( DayOfWeek.Saturday, "sat");
            dayDictionary.Add( DayOfWeek.Sunday, "sun");
        }
        public static bool ValidateDate(string startString, string endString,string frequency,string daysOfWeek)
        {
            DateTime startTimestamp = DateTime.ParseExact(startString, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            DateTime endTimestamp = DateTime.ParseExact(endString, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            DateTime startDate=startTimestamp.Date;
            DateTime endDate = endTimestamp.Date;

            TimeSpan startTime = startTimestamp.TimeOfDay;
            TimeSpan endTime = endTimestamp.TimeOfDay;

            DateTime now = DateTime.Now;
            DateTime todayDate = now.Date;
            DayOfWeek dayOfWeek = now.DayOfWeek;
            TimeSpan currentTime = now.TimeOfDay;

            frequency = frequency.ToUpper();
            daysOfWeek = daysOfWeek.ToLower();

            if (frequency== "CONTINUOUS")
            {
                bool isStartTimeValid = startTimestamp.CompareTo(now) <= 0;
                bool isEndTimeValid = now.CompareTo(endTimestamp) <= 0;
                return isEndTimeValid && isStartTimeValid;

            }
            else if(frequency == "DAILY")
            {
                bool isStartDateValid = startDate.CompareTo(todayDate) <= 0;
                bool isEndDateValid = todayDate.CompareTo(endDate) <= 0;

                bool isStartTimeValid = startTime.CompareTo(currentTime) <= 0;
                bool isEndTimeValid = currentTime.CompareTo(endTime) <= 0;
                return isEndDateValid && isEndTimeValid && isStartDateValid && isStartTimeValid;
            }
            else if(frequency == "WEEKLY")
            {
                bool isStartDateValid = startDate.CompareTo(todayDate) <= 0;
                bool isEndDateValid = todayDate.CompareTo(endDate) <= 0;
                bool isInDaysOfWeek = daysOfWeek.Contains(dayDictionary[dayOfWeek]);

                bool isStartTimeValid = startTime.CompareTo(currentTime) <= 0;
                bool isEndTimeValid = currentTime.CompareTo(endTime) <= 0;
                return isEndDateValid && isEndTimeValid && isStartDateValid && isStartTimeValid && isInDaysOfWeek;
            }
            return false;
        }
    }
}
