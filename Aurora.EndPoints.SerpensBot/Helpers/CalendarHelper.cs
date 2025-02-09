namespace Aurora.EndPoints.SerpensBot.Helpers;

public class CalendarHelper
{
    public static bool IsWorkingDay(DateTime date)
    {
        // if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || date.Hour is <= 9 or >= 18)
        // {
        //     return false;
        // }

        return true;
    }
}