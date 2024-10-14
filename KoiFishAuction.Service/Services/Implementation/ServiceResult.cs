using KoiFishAuction.Service.Services.Interface;

namespace KoiFishAuction.Service.Services.Implementation
{
    public class ServiceResult : IServiceResult
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        public ServiceResult(int status)
        {
            Status = status;
        }
        public ServiceResult(int status, string message)
        {
            Status = status;
            Message = message;
        }

        public ServiceResult(int status, object data)
        {
            Status = status;
            Data = data;
        }
        public ServiceResult(int status, string message, object data)
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}
