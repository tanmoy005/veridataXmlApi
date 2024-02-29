using System.Net;


namespace VERIDATA.Model.Base
{

    public class BaseResponse<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseResponse{T}"/> class.
        /// </summary>

        public BaseResponse(HttpStatusCode statusCode, T _ResponseInfo)
        {
            StatusCode = statusCode;
            ResponseInfo = _ResponseInfo;
        }

        public BaseResponse(HttpStatusCode statusCode, List<T> _ResponseInfos)
        {
            StatusCode = statusCode;
            ResponseInfos = _ResponseInfos;
        }
        public BaseResponse(HttpStatusCode statusCode, ErrorResponse ResponseInfo)
        {
            StatusCode = statusCode;
            ErrorResponse = ResponseInfo;
        }
        public HttpStatusCode StatusCode { get; set; }
        public T ResponseInfo { get; set; }
        public List<T> ResponseInfos { get; set; }
        public ErrorResponse ErrorResponse { get; set; }

    }
}

