namespace KoiFishAuction.Service.Services
{
    public class ServiceResult<T>
    {
        public int Status { get; set; }
        public string? Message { get; set; }
        public T Data { get; set; }

        public ServiceResult() { }

        public ServiceResult(int status)
        {
            Status = status;
        }

        public ServiceResult(int status, string message)
        {
            Status = status;
            Message = message;
        }

        public ServiceResult(int status, T data)
        {
            Status = status;
            Data = data;
        }

        public ServiceResult(int status, string message, T data)
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }

}
