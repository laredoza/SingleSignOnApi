using System;
using SingleSignOn.IdentityServerAspNetIdentity.Models;

namespace AdminApi.V1.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockOutEnd { get; set; }
        public bool LockOutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string Password { get; set; }

        public UserDto(ApplicationUser user)
        {
            if (user != null)
            {
                this.Id = new Guid(user.Id);
                this.UserName = user.UserName;
                this.Email = user.Email;
                this.EmailConfirmed = user.EmailConfirmed;
                this.PhoneNumber = user.PhoneNumber;
                this.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                this.TwoFactorEnabled = user.TwoFactorEnabled;
                this.LockOutEnd = user.LockoutEnd;
                this.LockOutEnabled = user.LockoutEnabled;
                this.AccessFailedCount = user.AccessFailedCount;
            }
        }

        public void UpdateApplicationUser(ApplicationUser user)
        {
            user.UserName = this.UserName;
            user.Email = this.Email;
            user.EmailConfirmed = this.EmailConfirmed;
            user.PhoneNumber = this.PhoneNumber;
            user.PhoneNumberConfirmed = this.PhoneNumberConfirmed;
            user.TwoFactorEnabled = this.TwoFactorEnabled;
            user.LockoutEnd = this.LockOutEnd;
            user.LockoutEnabled = this.LockOutEnabled;
            user.AccessFailedCount = this.AccessFailedCount;
        }
    }
}