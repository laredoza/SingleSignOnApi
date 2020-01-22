// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Config.cs" company="">
// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.// </copyright>
// <summary>
//   The config.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SingleSignOn.Data.Seeding
{
    #region Usings

    using System.Collections.Generic;
    using IdentityServer4;
    using IdentityServer4.Models;

    #endregion

    /// <summary>
    /// The config.
    /// </summary>
    public static class Config
    {
        #region Public Methods And Operators

        /// <summary>
        /// The get apis.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
                       {
                           new ApiResource("api1", "My API")
                           {
                               UserClaims = new[] { "admin_users" }
                           },
                           new ApiResource("admin-api", "Admin Api Resource")
                               {
                                    UserClaims = new[] { "admin_users", "admin_roles", "admin_api_resource", "admin_identity_resource", "admin_clients" }
                               }
                       };
        }

        /// <summary>
        /// The get clients.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public static IEnumerable<Client> GetClients(string serverUrl)
        {
            return new List<Client>
                       {
                           new Client
                               {
                                   ClientId = "Service",
                                   ClientName = "Service",
                                   Description = "Access this client using a client secret",
                                   Enabled = false,
                                   // no interactive user, use the clientid/secret for authentication -- Service
                                   AllowedGrantTypes = GrantTypes.ClientCredentials,
                                   // secret for authentication
                                   ClientSecrets = {
                                                      new Secret("secret".Sha256(), "Default") 
                                                   },
                                   // scopes that client has access to
                                   AllowedScopes = {
                                                      "api1" 
                                                   }
                               },

                           // resource owner password grant client
                           new Client
                               {
                                   ClientId = "ro.client",
                                   ClientName = "Resource Owner Password",
                                   Description = "Access this client using a resource owner password",
                                   Enabled = false,
                                   AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                                   ClientSecrets = {
                                                      new Secret("secret".Sha256(), "Default") 
                                                   },
                                   AllowedScopes = {
                                                      "api1" 
                                                   }
                               },

                           // OpenID Connect hybrid flow client (MVC)
                           new Client
                               {
                                   ClientId = "mvc",
                                   ClientName = "MVC Client",
                                   Description = "Access this client using a hybrid method",
                                   AllowedGrantTypes = GrantTypes.Hybrid,
                                   Enabled = false,
                                   ClientSecrets = {
                                                      new Secret("secret".Sha256(), "Default") 
                                                   },
                                   RedirectUris = {
                                                     "http://localhost:5002/signin-oidc",
                                                     $"{serverUrl}:5002/signin-oidc" 
                                                  },
                                   PostLogoutRedirectUris = {
                                                               "http://localhost:5002/signout-callback-oidc",
                                                               $"{serverUrl}:5002/signout-callback-oidc" 
                                                            },
                                   AllowedScopes =
                                       {
                                           IdentityServerConstants.StandardScopes.OpenId,
                                           IdentityServerConstants.StandardScopes.Profile,
                                           "api1"
                                       },
                                   AllowOfflineAccess = true
                               },

                           // JavaScript Client
                           new Client
                               {
                                   ClientId = "js",
                                   ClientName = "JavaScript Client",
                                   Description = "Access the client using a code method",
                                   Enabled = false,
                                   AllowedGrantTypes = GrantTypes.Code,
                                   RequirePkce = true,
                                   RequireClientSecret = false,
                                   RedirectUris = {
                                                     "http://localhost:5003/callback.html",
                                                     $"{serverUrl}:5003/callback.html"
                                                  },
                                   PostLogoutRedirectUris = {
                                                               "http://localhost:5003/index.html",
                                                               $"{serverUrl}:5003/index.html"
                                                            },
                                   AllowedCorsOrigins = {
                                                           "http://localhost:5003",
                                                           $"{serverUrl}:5003"
                                                        },
                                   AllowedScopes =
                                       {
                                           IdentityServerConstants.StandardScopes.OpenId,
                                           IdentityServerConstants.StandardScopes.Profile,
                                           "api1"
                                       }
                               },

                           // Spa Client
                           new Client
                               {
                                   ClientId = "single_sign_on_server",
                                   ClientName = "Single Sign On Server",
                                   Description = "Access this client using an implicit method",
                                   Enabled = true,
                                   AllowedGrantTypes = GrantTypes.Implicit,
                                   AllowedScopes = new List<string>
                                                       {
                                                           "openid",
                                                           "profile",
                                                           "identity_admin",
                                                           "api1",
                                                           "admin-api",
                                                           "admin_users",
                                                           "admin_roles",
                                                           "admin_identity_resource",
                                                           "admin_clients"
                                                       },
                                   RedirectUris = new List<string> { 
                                       "http://localhost:4200/auth-callback", 
                                       $"{serverUrl}:4200/auth-callback" },
                                   PostLogoutRedirectUris = new List<string> { 
                                       "http://localhost:4200/", 
                                       $"{serverUrl}:4200/" },
                                   AllowedCorsOrigins = new List<string> { 
                                       "http://localhost:4200", 
                                       $"{serverUrl}:4200" },
                                   AllowAccessTokensViaBrowser = true
                               }
                       };
        }

        /// <summary>
        /// The get identity resources.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
                       {
                           new IdentityResources.OpenId(),
                           new IdentityResources.Profile(),
                           new IdentityResource { Name = "role", UserClaims = new List<string> { "role" } },
                           new IdentityResource
                               {
                                   Name = "identity_admin",
                                   Description = "Admin Claims",
                                   UserClaims = new List<string>
                                                    {
                                                        "admin_clients",
                                                        "admin_users",
                                                        "admin_api_resource",
                                                        "admin_identity_resource",
                                                        "admin_roles"
                                                    }
                               }
                       };
        }

        #endregion
    }
}