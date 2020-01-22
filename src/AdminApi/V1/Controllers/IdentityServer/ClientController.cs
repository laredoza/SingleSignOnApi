// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientController.cs" company="">
// </copyright>
// <summary>
//  This class is used to manage Clients 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SingleSignOn.IdentityServerAspNetIdentity.V1.Controllers.IdentityServer
{
    #region Usings

    using System.Threading.Tasks;
    using global::AdminApi.V1.Services;
    using global::AdminApi.V1.Dtos;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    #endregion

    /// <summary>
    /// This class is used to manage Clients. 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]

    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class ClientController : Controller
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
        /// Initializes a new instance of the <see cref="ClientController"/> class.
        /// </summary>
        /// <param name="configurationManagementService">
        /// The config context.
        /// </param>
        public ClientController(IConfigurationManagementService configurationManagementService)
        {
            this._configurationManagementService = configurationManagementService;
        }

        #endregion

        #region Public Methods And Operators
        [HttpPost("AddClientAsync")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                             nameof(DefaultApiConventions.Post))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult> AddClientAsync([FromBody] ClientDto client)
        {
            if (client is null)
            {
                return BadRequest();
            }

            int newId = await this._configurationManagementService.AddClientAsync(client);
            return this.CreatedAtAction(nameof(this.ReturnClientAsync), new { id = client.Id, version = _apiVersion.ToString() }, client);
        }

        [HttpPut("UpdateClientAsync")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Put))]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult> UpdateClientAsync([FromBody] ClientDto client)
        {
            var currentClient = await this._configurationManagementService.ReturnClientAsync(client.Id);

            if (currentClient == null)
            {
                return NotFound();
            }

            await this._configurationManagementService.UpdateClientAsync(client);

            return NoContent();
        }

        [HttpGet("ReturnClientAsync/{id}")]
        [Authorize(Policy = "ClientManagement")]
        [ApiConventionMethod(typeof(DefaultApiConventions),
                     nameof(DefaultApiConventions.Get))]
        public async Task<ActionResult<ClientDto>> ReturnClientAsync(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            return this.Ok(await this._configurationManagementService.ReturnClientAsync(id));
        }

        /// <summary>
        /// The return clients.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        [HttpPost("ReturnClients")]
        [Authorize(Policy = "ClientManagement")]
        public async Task<ActionResult<Kendo.DynamicLinq.DataSourceResult>> ReturnClients(DataSourceRequest request)
        {
            return this.Ok(await this._configurationManagementService.ReturnClientsAync(request));
        }

        #endregion
    }
}