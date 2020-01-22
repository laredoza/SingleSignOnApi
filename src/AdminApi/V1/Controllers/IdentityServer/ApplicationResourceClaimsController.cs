// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationResourceClaimsController.cs" company="">
// </copyright>
// <summary>
//  This class is used to manage Application Resource Claims
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SingleSignOn.IdentityServerAspNetIdentity.V1.Controllers.IdentityServer
{
    #region Usings

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using global::AdminApi.V1.Services;
    using global::AdminApi.V1.Dtos;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using IdentityServer4.Models;

    #endregion

    /// <summary>
    /// This class is used to manage Application Resource Claims. 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]

    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class ApplicationResourceClaimsController : Controller
    {
        #region Fields

        /// <summary>
        /// The _context.
        /// </summary>
        private readonly IConfigurationManagementService _configurationManagementService;
        private const int _apiVersion = 1;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationResourceClaimsController"/> class.
        /// </summary>
        /// <param name="configurationManagementService">
        /// The config context.
        /// </param>
        public ApplicationResourceClaimsController(IConfigurationManagementService configurationManagementService)
        {
            this._configurationManagementService = configurationManagementService;
        }

        #endregion

        #region Public Methods And Operators

        [HttpPost("ReturnApiResourceClaims/{name}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        [Authorize(Policy = "ApiManagement")]
        public async Task<ActionResult<Kendo.DynamicLinq.DataSourceResult>> ReturnApiResourceClaims(DataSourceRequest request, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return NotFound();
            }

            return this.Ok(await this._configurationManagementService.ReturnApiResourceClaimsAsync(request, name));
        }

        [HttpGet("ReturnApiResourceClaim/{apiClaimName}/{id}")]
        [Authorize(Policy = "ApiManagement")]
        public async Task<ActionResult<ApiClaimDto>> ReturnApiResourceClaim(string apiClaimName, int id)
        {
            return this.Ok(await this._configurationManagementService.ReturnApiResourceClaimAsync(apiClaimName, id));
        }

        [HttpPost("AddApiResourceClaim")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "ApiManagement")]
        public async Task<ActionResult> AddApiResourceClaim([FromBody] ApiClaimDto claim)
        {
            if (claim.Type == string.Empty)
            {
                return BadRequest();
            }

            int newId = await this._configurationManagementService.AddApiResourceClaimAsync(claim);
            return this.CreatedAtAction(nameof(this.ReturnApiResourceClaim), new { ApiClaimName = claim.ApiClaimName, id = newId, version = _apiVersion.ToString() }, claim);
        }

        [HttpPut("UpdateApiResourceClaim")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                             nameof(DefaultApiConventions.Put))]
        [Authorize(Policy = "ApiManagement")]
        public async Task<ActionResult> UpdateApiResourceClaim([FromBody] ApiClaimDto claim)
        {
            var currentIdentityResource = await this._configurationManagementService.ReturnApiResourceClaimAsync(claim.ApiClaimName, claim.Id);

            if (currentIdentityResource == null)
            {
                return NotFound();
            }

            await this._configurationManagementService.UpdateApiResourceClaimAsync(claim);

            return NoContent();
        }

        [HttpPost("RemoveClaimsFromApiResource")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "ApiManagement")]
        public async Task<ActionResult> RemoveClaimsFromApiResource([FromBody] ApiClaimDto[] claims)
        {
            var results = new List<int>();
            foreach (var claim in claims)
            {
                var currentIdentityResource = await this._configurationManagementService.ReturnApiResourceClaimAsync(claim.ApiClaimName, claim.Id);

                if (currentIdentityResource == null)
                {
                    return NotFound();
                }

                await this._configurationManagementService.RemoveClaimFromApiResourceAsync(claim, false);
                results.Add(claim.Id);
            }

            await this._configurationManagementService.SaveChangesAsync();

            return Ok(results);
        }

        #endregion
    }
}