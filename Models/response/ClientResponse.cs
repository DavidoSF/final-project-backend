using final_project_backend.DTOs;

namespace final_project_backend.Models.response;

public class ClientResponse : CommonResponseModel
{
    public ClientDto? Data { get; set; }
}

public class ClientListResponse : CommonResponseModel
{
    public List<ClientDto>? Data { get; set; }
}
