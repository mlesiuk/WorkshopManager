namespace workshopManager.Application.Utils;

public static class DateUtils
{
    public static bool IsWeekendDay(this DateTime dateTime)
    {
        return (dateTime.DayOfWeek == DayOfWeek.Saturday) || (dateTime.DayOfWeek == DayOfWeek.Sunday);
    }
}
