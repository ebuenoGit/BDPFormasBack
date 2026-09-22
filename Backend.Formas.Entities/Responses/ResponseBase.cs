using Newtonsoft.Json;
using System;
using System.Net;

namespace Backend.Formas.Entities.Responses
{
    public class ResponseBase<T>
    {
        public ResponseBase(HttpStatusCode code = HttpStatusCode.OK, string message = null, T data = default,
            int count = 0)
        {
            ResponseTime = DateTime.UtcNow.AddHours(-5);
            Code = (int)code;
            Message = message;
            Data = data;
            Count = count;
        }

        [JsonProperty("message")] public string Message { get; set; }

        [JsonProperty("count")] public int Count { get; set; }

        [JsonProperty("responseTime")] public DateTime ResponseTime { get; set; }

        [JsonProperty("data")] public T Data { get; set; }

        [JsonProperty("code")] public int Code { get; set; }
    }
}