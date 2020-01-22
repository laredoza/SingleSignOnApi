// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientScopeController.cs" company="">
// </copyright>
// <summary>
//  This class is used to manage Client Scopes 
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
    /// This class is used to manage Client Scopes. 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]

    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class ClientScopeController : Controller
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
        /// Initializes a new instance of the <see cref="ClientScopeController"/> class.
        /// </summary>
        /// <param name="configurationManagementService">
        /// The config context.
        /// </param>
        public ClientScopeController(IConfigurationManagementService configurationManagementService)
        {
            this._configurationManagementService = configurationManagementService;
        }

        #endregion

        #region Public Methods And Operators
        [HttpPost("RemoveScopeFromClaims")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                             nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult> RemoveScopeFromClaims([FromBody] ClientScopeDto[] scopes)
        {
            var results = new List<int>();
            foreach (var scope in scopes)
            {
                var currentClient = await this._configurationManagementService.ReturnClientAsync(scope.ClientId);

                if (currentClient == null)
                {
                    return NotFound();
                }

                await this._configurationManagementService.RemoveScopeFromClientAsync(scope, false);
                results.Add(scope.Id);
            }

            await this._configurationManagementService.SaveChangesAsync();
            return Ok(results);
        }

        [HttpPost("AddScopesToClient/{id}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult> AddScopesToClient(int id, [FromBody] List<string> scopes)
        {
            if (id == 0 || scopes == null || scopes.Count == 0)
            {
                return BadRequest();
            }

            var currentUser = await this._configurationManagementService.ReturnClientAsync(id);

            if (currentUser == null)
            {
                return NotFound();
            }

            var result = await this._configurationManagementService.AddScopesToClient(id, scopes);
            await this._configurationManagementService.SaveChangesAsync();
            return Ok(result);
        }

        /// <summary>
        /// Return Scopes for Client
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost("ReturnScopesForClientAsync/{clientId}")]
        [Authorize(Policy = "ClientManagement")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<string>> ReturnScopesForClientAsync(DataSourceRequest request, int clientId)
        {
            if (clientId == 0)
            {
                return NotFound();
            }

            return this.Ok(await this._configurationManagementService.ReturnScopesToAddForClient(request, clientId));
        }

        [HttpPost("ReturnClientScopesAsync/{clientId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult<Kendo.DynamicLinq.DataSourceResult>> ReturnClientScopesAsync(DataSourceRequest request, int clientId)
        {
            if (clientId == 0)
            {
                return NotFound();
            }

            return this.Ok(await this._configurationManagementService.ReturnClientScopesAsync(request, clientId));
        }

        #endregion
    }
}