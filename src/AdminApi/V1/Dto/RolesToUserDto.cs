using System;
using System.Collections.Generic;

namespace AdminApi.V1.Dtos
{
    public class RolesToUserDto
    {
        public Guid UserId { get; set; }
        public List<string> Roles { get; set; }
    }
}