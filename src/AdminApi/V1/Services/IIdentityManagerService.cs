// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIdentityManagerService" company="">
//
// </copyright>
// <summary>
//   The class IIdentityManagerService.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AdminApi.V1.Services
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AdminApi.V1.Dtos;
    using Microsoft.AspNetCore.Identity;
    using SingleSignOn.IdentityServerAspNetIdentity.Models;

    #endregion

    /// <summary>
    ///     The IIdentityManagerService.
    /// </summary>
    public interface IIdentityManagerService
    {
        #region Public Properties


        #endregion

        #region Public Methods And Operators
        Kendo.DynamicLinq.DataSourceResult ReturnUsers(DataSourceRequest request);

        /// <summary>
        /// Return users.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<ApplicationUser> ReturnUserAsync(Guid id);

        /// <summary>
        /// Return Roles 
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Kendo.DynamicLinq.DataSourceResult ReturnRoles(DataSourceRequest request);
        Task<IdentityResult> UpdateUserAsync(ApplicationUser applicationUser);
        Task<IdentityResult> CreateUserAsync(ApplicationUser applicationUser);
        Task<IdentityResult> AddPasswordAsync(ApplicationUser applicationUser, string newPassword);
        Task<IdentityResult> ChangePasswordAsync(ApplicationUser applicationUser, string currentPassword, string newPassword);
        Task<IdentityResult> RemovePasswordAsync(ApplicationUser applicationUser);
        Task<Kendo.DynamicLinq.DataSourceResult> ReturnClaimsForUserAsync(AdminApi.V1.Dtos.DataSourceRequest request, ApplicationUser user);
        Task<IdentityResult> UpdateRoleAsync(IdentityRole role);
        Task<IdentityResult> CreateRoleAsync(IdentityRole role);
        Task<IdentityRole> ReturnRoleAsync(Guid id);
        Task<List<string>> ReturnRolesForUserAsync(Guid id);
        Task<List<string>> ReturnRolesToAddForUser(Guid id);
        Task<IdentityResult> CreateRolesForUserAsync(ApplicationUser applicationUser, List<string> roles);
        Task<IdentityResult> RemoveRolesForUserAsync(ApplicationUser applicationUser, List<string> roles);
        Task<IdentityResult> AddClaimToUser(ApplicationUser applicationUser, ClaimDto claim);
        Task<IdentityResult> UpdateUserClaim(ClaimDto claim, ApplicationUser applicationUser, string oldClaimType); 
        Task DeleteUserClaimsAsync(List<ClaimDto> claims, ApplicationUser applicationUser); 
        Task<ClaimDto> ReturnClaimAsync(string claimType, ApplicationUser applicationUser);

        #endregion

        #region Other Methods

        #endregion
    }
}