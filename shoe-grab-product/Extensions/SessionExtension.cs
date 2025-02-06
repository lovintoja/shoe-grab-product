using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ShoeGrabProductManagement.Extensions;
public static class SessionExtensions
{
    // Method to store an object as JSON in session
    public static void SetObjectAsJson(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    // Method to retrieve an object from session by key and deserialize it from JSON
    public static T GetObjectFromJson<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
    }
}
