using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Random_Realistic_Flight.Extensions;

public static class TempDataExtensions
{
    /// <summary>
    /// Sets a value in the temp data store.
    /// </summary>
    /// <param name="tempData"></param>
    /// <param name="key">The key to store the value in</param>
    /// <param name="value">The value</param>
    public static void Set<T>(this ITempDataDictionary tempData, string key, T value) where T : class
    {
        tempData[key] = JsonSerializer.Serialize(value);
    }

    /// <summary>
    /// Gets a value from the temp data store.
    /// </summary>
    /// <param name="tempData"></param>
    /// <param name="key">The key to retrieve the value from</param>
    /// <returns>Null if the value is not found or if it is not a string</returns>
    public static T? Get<T>(this ITempDataDictionary tempData, string key) where T : class
    {
        var o = tempData.Peek(key);
        if (o is string s)
        {
            return JsonSerializer.Deserialize<T>(s);
        }

        return null;
    }
}
