using IdentityServer4.Models;

namespace AdminApi.V1.Dtos
{
    public class UpdateApiResourceDto : ApiResource
    {
        public string OriginalName { get; set; }
        
    }
}