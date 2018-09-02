using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Serialize.Json.JsonNet
{
    /// <summary>
    /// big number json converter
    /// </summary>
    public class BigNumberConverter : JsonConverter 
    {

        public override bool CanConvert(Type objectType)
        {
            string[] allowTypeValues = new string[] { typeof(long).FullName, typeof(ulong).FullName, typeof(decimal).FullName };
            return allowTypeValues.Contains(objectType.FullName);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string valueString = value.ToString();
            writer.WriteValue(valueString);
        }
    }
}
