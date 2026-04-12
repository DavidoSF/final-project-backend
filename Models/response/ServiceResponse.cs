using final_project_backend.DTOs;

namespace final_project_backend.Models.response;

public class ServiceResponse : CommonResponseModel
{
    public ServiceDto? Data { get; set; }
}

public class ServiceListResponse : CommonResponseModel
{
    public List<ServiceDto>? Data { get; set; }
}
