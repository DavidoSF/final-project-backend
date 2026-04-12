namespace final_project_backend.Models.response;

public class CommonResponseModel
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
}