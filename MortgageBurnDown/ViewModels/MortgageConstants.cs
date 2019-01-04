using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MortgageBurnDown
{
    public static class MortgageConstants
    {
        public const decimal OriginalBalance = 347000;
        public const decimal CurrentBalance = 267868.43M;
        public const int OriginalDurationInMonths = 180;
        public static readonly DateTime OriginalDate = DateTime.Parse("April 2015");
        public const decimal Interest = 0.03M;
        public const decimal MinimumPayment = 2396.32M;

        /// <summary>
        /// Returns the date associated with month n of the lifetime of the mortgage
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static DateTime GetDateFromMonthOfMortgage(int month)
        {
            int year = ((month + OriginalDate.Month) / 12) + OriginalDate.Year;
            string monthStr = string.Empty;

            switch ((month + OriginalDate.Month) % 12)
            {
                case 0:
                    monthStr = "January";
                    break;
                case 1:
                    monthStr = "February";
                    break;
                case 2:
                    monthStr = "March";
                    break;
                case 3:
                    monthStr = "April";
                    break;
                case 4:
                    monthStr = "May";
                    break;
                case 5:
                    monthStr = "June";
                    break;
                case 6:
                    monthStr = "July";
                    break;
                case 7:
                    monthStr = "August";
                    break;
                case 8:
                    monthStr = "September";
                    break;
                case 9:
                    monthStr = "October";
                    break;
                case 10:
                    monthStr = "November";
                    break;
                case 11:
                    monthStr = "December";
                    break;
            }

            var fullString = $"{monthStr} {year}";
            return DateTime.Parse(fullString);
        }
    }
}
