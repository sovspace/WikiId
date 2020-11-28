namespace Application.Features.Common
{
    public abstract class BaseResponse
    {
        public string Message { get; set; } = "Ok";
        public bool IsSuccessful { get; set; }
    }
}
