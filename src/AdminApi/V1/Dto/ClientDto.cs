// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClientDto" company="">
//
// </copyright>
// <summary>
//   The class ClientDto.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AdminApi.V1.Dtos
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using IdentityServer4.EntityFramework.Entities;

    #endregion

    /// <summary>
    ///     The ClientDto.
    /// </summary>
    public class ClientDto
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientDto"/> class.
        /// </summary>
        public ClientDto()
        {
            this.AllowedGrantTypes = new List<ClientGrantTypeDto>();
            this.Created = DateTime.UtcNow;
        }

        #endregion

        #region Public Properties

        public int Id { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string Description { get; set; }
        public List<ClientGrantTypeDto> AllowedGrantTypes { get; set; }
        public bool RequirePkce { get; set; }
        public bool RequireClientSecret { get; set; }
        public bool Enabled { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public DateTime? LastAccessed { get; set; }
        public bool AllowOfflineAccess { get; set; }
        public int AccessTokenLifetime { get; set; }
        public int IdentityTokenLifetime { get; set; }
        public bool RequireConsent { get; set; }
        public bool AllowAccessTokensViaBrowser { get; set; }

        #endregion

        #region Public Methods And Operators

        public Client ToEntity()
        {
            return this.ToEntity(new Client());
        }

        public Client ToEntity(Client client)
        {
            if (client.AllowedGrantTypes == null)
            {
                client.AllowedGrantTypes = new List<ClientGrantType>();
            }

            client.Id = this.Id;
            client.ClientName = this.ClientName;
            client.ClientId = this.ClientId;
            client.Description = this.Description;

            foreach (var allowedGrantType in this.AllowedGrantTypes)
            {
                var grantType = client.AllowedGrantTypes.FirstOrDefault(a => a.Id == allowedGrantType.Id);

                if (grantType == null)
                {
                    grantType = new ClientGrantType
                    {
                        Id = allowedGrantType.Id,
                        ClientId = this.Id
                    };
                    client.AllowedGrantTypes.Add(grantType);
                }
                else
                {
                    grantType.GrantType = allowedGrantType.GrantType;
                }

                switch (allowedGrantType.GrantType.ToLower())
                {
                    case "clientcredentials":
                        grantType.GrantType = "client_credentials";
                        break;
                    case "resourceownerpassword":
                        grantType.GrantType = "password";
                        break;
                    case "hybrid":
                        grantType.GrantType = "hybrid";
                        break;
                    case "code":
                        grantType.GrantType = "authorization_code";
                        break;
                    case "implicit":
                        grantType.GrantType = "implicit";
                        break;
                    default:
                        grantType.GrantType = allowedGrantType.GrantType;
                        break;
                }
            }

            client.RequirePkce = this.RequirePkce;
            client.RequireClientSecret = this.RequireClientSecret;
            client.Enabled = this.Enabled;
            client.Created = this.Created;
            client.Updated = this.Updated;
            client.LastAccessed = this.LastAccessed;
            client.AllowOfflineAccess = this.AllowOfflineAccess;
            client.IdentityTokenLifetime = this.IdentityTokenLifetime;
            client.RequireConsent = this.RequireConsent;
            client.AllowAccessTokensViaBrowser = this.AllowAccessTokensViaBrowser;

            return client;
        }

        public static ClientDto ToDto(IdentityServer4.EntityFramework.Entities.Client data)
        {
            var dto = new ClientDto
            {
                Id = data.Id,
                ClientId = data.ClientId,
                ClientName = data.ClientName,
                Description = data.Description,
                RequirePkce = data.RequirePkce,
                RequireClientSecret = data.RequireClientSecret,
                Enabled = data.Enabled,
                Created = data.Created,
                Updated = data.Updated,
                LastAccessed = data.LastAccessed,
                AllowOfflineAccess = data.AllowOfflineAccess,
                AccessTokenLifetime = data.AccessTokenLifetime,
                IdentityTokenLifetime = data.IdentityTokenLifetime,
                AllowAccessTokensViaBrowser = data.AllowAccessTokensViaBrowser,
                RequireConsent = data.RequireConsent
            };

            if (data.AllowedGrantTypes != null)
            {
                foreach (var allowGrantType in data.AllowedGrantTypes)
                {
                    var grant = "";

                    switch (allowGrantType.GrantType)
                    {
                        case "client_credentials":
                            grant = "ClientCredentials";
                            break;
                        case "password":
                            grant = "ResourceOwnerPassword";
                            break;
                        case "hybrid":
                            grant = "Hybrid";
                            break;
                        case "authorization_code":
                            grant = "Code";
                            break;
                        case "implicit":
                            grant = "Implicit";
                            break;
                        default:
                            grant = allowGrantType.GrantType;
                            break;
                    }
                    dto.AllowedGrantTypes.Add(new ClientGrantTypeDto
                    {
                        Id = allowGrantType.Id,
                        GrantType = grant
                    });

                }
            }

            return dto;
        }

        #endregion

        #region Other Methods

        #endregion
    }

}

// public class Client
//     {
//         public int AuthorizationCodeLifetime { get; set; }
//         public int? ConsentLifetime { get; set; }
//         public int AbsoluteRefreshTokenLifetime { get; set; }
//         public int SlidingRefreshTokenLifetime { get; set; }
//         public int RefreshTokenUsage { get; set; }
//         public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
//         public int RefreshTokenExpiration { get; set; }
//         public int AccessTokenType { get; set; }
//         public bool EnableLocalLogin { get; set; }
//         public List<ClientIdPRestriction> IdentityProviderRestrictions { get; set; }
//         public bool IncludeJwtId { get; set; }
//         public bool AlwaysSendClientClaims { get; set; }
//         public string ClientClaimsPrefix { get; set; }
//         public string PairWiseSubjectSalt { get; set; }
//         public List<ClientCorsOrigin> AllowedCorsOrigins { get; set; }
//         public List<ClientProperty> Properties { get; set; }
//         public int? UserSsoLifetime { get; set; }
//         public string UserCodeType { get; set; }
//         public List<ClientClaim> Claims { get; set; }
//         public List<ClientScope> AllowedScopes { get; set; }

//         public string ProtocolType { get; set; }
//         public List<ClientSecret> ClientSecrets { get; set; }


//         public string ClientUri { get; set; }
//         public string LogoUri { get; set; }
//         public bool AllowRememberConsent { get; set; }
//         public bool AlwaysIncludeUserClaimsInIdToken { get; set; }


//         public bool AllowPlainTextPkce { get; set; }
//         public bool AllowAccessTokensViaBrowser { get; set; }
//         public List<ClientRedirectUri> RedirectUris { get; set; }
//         public List<ClientPostLogoutRedirectUri> PostLogoutRedirectUris { get; set; }
//         public string FrontChannelLogoutUri { get; set; }
//         public bool FrontChannelLogoutSessionRequired { get; set; }
//         public string BackChannelLogoutUri { get; set; }
//         public bool BackChannelLogoutSessionRequired { get; set; }
//         public int DeviceCodeLifetime { get; set; }
//         public bool NonEditable { get; set; }
//     }