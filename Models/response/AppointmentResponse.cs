using final_project_backend.DTOs;

namespace final_project_backend.Models.response;

public class AppointmentResponse : CommonResponseModel
{
    public AppointmentDto? Data { get; set; }
}

public class AppointmentListResponse : CommonResponseModel
{
    public List<AppointmentDto>? Data { get; set; }
}

public class StaffListResponse : CommonResponseModel
{
    public List<UserDto>? Data { get; set; }
}
