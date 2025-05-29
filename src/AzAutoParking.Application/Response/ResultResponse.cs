namespace AzAutoParking.Application.Response;

public class ResultResponse<T>
{
    public int StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public T Data { get; set; }
    public List<LocalizedMessage>? Messages { get; set; }

    public ResultResponse<T> Success(int statusCode, T data)
    {
        return new ResultResponse<T>
        {
            StatusCode = statusCode,
            IsSuccess = true,
            Data = data,
        };
    }

    public ResultResponse<T> Fail(int statusCode, List<LocalizedMessage> messages)
    {
        return new ResultResponse<T>
        {
            StatusCode = statusCode,
            IsSuccess = false,
            Messages = messages,
        };
    }
}