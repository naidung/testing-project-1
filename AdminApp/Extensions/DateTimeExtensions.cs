namespace AdminApp.Extensions;

public static class DateTimeExtensions
{
    public static string? NulltableDateTime2Str(this DateTime? datetime, string format = "dd/MM/yy")
    {
        var result = datetime.HasValue ? datetime.Value.ToLocalTime().ToString(format) : null;
        return result;
    }

    public static string DateTime2Str(this DateTime datetime, string format = "dd/MM/yy")
    {
        var result = datetime.ToLocalTime().ToString(format);
        return result;
    }
}
