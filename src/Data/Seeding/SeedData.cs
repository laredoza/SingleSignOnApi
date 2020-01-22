// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeedData.cs" company="">
// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
// </copyright>
// <summary>
//   The seed data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SingleSignOn.Data.Seeding
{
    #region Usings

    using System;
    using System.Linq;
    using System.Security.Claims;
    using IdentityModel;
    using IdentityServer4.EntityFramework.DbContexts;
    using IdentityServer4.EntityFramework.Mappers;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using SingleSignOn.Data.Context;
    using SingleSignOn.IdentityServerAspNetIdentity.Models;

    #endregion

    /// <summary>
    /// Seed Identity and IdenityServer default data.
    /// </summary>
    public class SeedData : ISeedData
    {
        private readonly ApplicationDbContext _applicationContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PersistedGrantDbContext _persistedGrantDbContext;
        private readonly ConfigurationDbContext _configurationDbContext;
        public SeedData(
            ApplicationDbContext applicationContext,
            UserManager<ApplicationUser> userManager,
            PersistedGrantDbContext persistedDbContext,
            ConfigurationDbContext configurationDbContext)
        {
            this._applicationContext = applicationContext;
            this._userManager = userManager;
            this._persistedGrantDbContext = persistedDbContext;
            this._configurationDbContext = configurationDbContext;
        }

        #region Public Methods And Operators

        /// <summary>
        /// Seed Identity data. i.e) Users & Roles 
        /// </summary>
        /// <exception cref="Exception">
        /// </exception>
        public void SeedIdentity()
        {
            this._applicationContext.Database.Migrate();

            var admin = this._userManager.FindByNameAsync("admin").Result;
            if (admin == null)
            {
                admin = new ApplicationUser { UserName = "admin" };
                var result = this._userManager.CreateAsync(admin, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = this._userManager.AddClaimsAsync(
                    admin,
                    new[]
                        {
                                    new Claim(JwtClaimTypes.Name, "Admin User"),
                                    new Claim(JwtClaimTypes.GivenName, "Admin"),
                                    new Claim(JwtClaimTypes.FamilyName, "User"),
                                    new Claim(JwtClaimTypes.Email, "admin@email.com"),
                                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                                    new Claim(JwtClaimTypes.WebSite, "http://admin.com"),
                                    new Claim(
                                        JwtClaimTypes.Address,
                                        @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                                        IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                                    new Claim(JwtClaimTypes.Role, "admin"),
                                    new Claim("admin_clients", "true", ClaimValueTypes.Boolean),
                                    new Claim("admin_users", "true", ClaimValueTypes.Boolean),
                                    new Claim("admin_api_resource", "true", ClaimValueTypes.Boolean),
                                    new Claim("admin_identity_resource", "true", ClaimValueTypes.Boolean),
                                    new Claim("admin_roles", "true", ClaimValueTypes.Boolean)
                        }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                Console.WriteLine("alice created");
            }
            else
            {
                Console.WriteLine("alice already exists");
            }

            var bob = this._userManager.FindByNameAsync("bob").Result;
            if (bob == null)
            {
                bob = new ApplicationUser { UserName = "bob" };
                var result = this._userManager.CreateAsync(bob, "Pass123$").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = this._userManager.AddClaimsAsync(
                    bob,
                    new[]
                        {
                                    new Claim(JwtClaimTypes.Name, "Bob Smith"),
                                    new Claim(JwtClaimTypes.GivenName, "Bob"),
                                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                                    new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                                    new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                                    new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                                    new Claim(
                                        JwtClaimTypes.Address,
                                        @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                                        IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                                    new Claim("location", "somewhere")
                        }).Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                Console.WriteLine("bob created");
            }
            else
            {
                Console.WriteLine("bob already exists");
            }
        }

        /// <summary>
        /// Seed Identity Server setting. i.e) Clients, Servers, etc 
        /// </summary>
        public void SeedIdentityServer(string serverUrl)
        {
            this._persistedGrantDbContext.Database.Migrate();
            this._configurationDbContext.Database.Migrate();

            if (!_configurationDbContext.Clients.Any())
            {
                foreach (var client in Config.GetClients(serverUrl))
                {
                    _configurationDbContext.Clients.Add(client.ToEntity());
                }

                _configurationDbContext.SaveChanges();
            }

            if (!_configurationDbContext.IdentityResources.Any())
            {
                foreach (var resource in Config.GetIdentityResources())
                {
                    _configurationDbContext.IdentityResources.Add(resource.ToEntity());
                }

                _configurationDbContext.SaveChanges();
            }

            if (!_configurationDbContext.ApiResources.Any())
            {
                foreach (var resource in Config.GetApis())
                {
                    _configurationDbContext.ApiResources.Add(resource.ToEntity());
                }

                _configurationDbContext.SaveChanges();
            }
        }
    }

    #endregion
}