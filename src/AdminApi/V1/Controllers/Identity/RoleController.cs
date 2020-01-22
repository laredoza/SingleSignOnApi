// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RoleController.cs" company="">
//   
// </copyright>
// <summary>
//   The role management controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SingleSignOn.IdentityServerAspNetIdentity.V1.Controllers.Identity
{
    #region Usings

    using System.Threading.Tasks;
    using global::AdminApi.V1.Services;
    using global::AdminApi.V1.Dtos;
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    #endregion

    /// <summary>
    /// The role management controller.
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class RoleController : ControllerBase
    {
        #region Fields
        private readonly IIdentityManagerService _identityService;
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleController"/> class.
        /// </summary>
        /// <param name="identityService">
        /// The identity service.
        /// </param>
        public RoleController(
            IIdentityManagerService identityService
            )
        {
            this._identityService = identityService;
        }

        #endregion

        #region Public Methods And Operators

        /// <summary>
        /// Return Roles 
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost("ReturnRoles")]
        [Authorize(Policy = "RoleManagement")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public ActionResult<Kendo.DynamicLinq.DataSourceResult> ReturnRoles(DataSourceRequest request)
        {
            List<RoleDto> resultDtos = new List<RoleDto>();

            var result = this._identityService.ReturnRoles(request);

            foreach (var role in result.Data)
            {
                resultDtos.Add(new RoleDto(role as IdentityRole));
            }

            result.Data = resultDtos;
            return this.Ok(result);
        }

        [HttpPost("AddRole")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "RoleManagement")]
        public async Task<ActionResult> AddRole([FromBody] RoleDto roleViewModel)
        {
            IdentityRole newRole = new IdentityRole();
            roleViewModel.UpdateIdentityRole(newRole);
            var createUserResult = await this._identityService.CreateRoleAsync(newRole);

            if (createUserResult.Succeeded)
            {
                // return CreatedAtAction(nameof(ReturnRole), new { id = newRole.Id }, newRole);
                return Ok();
            }

            throw new Exception("Failed to add Role");
        }

        [HttpPut("UpdateRole")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Put))]
        [Authorize(Policy = "RoleManagement")]
        public async Task<ActionResult> UpdateRole([FromBody] RoleDto roleViewModel)
        {
            var currentRole = await this._identityService.ReturnRoleAsync(roleViewModel.Id.Value);

            if (currentRole == null)
            {
                return NotFound();
            }

            roleViewModel.UpdateIdentityRole(currentRole);

            await this._identityService.UpdateRoleAsync(currentRole);

            return NoContent();
        }

        /// <summary>
        /// The return users.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpGet("ReturnRole/{id}")]
        [Authorize(Policy = "RoleManagement")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<RoleDto>> ReturnRole(Guid id)
        {
            var role = await this._identityService.ReturnRoleAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            return this.Ok(new RoleDto(role));
        }

        #endregion
    }
}