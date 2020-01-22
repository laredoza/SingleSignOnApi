using System;
using Microsoft.AspNetCore.Identity;

namespace AdminApi.V1.Dtos
{
    public class RoleDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }

        public RoleDto(IdentityRole role)
        {
            if (role != null)
            {
                this.Id = new Guid(role.Id);
                this.Name = role.Name;
            }
        }

        public void UpdateIdentityRole(IdentityRole role)
        {
            role.Name = this.Name;
        }
    }
}