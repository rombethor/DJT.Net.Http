using System.Text.Json;

namespace DJT.Net.Http
{
    /// <summary>
    /// Typed Http Response
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonHttpResponse<T>
    {
        private readonly HttpResponseMessage _msg;

        /// <summary>
        /// The original response message
        /// </summary>
        public HttpResponseMessage Message => _msg;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        public JsonHttpResponse(HttpResponseMessage msg)
        {
            _msg = msg;
        }

        /// <summary>
        /// Retrieves an instance of the response content synchronously
        /// </summary>
        public T? Data => GetContentAsync().Result;

        /// <summary>
        /// Retrieve the deserialised response
        /// </summary>
        /// <returns></returns>
        public async Task<T?> GetContentAsync()
        {
            string text = await _msg.Content.ReadAsStringAsync();
            string? mediaType = _msg.Content.Headers.ContentType?.MediaType;
            if (mediaType == null)
            {
                return default;
            }
            if (mediaType.Contains("json"))
            {
                return JsonSerializer.Deserialize<T>(text) ?? default;
            }
            return default;
        }

        /// <summary>
        /// Implicit conversion from `JsonHttpResponse` to `HttpResponseMessage`
        /// </summary>
        /// <param name="msg"></param>
        public static implicit operator HttpResponseMessage(JsonHttpResponse<T> msg) => msg.Message;

        /// <summary>
        /// Rule for explicit conversion, in case it's needed.
        /// </summary>
        /// <param name="msg"></param>
        public static explicit operator JsonHttpResponse<T>(HttpResponseMessage msg) => new(msg);
    }
}