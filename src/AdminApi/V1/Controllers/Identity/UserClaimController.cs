// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserClaimController.cs" company="">
//   
// </copyright>
// <summary>
//   The user claims management controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SingleSignOn.IdentityServerAspNetIdentity.V1.Controllers.Identity
{
    #region Usings

    using System.Threading.Tasks;
    using global::AdminApi.V1.Services;
    using global::AdminApi.V1.Dtos;
    using System;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// The user claim management controller.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("api/v{version:apiVersion}/[controller]")]
    // [Authorize]
    public class UserClaimController : ControllerBase
    {
        #region Fields
        private readonly IIdentityManagerService _identityService;
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="identityService">
        /// The identity service.
        /// </param>
        public UserClaimController(
            IIdentityManagerService identityService
            )
        {
            this._identityService = identityService;
        }

        #endregion

        #region Public Methods And Operators

        [HttpPost("ReturnClaimsForUser/{id}")]
        [Authorize(Policy = "UserManagement")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                      nameof(DefaultApiConventions.Post))]
        public async Task<ActionResult<Kendo.DynamicLinq.DataSourceResult>> ReturnClaimsForUser(DataSourceRequest request, Guid id)
        {
            var user = await this._identityService.ReturnUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return this.Ok(await _identityService.ReturnClaimsForUserAsync(request, user));
        }

        [HttpPost("AddClaimToUser/{userId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "UserManagement")]
        public async Task<ActionResult> AddClaimToUser([FromBody] ClaimDto claim, Guid userId)
        {
            var user = await this._identityService.ReturnUserAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var addClaimResult = await this._identityService.AddClaimToUser(user, claim);

            if (addClaimResult.Succeeded)
            {
                // return CreatedAtAction(nameof(ReturnClaimForUser), new { claimType = claim.ClaimType, id = userId }, claim);
                return Ok();
            }

            throw new Exception("Failed to add User Claim");
        }

        [HttpPut("UpdateUserClaim/{userId}/{oldClaimType}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Put))]
        [Authorize(Policy = "UserManagement")]
        public async Task<ActionResult> UpdateUserClaim([FromBody] ClaimDto claim, Guid userId, string oldClaimType)
        {
            var user = await this._identityService.ReturnUserAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var addClaimResult = await this._identityService.UpdateUserClaim(claim, user, oldClaimType);

            if (addClaimResult.Succeeded)
            {
                // return CreatedAtAction(nameof(ReturnClaimForUser), new { claimType = claim.ClaimType, id = userId }, claim);
                return Ok();
            }

            throw new Exception("Failed to update User Claim");
        }

        [HttpGet("ReturnClaimForUser/{claimType}/{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        // [Authorize(Policy = "UserManagement")]
        public async Task<ActionResult<ClaimDto>> ReturnClaimForUser(string claimType, Guid id)
        {
            var user = await this._identityService.ReturnUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(await this._identityService.ReturnClaimAsync(claimType, user));
        }

        [HttpPost("DeleteUserClaims/{userId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "UserManagement")]
        public async Task<ActionResult> DeleteUserClaims([FromBody] List<ClaimDto> claims, Guid userId)
        {
            var user = await this._identityService.ReturnUserAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            await this._identityService.DeleteUserClaimsAsync(claims, user);
            return Ok();
        }

        #endregion
    }
}