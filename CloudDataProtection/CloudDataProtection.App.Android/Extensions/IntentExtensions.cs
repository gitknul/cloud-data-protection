using System;
using Android.Content;
using Newtonsoft.Json;

namespace CloudDataProtection.App.Android.Extensions
{
    /// <summary>
    /// Extensio  to make use of Intents with strict typing
    /// </summary>
    public static class IntentExtensions
    {
        public static T GetExtra<T>(this Intent intent, string name)
        {
            return JsonConvert.DeserializeObject<T>(intent.GetStringExtra(name));
        }

        public static void PutExtra<T>(this Intent intent, string name, T data)
        {
            intent.PutExtra(name, JsonConvert.SerializeObject(data));
        }
    }
}
