using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Backend.Formas.Utilities
{
    /// <summary>
    ///     Json Extensions
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// The settings
        /// </summary>
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            Formatting = Formatting.None
        };

        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string Serialize<T>(this T value, JsonSerializerSettings settings = null)
        {
            try
            {
                if (value == null)
                {
                    return string.Empty;
                }

                return JsonConvert.SerializeObject(value, settings ?? Settings);
            }
            catch
            {
                return default;
            }
        }

        /// <summary>
        /// Deserializes the specified value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static T Deserialize<T>(this string value, JsonSerializerSettings settings = null) where T : class
        {
            if (value.IsJsonValid())
            {
                return JsonConvert.DeserializeObject<T>(value, settings ?? Settings);
            }

            return null;
        }

        /// <summary>
        /// Determines whether [is json valid].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if [is json valid] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsJsonValid(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            value = value.Trim();
            if ((value.StartsWith("{") && value.EndsWith("}")) ||
                (value.StartsWith("[") && value.EndsWith("]")))
            {
                try
                {
                    Newtonsoft.Json.Linq.JToken.Parse(value);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}