using System;

namespace ClassLibraryDateMethods
{
    public class DateMethods
    {
        public static bool FutureDate(DateTime date)
        {
            DateTime now = DateTime.Today;
            TimeSpan isFuture = now - date;
            int days = Convert.ToInt32(isFuture.Days);

            if (days < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool StudyIsLate(DateTime startDate, int estimatedTime)
        {
            TimeSpan timeSpan = startDate.AddDays(estimatedTime) - DateTime.Today;
            int toCompare = Convert.ToInt32(timeSpan.Days);

            if (toCompare < 0)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
