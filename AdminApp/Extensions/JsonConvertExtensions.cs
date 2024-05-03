using Newtonsoft.Json;

namespace AdminApp.Extensions;

public static class JsonConvertExtensions
{
    public static string SerializeObject(this object? obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    public static T? DeserializeObject<T>(this object? obj)
    {
        return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
    }

    public static T? DeserializeString<T>(this string str)
    {
        return JsonConvert.DeserializeObject<T>(str);
    }

}
