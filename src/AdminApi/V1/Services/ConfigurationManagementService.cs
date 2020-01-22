// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConfigurationManagementService" company="">
//
// </copyright>
// <summary>
//   The class ConfigurationManagementService.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AdminApi.V1.Services
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using IdentityServer4.EntityFramework.DbContexts;
    using Microsoft.EntityFrameworkCore;
    using IdentityServer4.EntityFramework.Mappers;
    using IdentityServer4.Models;
    using AdminApi.V1.Dtos;
    using System.Linq;
    using Kendo.DynamicLinq;

    #endregion

    /// <summary>
    ///     The ConfigurationManagementService.
    /// </summary>
    public class ConfigurationManagementService : IConfigurationManagementService
    {
        #region Fields
        /// <summary>
        /// The _application db context.
        /// </summary>
        private readonly ConfigurationDbContext _configContext;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationManagementService"/> class.
        /// </summary>
        public ConfigurationManagementService(ConfigurationDbContext configContext)
        {
            this._configContext = configContext;
        }


        #endregion

        #region Public Properties


        #endregion

        #region Public Methods And Operators



        /// <summary>
        /// Return Api Resources
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Kendo.DynamicLinq.DataSourceResult ReturnApiResources(AdminApi.V1.Dtos.DataSourceRequest request)
        {
            var result = this._configContext.ApiResources.ToDataSourceResult(request.PageSize, request.Take, request.Sort, request.Filter);

            var apiResults = new List<ApiResource>();

            foreach (var data in result.Data)
            {
                var apiResource = data as IdentityServer4.EntityFramework.Entities.ApiResource;
                apiResults.Add(apiResource.ToModel());
            }

            result.Data = apiResults;
            return result;
        }

        public async Task UpdateClientAsync(ClientDto client)
        {
            DateTime now = DateTime.Now;
            var currentClient = await this._configContext.Clients
                    .Include(c => c.AllowedGrantTypes)
                    .FirstOrDefaultAsync(a => a.Id == client.Id);

            this._configContext.Clients.Update(client.ToEntity(currentClient));
            await this._configContext.SaveChangesAsync();
        }

        public async Task<ClientDto> ReturnClientAsync(int id)
        {
            var result = await this._configContext.Clients
                .Include(a => a.AllowedGrantTypes).FirstOrDefaultAsync(c => c.Id == id);

            return ClientDto.ToDto(result);
        }

        /// <summary>
        /// Return clients
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<Kendo.DynamicLinq.DataSourceResult> ReturnClientsAync(AdminApi.V1.Dtos.DataSourceRequest request)
        {
            var resultData = new List<ClientDto>();
            var result = this._configContext.Clients.Include(a => a.AllowedGrantTypes).ToDataSourceResult(request.PageSize, request.Take, request.Sort, request.Filter);

            foreach (var dataObject in result.Data)
            {
                resultData.Add(ClientDto.ToDto(dataObject as IdentityServer4.EntityFramework.Entities.Client));
            }

            result.Data = resultData;
            return result;
        }
        public async Task<int> AddClientAsync(ClientDto client)
        {
            var result = await this._configContext.Clients.AddAsync(client.ToEntity());
            await this._configContext.SaveChangesAsync();
            return result.Entity.Id;
        }

        /// <summary>
        /// Return Identity Resources
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<List<IdentityServer4.Models.IdentityResource>> ReturnIdentityResourcesAsync()
        {
            var returnedResults = await this._configContext.IdentityResources.ToListAsync();

            var results = new List<IdentityResource>();
            foreach (var result in returnedResults)
            {
                results.Add(result.ToModel());
            }

            return results;
        }

        /// <summary>
        /// Return Identity Resources
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public Kendo.DynamicLinq.DataSourceResult ReturnIdentityResources(AdminApi.V1.Dtos.DataSourceRequest request)
        {
            var returnedResults = this._configContext.IdentityResources.ToDataSourceResult(request.PageSize, request.Take, request.Sort, request.Filter);

            var results = new List<IdentityResource>();
            foreach (var result in returnedResults.Data)
            {
                results.Add((result as IdentityServer4.EntityFramework.Entities.IdentityResource).ToModel());
            }

            returnedResults.Data = results;

            return returnedResults; 
        }

        public async Task<IdentityResource> ReturnIdentityResourceAsync(string name)
        {
            var result = await this._configContext.IdentityResources.FirstOrDefaultAsync(i => i.Name.ToLower() == name.ToLower());

            if (result != null)
            {
                return result.ToModel();
            }

            return null;
        }

        public async Task<int> AddIdentityResourcesAsync(IdentityResource resource)
        {
            var result = await this._configContext.IdentityResources.AddAsync(resource.ToEntity());
            await this._configContext.SaveChangesAsync();
            resource = result.Entity.ToModel();

            return result.Entity.Id;
        }

        public async Task UpdateIdentityResourcesAsync(UpdateIdentityResourceDto resource)
        {
            DateTime now = DateTime.Now;
            var currentIdentityResource = await this._configContext.IdentityResources.FirstOrDefaultAsync(a => a.Name == resource.OriginalName);

            currentIdentityResource.Name = resource.Name;
            currentIdentityResource.Required = resource.Required;
            currentIdentityResource.ShowInDiscoveryDocument = resource.ShowInDiscoveryDocument;
            currentIdentityResource.Updated = now;
            currentIdentityResource.Description = resource.Description;
            currentIdentityResource.DisplayName = resource.DisplayName;
            currentIdentityResource.Emphasize = resource.Emphasize;
            currentIdentityResource.Enabled = resource.Enabled;

            this._configContext.IdentityResources.Update(currentIdentityResource);
            await this._configContext.SaveChangesAsync();
        }

        public async Task<List<IdentityClaimDto>> ReturnIdentityResourceClaimsAsync(string name)
        {
            var resultIdentityResource = await this._configContext.IdentityResources.Include(i => i.UserClaims).FirstOrDefaultAsync(i => i.Name.ToLower() == name.ToLower());

            if (resultIdentityResource != null)
            {
                List<IdentityClaimDto> results = new List<IdentityClaimDto>();

                foreach (var claim in resultIdentityResource.UserClaims)
                {
                    results.Add(new IdentityClaimDto
                    {
                        Id = claim.Id,
                        Type = claim.Type,
                        IdentityClaimName = resultIdentityResource.Name
                    });
                }

                return results;
            }

            return null;
        }
        public async Task<Kendo.DynamicLinq.DataSourceResult> ReturnIdentityResourceClaimsAsync(AdminApi.V1.Dtos.DataSourceRequest request, string name)
        {
            var resultIdentityResource = await this._configContext.IdentityResources.Include(i => i.UserClaims).FirstOrDefaultAsync(i => i.Name.ToLower() == name.ToLower());

            if (resultIdentityResource != null)
            {
                List<IdentityClaimDto> results = new List<IdentityClaimDto>();

                foreach (var claim in resultIdentityResource.UserClaims)
                {
                    results.Add(new IdentityClaimDto
                    {
                        Id = claim.Id,
                        Type = claim.Type,
                        IdentityClaimName = resultIdentityResource.Name
                    });
                }

                return results.AsQueryable().ToDataSourceResult(request.PageSize, request.Take, request.Sort, request.Filter);
            }

            return null;
        }

        public async Task<int> AddIdentityResourceClaimAsync(IdentityClaimDto claim)
        {
            IdentityServer4.EntityFramework.Entities.IdentityClaim newclaim = new IdentityServer4.EntityFramework.Entities.IdentityClaim();
            var entityResult = await this._configContext.IdentityResources.FirstOrDefaultAsync(i => i.Name.ToLower() == claim.IdentityClaimName.ToLower());
            newclaim.Type = claim.Type;
            newclaim.IdentityResourceId = entityResult.Id;
            var result = await this._configContext.AddAsync(newclaim);
            await this._configContext.SaveChangesAsync();
            claim.IdentityClaimName = entityResult.Name;
            return result.Entity.Id;
        }

        public async Task UpdateIdentityResourceClaimAsync(IdentityClaimDto claim)
        {
            var entityResult = await this._configContext.IdentityResources.FirstOrDefaultAsync(i => i.Name.ToLower() == claim.IdentityClaimName.ToLower());

            if (entityResult == null)
            {
                throw new Exception("Entity Not Found");
            }

            var foundClaim = entityResult.UserClaims.FirstOrDefault(c => c.Id == claim.Id);

            if (foundClaim == null)
            {
                throw new Exception("Claim not found");
            }

            foundClaim.Type = claim.Type;

            this._configContext.Update(foundClaim);
            await this._configContext.SaveChangesAsync();
        }

        public async Task RemoveClaimFromIdentityResourceAsync(IdentityClaimDto claim, bool saveChanges)
        {
            var entityResult = await this._configContext.IdentityResources.FirstOrDefaultAsync(i => i.Name.ToLower() == claim.IdentityClaimName.ToLower());

            if (entityResult == null)
            {
                throw new Exception("Entity Not Found");
            }

            var foundClaim = entityResult.UserClaims.FirstOrDefault(c => c.Id == claim.Id);

            if (foundClaim == null)
            {
                throw new Exception("Claim not found");
            }

            this._configContext.Remove(foundClaim);

            if (saveChanges)
            {
                await this._configContext.SaveChangesAsync();
            }
        }

        public async Task<IdentityClaimDto> ReturnIdentityResourceClaimAsync(string identityClaimName, int id)
        {
            // var entityResult = this._configContext..FirstOrDefaultAsync()
            var resultIdentityResource = await this._configContext.IdentityResources.Include(i => i.UserClaims).FirstOrDefaultAsync(i => i.Name.ToLower() == identityClaimName.ToLower());

            if (resultIdentityResource == null)
            {
                throw new Exception("Unknown Identity Resource");
            }

            var resultEntityClaim = resultIdentityResource.UserClaims.FirstOrDefault(u => u.Id == id);

            if (resultEntityClaim == null)
            {
                throw new Exception("Invalid claim");
            }

            return new IdentityClaimDto
            {
                Id = resultEntityClaim.Id,
                Type = resultEntityClaim.Type,
                IdentityClaimName = resultIdentityResource.Name
            };
        }

        public async Task<int> SaveChangesAsync()
        {
            return await this._configContext.SaveChangesAsync();
        }

        public async Task<ApiResource> ReturnApiResourceAsync(string name)
        {
            var result = await this._configContext.ApiResources.Include(p => p.Properties).FirstOrDefaultAsync(i => i.Name.ToLower() == name.ToLower());

            if (result != null)
            {
                return result.ToModel();
            }

            return null;
        }

        public async Task<int> AddApiResourceAsync(ApiResource resource)
        {
            var result = await this._configContext.ApiResources.AddAsync(resource.ToEntity());
            await this._configContext.SaveChangesAsync();
            resource = result.Entity.ToModel();

            return result.Entity.Id;
        }

        public async Task UpdateApiResourceAsync(UpdateApiResourceDto resource)
        {
            DateTime now = DateTime.Now;
            var currentIdentityResource = await this._configContext.ApiResources.Include(p => p.Properties).FirstOrDefaultAsync(a => a.Name == resource.OriginalName);

            currentIdentityResource.Name = resource.Name;
            currentIdentityResource.Updated = now;
            currentIdentityResource.Description = resource.Description;
            currentIdentityResource.DisplayName = resource.DisplayName;
            currentIdentityResource.Enabled = resource.Enabled;
            if (resource.Properties != null)
            {
                foreach (var property in resource.Properties)
                {
                    IdentityServer4.EntityFramework.Entities.ApiResourceProperty foundItem = null;

                    if (currentIdentityResource.Properties != null)
                    {
                        foundItem = currentIdentityResource.Properties.FirstOrDefault(a => a.Key == property.Key && a.ApiResourceId == currentIdentityResource.Id);
                    }
                    else
                    {
                        currentIdentityResource.Properties = new List<IdentityServer4.EntityFramework.Entities.ApiResourceProperty>();
                    }

                    if (foundItem == null)
                    {
                        currentIdentityResource.Properties.Add(new IdentityServer4.EntityFramework.Entities.ApiResourceProperty
                        {
                            Key = property.Key,
                            Value = property.Value,
                            ApiResourceId = currentIdentityResource.Id
                        });
                    }
                    else
                    {
                        foundItem.Value = property.Value;
                    }
                };
            }

            this._configContext.ApiResources.Update(currentIdentityResource);
            await this._configContext.SaveChangesAsync();
        }

        public async Task<ApiClaimDto> ReturnApiResourceClaimAsync(string apiClaimName, int id)
        {
            var resultIdentityResource = await this._configContext.ApiResources.Include(i => i.UserClaims).FirstOrDefaultAsync(i => i.Name.ToLower() == apiClaimName.ToLower());

            if (resultIdentityResource == null)
            {
                throw new Exception("Unknown Api Resource");
            }

            var resultEntityClaim = resultIdentityResource.UserClaims.FirstOrDefault(u => u.Id == id);

            if (resultEntityClaim == null)
            {
                throw new Exception("Invalid claim");
            }

            return new ApiClaimDto
            {
                Id = resultEntityClaim.Id,
                Type = resultEntityClaim.Type,
                ApiClaimName = resultIdentityResource.Name
            };
        }

        public async Task<Kendo.DynamicLinq.DataSourceResult> ReturnApiResourceClaimsAsync(AdminApi.V1.Dtos.DataSourceRequest request, string name)
        {
            var resultIdentityResource = await this._configContext.ApiResources.Include(i => i.UserClaims).FirstOrDefaultAsync(i => i.Name.ToLower() == name.ToLower());

            if (resultIdentityResource != null)
            {
                List<ApiClaimDto> results = new List<ApiClaimDto>();

                foreach (var claim in resultIdentityResource.UserClaims)
                {
                    results.Add(new ApiClaimDto
                    {
                        Id = claim.Id,
                        Type = claim.Type,
                        ApiClaimName = resultIdentityResource.Name
                    });
                }

                return results.AsQueryable().ToDataSourceResult(request.PageSize, request.Take, request.Sort, request.Filter);
            }

            return null;
        }
        public async Task<bool> AddRedirectUriToClient(int clientId, ClientRedirectUrlDto redirectUriDto)
        {
            await this._configContext.AddAsync(new IdentityServer4.EntityFramework.Entities.ClientRedirectUri
            {
                ClientId = redirectUriDto.ClientId,
                RedirectUri = redirectUriDto.RedirectUri
            });

            return true;
        }

        public async Task<bool> AddScopesToClient(int id, List<string> scopes)
        {
            foreach (var scope in scopes)
            {
                var result = await this._configContext.AddAsync(new IdentityServer4.EntityFramework.Entities.ClientScope
                {
                    ClientId = id,
                    Scope = scope
                });
            }

            return true;
        }

        public async Task<List<string>> ReturnScopesToAddForClient(long id)
        {
            var result = new List<string>();

            var apiResources = await this._configContext.ApiResources.ToListAsync();
            var identityResource = await this._configContext.IdentityResources.ToListAsync();

            result.AddRange(apiResources.Select(a => a.Name));
            result.AddRange(identityResource.Select(i => i.Name));
            result = result.Distinct().ToList();

            var client = await this._configContext.Clients.Include(c => c.AllowedScopes).FirstOrDefaultAsync(c => c.Id == id);
            var currentScopes = client.AllowedScopes.Select(a => a.Scope);

            foreach (var scope in currentScopes)
            {
                result.Remove(scope);
            }

            result.Sort();

            return result;
        }

        public async Task<Kendo.DynamicLinq.DataSourceResult> ReturnScopesToAddForClient(AdminApi.V1.Dtos.DataSourceRequest request, int clientId)
        {
            var resultStrings = new List<string>();

            var apiResources = await this._configContext.ApiResources.ToListAsync();
            var identityResource = await this._configContext.IdentityResources.ToListAsync();

            resultStrings.AddRange(apiResources.Select(a => a.Name));
            resultStrings.AddRange(identityResource.Select(i => i.Name));
            resultStrings = resultStrings.Distinct().ToList();

            var client = await this._configContext.Clients.Include(c => c.AllowedScopes).FirstOrDefaultAsync(c => c.Id == clientId);
            var currentScopes = client.AllowedScopes.Select(a => a.Scope);

            foreach (var scope in currentScopes)
            {
                resultStrings.Remove(scope);
            }

            var result = new List<ToAddDto>();

            foreach (var scope in resultStrings)
            {
                result.Add(new ToAddDto { Name = scope });
            }

            return result.AsQueryable().ToDataSourceResult(request.PageSize, request.Take, request.Sort, request.Filter);
        }

        public async Task<Kendo.DynamicLinq.DataSourceResult> ReturnClientRedirectUrisAsync(AdminApi.V1.Dtos.DataSourceRequest request, int clientId)
        {
            var client = await this._configContext.Clients
                .Include(i => i.RedirectUris)
                .FirstOrDefaultAsync(i => i.Id == clientId);

            if (client != null)
            {
                List<ClientRedirectUrlDto> results = new List<ClientRedirectUrlDto>();

                foreach (var redirrectUrl in client.RedirectUris)
                {
                    results.Add(new ClientRedirectUrlDto
                    {
                        Id = redirrectUrl.Id,
                        RedirectUri = redirrectUrl.RedirectUri,
                        ClientId = client.Id
                    });
                }

                return results.AsQueryable().ToDataSourceResult(request.PageSize, request.Take, request.Sort, request.Filter);
            }

            return null;

        }

        public async Task<Kendo.DynamicLinq.DataSourceResult> ReturnClientScopesAsync(AdminApi.V1.Dtos.DataSourceRequest request, int clientId)
        {
            var client = await this._configContext.Clients
                .Include(i => i.AllowedScopes)
                .FirstOrDefaultAsync(i => i.Id == clientId);

            if (client != null)
            {
                List<ClientScopeDto> results = new List<ClientScopeDto>();

                foreach (var scope in client.AllowedScopes)
                {
                    results.Add(new ClientScopeDto
                    {
                        Id = scope.Id,
                        Scope = scope.Scope,
                        ClientId = client.Id
                    });
                }

                return results.AsQueryable().ToDataSourceResult(request.PageSize, request.Take, request.Sort, request.Filter);
            }

            return null;
        }

        public async Task<int> AddApiResourceClaimAsync(ApiClaimDto claim)
        {
            IdentityServer4.EntityFramework.Entities.ApiResourceClaim newclaim = new IdentityServer4.EntityFramework.Entities.ApiResourceClaim();
            var entityResult = await this._configContext.ApiResources.FirstOrDefaultAsync(i => i.Name.ToLower() == claim.ApiClaimName.ToLower());
            newclaim.Type = claim.Type;
            newclaim.ApiResourceId = entityResult.Id;
            var result = await this._configContext.AddAsync(newclaim);
            await this._configContext.SaveChangesAsync();
            claim.ApiClaimName = entityResult.Name;
            return result.Entity.Id;
        }

        public async Task UpdateApiResourceClaimAsync(ApiClaimDto claim)
        {
            var entityResult = await this._configContext.ApiResources.FirstOrDefaultAsync(i => i.Name.ToLower() == claim.ApiClaimName.ToLower());

            if (entityResult == null)
            {
                throw new Exception("Entity Not Found");
            }

            var foundClaim = entityResult.UserClaims.FirstOrDefault(c => c.Id == claim.Id);

            if (foundClaim == null)
            {
                throw new Exception("Claim not found");
            }

            foundClaim.Type = claim.Type;

            this._configContext.Update(foundClaim);
            await this._configContext.SaveChangesAsync();
        }

        public async Task RemoveClaimFromApiResourceAsync(ApiClaimDto claim, bool saveChanges)
        {
            var entityResult = await this._configContext.ApiResources.FirstOrDefaultAsync(i => i.Name.ToLower() == claim.ApiClaimName.ToLower());

            if (entityResult == null)
            {
                throw new Exception("Entity Not Found");
            }

            var foundClaim = entityResult.UserClaims.FirstOrDefault(c => c.Id == claim.Id);

            if (foundClaim == null)
            {
                throw new Exception("Claim not found");
            }

            this._configContext.Remove(foundClaim);

            if (saveChanges)
            {
                await this._configContext.SaveChangesAsync();
            }
        }

        public async Task RemoveScopeFromClientAsync(ClientScopeDto scope, bool saveChanges)
        {
            var client = await this._configContext
                                    .Clients.Include(c => c.AllowedScopes)
                                    .FirstOrDefaultAsync(i => i.Id == scope.ClientId);

            if (client == null)
            {
                throw new Exception("Entity Not Found");
            }

            var foundScope = client.AllowedScopes.FirstOrDefault(c => c.Id == scope.Id);

            if (foundScope == null)
            {
                throw new Exception("Claim not found");
            }

            this._configContext.Remove(foundScope);

            if (saveChanges)
            {
                await this._configContext.SaveChangesAsync();
            }
        }

        public async Task RemoveRedirectUrisFromClaimsAsync(ClientRedirectUrlDto redirectUrlDtos)
        {
            var client = await this._configContext
                                    .Clients.Include(c => c.RedirectUris)
                                    .FirstOrDefaultAsync(i => i.Id == redirectUrlDtos.ClientId);

            if (client == null)
            {
                throw new Exception("Entity Not Found");
            }

            var foundRedirectUril = client.RedirectUris.FirstOrDefault(c => c.Id == redirectUrlDtos.Id);

            if (foundRedirectUril == null)
            {
                throw new Exception("RedirectUri not found");
            }

            this._configContext.Remove(foundRedirectUril);
        }

        public async Task<ClientRedirectUrlDto> ReturnClientRedirectUriAsync(int clientId, int redirectUriId)
        {
            var client = await this._configContext
                                    .Clients.Include(c => c.RedirectUris)
                                    .FirstOrDefaultAsync(i => i.Id == clientId);

            if (client == null)
            {
                throw new Exception("Entity Not Found");
            }

            var foundRedirectUril = client.RedirectUris.FirstOrDefault(c => c.Id == redirectUriId);

            if (foundRedirectUril == null)
            {
                throw new Exception("RedirectUri not found");
            }

            return new ClientRedirectUrlDto
            {
                Id = foundRedirectUril.Id,
                ClientId = foundRedirectUril.ClientId,
                RedirectUri = foundRedirectUril.RedirectUri
            };
        }

        public async Task<int> UpdateClientRedirectUriAsync(ClientRedirectUrlDto redirectUri)
        {
            var client = await this._configContext
                                    .Clients.Include(c => c.RedirectUris)
                                    .FirstOrDefaultAsync(i => i.Id == redirectUri.ClientId);

            if (client == null)
            {
                throw new Exception("Entity Not Found");
            }

            var foundRedirectUri = client.RedirectUris.FirstOrDefault(c => c.Id == redirectUri.Id);

            if (foundRedirectUri == null)
            {
                throw new Exception("RedirectUri not found");
            }

            foundRedirectUri.RedirectUri = redirectUri.RedirectUri;
            return this._configContext.Update(foundRedirectUri).Entity.Id;
        }

        public async Task<Kendo.DynamicLinq.DataSourceResult> ReturnClientPostLogoutUrisAsync(AdminApi.V1.Dtos.DataSourceRequest request, int clientId)
        {
            var client = await this._configContext.Clients
                .Include(i => i.PostLogoutRedirectUris)
                .FirstOrDefaultAsync(i => i.Id == clientId);

            if (client != null)
            {
                List<ClientPostLogoutUrlDto> results = new List<ClientPostLogoutUrlDto>();

                foreach (var postLogoutUri in client.PostLogoutRedirectUris)
                {
                    results.Add(new ClientPostLogoutUrlDto
                    {
                        Id = postLogoutUri.Id,
                        PostLogoutRedirectUri = postLogoutUri.PostLogoutRedirectUri,
                        ClientId = client.Id
                    });
                }

                return results.AsQueryable().ToDataSourceResult(request.PageSize, request.Take, request.Sort, request.Filter);
            }

            return null;

        }

        public async Task<ClientPostLogoutUrlDto> ReturnClientPostLogoutUriAsync(int clientId, int postLogoutUriId)
        {
            var client = await this._configContext
                                    .Clients.Include(c => c.PostLogoutRedirectUris)
                                    .FirstOrDefaultAsync(i => i.Id == clientId);

            if (client == null)
            {
                throw new Exception("Entity Not Found");
            }

            var foundRedirectUril = client.PostLogoutRedirectUris.FirstOrDefault(c => c.Id == postLogoutUriId);

            if (foundRedirectUril == null)
            {
                throw new Exception("PostLogout Uri not found");
            }

            return new ClientPostLogoutUrlDto
            {
                Id = foundRedirectUril.Id,
                ClientId = foundRedirectUril.ClientId,
                PostLogoutRedirectUri = foundRedirectUril.PostLogoutRedirectUri
            };
        }

        public async Task<bool> AddPostLogoutUriToClient(int clientId, ClientPostLogoutUrlDto clientPostLogoutDto)
        {
            await this._configContext.AddAsync(new IdentityServer4.EntityFramework.Entities.ClientPostLogoutRedirectUri
            {
                ClientId = clientPostLogoutDto.ClientId,
                PostLogoutRedirectUri = clientPostLogoutDto.PostLogoutRedirectUri
            });

            return true;
        }
        public async Task<int> UpdateClientPostLogoutUriAsync(ClientPostLogoutUrlDto clientPostLogoutDto)
        {
            var client = await this._configContext
                                    .Clients.Include(c => c.PostLogoutRedirectUris)
                                    .FirstOrDefaultAsync(i => i.Id == clientPostLogoutDto.ClientId);

            if (client == null)
            {
                throw new Exception("Entity Not Found");
            }

            var foundRedirectUri = client.PostLogoutRedirectUris.FirstOrDefault(c => c.Id == clientPostLogoutDto.Id);

            if (foundRedirectUri == null)
            {
                throw new Exception("RedirectUri not found");
            }

            foundRedirectUri.PostLogoutRedirectUri = clientPostLogoutDto.PostLogoutRedirectUri;
            return this._configContext.Update(foundRedirectUri).Entity.Id;
        }
        public async Task RemovePostLogoutsFromClaims(ClientPostLogoutUrlDto postLogoutUriDto)
        {
            var client = await this._configContext
                                .Clients.Include(c => c.PostLogoutRedirectUris)
                                .FirstOrDefaultAsync(i => i.Id == postLogoutUriDto.ClientId);

            if (client == null)
            {
                throw new Exception("Entity Not Found");
            }

            var foundRedirectUril = client.PostLogoutRedirectUris.FirstOrDefault(c => c.Id == postLogoutUriDto.Id);

            if (foundRedirectUril == null)
            {
                throw new Exception("PostLogoutUriDto not found");
            }

            this._configContext.Remove(foundRedirectUril);
        }
        public async Task<Kendo.DynamicLinq.DataSourceResult> ReturnClientCorsUrisAsync(AdminApi.V1.Dtos.DataSourceRequest request, int clientId)
        {
            var client = await this._configContext.Clients
                .Include(i => i.AllowedCorsOrigins)
                .FirstOrDefaultAsync(i => i.Id == clientId);

            if (client != null)
            {
                List<ClientAllowedCorsUrlDto> results = new List<ClientAllowedCorsUrlDto>();

                foreach (var allowedCorsOriginUri in client.AllowedCorsOrigins)
                {
                    results.Add(new ClientAllowedCorsUrlDto
                    {
                        Id = allowedCorsOriginUri.Id,
                        Origin = allowedCorsOriginUri.Origin,
                        ClientId = client.Id
                    });
                }

                return results.AsQueryable().ToDataSourceResult(request.PageSize, request.Take, request.Sort, request.Filter);
            }

            return null;

        }
        public async Task<ClientAllowedCorsUrlDto> ReturnClientCorsUrisAsync(int clientId, int corsOriginUriId)
        {
            var client = await this._configContext
                                    .Clients.Include(c => c.AllowedCorsOrigins)
                                    .FirstOrDefaultAsync(i => i.Id == clientId);

            if (client == null)
            {
                throw new Exception("Entity Not Found");
            }

            var foundRedirectUril = client.AllowedCorsOrigins.FirstOrDefault(c => c.Id == corsOriginUriId);

            if (foundRedirectUril == null)
            {
                throw new Exception("Cors Origin Uri not found");
            }

            return new ClientAllowedCorsUrlDto
            {
                Id = foundRedirectUril.Id,
                ClientId = foundRedirectUril.ClientId,
                Origin = foundRedirectUril.Origin
            };
        }
        public async Task<bool> AddCorsOriginToClient(int clientId, ClientAllowedCorsUrlDto corsAllowedUriDto)
        {
            await this._configContext.AddAsync(new IdentityServer4.EntityFramework.Entities.ClientCorsOrigin
            {
                ClientId = corsAllowedUriDto.ClientId,
                Origin = corsAllowedUriDto.Origin
            });

            return true;
        }
        public async Task<int> UpdateClientCorsOriginAsync(ClientAllowedCorsUrlDto allowedCorsUri)
        {
            var client = await this._configContext
                                    .Clients.Include(c => c.AllowedCorsOrigins)
                                    .FirstOrDefaultAsync(i => i.Id == allowedCorsUri.ClientId);

            if (client == null)
            {
                throw new Exception("Entity Not Found");
            }

            var foundAllowedCorsUri = client.AllowedCorsOrigins.FirstOrDefault(c => c.Id == allowedCorsUri.Id);

            if (foundAllowedCorsUri == null)
            {
                throw new Exception("Cors Allowed Origins not found");
            }

            foundAllowedCorsUri.Origin = allowedCorsUri.Origin;
            return this._configContext.Update(foundAllowedCorsUri).Entity.Id;
        }
        public async Task RemoveCorsOriginFromClaims(ClientAllowedCorsUrlDto allowedCorsUri)
        {
            var client = await this._configContext
                                .Clients.Include(c => c.AllowedCorsOrigins)
                                .FirstOrDefaultAsync(i => i.Id == allowedCorsUri.ClientId);

            if (client == null)
            {
                throw new Exception("Entity Not Found");
            }

            var foundAllowedCorsUri = client.AllowedCorsOrigins.FirstOrDefault(c => c.Id == allowedCorsUri.Id);

            if (foundAllowedCorsUri == null)
            {
                throw new Exception("Allowed Cors Url not found");
            }

            this._configContext.Remove(foundAllowedCorsUri);
        }
        public async Task<Kendo.DynamicLinq.DataSourceResult> ReturnClientSecretsAsync(AdminApi.V1.Dtos.DataSourceRequest request, int clientId)
        {
            var client = await this._configContext.Clients
                .Include(i => i.ClientSecrets)
                .FirstOrDefaultAsync(i => i.Id == clientId);

            if (client != null)
            {
                List<ClientSecretDto> results = new List<ClientSecretDto>();

                foreach (var secret in client.ClientSecrets)
                {
                    results.Add(new ClientSecretDto
                    {
                        Id = secret.Id,
                        ClientId = client.Id,
                        Description = secret.Description,
                        Value = $"************{secret.Value.Remove(0, secret.Value.Length - 6)}",
                        Expiration = secret.Expiration,
                        Type = secret.Type,
                        Created = secret.Created
                    });
                }


                return results.AsQueryable().ToDataSourceResult(request.PageSize, request.Take, request.Sort, request.Filter);
            }

            return null;

        }
        public async Task<bool> AddSecretToClient(int clientId, ClientSecretDto secret)
        {
            await this._configContext.AddAsync(new IdentityServer4.EntityFramework.Entities.ClientSecret
            {
                ClientId = secret.ClientId,
                Description = secret.Description,
                Value = secret.Value.Sha256(),
                Expiration = secret.Expiration,
                Type = secret.Type,
                Created = secret.Created
            });

            return true;
        }
        public async Task RemoveSecretFromClaims(ClientSecretDto secret)
        {
            var client = await this._configContext
                                .Clients.Include(c => c.ClientSecrets)
                                .FirstOrDefaultAsync(i => i.Id == secret.ClientId);

            if (client == null)
            {
                throw new Exception("Entity Not Found");
            }

            var foundSecret = client.ClientSecrets.FirstOrDefault(c => c.Id == secret.Id);

            if (foundSecret == null)
            {
                throw new Exception("Secret not found");
            }

            this._configContext.Remove(foundSecret);
        }

        #endregion

        #region Other Methods

        #endregion
    }
}