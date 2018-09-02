using MicBeach.Util.CustomerException;
using MicBeach.Util.IoC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MicBeach.Util.Serialize
{
    /// <summary>
    /// json serialize
    /// </summary>
    public static class JsonSerialize
    {
        static readonly IJsonSerializer _jsonSerializer=null;
        static JsonSerialize()
        {
            _jsonSerializer = ContainerManager.Resolve<IJsonSerializer>();
        }

        #region DataContractJsonSerializer

        /// <summary>
        /// serialization an object to JSON string by DataContract
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="obj">object</param>
        /// <returns>Json String</returns>
        public static string DataContractObjectToJson<T>(T obj)
        {
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream())
            {
                js.WriteObject(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        /// <summary>
        /// deserialization a json string to an object
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="jsonValue">json value</param>
        /// <returns>data object</returns>
        public static T JsonToDataContractObject<T>(string jsonValue)
        {
            if (string.IsNullOrEmpty(jsonValue))
            {
                return default(T);
            }
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T));
            var byteValues = Encoding.UTF8.GetBytes(jsonValue);
            using (MemoryStream stream = new MemoryStream(byteValues))
            {
                return (T)js.ReadObject(stream);
            }
        }

        #endregion

        #region JavaScriptSerializer

        /// <summary>
        /// serialization an object to JSON string by IJsonSerializer
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="obj">data object</param>
        /// <returns>Json string</returns>
        public static string ObjectToJson<T>(T obj)
        {
            if (_jsonSerializer == null)
            {
                throw new AppException("haven't initialized the IJsonSerializer");
            }
            return _jsonSerializer.ObjectToJson(obj);
        }

        /// <summary>
        /// deserialization a json string to an object by IJsonSerializer
        /// </summary>
        /// <param name="json">json string</param>
        /// <returns>data object</returns>
        public static T JsonToObject<T>(string json)
        {
            if (_jsonSerializer == null)
            {
                throw new AppException("haven't initialized the IJsonSerializer");
            }
            return _jsonSerializer.JsonToObject<T>(json);
        }

        #endregion
    }
}
