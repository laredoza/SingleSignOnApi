// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISeedData" company="">
//
// </copyright>
// <summary>
//   The class ISeedData.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SingleSignOn.Data.Seeding
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    ///     The ISeedData.
    /// </summary>
    public interface ISeedData
    {
        #region Public Properties


        #endregion

        #region Public Methods And Operators
        void SeedIdentity();
        void SeedIdentityServer(string serverUrl);

        #endregion

        #region Other Methods

        #endregion
    }
}