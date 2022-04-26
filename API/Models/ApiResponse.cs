namespace API.Models
{
    public class ApiResponse
    {
        public ApiResponse(string _status, string _message, object _result)
        {
            status = _status;
            message = _message;
            result = _result;
        }

        public ApiResponse(string _status, string _message)
        {
            status = _status;
            message = _message;
            result = new { };
        }

        public string status { get; set; }
        public string message { get; set; }
        public object result { get; set; }
    }
}
