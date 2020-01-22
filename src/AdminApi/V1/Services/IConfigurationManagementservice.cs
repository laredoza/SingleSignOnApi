using System.Collections.Generic;
using System.Threading.Tasks;
using AdminApi.V1.Dtos;
using IdentityServer4.Models;

namespace AdminApi.V1.Services
{
    public interface IConfigurationManagementService
    {
        Task<bool> AddRedirectUriToClient(int clientId, ClientRedirectUrlDto redirectUriDto);
        Task<bool> AddScopesToClient(int id, List<string> scopes);
        Task<int> AddClientAsync(ClientDto client);
        Task UpdateClientAsync(ClientDto resource);
        Task<ClientDto> ReturnClientAsync(int id);
        /// <summary>
        /// Return clients
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<Kendo.DynamicLinq.DataSourceResult> ReturnClientsAync(AdminApi.V1.Dtos.DataSourceRequest request);
        /// <summary>
        /// Return Identity Resources
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Task<List<IdentityResource>> ReturnIdentityResourcesAsync();
        Kendo.DynamicLinq.DataSourceResult ReturnIdentityResources(DataSourceRequest request);
        Task<IdentityResource> ReturnIdentityResourceAsync(string name);
        Task<int> AddIdentityResourcesAsync(IdentityResource resource);

        Task<int> AddIdentityResourceClaimAsync(IdentityClaimDto claim);
        Task UpdateIdentityResourceClaimAsync(IdentityClaimDto claim);
        Task RemoveClaimFromIdentityResourceAsync(IdentityClaimDto claim, bool saveChanges);
        Task<IdentityClaimDto> ReturnIdentityResourceClaimAsync(string identityClaimName, int id);
        Task UpdateIdentityResourcesAsync(UpdateIdentityResourceDto resource);
        Task<List<IdentityClaimDto>> ReturnIdentityResourceClaimsAsync(string name);
        Task<Kendo.DynamicLinq.DataSourceResult> ReturnIdentityResourceClaimsAsync(DataSourceRequest request, string name);
        Task<int> SaveChangesAsync();
        /// <summary>
        /// Return Api Resources
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        Kendo.DynamicLinq.DataSourceResult ReturnApiResources(DataSourceRequest request);
        Task<ApiResource> ReturnApiResourceAsync(string name);
        Task<int> AddApiResourceAsync(ApiResource resource);
        Task UpdateApiResourceAsync(UpdateApiResourceDto resource);
        Task<Kendo.DynamicLinq.DataSourceResult> ReturnApiResourceClaimsAsync(DataSourceRequest request, string name);
        Task<Kendo.DynamicLinq.DataSourceResult> ReturnClientScopesAsync(DataSourceRequest request, int clientId);
        Task<Kendo.DynamicLinq.DataSourceResult> ReturnClientRedirectUrisAsync(DataSourceRequest request, int clientId);
        Task<Kendo.DynamicLinq.DataSourceResult> ReturnScopesToAddForClient(DataSourceRequest request, int clientId);
        Task<ApiClaimDto> ReturnApiResourceClaimAsync(string apiClaimName, int id);
        Task<int> AddApiResourceClaimAsync(ApiClaimDto claim);
        Task UpdateApiResourceClaimAsync(ApiClaimDto claim);
        Task RemoveClaimFromApiResourceAsync(ApiClaimDto claim, bool saveChanges);
        Task RemoveScopeFromClientAsync(ClientScopeDto scope, bool saveChanges);
        Task RemoveRedirectUrisFromClaimsAsync(ClientRedirectUrlDto redirectUrlDtos);
        Task<ClientRedirectUrlDto> ReturnClientRedirectUriAsync(int clientId, int redirectUriId);
        Task<int> UpdateClientRedirectUriAsync(ClientRedirectUrlDto redirectUri);
        Task<Kendo.DynamicLinq.DataSourceResult> ReturnClientPostLogoutUrisAsync(DataSourceRequest request, int clientId);
        Task<ClientPostLogoutUrlDto> ReturnClientPostLogoutUriAsync(int clientId, int postLogoutUriId);
        Task<bool> AddPostLogoutUriToClient(int clientId, ClientPostLogoutUrlDto clientPostLogoutDto);
        Task<int> UpdateClientPostLogoutUriAsync(ClientPostLogoutUrlDto clientPostLogoutDto);
        Task RemovePostLogoutsFromClaims(ClientPostLogoutUrlDto postLogoutUriDtos);
        Task<Kendo.DynamicLinq.DataSourceResult> ReturnClientCorsUrisAsync(DataSourceRequest request, int clientId);
        Task<ClientAllowedCorsUrlDto> ReturnClientCorsUrisAsync(int clientId, int corsOriginUriId);
        Task<bool> AddCorsOriginToClient(int clientId, ClientAllowedCorsUrlDto corsAllowedUriDto);
        Task<int> UpdateClientCorsOriginAsync(ClientAllowedCorsUrlDto allowedCorsUri); 
        Task RemoveCorsOriginFromClaims(ClientAllowedCorsUrlDto allowedCorsUri); 
        Task<Kendo.DynamicLinq.DataSourceResult> ReturnClientSecretsAsync(DataSourceRequest request, int clientId);
        Task<bool> AddSecretToClient(int clientId, ClientSecretDto secret);
        Task RemoveSecretFromClaims(ClientSecretDto secret); 
    }
}