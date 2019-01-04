namespace SimpleListener.Client
{
    using RestSharp;
    using System.Net;

    public class Response
    {
        /// <summary>
        /// Response's status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// Response's content.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// Restsharp's Rest Response.
        /// </summary>
        private IRestResponse RestResponse { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Response"/> class.
        /// </summary>
        /// <param name="RestResponse"></param>
        public Response(IRestResponse RestResponse)
        {
            StatusCode = RestResponse.StatusCode;
            Content = RestResponse.Content;
            this.RestResponse = RestResponse;
        }
    }
}
