using System.Security.Cryptography;
using System;

namespace CanYouC_
{
    enum DayOfWeek
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    public class DateOnly
    {
        public int Day;
        public int Month;
        public int Year;
        public int DayOfWeek;
        /// <summary>
        /// Enumerated value of the day of the week
        /// </summary>
        public readonly int DayNumber;
        public readonly int DayOfYear;

        public DateOnly()
        {
            Day = 0;
            Month = 0;
            Year = 0;
            DayOfWeek = 0;
            DayNumber = 0;
            DayOfYear = 0;
        }

        public DateOnly(string date)
        {
            if (date == "")
            {
                Day = 0;
                Month = 0;
                Year = 0;
                DayOfWeek = 0;
                DayNumber = 0;
                DayOfYear = 0;
                return;
            }
            // Example date format: 24.07.1989
            string[] dateParts = date.Split('.');
            // Extract the day, month and year from the date string
            Day = int.Parse(dateParts[0]);
            Month = int.Parse(dateParts[1]);
            Year = int.Parse(dateParts[2]);

            // Create a DateTime object to get the DayOfWeek and DayOfYear
            DateTime dateTime = new(Year, Month, Day);
            DayOfWeek = (int)dateTime.DayOfWeek;
            DayOfYear = dateTime.DayOfYear;

            // Unsure what this is representing
            DayNumber = 0;
        }
    }
}
