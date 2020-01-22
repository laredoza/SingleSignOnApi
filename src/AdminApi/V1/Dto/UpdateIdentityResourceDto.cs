using IdentityServer4.Models;

namespace AdminApi.V1.Dtos
{
    public class UpdateIdentityResourceDto : IdentityResource
    {
        public string OriginalName { get; set; }
        
    }
}