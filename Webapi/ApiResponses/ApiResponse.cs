namespace EnrollmentSystem.API.ApiResponses
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }

        public ApiResponse(bool success, T data, string message)
        {
            Success = success;
            Data = data;
            Message = message;
        }
    }
    public class ApiResponse : ApiResponse<object>
    {
        public ApiResponse(bool success, object data, string message) : base(success, data, message)
        {
        }
    }

}
