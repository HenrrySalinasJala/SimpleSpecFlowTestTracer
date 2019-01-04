namespace SimpleListener.Client
{
    using log4net;
    using Newtonsoft.Json.Linq;
    using RestSharp;

    /// <summary>
    /// Utility class to handle the different Request types.
    /// </summary>
    public static class RequestUtil
    {
        /// <summary>
        /// The logger instance.
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        const string TestURL = "https://jsonplaceholder.typicode.com/posts";

        /// <summary>
        /// The RestSharp RestClient.
        /// </summary>
        private static RestClient _client = new RestClient(TestURL);

        /// <summary>
        /// ApplicationJson Constant.
        /// </summary>
        public const string ApplicationJson = "application/json";

        /// <summary>
        /// AuthorizationToken Constant.
        /// </summary>
        public const string AuthorizationToken = "Authorization-Token";

        /// <summary>
        /// Performs a POST request by RestSharp.
        /// </summary>
        /// <param name="endPoint">Request's endPoint</param>
        /// <param name="jsonBody">Request's body sent as string</param>
        /// <param name="token">Request's header set as Authorization-Token</param>
        /// <param name="baseUrl">The base Url to work with.</param>
        /// <returns>Returns a new instance of the <see cref="Response"/> class with proper
        /// values or Request done.</returns>
        public static Response Post(string endPoint, string jsonBody, string token = "", string baseUrl = "")
        {
            return PostMethod(endPoint, jsonBody, token, baseUrl);
        }

        /// <summary>
        /// Performs a POST request by RestSharp.
        /// </summary>
        /// <param name="endPoint">Request's endPoint</param>
        /// <param name="jsonBody">Request's body sent as object</param>
        /// <param name="token">Request's header set as Authorization-Token</param>
        /// <param name="baseUrl">The base Url to work with.</param>
        /// <returns>Returns a new instance of the <see cref="Response"/> class with proper
        /// values or Request done.</returns>
        public static Response Post(string endPoint, object jsonBody, string token = "", string baseUrl = "")
        {
            return PostMethod(endPoint, jsonBody, token, baseUrl);
        }

        /// <summary>
        /// Performs a POST request by RestSharp.
        /// </summary>
        /// <param name="endPoint">Request's endPoint</param>
        /// <param name="jsonBody">Request's body sent as JObject</param>
        /// <param name="token">Request's header set as Authorization-Token</param>
        /// <param name="baseUrl">The base Url to work with.</param>
        /// <returns>Returns a new instance of the <see cref="Response"/> class with proper
        /// values or Request done.</returns>
        public static Response Post(string endPoint, JObject jsonBody, string token = "", string baseUrl = "")
        {
            return PostMethod(endPoint, jsonBody, token, baseUrl);
        }

        /// <summary>
        /// Performs a POST request by RestSharp.
        /// </summary>
        /// <param name="endPoint">Request's endPoint</param>
        /// <param name="jsonBody">Request's body sent as object</param>
        /// <param name="token">Request's header set as Authorization-Token</param>
        /// <param name="baseUrl">The base Url to work with.</param>
        /// <returns>Returns a new instance of the <see cref="Response"/> class with proper
        /// values or Request done.</returns>
        private static Response PostMethod(string endPoint, object jsonBody, string token = "", string baseUrl = "")
        {
            SetBaseUrl(baseUrl);
            var request = CreateRequest(Method.POST, endPoint, token, jsonBody);
            log.Info($"POST {endPoint}");
            log.Debug($"BODY {jsonBody}");
            var response = new Response(_client.Execute(request));
            log.Info($"Status Code: {response.StatusCode}");
            log.Info($"Response {response.Content}");
            return response;
        }

        /// <summary>
        /// Performs a PUT request by RestSharp.
        /// </summary>
        /// <param name="endPoint">Request's endPoint</param>
        /// <param name="jsonBody">Request's body sent as object</param>
        /// <param name="token">Request's header set as Authorization-Token</param>
        /// <returns>Returns a new instance of the <see cref="Response"/> class with proper
        /// values or Request done.</returns>
        public static Response Put(string endPoint, object jsonBody, string token)
        {
            return SendRequest(Method.PUT, endPoint, token, jsonBody);
        }

        /// <summary>
        /// Performs a PUT request by RestSharp.
        /// </summary>
        /// <param name="endPoint">Request's endPoint</param>
        /// <param name="jsonBody">Request's body sent as string</param>
        /// <param name="token">Request's header set as Authorization-Token</param>
        /// <returns>Returns a new instance of the <see cref="Response"/> class with proper
        /// values or Request done.</returns>
        public static Response Put(string endPoint, string jsonBody, string token)
        {
            return SendRequest(Method.PUT, endPoint, token, jsonBody);
        }

        /// <summary>
        /// Performs a PUT request by RestSharp.
        /// </summary>
        /// <param name="endPoint">Request's endPoint</param>
        /// <param name="jsonBody">Request's body sent as JObject</param>
        /// <param name="token">Request's header set as Authorization-Token</param>
        /// <returns>Returns a new instance of the <see cref="Response"/> class with proper
        /// values or Request done.</returns>
        public static Response Put(string endPoint, JObject jsonBody, string token)
        {
            return SendRequest(Method.PUT, endPoint, token, jsonBody);
        }

        /// <summary>
        /// Performs a GET request by RestSharp.
        /// </summary>
        /// <param name="endPoint">Request's endPoint</param>
        /// <param name="token">Request's header set as Authorization-Token</param>
        /// <returns>Returns a new instance of the <see cref="Response"/> class with proper
        /// values or Request done.</returns>
        public static Response Get(string endPoint, string token)
        {
            return SendRequest(Method.GET, endPoint, token);
        }

        /// <summary>
        /// Performs a Get request.
        /// </summary>
        /// <param name="endPoint">The endpoint.</param>
        /// <param name="token">The token.</param>
        /// <param name="baseUrl">The base url.</param>
        /// <returns></returns>
        internal static Response Get(string endPoint, string token, string baseUrl)
        {
            SetBaseUrl(baseUrl);
            var request = CreateRequest(Method.GET, endPoint, token);
            log.Info($"GET {endPoint}");
            var response = new Response(_client.Execute(request));
            log.Info($"Status Code: {response.StatusCode}");
            log.Info($"Response {response.Content}");
            return response;
        }

        /// <summary>
        /// Performs a DELETE request by RestSharp.
        /// </summary>
        /// <param name="endPoint">Request's endPoint</param>
        /// <param name="token">Request's header set as Authorization-Token</param>
        /// <returns>Returns a new instance of the <see cref="Response"/> class with proper
        /// values or Request done.</returns>
        public static Response Delete(string endPoint, string token)
        {
            return SendRequest(Method.DELETE, endPoint, token);
        }

        /// <summary>
        /// Sends a Delete request to the given end point.
        /// </summary>
        /// <param name="endPoint">Request's endPoint.</param>
        /// <param name="jsonBody">Request's body sent as string.</param>
        /// <param name="token">Request's header set as Authorization-Token.</param>
        /// <returns>The request response.</returns>
        public static Response Delete(string endPoint, string token = "", string baseUrl = "")
        {
            SetBaseUrl(baseUrl);
            var request = CreateRequest(Method.DELETE, endPoint, token);
            log.Info($"DELETE {endPoint}");
            var response = new Response(_client.Execute(request));
            log.Info($"Status Code: {response.StatusCode}");
            log.Info($"Response {response.Content}");
            return response;
        }

        /// <summary>
        /// Creates the Rest Sharp Rest Request.
        /// </summary>
        /// <param name="method">The Request's method</param>
        /// <param name="endPoint">Request's endPoint</param>
        /// <param name="token">Request's header set as Authorization-Token</param>
        /// <param name="jsonBody">The Json body</param>
        /// <returns>Returns a new instance of Restsharp's RestRequest</returns>
        private static RestRequest CreateRequest(Method method, string endPoint, string token = "", object jsonBody = null)
        {
            RestRequest restRequest = new RestRequest(endPoint, method);
            if (jsonBody != null)
            {
                if ((jsonBody is string) || (jsonBody is JObject))
                {
                    restRequest.AddParameter(ApplicationJson, jsonBody, ParameterType.RequestBody);
                }
                else
                {
                    restRequest.AddJsonBody(jsonBody);
                }
            }

            if (!string.IsNullOrEmpty(token))
            {
                restRequest.AddHeader(AuthorizationToken, token);
            }

            return restRequest;
        }

        /// <summary>
        /// Verifies RestSharp Client's contains ApiServiceUrl.
        /// </summary>
        private static void VerifyApiServiceUrl()
        {
            if (!_client.BaseUrl.Equals(TestURL))
            {
                _client = new RestClient(TestURL);
            }
        }


        /// <summary>
        /// Sends the Request.
        /// </summary>
        /// <param name="method">The Request's method</param>
        /// <param name="endPoint">Request's endPoint</param>
        /// <param name="token">Request's header set as Authorization-Token</param>
        /// <param name="jsonBody">The Json body</param>
        /// <returns>Returns a new instance of the <see cref="Response"/> class</returns>
        private static Response SendRequest(Method method, string endPoint, string token = "", object jsonBody = null)
        {
            VerifyApiServiceUrl();
            var request = CreateRequest(method, endPoint, token, jsonBody);
            log.Info($"{method.ToString()} {endPoint}");
            log.Debug($"BODY {jsonBody}");
            var response = new Response(_client.Execute(request));
            log.Info($"Status Code: {response.StatusCode}");
            log.Info($"Response {response.Content}");
            return response;


        }

        /// <summary>
        /// Sets the Base URL.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        private static void SetBaseUrl(string baseUrl)
        {
            if (!string.IsNullOrEmpty(baseUrl))
            {
                _client = new RestClient(baseUrl);
            }
            else
            {
                VerifyApiServiceUrl();
            }
        }
    }
}
