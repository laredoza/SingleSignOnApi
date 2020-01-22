// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientRedirectController.cs" company="">
// </copyright>
// <summary>
//  This class is used to manage Client Redirect Uri 
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
    /// This class is used to manage Client Client Redirect Uri Controller. 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]

    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class ClientRedirectController : Controller
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
        /// Initializes a new instance of the <see cref="ClientRedirectController"/> class.
        /// </summary>
        /// <param name="configurationManagementService">
        /// The config context.
        /// </param>
        public ClientRedirectController(IConfigurationManagementService configurationManagementService)
        {
            this._configurationManagementService = configurationManagementService;
        }

        #endregion

        #region Public Methods And Operators

        [HttpPut("UpdateClientRedirectUriAsync")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                             nameof(DefaultApiConventions.Put))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult<int>> UpdateClientRedirectUriAsync([FromBody] ClientRedirectUrlDto redirectUri)
        {
            var currentClient = await this._configurationManagementService.ReturnClientAsync(redirectUri.ClientId);

            if (currentClient == null)
            {
                return NotFound();
            }

            await this._configurationManagementService.UpdateClientRedirectUriAsync(redirectUri);
            await this._configurationManagementService.SaveChangesAsync();

            return NoContent();
        }


        [HttpGet("ReturnClientRedirectUriAsync/{clientId}/{redirectUriId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult<ClientRedirectUrlDto>> ReturnClientRedirectUriAsync(int clientId, int redirectUriId)
        {
            if (clientId == 0 || redirectUriId == 0)
            {
                return BadRequest();
            }

            var currentClient = await this._configurationManagementService.ReturnClientAsync(clientId);

            if (currentClient == null)
            {
                return NotFound();
            }

            var result = await this._configurationManagementService.ReturnClientRedirectUriAsync(clientId, redirectUriId);
            return Ok(result);
        }


        [HttpPost("RemoveRedirectUrisFromClaims")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult> RemoveRedirectUrisFromClaims([FromBody] ClientRedirectUrlDto[] redirectUrlDtos)
        {
            var results = new List<int>();
            foreach (var redirectUri in redirectUrlDtos)
            {
                var currentClient = await this._configurationManagementService.ReturnClientAsync(redirectUri.ClientId);

                if (currentClient == null)
                {
                    return NotFound();
                }

                await this._configurationManagementService.RemoveRedirectUrisFromClaimsAsync(redirectUri);
                results.Add(redirectUri.Id);
            }

            await this._configurationManagementService.SaveChangesAsync();
            return Ok(results);
        }

        [HttpPost("AddRedirectUriToClient/{clientId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult> AddRedirectUriToClient(int clientId, [FromBody] ClientRedirectUrlDto redirectUriDto)
        {
            if (clientId == 0 || redirectUriDto == null)
            {
                return BadRequest();
            }

            var currentClient = await this._configurationManagementService.ReturnClientAsync(clientId);

            if (currentClient == null)
            {
                return NotFound();
            }

            var result = await this._configurationManagementService.AddRedirectUriToClient(clientId, redirectUriDto);
            await this._configurationManagementService.SaveChangesAsync();
            return Ok(result);
        }

        [HttpPost("ReturnClientRedirectUrisAsync/{clientId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult<Kendo.DynamicLinq.DataSourceResult>> ReturnClientRedirectUrisAsync(DataSourceRequest request, int clientId)
        {
            if (clientId == 0)
            {
                return NotFound();
            }

            return this.Ok(await this._configurationManagementService.ReturnClientRedirectUrisAsync(request, clientId));
        }

        #endregion
    }
}