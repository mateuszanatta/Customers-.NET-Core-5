using System;
using System.Globalization;
namespace Customers.Helpers
{
    public class DateManager
    {
        public static DateTime? GetDate(string d)
        {
            bool IsMonthAssigned = false;
            char[] splitsoptions = { '/', '-', ' ' };
            foreach (var i in splitsoptions)
            {
                var y = 0;
                var m = 0;
                var day = 0;
                if (d.IndexOf(i) > 0)
                {
                    try
                    {
                        foreach (var e in d.Split(i))
                        {
                            if (e.Length == 4)
                            {
                                y = Convert.ToInt32(e);
                                continue;
                            }
                            if (Convert.ToInt32(e) <= 12 && !IsMonthAssigned)
                            {
                                m = Convert.ToInt32(e);
                                IsMonthAssigned = true;
                                continue;
                            }
                            day = Convert.ToInt32(e);


                        }
                        return new DateTime(y, m, day);
                    }
                    catch
                    {

                    }
                }
            }
            return null;
        }

        public static DateTime? GetDate(string d, bool custom)
        {
            CultureInfo culture = new CultureInfo("en-US");

            string[] dateFormats =
            {
                "dd/MM/yyyy", "MM/dd/yyyy", "yyyy/MM/dd", "yyyy/dd/MM", "dd-MM-yyyy", "MM-dd-yyyy", "yyyy-MM-dd",
                "yyyy-dd-MM", "dd MM yyyy", "MM dd yyyy", "yyyy MM dd", "yyyy dd MM", "dd.MM.yyyy", "MM.dd.yyyy",
                "yyyy.MM.dd", "yyyy.dd.MM","yyyyMMdd","yyyyddMM","MMddyyyy","ddMMyyyy"
            };

            culture.DateTimeFormat.SetAllDateTimePatterns(dateFormats, 'Y');

            if (DateTime.TryParseExact(d, dateFormats, culture, DateTimeStyles.None, out var date))
                return date;

            return null;


        }
    }
}