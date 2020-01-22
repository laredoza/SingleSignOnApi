// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserRoleController.cs" company="">
//   
// </copyright>
// <summary>
//   The user role management controller.
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
    using SingleSignOn.IdentityServerAspNetIdentity.Models;

    #endregion

    /// <summary>
    /// The user management controller.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class UserRoleController : ControllerBase
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
        public UserRoleController(
            IIdentityManagerService identityService
            )
        {
            this._identityService = identityService;
        }

        #endregion

        #region Public Methods And Operators
        [HttpPost("AddRolesToUser")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "UserManagement")]
        public async Task<ActionResult> AddRolesToUser([FromBody] RolesToUserDto addRolesToUserViewModel)
        {
            if (addRolesToUserViewModel.UserId == Guid.Empty || addRolesToUserViewModel.Roles == null || addRolesToUserViewModel.Roles.Count == 0)
            {
                return BadRequest();
            }

            ApplicationUser currentUser = await this._identityService.ReturnUserAsync(addRolesToUserViewModel.UserId); 

            if (currentUser == null)
            {
                return NotFound();
            }

            return Ok(await this._identityService.CreateRolesForUserAsync(currentUser, addRolesToUserViewModel.Roles));
        }

        [HttpPost("RemoveRolesFromUser")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "UserManagement")]
        public async Task<ActionResult> RemoveRolesFromUser([FromBody] RolesToUserDto removeRolesToUserViewModel)
        {
            if (removeRolesToUserViewModel.UserId == Guid.Empty || removeRolesToUserViewModel.Roles == null || removeRolesToUserViewModel.Roles.Count == 0)
            {
                return BadRequest();
            }

            ApplicationUser currentUser = await this._identityService.ReturnUserAsync(removeRolesToUserViewModel.UserId); 

            if (currentUser == null)
            {
                return NotFound();
            }

            return Ok(await this._identityService.RemoveRolesForUserAsync(currentUser, removeRolesToUserViewModel.Roles));
        }

        /// <summary>
        /// The return roles for user.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet("ReturnRolesForUser/{id}")]
        [Authorize(Policy = "RoleManagement")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<string>> ReturnRolesForUser(Guid id)
        {
            return this.Ok(await this._identityService.ReturnRolesForUserAsync(id));
        }

        /// <summary>
        /// The return roles for user.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet("ReturnRolesToAddForUser/{id}")]
        [Authorize(Policy = "UserManagement")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<string>> ReturnRolesToAddForUser(Guid id)
        {
            return this.Ok(await this._identityService.ReturnRolesToAddForUser(id));
        }

        #endregion
    }
}