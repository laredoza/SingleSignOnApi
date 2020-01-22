// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientCorsController.cs" company="">
// </copyright>
// <summary>
//  This class is used to manage Client Core Uri 
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
    /// This class is used to manage Client Cors Uri. 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]

    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class ClientCorsController : Controller
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
        /// Initializes a new instance of the <see cref="ClientCorsController"/> class.
        /// </summary>
        /// <param name="configurationManagementService">
        /// The config context.
        /// </param>
        public ClientCorsController(IConfigurationManagementService configurationManagementService)
        {
            this._configurationManagementService = configurationManagementService;
        }

        #endregion

        #region Public Methods And Operators

        [HttpPost("RemoveCorsOriginFromClaims")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult> RemoveCorsOriginFromClaims([FromBody] ClientAllowedCorsUrlDto[] allowedCorsUris)
        {
            var results = new List<int>();
            foreach (var allowedCorsUri in allowedCorsUris)
            {
                var currentClient = await this._configurationManagementService.ReturnClientAsync(allowedCorsUri.ClientId);

                if (currentClient == null)
                {
                    return NotFound();
                }

                await this._configurationManagementService.RemoveCorsOriginFromClaims(allowedCorsUri);
                results.Add(allowedCorsUri.Id);
            }

            await this._configurationManagementService.SaveChangesAsync();
            return Ok(results);
        }

        [HttpPut("UpdateClientCorsOriginAsync")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Put))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult<int>> UpdateClientCorsOriginAsync([FromBody] ClientAllowedCorsUrlDto allowedCorsUri)
        {
            var currentClient = await this._configurationManagementService.ReturnClientAsync(allowedCorsUri.ClientId);

            if (currentClient == null)
            {
                return NotFound();
            }

            await this._configurationManagementService.UpdateClientCorsOriginAsync(allowedCorsUri);
            await this._configurationManagementService.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("AddCorsOriginToClient/{clientId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult> AddCorsOriginToClient(int clientId, [FromBody] ClientAllowedCorsUrlDto corsAllowedUriDto)
        {
            if (clientId == 0 || corsAllowedUriDto == null)
            {
                return BadRequest();
            }

            var currentClient = await this._configurationManagementService.ReturnClientAsync(clientId); 

            if (currentClient == null)
            {
                return NotFound();
            }

            var result = await this._configurationManagementService.AddCorsOriginToClient(clientId, corsAllowedUriDto);
            await this._configurationManagementService.SaveChangesAsync();
            return Ok(result);
        }

        [HttpGet("ReturnClientCorsUrisAsync/{clientId}/{corsOriginUriId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult<ClientAllowedCorsUrlDto>> ReturnClientCorsUrisAsync(int clientId, int corsOriginUriId)
        {
            if (clientId == 0 || corsOriginUriId == 0)
            {
                return BadRequest();
            }

            var currentClient = await this._configurationManagementService.ReturnClientAsync(clientId); 

            if (currentClient == null)
            {
                return NotFound();
            }

            var result = await this._configurationManagementService.ReturnClientCorsUrisAsync(clientId, corsOriginUriId);
            return Ok(result);
        }

        [HttpPost("ReturnClientCorsUrisAsync/{clientId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult<Kendo.DynamicLinq.DataSourceResult>> ReturnClientCorsUrisAsync(DataSourceRequest request, int clientId)
        {
            if (clientId == 0)
            {
                return NotFound();
            }

            return this.Ok(await this._configurationManagementService.ReturnClientCorsUrisAsync(request, clientId));
        }

        #endregion
    }
}