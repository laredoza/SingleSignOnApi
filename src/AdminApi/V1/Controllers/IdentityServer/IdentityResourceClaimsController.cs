// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentityResourceClaimsController.cs" company="">
// </copyright>
// <summary>
//  This class is used to manage Identity Resources Claims
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

    #endregion

    /// <summary>
    /// This class is used to manage Identity Resources Claims. 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]

    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class IdentityResourceClaimsController : Controller
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
        /// Initializes a new instance of the <see cref="IdentityResourceClaimsController"/> class.
        /// </summary>
        /// <param name="configurationManagementService">
        /// The config context.
        /// </param>
        public IdentityResourceClaimsController(IConfigurationManagementService configurationManagementService)
        {
            this._configurationManagementService = configurationManagementService;
        }

        #endregion

        #region Public Methods And Operators
        [HttpPost("ReturnIdentityResourceClaims/{name}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                             nameof(DefaultApiConventions.Get))]
        [Authorize(Policy = "IdentityManagement")]
        public async Task<ActionResult<Kendo.DynamicLinq.DataSourceResult>> ReturnIdentityResourceClaimsAsync(DataSourceRequest request, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return NotFound();
            }

            return this.Ok(await this._configurationManagementService.ReturnIdentityResourceClaimsAsync(request, name));
        }

        [HttpPost("AddIdentityResourceClaim")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "IdentityManagement")]
        public async Task<ActionResult> AddIdentityResourceClaim([FromBody] IdentityClaimDto claim)
        {
            if (claim.Type == string.Empty)
            {
                return BadRequest();
            }

            int newId = await this._configurationManagementService.AddIdentityResourceClaimAsync(claim);
            return this.CreatedAtAction(nameof(this.ReturnIdentityResourceClaim), new { identityClaimName = claim.IdentityClaimName, id = newId, version = _apiVersion.ToString() }, claim);
        }

        [HttpGet("ReturnIdentityResourceClaim/{identityClaimName}/{id}")]
        [Authorize(Policy = "IdentityManagement")]
        public async Task<ActionResult<ApiClaimDto>> ReturnIdentityResourceClaim(string identityClaimName, int id)
        {
            return this.Ok(await this._configurationManagementService.ReturnIdentityResourceClaimAsync(identityClaimName, id));
        }

        [HttpPut("UpdateIdentityResourceClaim")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Put))]
        [Authorize(Policy = "IdentityManagement")]
        public async Task<ActionResult> UpdateIdentityResourceClaim([FromBody] IdentityClaimDto claim)
        {
            var currentIdentityResource = await this._configurationManagementService.ReturnIdentityResourceClaimAsync(claim.IdentityClaimName, claim.Id);

            if (currentIdentityResource == null)
            {
                return NotFound();
            }

            await this._configurationManagementService.UpdateIdentityResourceClaimAsync(claim);

            return NoContent();
        }

        [HttpPost("RemoveClaimsFromIdentityResource")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "IdentityManagement")]
        public async Task<ActionResult> RemoveClaimsFromIdentityResource([FromBody] IdentityClaimDto[] claims)
        {
            var results = new List<int>();
            foreach (var claim in claims)
            {
                var currentIdentityResource = await this._configurationManagementService.ReturnIdentityResourceClaimAsync(claim.IdentityClaimName, claim.Id);

                if (currentIdentityResource == null)
                {
                    return NotFound();
                }

                await this._configurationManagementService.RemoveClaimFromIdentityResourceAsync(claim, false);
                results.Add(claim.Id);
            }

            await this._configurationManagementService.SaveChangesAsync();

            return Ok(results);
        }

        #endregion
    }
}