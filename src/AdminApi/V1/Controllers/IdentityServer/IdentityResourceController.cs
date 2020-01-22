// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationDbContextManagementController.cs" company="">
// </copyright>
// <summary>
//  This class is used to manage Identity Resources
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
    /// This class is used to manage Identity Resources. 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]

    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class IdentityResourceController : Controller
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
        /// Initializes a new instance of the <see cref="IdentityResourceController"/> class.
        /// </summary>
        /// <param name="configurationManagementService">
        /// The config context.
        /// </param>
        public IdentityResourceController(IConfigurationManagementService configurationManagementService)
        {
            this._configurationManagementService = configurationManagementService;
        }

        #endregion

        #region Public Methods And Operators
        /// <summary>
        /// Return Identity Resources
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost("ReturnIdentityResources")]
        [Authorize(Policy = "IdentityManagement")]
        public ActionResult<Kendo.DynamicLinq.DataSourceResult> ReturnIdentityResources(DataSourceRequest request)
        {
            return this.Ok(this._configurationManagementService.ReturnIdentityResources(request));
        }

        [HttpPost("AddIdentityResource")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "IdentityManagement")]
        public async Task<ActionResult> AddIdentityResource([FromBody] IdentityResource identityResource)
        {
            if (identityResource.Name == string.Empty)
            {
                return BadRequest();
            }

            int newId = await this._configurationManagementService.AddIdentityResourcesAsync(identityResource);
            return this.CreatedAtAction(nameof(this.ReturnIdentityResource), new { name = identityResource.Name, version = _apiVersion.ToString() }, identityResource);
        }

        [HttpPut("UpdateIdentityResource")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Put))]
        [Authorize(Policy = "IdentityManagement")]
        public async Task<ActionResult> UpdateIdentityResource([FromBody] UpdateIdentityResourceDto identityResource)
        {
            var currentIdentityResource = await this._configurationManagementService.ReturnIdentityResourceAsync(identityResource.OriginalName);

            if (currentIdentityResource == null)
            {
                return NotFound();
            }

            await this._configurationManagementService.UpdateIdentityResourcesAsync(identityResource);

            return NoContent();
        }

        [HttpGet("ReturnIdentityResource/{name}")]
        [Authorize(Policy = "IdentityManagement")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<IdentityResource>> ReturnIdentityResource(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return NotFound();
            }

            return this.Ok(await this._configurationManagementService.ReturnIdentityResourceAsync(name));
        }
        
        #endregion
    }
}