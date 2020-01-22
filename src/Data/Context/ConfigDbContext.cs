// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationDbContext.cs" company="">
// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.   
// </copyright>
// <summary>
//   The application db context.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SingleSignOn.Data.Context
{
    #region Usings

    using Microsoft.EntityFrameworkCore;
    using IdentityServer4.EntityFramework.DbContexts;
    using IdentityServer4.EntityFramework.Options;

    #endregion

    /// <summary>
    /// The ConfigDbContext context.
    /// </summary>
    public class ConfigDbContext : ConfigurationDbContext
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationDbContext"/> class.
        /// </summary>
        /// <param name="options">
        /// The options.
        /// </param>
        public ConfigDbContext(DbContextOptions<ConfigurationDbContext> options, ConfigurationStoreOptions storeOptions = null)
            : base(options,storeOptions)
        {
        }

        // /// <summary>
        // /// Initializes a new instance of the <see cref="ConfigurationDbContext"/> class.
        // /// </summary>
        // /// <param name="options">
        // /// The options.
        // /// </param>
        // public ConfigDbContext(DbContextOptions<ConfigurationDbContext> options)
        //     : base(options,new ConfigurationStoreOptions())
        // {
        // }

        #endregion

        #region Other Methods

        /// <summary>
        /// The on model creating.
        /// </summary>
        /// <param name="builder">
        /// The builder.
        /// </param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        #endregion
    }
}