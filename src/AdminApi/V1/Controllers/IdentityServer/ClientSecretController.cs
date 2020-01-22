// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationDbContextManagementController.cs" company="">
// </copyright>
// <summary>
//  This class is used to manage Client Secrets 
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
    /// This class is used to manage Client Secrets. 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]

    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class ClientSecretController : Controller
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
        /// Initializes a new instance of the <see cref="ClientSecretController"/> class.
        /// </summary>
        /// <param name="configurationManagementService">
        /// The config context.
        /// </param>
        public ClientSecretController(IConfigurationManagementService configurationManagementService)
        {
            this._configurationManagementService = configurationManagementService;
        }

        #endregion

        #region Public Methods And Operators

        [HttpPost("RemoveSecretsFromClaims")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult> RemoveSecretsFromClaims([FromBody] ClientSecretDto[] secrets)
        {
            var results = new List<int>();
            foreach (var secret in secrets)
            {
                var currentClient = await this._configurationManagementService.ReturnClientAsync(secret.ClientId);

                if (currentClient == null)
                {
                    return NotFound();
                }

                await this._configurationManagementService.RemoveSecretFromClaims(secret);
                results.Add(secret.Id);
            }

            await this._configurationManagementService.SaveChangesAsync();
            return Ok(results);
        }

        [HttpPost("AddSecretToClient/{clientId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult> AddSecretToClient(int clientId, [FromBody] ClientSecretDto secret)
        {
            if (clientId == 0 || secret == null)
            {
                return BadRequest();
            }

            var currentClient = await this._configurationManagementService.ReturnClientAsync(clientId); 

            if (currentClient == null)
            {
                return NotFound();
            }

            var result = await this._configurationManagementService.AddSecretToClient(clientId, secret);
            await this._configurationManagementService.SaveChangesAsync();
            return Ok(result);
        }

        [HttpPost("ReturnClientSecretsAsync/{clientId}")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult<Kendo.DynamicLinq.DataSourceResult>> ReturnClientSecretsAsync(DataSourceRequest request, int clientId)
        {
            if (clientId == 0)
            {
                return NotFound();
            }

            return this.Ok(await this._configurationManagementService.ReturnClientSecretsAsync(request, clientId));
        }

        #endregion
    }
}