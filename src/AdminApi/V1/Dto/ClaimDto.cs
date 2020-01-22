using System.Security.Claims;

namespace AdminApi.V1.Dtos
{
    public class ClaimDto
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }

        public ClaimDto()
        {

        }

        public ClaimDto(Claim claim)
        {
            this.ClaimType = claim.Type;
            this.ClaimValue = claim.Value;
        }
    }
}