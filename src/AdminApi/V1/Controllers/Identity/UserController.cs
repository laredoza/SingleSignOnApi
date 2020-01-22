// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserController.cs" company="">
//   
// </copyright>
// <summary>
//   The user management controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SingleSignOn.IdentityServerAspNetIdentity.V1.Controllers.Identity
{
    #region Usings

    using System.Linq;
    using System.Threading.Tasks;
    using global::AdminApi.V1.Services;
    using global::AdminApi.V1.Dtos;
    using System;
    using System.Collections.Generic;
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
    public class UserController : ControllerBase
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
        public UserController(
            IIdentityManagerService identityService
            )
        {
            this._identityService = identityService;
        }

        #endregion

        #region Public Methods And Operators

        /// <summary>
        /// The return users.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost("ReturnUsers")]
        [Authorize(Policy = "UserManagement")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        public ActionResult<Kendo.DynamicLinq.DataSourceResult> ReturnUsers(DataSourceRequest request)
        {
            return this.Ok(this._identityService.ReturnUsers(request));
        }

        /// <summary>
        /// The return users.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet("ReturnUser/{id}")]
        [Authorize(Policy = "UserManagement")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<UserDto>> ReturnUser(Guid id)
        {
            var user = await this._identityService.ReturnUserAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return this.Ok(new UserDto(user));
        }

        [HttpPut("UpdateUser")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Put))]
        [Authorize(Policy = "UserManagement")]
        public async Task<ActionResult> UpdateUser([FromBody] UserDto userViewModel)
        {
            if (userViewModel.Id == Guid.Empty)
            {
                return BadRequest();
            }

            var currentUser = await this._identityService.ReturnUserAsync(userViewModel.Id);

            if (currentUser == null)
            {
                return NotFound();
            }

            userViewModel.UpdateApplicationUser(currentUser);

            await this._identityService.UpdateUserAsync(currentUser);

            return NoContent();
        }

        [HttpPost("AddUser")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "UserManagement")]
        public async Task<ActionResult> AddUser([FromBody] UserDto userViewModel)
        {
            ApplicationUser newUser = new ApplicationUser();
            userViewModel.UpdateApplicationUser(newUser);
            var createUserResult = await this._identityService.CreateUserAsync(newUser);

            if (createUserResult.Succeeded)
            {
                var addPasswordResult = await this._identityService.AddPasswordAsync(newUser, userViewModel.Password);

                if (addPasswordResult.Succeeded)
                {
                    // return CreatedAtAction(nameof(ReturnUser), new { id = userViewModel.Id }, userViewModel);
                    return Ok();
                }
            }

            throw new Exception("Failed to add User");
        }

        [HttpPut("ChangeUserPassword")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Put))]
        [Authorize(Policy = "UserManagement")]
        public async Task<ActionResult> ChangeUserPassword([FromBody] UserDto userViewModel)
        {
            if (userViewModel.Password == string.Empty || userViewModel.Id == Guid.Empty)
            {
                return BadRequest();
            }

            var currentUser = await this._identityService.ReturnUserAsync(userViewModel.Id);

            if (currentUser == null)
            {
                return NotFound();
            }

            await this._identityService.RemovePasswordAsync(currentUser);
            await this._identityService.AddPasswordAsync(currentUser, userViewModel.Password);

            return NoContent();
        }

        #endregion
    }
}