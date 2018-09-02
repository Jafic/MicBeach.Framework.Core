using System;
using MicBeach.Util.Serialize;
using Newtonsoft.Json;

namespace MicBeach.Serialize.Json.JsonNet
{
    /// <summary>
    /// Json.Net Serializer
    /// </summary>
    public class JsonNetSerializer : IJsonSerializer
    {
        /// <summary>
        /// serialization an object to a JSON string
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="obj">object</param>
        /// <returns>json string</returns>
        public string ObjectToJson<T>(T obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj.GetType().FullName == typeof(string).FullName)
            {
                return obj.ToString();
            }
            string jsonString = JsonConvert.SerializeObject(obj, new BigNumberConverter());
            return jsonString;
        }

        /// <summary>
        /// deserialization a JSON string to an object
        /// </summary>
        /// <param name="json">JSON string</param>
        /// <returns>object</returns>
        public T JsonToObject<T>(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return default(T);
            }
            if (typeof(T).FullName == typeof(string).FullName)
            {
                return (dynamic)json;
            }
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
