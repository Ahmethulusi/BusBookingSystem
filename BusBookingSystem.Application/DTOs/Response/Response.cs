namespace BusBookingSystem.Application.DTOs.Response
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public T? Body { get; set; }
        public string Message { get; set; }

        public Response(bool success, T? body, string message)
        {
            Success = success;
            Body = body;
            Message = message;
        }

        public static Response<T> Successful(T body, string message = "")
        {
            return new Response<T>(true, body, message);
        }

        public static Response<T> Fail(string message)
        {
            return new Response<T>(false, default, message);
        }
    }
}
