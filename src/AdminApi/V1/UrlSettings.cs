// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UrlSettings" company="">
//
// </copyright>
// <summary>
//   The class UrlSettings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SingleSignOn.AdminApi
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    ///     The Config.
    /// </summary>
    public class UrlSettings
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Config"/> class.
        /// </summary>
        public UrlSettings()
        {
        }

        #endregion

        #region Public Properties

        public string Authority { get; set; }
        public string CorsUrl { get; set; }

        #endregion

        #region Public Methods And Operators


        #endregion

        #region Other Methods

        #endregion
    }
}