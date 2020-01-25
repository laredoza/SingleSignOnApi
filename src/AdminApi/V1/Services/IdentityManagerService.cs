// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentityManagerService" company="">
//
// </copyright>
// <summary>
//   The class IdentityManagerService.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AdminApi.V1.Services
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AdminApi.V1.Dtos;
    using IdentityServer4.EntityFramework.DbContexts;
    using Kendo.DynamicLinq;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using SingleSignOn.AdminApi.Extensions;
    using SingleSignOn.Data.Context;
    using SingleSignOn.IdentityServerAspNetIdentity.Models;

    #endregion

    /// <summary>
    ///     The IdentityManagerService.
    /// </summary>
    public class IdentityManagerService : IIdentityManagerService
    {
        #region Fields

        /// <summary>
        /// The _user manager.
        /// </summary>
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IServiceScope scope;
        private readonly ServiceProvider serviceProvider;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityManagerService"/> class.
        /// </summary>
        /// <param name="persistedGrantDbContext">
        /// The persisted grant db context.
        /// </param>
        /// <param name="applicationDbContext">
        /// The application db context.
        /// </param>
        /// <param name="userManager">
        /// The user manager.
        /// </param>
        public IdentityManagerService(
            PersistedGrantDbContext persistedGrantDbContext,
            ApplicationDbContext applicationDbContext,
            IConfiguration config
        )
        {
            // Adding this to the startup dependency injection conflicts with identityserver authentication.
            var services = new ServiceCollection();
            // services.AddDbContext<ApplicationDbContext>(options =>
            //     options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

            services.AddApplicationContext(config);

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            serviceProvider = services.BuildServiceProvider();
            scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            _roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
            _userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
        }


        #endregion

        #region Public Properties


        #endregion

        #region Public Methods And Operators

        /// <summary>
        /// Return Roles 
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Kendo.DynamicLinq.DataSourceResult ReturnRoles(AdminApi.V1.Dtos.DataSourceRequest request)
        {
            return this._roleManager.Roles.ToDataSourceResult(request.PageSize, request.Take, request.Sort, request.Filter);
        }

        /// <summary>
        /// Return Roles 
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<List<IdentityRole>> ReturnRoles()
        {
            return await this._roleManager.Roles.ToListAsync();
        }

        /// <summary>
        /// Return users.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Kendo.DynamicLinq.DataSourceResult ReturnUsers(AdminApi.V1.Dtos.DataSourceRequest request)
        {
            var returnedResults = this._userManager.Users.ToDataSourceResult(request.PageSize, request.Take, request.Sort, request.Filter);

            var results = new List<UserDto>();
            foreach (var user in returnedResults.Data)
            {
                results.Add(new UserDto(user as ApplicationUser));
            }

            returnedResults.Data = results;

            return returnedResults;
        }


        public async Task<ApplicationUser> ReturnUserAsync(Guid id)
        {
            return await this._userManager.Users.FirstAsync(u => u.Id == id.ToString());
        }

        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser applicationUser)
        {
            return await this._userManager.UpdateAsync(applicationUser);
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser applicationUser)
        {
            return await this._userManager.CreateAsync(applicationUser);
        }

        public async Task<IdentityResult> AddPasswordAsync(ApplicationUser applicationUser, string newPassword)
        {
            return await this._userManager.AddPasswordAsync(applicationUser, newPassword);
        }

        public async Task<IdentityResult> ChangePasswordAsync(ApplicationUser applicationUser, string currentPassword, string newPassword)
        {
            return await this._userManager.ChangePasswordAsync(applicationUser, currentPassword, newPassword);
        }
        public async Task<IdentityResult> RemovePasswordAsync(ApplicationUser applicationUser)
        {
            return await this._userManager.RemovePasswordAsync(applicationUser);
        }

        public async Task<Kendo.DynamicLinq.DataSourceResult> ReturnClaimsForUserAsync(AdminApi.V1.Dtos.DataSourceRequest request, ApplicationUser user)
        {
            var claims = await this._userManager.GetClaimsAsync(user) as List<Claim>;
            var results = new List<ClaimDto>();

            foreach (var resultClaim in claims)
            {
                results.Add(new ClaimDto(resultClaim));
            }

            return results.AsQueryable().ToDataSourceResult(request.PageSize, request.Take, request.Sort, request.Filter);
        }

        public async Task<IdentityResult> UpdateRoleAsync(IdentityRole role)
        {
            return await this._roleManager.UpdateAsync(role);
        }

        public async Task<IdentityResult> CreateRoleAsync(IdentityRole role)
        {
            return await this._roleManager.CreateAsync(role);
        }

        public async Task<IdentityRole> ReturnRoleAsync(Guid id)
        {
            return await this._roleManager.FindByIdAsync(id.ToString());
        }

        public async Task<List<string>> ReturnRolesForUserAsync(Guid id)
        {
            var user = await this.ReturnUserAsync(id);
            return await this._userManager.GetRolesAsync(user) as List<string>;
        }
        public async Task<List<string>> ReturnRolesToAddForUser(Guid id)
        {
            var user = await this.ReturnUserAsync(id);
            var currentRoles = await this._userManager.GetRolesAsync(user) as List<string>;
            var allRoles = await this.ReturnRoles();
            var allRoleNames = allRoles.Select(a => a.Name).ToList();

            if (allRoleNames.Count > 0)
            {
                foreach (var currentRole in currentRoles)
                {
                    allRoleNames.Remove(currentRole);
                }

                return allRoleNames.OrderBy(a => a).ToList();
            }
            else
            {
                return new List<string>();
            }
        }

        public async Task<IdentityResult> CreateRolesForUserAsync(ApplicationUser applicationUser, List<string> roles)
        {
            return await this._userManager.AddToRolesAsync(applicationUser, roles);
        }

        public async Task<IdentityResult> RemoveRolesForUserAsync(ApplicationUser applicationUser, List<string> roles)
        {
            return await this._userManager.RemoveFromRolesAsync(applicationUser, roles);
        }

        public async Task<IdentityResult> AddClaimToUser(ApplicationUser applicationUser, ClaimDto claim)
        {
            return await this._userManager.AddClaimAsync(applicationUser, new Claim(claim.ClaimType, claim.ClaimValue));
        }
        public async Task<IdentityResult> UpdateUserClaim(ClaimDto claim, ApplicationUser applicationUser, string oldClaimType)  
        {
            var oldClaim = await this.ReturnClaimAsync(oldClaimType, applicationUser);
            return await this._userManager.ReplaceClaimAsync(applicationUser, new Claim(oldClaim.ClaimType, oldClaim.ClaimValue), new Claim(claim.ClaimType, claim.ClaimValue));
        }
        public async Task DeleteUserClaimsAsync(List<ClaimDto> claims, ApplicationUser applicationUser)
        {
            foreach (var claim in claims)
            {
                await this._userManager.RemoveClaimAsync(applicationUser, new Claim(claim.ClaimType, claim.ClaimValue));
            }
        }

        public async Task<ClaimDto> ReturnClaimAsync(string claimType, ApplicationUser applicationUser)
        {
            var claims = await this._userManager.GetClaimsAsync(applicationUser);
            return new ClaimDto(claims.FirstOrDefault(c => c.Type == claimType));
        }

        #endregion

        #region Other Methods

        #endregion
    }
}