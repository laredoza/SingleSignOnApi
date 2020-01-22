// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationResourceController.cs" company="">
// </copyright>
// <summary>
//  This class is used to manage Application Resources
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
    /// This class is used to manage Application Resources. 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]

    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class ApplicationResourceController : Controller
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
        /// Initializes a new instance of the <see cref="ApplicationResourceController"/> class.
        /// </summary>
        /// <param name="configurationManagementService">
        /// The config context.
        /// </param>
        public ApplicationResourceController(IConfigurationManagementService configurationManagementService)
        {
            this._configurationManagementService = configurationManagementService;
        }

        #endregion

        #region Public Methods And Operators
        /// <summary>
        /// Return Api Resources
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost("ReturnApiResources")]
        [Authorize(Policy = "ApiManagement")]
        public ActionResult<Kendo.DynamicLinq.DataSourceResult> ReturnApiResources(DataSourceRequest request)
        {
            return this.Ok(this._configurationManagementService.ReturnApiResources(request));
        }

        [HttpGet("ReturnApiResource/{name}")]
        [Authorize(Policy = "ApiManagement")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<ApiResource>> ReturnApiResource(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return NotFound();
            }

            return this.Ok(await this._configurationManagementService.ReturnApiResourceAsync(name));
        }

        [HttpPost("AddApiResource")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "ApiManagement")]
        public async Task<ActionResult> AddApiResource([FromBody] ApiResource apiResource)
        {
            if (apiResource.Name == string.Empty)
            {
                return BadRequest();
            }

            int newId = await this._configurationManagementService.AddApiResourceAsync(apiResource);
            return this.CreatedAtAction(nameof(this.ReturnApiResource), new { name = apiResource.Name, version = _apiVersion.ToString() }, apiResource);
        }

        [HttpPut("UpdateApiResource")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Put))]
        [Authorize(Policy = "ApiManagement")]
        public async Task<ActionResult> UpdateApiResource([FromBody] UpdateApiResourceDto apiResource)
        {
            var currentIdentityResource = await this._configurationManagementService.ReturnApiResourceAsync(apiResource.OriginalName);

            if (currentIdentityResource == null)
            {
                return NotFound();
            }

            await this._configurationManagementService.UpdateApiResourceAsync(apiResource);

            return NoContent();
        }

        #endregion
    }
}