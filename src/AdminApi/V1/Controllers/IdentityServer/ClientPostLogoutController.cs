// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientPostLogoutController.cs" company="">
// </copyright>
// <summary>
//  This class is used to manage Client Post Logout Uri 
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
    /// This class is used to manage Client Post Logout Uri. 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]

    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class ClientPostLogoutController : Controller
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
        /// Initializes a new instance of the <see cref="ClientPostLogoutController"/> class.
        /// </summary>
        /// <param name="configurationManagementService">
        /// The config context.
        /// </param>
        public ClientPostLogoutController(IConfigurationManagementService configurationManagementService)
        {
            this._configurationManagementService = configurationManagementService;
        }

        #endregion

        #region Public Methods And Operators
        [HttpPost("RemovePostLogoutsFromClaims")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                             nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult> RemovePostLogoutsFromClaims([FromBody] ClientPostLogoutUrlDto[] postLogoutUriDtos)
        {
            var results = new List<int>();
            foreach (var postLogoutUri in postLogoutUriDtos)
            {
                var currentClient = await this._configurationManagementService.ReturnClientAsync(postLogoutUri.ClientId);

                if (currentClient == null)
                {
                    return NotFound();
                }

                await this._configurationManagementService.RemovePostLogoutsFromClaims(postLogoutUri);
                results.Add(postLogoutUri.Id);
            }

            await this._configurationManagementService.SaveChangesAsync();
            return Ok(results);
        }

        [HttpPut("UpdateClientPostLogoutUriAsync")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Put))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult<int>> UpdateClientPostLogoutUriAsync([FromBody] ClientPostLogoutUrlDto clientPostLogoutDto)
        {
            var currentClient = await this._configurationManagementService.ReturnClientAsync(clientPostLogoutDto.ClientId);

            if (currentClient == null)
            {
                return NotFound();
            }

            await this._configurationManagementService.UpdateClientPostLogoutUriAsync(clientPostLogoutDto);
            await this._configurationManagementService.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("AddPostLogoutUriToClient/{clientId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult> AddPostLogoutUriToClient(int clientId, [FromBody] ClientPostLogoutUrlDto clientPostLogoutDto)
        {
            if (clientId == 0 || clientPostLogoutDto == null)
            {
                return BadRequest();
            }

            var currentClient = await this._configurationManagementService.ReturnClientAsync(clientId);

            if (currentClient == null)
            {
                return NotFound();
            }

            var result = await this._configurationManagementService.AddPostLogoutUriToClient(clientId, clientPostLogoutDto);
            await this._configurationManagementService.SaveChangesAsync();
            return Ok(result);
        }

        [HttpGet("ReturnClientPostLogoutUriAsync/{clientId}/{postLogoutUriId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult<ClientRedirectUrlDto>> ReturnClientPostLogoutUriAsync(int clientId, int postLogoutUriId)
        {
            if (clientId == 0 || postLogoutUriId == 0)
            {
                return BadRequest();
            }

            var currentClient = await this._configurationManagementService.ReturnClientAsync(clientId);

            if (currentClient == null)
            {
                return NotFound();
            }

            var result = await this._configurationManagementService.ReturnClientPostLogoutUriAsync(clientId, postLogoutUriId);
            return Ok(result);
        }

        [HttpPost("ReturnClientPostLogoutUrisAsync/{clientId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult<Kendo.DynamicLinq.DataSourceResult>> ReturnClientPostLogoutUrisAsync(DataSourceRequest request, int clientId)
        {
            if (clientId == 0)
            {
                return NotFound();
            }

            return this.Ok(await this._configurationManagementService.ReturnClientPostLogoutUrisAsync(request, clientId));
        }

        #endregion
    }
}